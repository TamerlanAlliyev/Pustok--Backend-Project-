using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok.Data;
using Pustok.Models;
using Pustok.ViewModels.Basket;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pustok.ViewComponents
{
    public class BasketViewComponent : ViewComponent
    {
        private readonly PustokContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        readonly UserManager<AppUser> _userManager;
        public BasketViewComponent(PustokContext context, IHttpContextAccessor contextAccessor, UserManager<AppUser> userManager)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            if (!User.Identity.IsAuthenticated)
            {
                List<BasketVM> items;
                if (_contextAccessor.HttpContext.Request.Cookies["basket"] != null)
                {
                    items = JsonConvert.DeserializeObject<List<BasketVM>>(_contextAccessor.HttpContext.Request.Cookies["basket"]);
                }
                else
                {
                    return View(new List<BasketItemProductVM>());
                }


                List<BasketItemProductVM> products = new List<BasketItemProductVM>();
                foreach (var item in items)
                {
                    var product = await _context.Products
                        .Where(p => !p.IsDeleted && p.Id == item.Id)
                        .Include(p => p.Images)
                        .FirstOrDefaultAsync();

                    if (product != null)
                    {
                        products.Add(new BasketItemProductVM
                        {
                            Count = item.Count,
                            Product = product,
                        });

                    }

                }


                if (products.Count == 0)
                {
                    return View(new List<BasketItemProductVM>());
                }

                TempData["BasketItemCount"] = products.Sum(p => p.Count);
                return View(products);
            }

            else
            {
                var userId = (User as ClaimsPrincipal)?.FindFirstValue(ClaimTypes.NameIdentifier);
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP Address";

                var baskets = await _context.Baskets.Where(b => b.UserId == userId).ToListAsync();


                List<BasketItemProductVM> products = new List<BasketItemProductVM>();
                foreach (var item in baskets)
                {
                    var product = await _context.Products
                        .Where(p => !p.IsDeleted && p.Id == item.ProductId)
                        .Include(p => p.Images)
                        .FirstOrDefaultAsync();

                    if (product != null)
                    {
                        products.Add(new BasketItemProductVM
                        {
                            Count = item.Count,
                            Product = product,
                        });

                    }

                }

                TempData["BasketItemCount"] = products.Sum(p => p.Count);
                
                return View(products);
            }

        }
    }
}
