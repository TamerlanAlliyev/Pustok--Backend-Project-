using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok.Data;
using Pustok.ViewModels.Basket;
using System.Security.Claims;

namespace Pustok.ViewComponents
{
    public class BasketListViewComponent:ViewComponent
    {
        readonly PustokContext _context;

        public BasketListViewComponent(PustokContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                List<BasketVM> items;
            if (HttpContext.Request.Cookies["basket"] != null)
            {
                items = JsonConvert.DeserializeObject<List<BasketVM>>(HttpContext.Request.Cookies["basket"]);
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

                return View(products);
            }
        }
    }
}
