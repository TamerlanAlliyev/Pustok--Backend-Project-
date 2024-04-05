using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Areas.Admin.ViewModels.Categories;
using Pustok.Data;

namespace Pustok.Areas.Admin.ViewComponents
{
    public class CategoryDeletedListVC : ViewComponent
    {
        readonly PustokContext _context;

        public CategoryDeletedListVC(PustokContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            CategoryListVM category = new CategoryListVM
            {
                Categories = await _context.Categories.Where(c => c.IsDeleted).ToListAsync()
            };

            if (category.Categories.Count == 0)
            {
                return Content("Category not found."); 
            }

            return View(category);
        }
    }
}
