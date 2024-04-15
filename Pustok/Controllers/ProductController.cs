using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pustok.Data;
using Pustok.Models;
using Pustok.ViewComponents;
using Pustok.ViewModels.Products;
using System.Drawing.Printing;
using Newtonsoft.Json;
using Pustok.ViewModels.Basket;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Pustok.ViewModels.Wish;

namespace Pustok.Controllers;

public class ProductController : Controller
{
    readonly PustokContext _context;
    readonly UserManager<AppUser> _userManager;
    public ProductController(PustokContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(int modalId)
    {


        var products = await _context.Products.Where(p => !p.IsDeleted)
                     .Include(p => p.Images)
                     .Include(p => p.Ratings)
                     .Include(p => p.BasketList)
                     .Include(p => p.WishList)
                     .Include(p => p.ProductCategory)
                        .ThenInclude(p => p.Category)
                     .Include(p => p.ProductTag)
                        .ThenInclude(p => p.Tag)
                     .ToListAsync();

        var modal = await _context.Products.Where(p => !p.IsDeleted)
                     .Include(p => p.Images)
                     .Include(p => p.Ratings)
                     .Include(p => p.BasketList)
                     .Include(p => p.WishList)
                     .Include(p => p.ProductCategory)
                        .ThenInclude(p => p.Category)
                     .Include(p => p.ProductTag)
                        .ThenInclude(p => p.Tag)
                        .FirstOrDefaultAsync(p => p.Id == modalId);

        var categories = await _context.Categories.Where(c => !c.IsDeleted).Include(c => c.ProductCategory).ThenInclude(c => c.Product).ToListAsync();
        var tags = await _context.Tags.Where(t => !t.IsDeleted).Include(t => t.ProductTag).ThenInclude(t => t.Product).ToListAsync();
        var TagCount = await _context.ProductTag.CountAsync();
        var ProductCategory = await _context.ProductCategories.CountAsync();
        var ProductTag = await _context.ProductTag.CountAsync();

        ProductListVM productList = new ProductListVM
        {
            ProductCount = products.Count,
            TagCount = TagCount,
            Tags = tags,
            Products = products,
            Categories = categories,
            CategoryCount = ProductCategory,
            Modal = modal
        };
        return View(productList);
    }



    public async Task<IActionResult> Detail(int? id)
    {
        if (id < 1 || id == null)
        {
            return View("Error404");
        }

        var product = await _context.Products
            .Where(p => !p.IsDeleted)
            .Include(p => p.Images)
            .Include(p => p.Ratings)
            .Include(p => p.BasketList)
            .Include(p => p.WishList)
            .Include(p => p.ProductCategory.Where(c => !c.Category.IsDeleted))
                .ThenInclude(p => p.Category)
            .Include(p => p.ProductTag.Where(t => !t.Tag.IsDeleted))
                .ThenInclude(p => p.Tag)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return View("Error404");
        }
        if (product.ClicketCount == null)
        {
            product.ClicketCount = 1;
        }
        else
        {
            product.ClicketCount++;
        }
        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        var categoryIds = product.ProductCategory.Select(pc => pc.Category.Id).ToList();

        var relatedProducts = await _context.Products
            .Where(p => !p.IsDeleted && p.ProductCategory.Any(pc => categoryIds.Contains(pc.Category.Id)))
            .Include(p => p.Images)
            .Include(p => p.ProductCategory.Where(c => !c.Category.IsDeleted))
            .ToListAsync();


        var tags = await _context.Tags
            .Where(t => !t.IsDeleted && t.ProductTag.Any(pt => pt.ProductId == product.Id))
            .ToListAsync();


        ProductDetailVM products = new ProductDetailVM
        {
            Tags = tags,
            Product = product,
            relatedProducts = relatedProducts
        };

    
        return View(products);
    }



    public IActionResult ProductCategoryFilter(int? categoryId, int? tagId, int? minPrice, int? maxPrice, int page = 1, int pageSize = 1)
    {

        return ViewComponent("ProductListViewComponenet", new { categoryId, tagId, minPrice, maxPrice, page, pageSize });
    }







    public IActionResult Filter(int? categoryId, int? tagId, int? minPrice, int? maxPrice)
    {
        return ViewComponent("Filter", new { categoryId = categoryId, tagId = tagId, minPrice = minPrice, maxPrice = maxPrice });
    }




















    public IActionResult Pagenate(int page, int pageSize = 1)
    {
        return ViewComponent("ProductListViewComponenet", new { page = page, pageSize = pageSize });
    }




