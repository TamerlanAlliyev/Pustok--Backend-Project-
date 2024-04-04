using Microsoft.AspNetCore.Mvc;

namespace Pustok.Controllers
{
	public class WishlistController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
