using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pustok.Areas.Admin.Services.Interfaces;
using Pustok.Data;
using Pustok.Models;

namespace Pustok.Areas.Admin.Services.Implements;

public class ProductService : IProductService
{
    readonly PustokContext _context;

    public ProductService(PustokContext context)
    {
        _context = context;
    }
    public async Task<List<Product>> GetAllAsync() => await _context.Products.Where(p => !p.IsDeleted)
                                                                             .Include(p => p.Images.Where(i => !i.IsDeleted && i.IsMain))
                                                                             .ToListAsync();

    public async Task<List<Product>> GetAllDeletedAsync() => await _context.Products.Where(p => p.IsDeleted)
                                                                                    .Include(p => p.Images.Where(i => !i.IsDeleted && i.IsMain))
                                                                                    .ToListAsync();


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
        category.Modified = DateTime.UtcNow;

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
        category.Modified = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return new OkResult();

    }
}


