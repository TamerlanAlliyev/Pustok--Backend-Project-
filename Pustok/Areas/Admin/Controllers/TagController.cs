using Microsoft.AspNetCore.Mvc;

namespace Pustok.Areas.Admin.Controllers
{
    public class TagController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
