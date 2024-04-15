using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Data;

namespace Pustok.ViewComponents;

public class HeadCategoriesViewComponent : ViewComponent
{
    private readonly PustokContext _context;

    public HeadCategoriesViewComponent(PustokContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync(string? text)
    {
        var parentCategories = await _context.Categories.Where(c=>!c.IsDeleted &&c.ParentCategories==null).ToListAsync();
        var Categories = await _context.Categories.Where(c=>!c.IsDeleted).ToListAsync();

        ViewData["ParentCategories"]= parentCategories;
        ViewData["Categories"] = Categories;

        return View();
    }
}