using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok.Data;
using Pustok.ViewModels.Basket;
using Pustok.ViewModels.Wish;
using System.Security.Claims;

namespace Pustok.Controllers;

public class CartController : Controller
{
    readonly PustokContext _context;

    public CartController(PustokContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult GetBasket()
    {
        return ViewComponent("BasketList");
    }






}
