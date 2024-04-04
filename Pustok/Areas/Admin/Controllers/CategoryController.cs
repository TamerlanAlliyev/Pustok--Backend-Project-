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
            CategoryListVM category = new CategoryListVM
            {
                Categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync()
            };
            return View(category);
        }



        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreatVM categoryVM)
        {
            var categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");


            if (string.IsNullOrEmpty(categoryVM.Name))
            {
                ModelState.AddModelError("Name", "Category name cannot be empty.");
                return View(categoryVM);
            }
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            Category category = new Category
            {
                Name = categoryVM.Name,
                ParentCategoryId = categoryVM.CategoryId,
                Created = DateTime.UtcNow,
                CreatedBy = 1,
                IPAddress = ipAddress,
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> SoftDelete(int id)
        {
            if (id < 1) return BadRequest();
           
            var categories = await _context.Categories.FindAsync(id);
            
            if(categories==null) return NotFound();

            categories.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
