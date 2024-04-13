using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok.Data;
using Pustok.ViewModels.Basket;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.ViewComponents
{
    public class BasketViewComponent : ViewComponent
    {
        private readonly PustokContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public BasketViewComponent(PustokContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
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

            TempData["BasketItemCount"] = products.Sum(p=>p.Count);


            return View(products);
        }
    }
}
