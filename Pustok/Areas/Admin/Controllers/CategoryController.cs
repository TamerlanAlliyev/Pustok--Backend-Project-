using Microsoft.AspNetCore.Mvc;

namespace Pustok.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
