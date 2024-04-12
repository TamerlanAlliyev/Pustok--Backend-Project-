using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pustok.ViewModels.Basket;

namespace Pustok.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        List<BasketVM> items = new List<BasketVM>();
        if (HttpContext.Request.Cookies["basket"] != null)
        {
            items = JsonConvert.DeserializeObject<List<BasketVM>>
                (HttpContext.Request.Cookies["basket"]);
        }
        TempData["BasketCount"]= items.Count();
        return View();
    }

    public IActionResult Detail()
    {
        return View();
    }
}