    public async Task<IActionResult> AddWish(int? id)
    {
        if (id == null || id <= 0)
        {
            return BadRequest();
        }

        if (!User.Identity.IsAuthenticated)
        {
            if (HttpContext.Request.Cookies.ContainsKey("wish"))
            {
                var wishCookieValue = HttpContext.Request.Cookies["wish"];
                var wishItem = JsonConvert.DeserializeObject<List<WishVM>>(wishCookieValue);
                var itemToRemove = wishItem.FirstOrDefault(item => item.Id == id);
                if (itemToRemove != null)
                {
                    wishItem.Remove(itemToRemove);
                    var updatedWishCookieValue = JsonConvert.SerializeObject(wishItem);
                    HttpContext.Response.Cookies.Append("wish", updatedWishCookieValue, new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(30)
                    });
                }
                else
                {
                    var productBasket = await _context.Products.Where(p => !p.IsDeleted).FirstOrDefaultAsync(p => p.Id == id);
                    if (productBasket != null)
                    {
                        wishItem.Add(new WishVM { Id = (int)id });
                        var updatedWishCookieValue = JsonConvert.SerializeObject(wishItem);
                        HttpContext.Response.Cookies.Append("wish", updatedWishCookieValue, new CookieOptions
                        {
                            Expires = DateTime.Now.AddDays(30)
                        });
                    }
                }
            }
            else
            {
                var productBasket = await _context.Products.Where(p => !p.IsDeleted).FirstOrDefaultAsync(p => p.Id == id);
                if (productBasket != null)
                {
                    var wish = HttpContext.Request.Cookies["wish"];
                    List<WishVM> wishItems = wish == null ? new List<WishVM>() : JsonConvert.DeserializeObject<List<WishVM>>(wish);
                    var item = wishItems.SingleOrDefault(i => i.Id == id);
                    if (item == null)
                    {
                        item = new WishVM { Id = (int)id };
                        wishItems.Add(item);
                        HttpContext.Response.Cookies.Append("wish", JsonConvert.SerializeObject(wishItems), new CookieOptions
                        {
                            Expires = DateTime.Now.AddDays(30)
                        });
                    }
                }
            }
        }
        else
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP Address";
            var wish = await _context.Wishes.Where(w => w.ProductId == id && w.UserId == userId).FirstOrDefaultAsync();
            if (wish != null)
            {
                _context.Wishes.Remove(wish);
                await _context.SaveChangesAsync();
            }
            else
            {
                Wish newWish = new Wish
                {
                    ProductId = (int)id,
                    UserId = userId,
                    IsDeleted = false,
                    CreatedBy = userId,
                    Created = DateTime.UtcNow.AddHours(4),
                    IPAddress = ipAddress,
                };
                await _context.Wishes.AddAsync(newWish);
                await _context.SaveChangesAsync();
            }
        }
        return Ok();
    }











    public async Task<IActionResult> AddBasket(int? id)
    {
        if (id == null || id <= 0)
        {
            return BadRequest();
        }

        if (!User.Identity.IsAuthenticated)
        {
            var productBasket = await _context.Products.Where(p => !p.IsDeleted).FirstOrDefaultAsync(p => p.Id == id);

            var basket = HttpContext.Request.Cookies["basket"];

            List<BasketVM> basketItems = basket == null ? new List<BasketVM>() :
                JsonConvert.DeserializeObject<List<BasketVM>>(basket);

            var item = basketItems.SingleOrDefault(i => i.Id == id);

            if (item == null)
            {
                item = new BasketVM
                {
                    Id = (int)id,
                    Count = 1,
                };
                basketItems.Add(item);
            }
            else
            {
                item.Count++;
            }

            HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketItems));
        }
        else
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP Address";

            if (await _context.Baskets.AnyAsync(b => b.ProductId == id))
            {
                var bask = await _context.Baskets.Where(b => b.ProductId == id).FirstOrDefaultAsync();
                bask.Count++;
                bask.IPAddress = ipAddress;
                bask.UserId = userId;
                bask.ModifiedBy = userId;
                bask.Modified = DateTime.UtcNow.AddHours(4);

                await _context.SaveChangesAsync();
            }
            else
            {
                Basket basket = new Basket
                {
                    Count = 1,
                    ProductId = (int)id,
                    UserId = userId,
                    IsDeleted = false,
                    CreatedBy = userId,
                    Created = DateTime.UtcNow.AddHours(4),
                    IPAddress = ipAddress,
                };

                await _context.Baskets.AddAsync(basket);
                await _context.SaveChangesAsync();
            }

        }

        return Ok();
    }




    public IActionResult GetBasket()
    {
        return ViewComponent("Basket");
    }














    public async Task<IActionResult> BasketDelete(int id)
    {
        if (!User.Identity.IsAuthenticated)
        {

            if (HttpContext.Request.Cookies.ContainsKey("basket"))
            {
                var basketCookieValue = HttpContext.Request.Cookies["basket"];

                var basketItems = JsonConvert.DeserializeObject<List<BasketVM>>(basketCookieValue);


                var itemToRemove = basketItems.FirstOrDefault(item => item.Id == id);
                if (itemToRemove != null)
                {
                    basketItems.Remove(itemToRemove);

                    var updatedBasketCookieValue = JsonConvert.SerializeObject(basketItems);

                    HttpContext.Response.Cookies.Append("basket", updatedBasketCookieValue, new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(30)
                    });

                }

            }
        }
        else
        {
            if (await _context.Baskets.AnyAsync(b => b.ProductId == id))
            {
                var basket = await _context.Baskets.FirstOrDefaultAsync(b => b.ProductId == id);
                _context.Baskets.Remove(basket);
                await _context.SaveChangesAsync();
            }
        }
        return ViewComponent("Basket");
    }










}
