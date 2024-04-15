using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pustok.Helpers.Interfaces;
using Pustok.Models;
using Pustok.ViewModels.Account;

namespace Pustok.Controllers
{
    public class AccountController : Controller
    {
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly IEmailService _emailService;
        public AccountController(UserManager<AppUser> userManager,
                                      SignInManager<AppUser> signInManager,
                                      IEmailService emailService = null!)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }






        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string ReturnUrl, AccountLoginVM LoginVM)
        {

            var user = await _userManager.FindByNameAsync(LoginVM.UsernameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(LoginVM.UsernameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "Username or Email  already exist");
                    return View(LoginVM);
                }
            }




            var time = TimeZoneInfo.Local;

            var result = await _signInManager.PasswordSignInAsync(user, LoginVM.Password, LoginVM.RememberMe, true);
            if (!result.IsLockedOut && user.LockoutEnd.HasValue)
            {
                ModelState.AddModelError("", "Wait until " + user.LockoutEnd.Value.AddHours(4).ToString("HH:mm:ss"));
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username,Email or Password is wrong");
                return View();

            }
            if (ReturnUrl != null)
            {
                return Redirect(ReturnUrl);
            }
            return RedirectToAction("Index", "Home");
        }






        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("Login");
        }






        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountRegisterVM registerVM)
        {
            if (!ModelState.IsValid)
                return View(registerVM);

            var userName = await _userManager.FindByNameAsync(registerVM.Username);
            if (userName != null)
            {
                ModelState.AddModelError("", "Username already exists");
                return View(registerVM);
            }

            var userEmail = await _userManager.FindByEmailAsync(registerVM.Email);
            if (userEmail != null)
            {
                ModelState.AddModelError("", "Email already exists");
                return View(registerVM);
            }

            AppUser user = new AppUser
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                FullName = $"{registerVM.Name} {registerVM.Surname}",
                Email = registerVM.Email,
                UserName = registerVM.Username,
            };

            var result = await _userManager.CreateAsync(user, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", $"{error.Code} - {error.Description}");
                }
                return View(registerVM);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "Customer");

            if (!roleResult.Succeeded)
            {
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerVM);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action("EmailConfirmation", "Account", new { token = token, user = user }, Request.Scheme);
            _emailService.Send(user.Email, "Email Confirmation", $"<a href='{link}'>Confirm</a>", true);

            return RedirectToAction("Login");
        }


        public async Task<IActionResult> EmailConfirmation(string token, string user)
        {
            if (String.IsNullOrWhiteSpace(token) || String.IsNullOrWhiteSpace(user))
                return BadRequest(ModelState);

            var us = await _userManager.FindByNameAsync(user);
            await _userManager.ConfirmEmailAsync(us, token);
            return RedirectToAction("Login");
        }



        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM passwordVM)
        {
            if (!ModelState.IsValid)
            {
                return View(passwordVM);
            }
            var userEmail = await _userManager.FindByEmailAsync(passwordVM.Email);
            if (userEmail == null)
            {
                ModelState.AddModelError("", "Email already exists");
                return View(userEmail);
            }


            var user = await _userManager.FindByIdAsync(userEmail.Id);

            if (user == null)
            {
                ModelState.AddModelError("", "User not found");
                return View(passwordVM);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var link = Url.Action("ResetPassword", "Account", new { token = token, user = user }, Request.Scheme);
            _emailService.Send(user.Email, "Reset Password", $"<a href='{link}'>Password Reset</a>", true);

            return RedirectToAction(nameof(Login));
        }

        public IActionResult ResetPassword(string token, string user)
        {
            if (String.IsNullOrWhiteSpace(token) || String.IsNullOrWhiteSpace(user))
                return BadRequest(ModelState);


            ResetPasswordVM reset = new ResetPasswordVM
            {
                user = user
            };

            return View(reset);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPassword)
        {
            var user = await _userManager.FindByNameAsync(resetPassword.user);


            await _userManager.ResetPasswordAsync(user, resetPassword.token, resetPassword.Password);

            return RedirectToAction("Login");
        }
    }
}
