using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Data;

namespace Pustok.ViewComponents
{
	public class ProductDetailViewComponent : ViewComponent
	{
		readonly PustokContext _context;

		public ProductDetailViewComponent(PustokContext context)
		{
			_context = context;
		}

		public async Task<IViewComponentResult> InvokeAsync(int id)
		{
			
			var product = await _context.Products.Where(p => !p.IsDeleted)
									 .Include(p => p.Images)
									 .Include(p => p.Ratings)
									 .Include(p => p.BasketList)
									 .Include(p => p.WishList)
									 .Include(p => p.ProductCategory)
										.ThenInclude(p => p.Category)
									 .Include(p => p.ProductTag)
										.ThenInclude(p => p.Tag)
									 .FirstOrDefaultAsync(p=>p.Id==id);

	

			return View(product);
		}
	}
}
