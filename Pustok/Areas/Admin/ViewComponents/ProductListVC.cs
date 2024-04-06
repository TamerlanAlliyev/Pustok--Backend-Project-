using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Areas.Admin.Services.Interfaces;
using Pustok.Data;

namespace Pustok.Areas.Admin.ViewComponents;

public class ProductListVC : ViewComponent
{
    readonly PustokContext _context;
    readonly IProductService _productService;

    public ProductListVC(PustokContext context, IProductService productService)
    {
        _context = context;
        _productService = productService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var products = await _productService.GetAllAsync();
        return View(products);
    }
}

