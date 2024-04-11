using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Data;
using Pustok.Models;
using Pustok.ViewModels.Products;

namespace Pustok.ViewComponents
{
	public class ProductViewComponent:ViewComponent
	{readonly PustokContext _context;

		public ProductViewComponent(PustokContext context)
		{
			_context = context;
		}
		public async Task<IViewComponentResult> InvokeAsync(int? categoryId)
		{
			IQueryable<Product> productsQuery = _context.Products.Where(p => !p.IsDeleted)
									 .Include(p => p.Images)
									 .Include(p => p.Ratings)
									 .Include(p => p.BasketList)
									 .Include(p => p.WishList)
									 .Include(p => p.ProductCategory)
										.ThenInclude(p => p.Category)
									 .Include(p => p.ProductTag)
										.ThenInclude(p => p.Tag);

			if (categoryId != null)
			{
				productsQuery = productsQuery.Where(p => p.ProductCategory!.Any(cp => cp.CategoryId == categoryId));
			}


			List<Product> products = await productsQuery
			   .OrderByDescending(p => p.Id)
			   .Take(20)
			   .ToListAsync();

			ProductListVM productList = new ProductListVM
			{
				Products = products,
			};

			return View(productList);
		}
	}
}
