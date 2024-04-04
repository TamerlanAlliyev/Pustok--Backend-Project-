using Microsoft.AspNetCore.Mvc;

namespace Pustok.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
