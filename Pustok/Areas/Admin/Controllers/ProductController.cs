using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pustok.Areas.Admin.Services.Implements;
using Pustok.Areas.Admin.Services.Interfaces;
using Pustok.Areas.Admin.ViewModels.ProductVM;
using Pustok.Data;
using Pustok.Extentsions;
using Pustok.Helpers.Interfaces;
using Pustok.Models;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace Pustok.Areas.Admin.Controllers;


[Area("Admin")]
public class ProductController : Controller
{

    readonly PustokContext _context;
    readonly IProductService _productService;
    readonly UserManager<AppUser> _userManager;
    readonly IWebHostEnvironment _webHostEnvironment;

    public ProductController(PustokContext context,
                             IProductService productService,
                             UserManager<AppUser> userManager,
                             IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _productService = productService;
        _userManager = userManager;
        _webHostEnvironment = webHostEnvironment;
    }



    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllAsync();
        return View(products);
    }



    public async Task<IActionResult> Detail(int? id)
    {
        if (id < 1 || id == null) return View("Error404");

        var product = await _productService.DetailGetAsync(id);

        if (product == null) return View("Error404");

        var vm = _productService.ProductDetailVM(product);

        return View(vm);
    }



    public IActionResult DeletedList()
    {
        return View();
    }





    public async Task<IActionResult> HardDelete(int? id)
    {
        if (id < 1 || id == null) return View("Error404");

        var result = await _productService.HardDeleteAsync(id);

        if (result is NotFoundResult)
            return View("Error404");

        return RedirectToAction("Index");
    }



    public async Task<IActionResult> SoftDelete(int? id)
    {
        if (id < 1 || id == null) return View("Error404");

        var user = await _userManager.GetUserAsync(User);
        string currentUser = user?.FullName ?? "Unknown Person";

        var result = await _productService.SoftDeleteAsync(id, currentUser);

        if (result is NotFoundResult)
            return View("Error404");

        return RedirectToAction("Index");
    }



    public async Task<IActionResult> ReturnIt(int? id)
    {
        if (id < 1 || id == null) return View("Error404");

        var user = await _userManager.GetUserAsync(User);
        string currentUser = user?.FullName ?? "Unknown Person";

        var result = await _productService.ReturnItAsync(id, currentUser);
        if (result is NotFoundResult)
            return View("Error404");

        return RedirectToAction("Index");
    }







    public async Task<IActionResult> Create()
    {

        ProductCreateVM productCreateVM = new ProductCreateVM
        {
            Categories = await _productService.CategorySelectAsync(),
            Tags = await _productService.TagSelectAsync()
        };

        return View(productCreateVM);
    }


    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateVM productVM)
    {
        if (!ModelState.IsValid) { return View(productVM); }

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP Address";

        var user = await _userManager.GetUserAsync(User);
        string currentUser = user?.FullName ?? "Unknown Person";

        Product product = new Product
        {
            Name = productVM.Name,
            ProductCode = productVM.ProductCode,
            Description = productVM.Description,
            RewardPoint = productVM.RewardPoint,
            Count = productVM.Count,
            ExTax = productVM.ExTax,
            Price = productVM.Price,
            DiscountPrice = productVM.DiscountPrice,
            Created = DateTime.UtcNow,
            CreatedBy = currentUser,
            IPAddress = ipAddress,
            IsDeleted = false,
        };

        await _context.Products.AddAsync(product);



        List<ProductCategory> productCategories = new List<ProductCategory>();

        if (productVM.Categories != null)
        {
            foreach (var category in productVM.Categories)
            {
                if (category.IsChecked)
                {
                    productCategories.Add(new ProductCategory
                    {
                        Product = product,
                        CategoryId = category.Id,
                    });
                }
            }
        }

        await _context.ProductCategories.AddRangeAsync(productCategories);


        List<ProductTag> productTags = new List<ProductTag>();

        if (productVM.Tags != null)
        {
            foreach (var tag in productVM.Tags)
            {
                if (tag.IsChecked)
                {
                    productTags.Add(new ProductTag
                    {
                        Product = product,
                        TagId = tag.Id,
                    });
                }
            }
        }

        await _context.ProductTag.AddRangeAsync(productTags);



        List<ProductImages> images = new List<ProductImages>();

        if (productVM.MainFile != null)
        {
            if (!productVM.MainFile.FileSize(2))
            {
                ModelState.AddModelError("MainFile", "Files cannot be more than 2mb");
                return View(productVM);

            }
            if (!productVM.MainFile.FileTypeAsync("image"))
            {
                ModelState.AddModelError("MainFile", "Files must be image type!");
                return View(productVM);
            }
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "cilent", "image", "products");
            var fileName = await productVM.MainFile.SaveToAsync(path);

            var productImage = _productService.CreatImage(fileName, true, false, currentUser, ipAddress, product);
            images.Add(productImage);
        }


        if (productVM.HoverFile != null)
        {
            if (!productVM.HoverFile.FileSize(2))
            {
                ModelState.AddModelError("HoverFile", "Files cannot be more than 2mb");
                return View(productVM);

            }
            if (!productVM.HoverFile.FileTypeAsync("image"))
            {
                ModelState.AddModelError("HoverFile", "Files must be image type!");
                return View(productVM);
            }
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "cilent", "image", "products");
            var fileName = await productVM.HoverFile.SaveToAsync(path);

            var productImage = _productService.CreatImage(fileName, false, true, currentUser, ipAddress, product);
            images.Add(productImage);
        }


        if (productVM.AdditionFiles != null)
        {
            foreach (var files in productVM.AdditionFiles)
            {
                if (!files.FileSize(2))
                {
                    ModelState.AddModelError("Files", "Files cannot be more than 2mb");
                    return View(productVM);
                }

                if (!files.FileTypeAsync("image"))
                {
                    ModelState.AddModelError("Files", "Files must be image type!");
                    return View(productVM);
                }

                var path = Path.Combine(_webHostEnvironment.WebRootPath, "cilent", "image", "products");
                var fileName = await files.SaveToAsync(path);

                var productImage = _productService.CreatImage(fileName, false, false, currentUser, ipAddress, product);

                images.Add(productImage);
            }
        }

        await _context.ProductImages.AddRangeAsync(images);


        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }





    public async Task<IActionResult> DeleteImages(int? id)
    {
        if (id < 1 || id == null) return View("Error404");

        var path = Path.Combine(_webHostEnvironment.WebRootPath, "cilent", "image", "products");

        ProductImages? productImage = await _context.ProductImages.FirstOrDefaultAsync(pi => pi.Id == id);

        if (productImage == null)
            return View("Error404");

        string fileName = productImage.Url;

        if (fileName == null)
            return View("Error404");

        _productService.DeleteImagesService(path, fileName);

        var deleteImage = await _context.ProductImages.FirstOrDefaultAsync(pi => pi.Url == fileName);
        if (deleteImage != null)
            _context.ProductImages.Remove(deleteImage);

        await _context.SaveChangesAsync();
        var product = await _context.ProductImages.Where(pi => !pi.IsDeleted && !pi.IsMain && !pi.IsHover && pi.ProductId == productImage.ProductId).ToListAsync();

        return PartialView("_ProductPartialImage", product);
    }




    public async Task<IActionResult> Update(int? id)
    {
        if (id == null || id < 1)
            return View("Error404");

        var product = await _productService.ProductGetAsync(id.Value);

        if (product == null)
        {
            return View("Error404");
        }

        var categories = await _productService.ProductSelectedCategories(product);

        var tags = await _productService.ProductSelectedTags(product);


        string? mainUrl = _productService.GetMainImageUrl(product);

        string? hoverUrl = _productService.GetHoverImageUrl(product);

        List<string>? additionUrls = _productService.GetAdditionalImageUrls(product);



        ProductUpdateVM vm = new ProductUpdateVM
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            ExTax = product.ExTax,
            Price = product.Price,
            DiscountPrice = product.DiscountPrice,
            Count = product.Count,
            ProductCode = product.ProductCode,
            RewardPoint = product.RewardPoint,
            Categories = categories,
            Tags = tags
        };

        vm.ProductImages = await _context.ProductImages.Where(pi => !pi.IsDeleted && !pi.IsMain && !pi.IsHover && pi.ProductId == id).ToListAsync();

        if (mainUrl != null)
        {
            vm.MainUrl = mainUrl;
        }

        if (hoverUrl != null)
        {
            vm.HoverUrl = hoverUrl;
        }

        if (additionUrls != null && additionUrls.Count > 0)
        {
            vm.AdditionUrl = additionUrls;
        }

        return View(vm);
    }



    [HttpPost]
    public async Task<IActionResult> Update(int? id, ProductUpdateVM updateVM)
    {
        if (id < 1 || id != updateVM.Id || id == null)
            return View("Error404");

        //if (!ModelState.IsValid)
        //    return View(updateVM);

        var product = await _productService.ProductGetAsync(id.Value);

        if (product == null)
            return View("Error404");

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP Address";

        var user = await _userManager.GetUserAsync(User);
        string currentUser = user?.FullName ?? "Unknown Person";


        product.Name = updateVM.Name;
        product.Description = updateVM.Description;
        product.ExTax = updateVM.ExTax;
        product.Price = updateVM.Price;
        product.DiscountPrice = updateVM.DiscountPrice;
        product.Count = updateVM.Count;
        product.ProductCode = updateVM.ProductCode;
        product.RewardPoint = updateVM.RewardPoint;
        product.Created = DateTime.UtcNow.AddHours(4);
        product.CreatedBy = currentUser;
        product.IPAddress = ipAddress;



        List<ProductCategory> productCategories = new List<ProductCategory>();

        if (updateVM.Categories != null)
        {
            foreach (var category in updateVM.Categories)
            {
                if (category.IsChecked)
                {
                    productCategories.Add(new ProductCategory
                    {
                        Product = product,
                        CategoryId = category.Id,
                    });
                }
            }
        }
        await _context.ProductCategories.AddRangeAsync(productCategories);


        List<ProductTag> productTags = new List<ProductTag>();

        if (updateVM.Tags != null)
        {
            foreach (var tag in updateVM.Tags)
            {
                if (tag.IsChecked)
                {
                    productTags.Add(new ProductTag
                    {
                        Product = product,
                        TagId = tag.Id,
                    });
                }
            }
        }

        await _context.ProductTag.AddRangeAsync(productTags);


        if (updateVM.MainFile == null && updateVM.MainUrl == null)
            return View(updateVM);

        List<ProductImages> images = new List<ProductImages>();

        if (updateVM.MainFile != null)
        {
            if (!updateVM.MainFile.FileSize(2))
            {
                ModelState.AddModelError("MainFile", "Files cannot be more than 2mb");
                return View(updateVM);

            }
            if (!updateVM.MainFile.FileTypeAsync("image"))
            {
                ModelState.AddModelError("MainFile", "Files must be image type!");
                return View(updateVM);
            }
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "cilent", "image", "products");
            var fileName = await updateVM.MainFile.SaveToAsync(path);

            _productService.DeleteImagesService(path, updateVM.MainUrl);

            var deleteImage = await _context.ProductImages.FirstOrDefaultAsync(pi => pi.Url == updateVM.MainUrl);
            if (deleteImage != null)
                _context.ProductImages.Remove(deleteImage);

            var productImage = _productService.CreatImage(fileName, true, false, currentUser, ipAddress, product);
            images.Add(productImage);
        }







        if (updateVM.MainFile == null && updateVM.MainUrl == null)
            return View(updateVM);

        if (updateVM.HoverFile != null)
        {
            if (!updateVM.HoverFile.FileSize(2))
            {
                ModelState.AddModelError("HoverFile", "Files cannot be more than 2mb");
                return View(updateVM);

            }
            if (!updateVM.HoverFile.FileTypeAsync("image"))
            {
                ModelState.AddModelError("HoverFile", "Files must be image type!");
                return View(updateVM);
            }
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "cilent", "image", "products");
            var fileName = await updateVM.HoverFile.SaveToAsync(path);

            _productService.DeleteImagesService(path, updateVM.HoverUrl);

            var deleteImage = await _context.ProductImages.FirstOrDefaultAsync(pi => pi.Url == updateVM.HoverUrl);
            if (deleteImage != null)
                _context.ProductImages.Remove(deleteImage);

            var productImage = _productService.CreatImage(fileName, false, true, currentUser, ipAddress, product);
            images.Add(productImage);
        }


        if (updateVM.AdditionFiles != null)
        {
            foreach (var files in updateVM.AdditionFiles)
            {
                if (!files.FileSize(2))
                {
                    ModelState.AddModelError("Files", "Files cannot be more than 2mb");
                    return View(updateVM);
                }

                if (!files.FileTypeAsync("image"))
                {
                    ModelState.AddModelError("Files", "Files must be image type!");
                    return View(updateVM);
                }
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "cilent", "image", "products");
                var fileName = await files.SaveToAsync(path);


                var productImage = _productService.CreatImage(fileName, false, false, currentUser, ipAddress, product);

                images.Add(productImage);
            }
        }

        await _context.ProductImages.AddRangeAsync(images);


        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }






}
