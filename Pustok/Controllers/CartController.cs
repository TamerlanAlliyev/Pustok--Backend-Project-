using Microsoft.AspNetCore.Mvc;

namespace Pustok.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
