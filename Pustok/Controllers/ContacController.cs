using Microsoft.AspNetCore.Mvc;

namespace Pustok.Controllers
{
    public class ContacController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
