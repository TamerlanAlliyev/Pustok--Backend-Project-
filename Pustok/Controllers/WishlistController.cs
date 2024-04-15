using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok.Data;
using Pustok.ViewModels.Wish;
using System.Security.Claims;

namespace Pustok.Controllers
{
    public class WishlistController : Controller
    {
        readonly PustokContext _context;

        public WishlistController(PustokContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> RemoveWish(int? id)
        {
            if (id == null || id <= 0)
            {
                return BadRequest();
            }

            if (!User.Identity.IsAuthenticated)
            {
                if (HttpContext.Request.Cookies.ContainsKey("wish"))
                {
                    var wishCookieValue = HttpContext.Request.Cookies["wish"];
                    var wishItem = JsonConvert.DeserializeObject<List<WishVM>>(wishCookieValue);
                    var itemToRemove = wishItem.FirstOrDefault(item => item.Id == id);
                    if (itemToRemove != null)
                    {
                        wishItem.Remove(itemToRemove);
                        var updatedWishCookieValue = JsonConvert.SerializeObject(wishItem);
                        HttpContext.Response.Cookies.Append("wish", updatedWishCookieValue, new CookieOptions
                        {
                            Expires = DateTime.Now.AddDays(30)
                        });
                    }
                }
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP Address";
                var wish = await _context.Wishes.Where(w => w.ProductId == id && w.UserId == userId).FirstOrDefaultAsync();
                if (wish != null)
                {
                    _context.Wishes.Remove(wish);
                    await _context.SaveChangesAsync();
                }
            }

            return ViewComponent("Wish");
        }

    }
}
