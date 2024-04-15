using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Data;
using Pustok.Models;
using Pustok.Services.Interfaces;
using Pustok.ViewModels.Products;
using System.Linq;
using System.Threading.Tasks;

namespace Pustok.ViewComponents
{
    public class FilterViewComponent : ViewComponent
    {
        private readonly PustokContext _context;
        private readonly IShopService _shopService;

        public FilterViewComponent(PustokContext context, IShopService shopService)
        {
            _context = context;
            _shopService = shopService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? categoryId, int? tagId, int? minPrice, int? maxPrice)
        {
            IQueryable<Tag> tagsQuery = _context.Tags.Where(t => !t.IsDeleted).Include(t => t.ProductTag).ThenInclude(pt => pt.Product);
            IQueryable<Category> categoriesQuery = _context.Categories.Where(c => !c.IsDeleted).Include(c => c.ProductCategory).ThenInclude(pc => pc.Product);

            if (minPrice != null)
            {
                tagsQuery = tagsQuery.Where(t => t.ProductTag.Any(pt => pt.Product.Price >= minPrice));
                categoriesQuery = categoriesQuery.Where(c => c.ProductCategory.Any(pc => pc.Product.Price >= minPrice));
            }

            if (maxPrice != null)
            {
                tagsQuery = tagsQuery.Where(t => t.ProductTag.Any(pt => pt.Product.Price <= maxPrice));
                categoriesQuery = categoriesQuery.Where(c => c.ProductCategory.Any(pc => pc.Product.Price <= maxPrice));
            }

            if (categoryId != null)
            {
                tagsQuery = tagsQuery.Where(t => t.ProductTag.Any(pt => pt.Product.ProductCategory.Any(pc => pc.CategoryId == categoryId)));
                categoriesQuery = categoriesQuery.Where(c => c.ProductCategory.Any(pc => pc.CategoryId == categoryId));
            }

            var tags = await tagsQuery.ToListAsync();
            var categories = await categoriesQuery.ToListAsync();

            var tagCount = tags.Count();
            var categoryCount = categories.Count();

            var productList = new ProductListVM
            {
                Tags = tags,
                Categories = categories,
                SelectedTag = tagId,
                SelectedCategory = categoryId,
                TagCount = tagCount,
                CategoryCount = categoryCount
            };

            return View(productList);
        }
    }
}
