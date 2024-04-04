using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Areas.Admin.ViewModels.Categories;
using Pustok.Data;
using System.Threading.Tasks;

namespace Pustok.Areas.Admin.ViewComponents
{
    public class CategoryListVC : ViewComponent
    {
        private readonly PustokContext _context;

        public CategoryListVC(PustokContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            CategoryListVM category = new CategoryListVM
            {
                Categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync()
            };
            return View(category);
        }
    }
}
