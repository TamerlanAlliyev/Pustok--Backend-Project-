using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Areas.Admin.Services.Implements;
using Pustok.Areas.Admin.Services.Interfaces;
using Pustok.Data;
using Pustok.Models;

namespace Pustok.Areas.Admin.Controllers;


[Area("Admin")]
public class ProductController : Controller
{
    readonly PustokContext _context;
    readonly IProductService _productService;
    readonly UserManager<AppUser> _userManager;
    public ProductController(PustokContext context, IProductService productService, UserManager<AppUser> userManager)
    {
        _context = context;
        _productService = productService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllAsync();
        return View(products);
    }

    public IActionResult DeletedList()
    {
        return View();
    }

    public async Task<IActionResult> HardDelete(int? id)
    {
        var result = await _productService.HardDeleteAsync(id);

        if (result is NotFoundResult)
            return View("Error404");

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> SoftDelete(int? id)
    {
        var user = await _userManager.GetUserAsync(User);
        string currentUser = user?.FullName ?? "Unknown Person";

        var result = await _productService.SoftDeleteAsync(id, currentUser);

        if (result is NotFoundResult)
            return View("Error404");

        return RedirectToAction("Index");
    }


    public async Task<IActionResult> ReturnIt(int? id)
    {
        var user = await _userManager.GetUserAsync(User);
        string currentUser = user?.FullName ?? "Unknown Person";

        var result = await _productService.ReturnItAsync(id, currentUser);
        if (result is NotFoundResult)
            return View("Error404");

        return RedirectToAction("Index");
    }
}
