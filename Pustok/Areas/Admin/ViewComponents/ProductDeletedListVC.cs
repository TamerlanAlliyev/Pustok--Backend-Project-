using Microsoft.AspNetCore.Mvc;
using Pustok.Areas.Admin.Services.Interfaces;

namespace Pustok.Areas.Admin.ViewComponents;

public class ProductDeletedListVC:ViewComponent
{
    readonly IProductService _productService;

    public ProductDeletedListVC(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var products =await _productService.GetAllDeletedAsync();
        return View(products);
    }
}
