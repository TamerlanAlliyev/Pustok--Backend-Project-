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

        public CategoryController(PustokContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }



        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(CategoryListVM categoryVM)
        {
            var categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");


            if (string.IsNullOrEmpty(categoryVM.CreatVM.Name))
            {
                ModelState.AddModelError("Name", "Category name cannot be empty.");
                return View(categoryVM);
            }
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            Category category = new Category
            {
                Name = categoryVM.CreatVM.Name,
                ParentCategoryId = categoryVM.CreatVM.CategoryId,
                Created = DateTime.UtcNow,
                CreatedBy = 1,
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

            if (categories == null) return NotFound();

            _context.Categories.Remove(categories);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SoftDelete(int? id)
        {
            if (id < 1 && id == null) return BadRequest();


            var categories = await _context.Categories.FindAsync(id);

            if (categories == null) return NotFound();

            categories.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ReturnIt(int? id)
        {
            if (id < 1 && id == null) return BadRequest();
            ;

            var categories = await _context.Categories.FindAsync(id);

            if (categories == null) return NotFound();

            categories.IsDeleted = false;
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

            if (category == null) return NotFound();

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
            var categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            if (category == null) return NotFound();

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
                var categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                ModelState.AddModelError("Name", "Category name cannot be empty.");
                return View(categoryVM);
            }

            var category = await _context.Categories.FindAsync(categoryVM.Id);
            if (category == null)
            {
                return NotFound();
            }

            category.Id = categoryVM.Id;
            category.Name = categoryVM.Name;
            category.ParentCategoryId = categoryVM.CategoryId;


            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
