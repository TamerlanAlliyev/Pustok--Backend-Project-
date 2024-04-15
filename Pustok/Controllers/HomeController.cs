using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok.Data;
using Pustok.Models;
using Pustok.ViewModels.Basket;
using Pustok.ViewModels.Home;

namespace Pustok.Controllers;

public class HomeController : Controller
{
    readonly PustokContext _context;

    public HomeController(PustokContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        List<BasketVM> items = new List<BasketVM>();
        if (HttpContext.Request.Cookies["basket"] != null)
        {
            string basketJson = HttpContext.Request.Cookies["basket"] ?? "0";
            if (basketJson != null)
            {
                items = JsonConvert.DeserializeObject<List<BasketVM>>(basketJson) ?? new List<BasketVM>();
            }
        }

        TempData["BasketCount"] = items.Count;
        var products = _context.Products.Where(p => !p.IsDeleted).Include(pi => pi.Images);

        var ChildrenProduct = await products.Where(p => p.ProductCategory.Any(pc => pc.Category.Name == "Children's")).ToListAsync();
        var ArtsPhotographyProduct = await products.Where(p => p.ProductCategory.Any(pc => pc.Category.Name == "ARTS & PHOTOGRAPHY")).ToListAsync();


        var headerSlider = await _context.HeaderSlider.Where(hp => !hp.IsDeleted).ToListAsync();
        var sliderProducts = new List<Product>();

        foreach (var slider in headerSlider)
        {
            var product = await products.FirstOrDefaultAsync(p => p.Id == slider.ProductId);
            if (product != null)
            {
                sliderProducts.Add(product);
            }
        }


        HomeHeadProductsVM homeHeadProductsVM = new HomeHeadProductsVM
        {
            ChildrenProduct = ChildrenProduct,
            ArtsPhotographyProduct = ArtsPhotographyProduct,
            SliderProducts = sliderProducts
        };
        return View(homeHeadProductsVM);


    }

    public IActionResult Detail()
    {
        return View();
    }

    public IActionResult Search(string? text)
    {
       
        return ViewComponent("Search", new {text=text});
    }

}
