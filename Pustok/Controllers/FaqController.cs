using Microsoft.AspNetCore.Mvc;

namespace Pustok.Controllers
{
    public class FaqController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
