using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Data;

namespace Pustok.ViewComponents
{
    public class BestSellerProductViewComponent:ViewComponent
    {
        readonly PustokContext _context;

        public BestSellerProductViewComponent(PustokContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await _context.Products.Where(p=>!p.IsDeleted).Include(pi=>pi.Images).Take(10).ToListAsync();
            return View(products);
        }
    }
}
