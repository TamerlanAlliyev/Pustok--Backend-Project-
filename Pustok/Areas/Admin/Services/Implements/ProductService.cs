using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pustok.Areas.Admin.Services.Interfaces;
using Pustok.Areas.Admin.ViewModels.ProductVM;
using Pustok.Data;
using Pustok.Extentsions;
using Pustok.Models;

namespace Pustok.Areas.Admin.Services.Implements;

public class ProductService : IProductService
{
    readonly PustokContext _context;
    readonly IWebHostEnvironment _webHostEnvironment;
    public ProductService(PustokContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }
    public async Task<List<Product>> GetAllAsync() => await _context.Products.Where(p => !p.IsDeleted)
                                                                             .Include(p => p.Images.Where(i => !i.IsDeleted && i.IsMain))
                                                                             .ToListAsync();

    public async Task<List<Product>> GetAllDeletedAsync() => await _context.Products.Where(p => p.IsDeleted)
                                                                                    .Include(p => p.Images.Where(i => !i.IsDeleted && i.IsMain))
                                                                                    .ToListAsync();


    public async Task<Product?> DetailGetAsync(int? id)
    {
        if (id < 1 || id == null)
        {
            return null;
        }
        var product = await _context.Products.Where(p => !p.IsDeleted)
                                                   .Include(p => p.Images)
                                                   .Include(p => p.Ratings)
                                                   .Include(p => p.BasketList)
                                                   .Include(p => p.WishList)
                                                   .Include(p => p.ProductCategory)
                                                      .ThenInclude(p => p.Category)
                                                   .Include(p => p.ProductTag)
                                                      .ThenInclude(p => p.Tag)
                                                   .FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            return null;
        }
        return product;
    }

    public ProductDetailVM ProductDetailVM(Product product)
    {
        ProductDetailVM vm = new ProductDetailVM
        {
            Id = product.Id,
            Name = product.Name,
            Author = product.Author,
            Description = product.Description,
            ExTax = product.ExTax,
            Price = product.Price,
            DiscountPrice = product.DiscountPrice,
            Count = product.Count,
            RewardPoint = product.RewardPoint,
            Ratings = product.Ratings,
            ProductCategory = product.ProductCategory,
            ProductTag = product.ProductTag,
            BasketList = product.BasketList,
            WishList = product.WishList,
            ProductCode = product.ProductCode,
            HeaderSlider = product.HeaderSlider,
            Created = product.Created,
            CreatedBy = product.CreatedBy,
            Modified = product.Modified,
            ModifiedBy = product.ModifiedBy,
            IPAddress = product.IPAddress,
            MainImage = product.Images.FirstOrDefault(pi => !pi.IsDeleted && pi.IsMain)?.Url ?? "",
            HoverImage = product.Images.FirstOrDefault(pi => !pi.IsDeleted && pi.IsHover)?.Url ?? "",
            AdditionalImages = product.Images.Where(pi => !pi.IsDeleted && !pi.IsMain && !pi.IsHover)
                                   .Select(pi => pi.Url)
                                   .ToList(),

        };
        return vm;
    }

    public async Task<Product?> GetAsync(int? id)
    {
        if (id < 0 || id == null)
        {
            return null;
        }

        return await _context.Products.Where(p => !p.IsDeleted && p.Id == id)
                                      .Include(p => p.ProductTag)
                                      .Include(p => p.ProductCategory)
                                      .Include(p => p.Images)
                                      .Include(p => p.Ratings)
                                      .Include(p => p.WishList)
                                      .Include(p => p.BasketList)
                                      .FirstOrDefaultAsync();
    }



    public async Task<IActionResult> HardDeleteAsync(int? id)
    {
        if (id < 1 || id == null)
            return new BadRequestResult();

        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return new NotFoundResult();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return new OkResult();
    }

    public async Task<IActionResult> SoftDeleteAsync(int? id, string currentUser)
    {
        if (id < 1 || id == null)
            return new BadRequestResult();

        var category = await _context.Products.FindAsync(id);

        if (category == null)
            return new NotFoundResult();

        category.IsDeleted = true;
        category.IPAddress = "";
        category.ModifiedBy = currentUser;
        category.Modified = DateTime.UtcNow.AddHours(4);

        await _context.SaveChangesAsync();

        return new OkResult();
    }



    public async Task<IActionResult> ReturnItAsync(int? id, string currentUser)
    {
        if (id < 1 || id == null)
            return new BadRequestResult();

        var category = await _context.Products.FindAsync(id);

        if (category == null)
            return new NotFoundResult();

        category.IsDeleted = false;
        category.IPAddress = "";
        category.ModifiedBy = currentUser;
        category.Modified = DateTime.UtcNow.AddHours(4);

        await _context.SaveChangesAsync();
        return new OkResult();

    }




    public async Task<List<CheckCategory>?> CategorySelectAsync()
    {
        var categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();

        List<CheckCategory> categoryList = new List<CheckCategory>();

        foreach (var _category in categories)
        {
            categoryList.Add(new CheckCategory
            {
                Id = _category.Id,
                Name = _category.Name,
                IsChecked = false
            });
        }

        return categoryList;
    }


    public async Task<List<CheckTag>?> TagSelectAsync()
    {
        var tags = await _context.Tags.Where(t => !t.IsDeleted).ToListAsync();


        List<CheckTag> tagList = new List<CheckTag>();

        foreach (var _tag in tags)
        {
            tagList.Add(new CheckTag
            {
                Id = _tag.Id,
                Name = _tag.Name,
                IsChecked = false
            });
        }



        return tagList;
    }



    public ProductImages CreatImage(string Url, bool Main, bool Hover, string currentUser, string ipAddress, Product product)
    {
        return new ProductImages
        {
            Url = Url,
            IsMain = Main,
            IsHover = Hover,
            Product = product,
            IsDeleted = false,
            Created = DateTime.UtcNow,
            CreatedBy = currentUser,
            IPAddress = ipAddress,
        };
    }









    public async Task<Product?> ProductGetAsync(int id)
    {
        var product = await _context.Products
                        .Where(p => !p.IsDeleted)
                        .Include(p => p.Images)
                        .Include(p => p.ProductCategory)
                            .ThenInclude(p => p.Category)
                        .Include(p => p.ProductTag)
                            .ThenInclude(p => p.Tag)
                        .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return null;
        }
        return product;
    }



    public async Task<List<CheckCategory>> ProductSelectedCategories(Product product)
    {
        var selectedCategories = product.ProductCategory.Select(pc => pc.Category.Id).ToList();

        var categories = await CategorySelectAsync();

        if (categories != null)
        {
            foreach (var category in categories)
            {
                if (selectedCategories.Contains(category.Id))
                {
                    category.IsChecked = true;
                }
            }
        }
        else
        {
            categories = new List<CheckCategory>();
        }

        return categories;
    }


    public async Task<List<CheckTag>> ProductSelectedTags(Product product)
    {
        var selectedTags = product.ProductTag.Select(pt => pt.Tag.Id).ToList();

        var tags = await TagSelectAsync();


        if (tags != null)
        {
            foreach (var tag in tags)
            {
                if (selectedTags.Contains(tag.Id))
                {
                    tag.IsChecked = true;
                }
            }
        }
        else
        {
            tags = new List<CheckTag>();
        }
        return tags;
    }







    public string? GetMainImageUrl(Product product)
    {
        if (product.Images != null && product.Images.Any(p => p.IsMain))
        {
            return product.Images.FirstOrDefault(p => p.IsMain)?.Url;
        }
        return null;
    }

    public string? GetHoverImageUrl(Product product)
    {
        if (product.Images != null && product.Images.Any(p => p.IsHover))
        {
            return product.Images.FirstOrDefault(p => p.IsHover)?.Url;
        }
        return null;
    }

    public List<string>? GetAdditionalImageUrls(Product product)
    {
        List<string> additionUrls = new List<string>();

        if (product.Images != null)
        {
            foreach (var image in product.Images.Where(p => !p.IsHover && !p.IsMain))
            {
                additionUrls.Add(image.Url);
            }
        }

        return additionUrls;
    }



    public void DeleteImagesService(string path, string fileName)
    {

        if (!string.IsNullOrEmpty(fileName))
        {
            var oldImagePath = Path.Combine(path, fileName);
            if (File.Exists(oldImagePath))
            {
                File.Delete(oldImagePath);
            }
        }
    }
}


