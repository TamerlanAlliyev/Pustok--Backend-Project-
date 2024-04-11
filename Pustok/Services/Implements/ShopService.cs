using Microsoft.EntityFrameworkCore;
using Pustok.Data;
using Pustok.Services.Interfaces;

namespace Pustok.Services.Implements;

public class ShopService:IShopService
{
    readonly PustokContext _context;

    public ShopService(PustokContext context)
    {
        _context = context;
    }

    public int GetPageCount(int pageSize)
    {
        var productCount = _context.Products.Count();
        return (int)Math.Ceiling((decimal)productCount / pageSize);
    }

}
