using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok.Data;
using Pustok.Models;
using Pustok.ViewModels.Basket;
using Pustok.ViewModels.Wish;
using System.Security.Claims;

namespace Pustok.ViewComponents
{
    public class WishViewComponent:ViewComponent
    {
        private readonly PustokContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        readonly UserManager<AppUser> _userManager;
        public WishViewComponent(PustokContext context, IHttpContextAccessor contextAccessor, UserManager<AppUser> userManager)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            if (!User.Identity.IsAuthenticated)
            {
                List<WishVM> items;
                if (_contextAccessor.HttpContext.Request.Cookies["wish"] != null)
                {
                    items = JsonConvert.DeserializeObject<List<WishVM>>(_contextAccessor.HttpContext.Request.Cookies["wish"]);
                }
                else
                {
                    return View(new List<WishItemProductVM>());
                }


                List<WishItemProductVM> products = new List<WishItemProductVM>();
                foreach (var item in items)
                {
                    var product = await _context.Products
                        .Where(p => !p.IsDeleted && p.Id == item.Id)
                        .Include(p => p.Images)
                        .FirstOrDefaultAsync();

                    if (product != null)
                    {
                        products.Add(new WishItemProductVM
                        {
                            Product = product,
                        });

                    }

                }


                if (products.Count == 0)
                {
                    return View(new List<WishItemProductVM>());
                }
    
                return View(products);
            }

            else
            {
                var userId = (User as ClaimsPrincipal)?.FindFirstValue(ClaimTypes.NameIdentifier);
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP Address";

                var wishes = await _context.Wishes.Where(b => b.UserId == userId).ToListAsync();


                List<WishItemProductVM> products = new List<WishItemProductVM>();
                foreach (var item in wishes)
                {
                    var product = await _context.Products
                        .Where(p => !p.IsDeleted && p.Id == item.ProductId)
                        .Include(p => p.Images)
                        .FirstOrDefaultAsync();

                    if (product != null)
                    {
                        products.Add(new WishItemProductVM
                        {
                            Product = product
                        });

                    }

                }

                return View(products);
            }

        }
    }
}
