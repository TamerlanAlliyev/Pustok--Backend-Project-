using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pustok.Data;
using Pustok.Models;
using Pustok.ViewModels;
using Pustok.ViewComponents;
using Pustok.ViewModels.Products;
using System.Drawing.Printing;

namespace Pustok.Controllers
{
    public class ProductController : Controller
    {
        readonly PustokContext _context;

        public ProductController(PustokContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int modalId)
        {


			var products = await _context.Products.Where(p => !p.IsDeleted)
						 .Include(p => p.Images)
						 .Include(p => p.Ratings)
						 .Include(p => p.BasketList)
						 .Include(p => p.WishList)
						 .Include(p => p.ProductCategory)
							.ThenInclude(p => p.Category)
						 .Include(p => p.ProductTag)
							.ThenInclude(p => p.Tag)
						 .ToListAsync();

			var modal = await _context.Products.Where(p => !p.IsDeleted)
						 .Include(p => p.Images)
						 .Include(p => p.Ratings)
						 .Include(p => p.BasketList)
						 .Include(p => p.WishList)
						 .Include(p => p.ProductCategory)
							.ThenInclude(p => p.Category)
						 .Include(p => p.ProductTag)
							.ThenInclude(p => p.Tag)
							.FirstOrDefaultAsync(p=>p.Id==modalId);

            var categories = await _context.Categories.Where(c => !c.IsDeleted).Include(c => c.ProductCategory).ThenInclude(c => c.Product).ToListAsync();
            var tags = await _context.Tags.Where(t => !t.IsDeleted).Include(t => t.ProductTag).ThenInclude(t => t.Product).ToListAsync();
            var TagCount = await _context.ProductTag.CountAsync();
            var ProductCategory = await _context.ProductCategories.CountAsync();
            var ProductTag = await _context.ProductTag.CountAsync();

            ProductListVM productList = new ProductListVM
            {
                ProductCount = products.Count,
                TagCount = TagCount,
                Tags = tags,
                Products = products,
                Categories = categories,
                CategoryCount= ProductCategory,
                Modal = modal
            };
            return View(productList);
        }



        public async Task<IActionResult> Detail(int? id)
        {
            if (id < 1 || id == null)
            {
                return View("Error404");
            }

            var product = await _context.Products
                .Where(p => !p.IsDeleted)
                .Include(p => p.Images)
                .Include(p => p.Ratings)
                .Include(p => p.BasketList)
                .Include(p => p.WishList)
                .Include(p => p.ProductCategory.Where(c => !c.Category.IsDeleted))
                    .ThenInclude(p => p.Category)
                .Include(p => p.ProductTag.Where(t => !t.Tag.IsDeleted))
                    .ThenInclude(p => p.Tag)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return View("Error404");
            }

            var categoryIds = product.ProductCategory.Select(pc => pc.Category.Id).ToList();

            var relatedProducts = await _context.Products
                .Where(p => !p.IsDeleted && p.ProductCategory.Any(pc => categoryIds.Contains(pc.Category.Id)))
                .Include(p => p.Images)
                .Include(p => p.ProductCategory.Where(c => !c.Category.IsDeleted))
                .ToListAsync();


            var tags = await _context.Tags
                .Where(t => !t.IsDeleted && t.ProductTag.Any(pt => pt.ProductId == product.Id))
                .ToListAsync();


            ProductDetailVM products = new ProductDetailVM
			{
				Tags = tags,
				Product = product,
				relatedProducts = relatedProducts
			};

            return View(products);
        }



		public IActionResult ProductCategoryFilter(int? categoryId, int? tagId,int? minPrice,int? maxPrice, int page = 1, int pageSize = 1)
		{
			
			return ViewComponent("ProductListViewComponenet", new { categoryId, tagId, minPrice, maxPrice , page , pageSize });
		}

	}
}
