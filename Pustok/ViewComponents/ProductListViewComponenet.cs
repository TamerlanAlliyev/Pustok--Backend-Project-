using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Data;
using Pustok.Models;
using Pustok.Services.Interfaces;
using Pustok.ViewModels.Pagenate;
using Pustok.ViewModels.Products;
using System.Linq;

namespace Pustok.ViewComponents
{
    [ViewComponent]
    public class ProductListViewComponenet : ViewComponent
    {
        readonly PustokContext _context;
        readonly IShopService _shopService;
        public ProductListViewComponenet(PustokContext context, IShopService shopService)
        {
            _context = context;
            _shopService = shopService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? categoryId, int? tagId, int? minPrice, int? maxPrice, int page = 1, int pageSize = 3)
        {
            if (page < 1)
            {
                page = 1;
            }
            IQueryable<Product> productsQuery = _context.Products
                 .Where(p => !p.IsDeleted)
                 .Include(p => p.Images)
                 .Include(p => p.Ratings)
                 .Include(p => p.BasketList)
                 .Include(p => p.WishList)
                 .Include(p => p.ProductCategory)
                     .ThenInclude(pc => pc.Category)
                 .Include(p => p.ProductTag)
                     .ThenInclude(pt => pt.Tag)
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize);

            var count = _shopService.GetPageCount(pageSize);

            if (categoryId != null)
            {
                productsQuery = productsQuery.Where(p => p.ProductCategory!.Any(cp => cp.CategoryId == categoryId));
            }
            if (tagId != null)
            {
                productsQuery = productsQuery.Where(p => p.ProductTag!.Any(cp => cp.TagId == tagId));
            }


            var tags = productsQuery
                      .Where(p => p.ProductCategory!.Any(c => c.CategoryId == categoryId))
                      .SelectMany(p => p.ProductTag)
                      .Where(pt => pt.ProductId != null)
                      .Select(pt => pt.Tag)
                      .Distinct()
                      .ToList();


            if (categoryId != null)
            {
                productsQuery = productsQuery.Where(p => p.ProductCategory.Any(pc => pc.CategoryId == categoryId));
            }
            if (tagId != null)
            {
                productsQuery = productsQuery.Where(p => p.ProductTag.Any(pt => pt.TagId == tagId));
            }

            if (minPrice != null && minPrice > 0)
            {
                productsQuery = productsQuery.Where(p => p.Price >= minPrice);
            }
            if (maxPrice != null && maxPrice > 0)
            {
                productsQuery = productsQuery.Where(p => p.Price <= maxPrice);
            }

            List<Models.Product> products = await productsQuery
                .OrderByDescending(p => p.Id)
                //.Take(20)
                .ToListAsync();


            ProductListVM productList = new ProductListVM
            {
                Products = products,
                TotalPageCount = count,
                CurrentPage = page,
                SelectedCategory = categoryId
                //SelectedTag = tagId
                //Tags = null
            };

            return View(productList);
        }
    }
}
