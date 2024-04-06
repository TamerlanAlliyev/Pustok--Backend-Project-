using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Data;

namespace Pustok.Areas.Admin.ViewComponents;

public class TagListVC:ViewComponent
{
    readonly PustokContext _context;

    public TagListVC(PustokContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var Tags = await _context.Tags.Where(t=>!t.IsDeleted).ToListAsync();
        return View(Tags);
    }
}
