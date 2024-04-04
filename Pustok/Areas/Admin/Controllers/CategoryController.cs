using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Data;

namespace Pustok.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        readonly PustokContext _context;

        public CategoryController(PustokContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.Where(c => !c.IsDeleted).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
