using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pustok.Areas.Admin.ViewModels.Categories;
using Pustok.Data;
using Pustok.Models;

namespace Pustok.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        readonly PustokContext _context;
        readonly UserManager<AppUser> _userManager;
        public CategoryController(PustokContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.Where(c => !c.IsDeleted && c.ParentCategoryId == null).ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }



        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.Where(c => !c.IsDeleted&&c.ParentCategoryId==null).ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(CategoryListVM categoryVM)
        {
            var categories = await _context.Categories.Where(c => !c.IsDeleted && c.ParentCategoryId == null).ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");


            if (string.IsNullOrEmpty(categoryVM.CreatVM.Name))
            {
                ModelState.AddModelError("Name", "Category name cannot be empty.");
                return View(categoryVM);
            }
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            var user = await _userManager.GetUserAsync(User);
            string CreatedBy = user?.FullName ?? "Unknown Person";

            Category category = new Category
            {
                Name = categoryVM.CreatVM.Name,
                ParentCategoryId = categoryVM.CreatVM.CategoryId,
                Created = DateTime.UtcNow,
                CreatedBy = CreatedBy,
                IPAddress = ipAddress,
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult DeletedList()
        {
            return View();
        }

        public async Task<IActionResult> HardDelete(int? id)
        {
            if (id < 1 && id == null) return BadRequest();


            var categories = await _context.Categories.FindAsync(id);

            if (categories == null) return View("Error404");


            _context.Categories.Remove(categories);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SoftDelete(int? id)
        {
            if (id < 1 && id == null) return BadRequest();


            var categories = await _context.Categories.FindAsync(id);

            if (categories == null) return View("Error404");
            var user = await _userManager.GetUserAsync(User);
            string ModifiedBy = user?.FullName ?? "Unknown Person";

            categories.IsDeleted = true;
            categories.IPAddress = "";
            categories.ModifiedBy = ModifiedBy;
            categories.Modified = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ReturnIt(int? id)
        {
            if (id < 1 && id == null) return BadRequest();
            ;

            var categories = await _context.Categories.FindAsync(id);

            if (categories == null) return View("Error404");

            var user = await _userManager.GetUserAsync(User);
            string ModifiedBy = user?.FullName ?? "Unknown Person";

            categories.IsDeleted = false;
            categories.IPAddress = "";
            categories.ModifiedBy = ModifiedBy;
            categories.Modified = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id < 1 && id == null) return BadRequest();


            var category = await _context.Categories
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == id);

            var sub = await _context.Categories.Where(c => !c.IsDeleted && c.ParentCategoryId == id).ToListAsync();

            if (category == null) return View("Error404");

            var categoryDetailVM = new CategoryDetailVM
            {
                Category = category,
                ChildCategories = sub
            };

            return View(categoryDetailVM);
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id < 1 && id == null) return BadRequest();
            var category = await _context.Categories.FindAsync(id);
            var categories = await _context.Categories.Where(c => !c.IsDeleted&&c.ParentCategoryId==null).ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            if (category == null) return View("Error404");

            CategoryUpdateVM categoryCreat = new CategoryUpdateVM
            {
                Id = category.Id,
                Name = category.Name,
                CategoryId = category.ParentCategoryId,
            };

            return View(categoryCreat);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, CategoryUpdateVM categoryVM)
        {
            if (id < 1 && id == null && categoryVM.CategoryId != id) return BadRequest();

            if (string.IsNullOrEmpty(categoryVM.Name))
            {
                var categories = await _context.Categories.Where(c => !c.IsDeleted && c.ParentCategoryId == null).ToListAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                ModelState.AddModelError("Name", "Category name cannot be empty.");
                return View(categoryVM);
            }

            var category = await _context.Categories.FindAsync(categoryVM.Id);
            if (category == null)
            {
                return View("Error404");
            }

            var user = await _userManager.GetUserAsync(User);
            string ModifiedBy = user?.FullName ?? "Unknown Person";

            category.Id = categoryVM.Id;
            category.Name = categoryVM.Name;
            category.ParentCategoryId = categoryVM.CategoryId;
            category.Modified = DateTime.UtcNow;
            category.ModifiedBy = ModifiedBy;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
