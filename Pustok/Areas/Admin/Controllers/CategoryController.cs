using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pustok.Areas.Admin.Services.Interfaces;
using Pustok.Areas.Admin.ViewModels.Categories;
using Pustok.Data;
using Pustok.Models;

namespace Pustok.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        readonly PustokContext _context;
        readonly UserManager<AppUser> _userManager;
        readonly ICategoryService _categoryService;


        public CategoryController(PustokContext context, UserManager<AppUser> userManager, ICategoryService categoryService)
        {
            _context = context;
            _userManager = userManager;
            _categoryService = categoryService;
        }





        public async Task<IActionResult> Index()
        {
            ViewBag.Categories = await _categoryService.SelectCategoriesAsync();
            return View();
        }



        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.SelectCategoriesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryListVM categoryVM)
        {
            ViewBag.Categories = await _categoryService.SelectCategoriesAsync();


            if (string.IsNullOrEmpty(categoryVM.CreatVM.Name))
            {
                ModelState.AddModelError("Name", "Category name cannot be empty.");
                return View(categoryVM);
            }
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP Address";

            var user = await _userManager.GetUserAsync(User);
            string currentUser = user?.FullName ?? "Unknown Person";

            try
            {
                await _categoryService.CreateAsync(categoryVM, currentUser, ipAddress);
            }
            catch
            {
                View("Error404");
            }

            return RedirectToAction(nameof(Index));
        }



        public IActionResult DeletedList()
        {
            return View();
        }



        public async Task<IActionResult> HardDelete(int? id)
        {
            try
            {
                var result = await _categoryService.HardDeleteAsync(id);

                if (result is NotFoundResult)
                    return View("Error404");

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error404");
            }

        }



        public async Task<IActionResult> SoftDelete(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            string currentUser = user?.FullName ?? "Unknown Person";

            try
            {
                var result = await _categoryService.SoftDeleteAsync(id, currentUser);
            }
            catch
            {
                return View("Error404");
            }


            return RedirectToAction("Index");
        }



        public async Task<IActionResult> ReturnIt(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            string currentUser = user?.FullName ?? "Unknown Person";

            try
            {
                var result = await _categoryService.ReturnItAsync(id, currentUser);
            }
            catch
            {
                return View("Error404");
            }

            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Detail(int? id)
        {
            try
            {
                var result = await _categoryService.DetailAsync(id);
                return View(result);
            }
            catch
            {
                return View("Error404");
            }

        }



        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Categories = await _categoryService.SelectCategoriesAsync();

            try
            {
                var result = await _categoryService.UpdateViewAsync(id);
                return View(result);
            }
            catch
            {
                return View("Error404");
            };

        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, CategoryUpdateVM categoryVM)
        {
            if (string.IsNullOrEmpty(categoryVM.Name))
            {
                ViewBag.Categories = await _categoryService.SelectCategoriesAsync();
                ModelState.AddModelError("Name", "Category name cannot be empty.");
                return View(categoryVM);
            }

            var user = await _userManager.GetUserAsync(User);
            string currentUser = user?.FullName ?? "Unknown Person";

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP Address";

            try
            {
                await _categoryService.UpdateAsync(categoryVM, id, currentUser, ipAddress);
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Error404");
            }

        }

    }
}
