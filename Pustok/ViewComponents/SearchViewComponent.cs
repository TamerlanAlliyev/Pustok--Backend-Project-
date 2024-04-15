using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Data;
using Pustok.Services.Interfaces;

namespace Pustok.ViewComponents;

public class SearchViewComponent : ViewComponent
{
    private readonly PustokContext _context;

    public SearchViewComponent(PustokContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync(string? text)
    {
        if (text == null)
        {
            return View();
        }

        text = text.ToLower().Trim();

        var products = await _context.Products.Where(p =>
             (p.Name != null && p.Name.Contains(text)) ||
             (p.Description != null && p.Description.Contains(text)) ||
             (p.Author != null && p.Author.Contains(text)) ||
             (p.Price != null && p.Price.ToString().Contains(text)) ||
             (p.DiscountPrice != null && p.DiscountPrice.ToString().Contains(text))
         ).Include(p=>p.Images).ToListAsync();

        if (products.Count == 0)
        {
            ViewData["Product"] = null;
        }
        else
        {
            ViewData["Product"] = products;
        }

        return View();
    }
}