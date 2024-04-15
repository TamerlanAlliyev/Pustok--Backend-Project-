using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Pustok.Data;
using Pustok.Models;
using Pustok.ViewModels.Home;

namespace Pustok.ViewComponents
{
    public class HomeHeadProductsViewComponent : ViewComponent
    {
        readonly PustokContext _context;

        public HomeHeadProductsViewComponent(PustokContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IQueryable<Product> Products = _context.Products.Where(p => !p.IsDeleted).Include(p => p.Images);

            var DiscountedProducts = await Products.Where(p => p.DiscountPrice != null).ToListAsync();

            var MostProducts = await Products.Where(p => p.ClicketCount >= 10).ToListAsync();

            var currentTime = DateTime.UtcNow.AddHours(4);
            var timeSought = currentTime - TimeSpan.FromDays(5);

            var NewArrivals = await Products.Where(p => p.Created >= timeSought).ToListAsync();


            HomeHeadProductsVM HomeProducts = new HomeHeadProductsVM
            {
                DiscountedProducts = DiscountedProducts,
                MostProducts = MostProducts,
                NewArrivals = NewArrivals
            };


            return View(HomeProducts);
        }
    }
}
