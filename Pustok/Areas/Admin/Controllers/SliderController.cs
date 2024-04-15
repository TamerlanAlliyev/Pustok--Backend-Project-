using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Areas.Admin.Services.Interfaces;
using Pustok.Areas.Admin.ViewModels.SliderCM;
using Pustok.Data;
using Pustok.Models;

namespace Pustok.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class SliderController : Controller
    {
        readonly PustokContext _context;
        readonly IProductService _productService;
        readonly UserManager<AppUser> _userManager;
        public SliderController(PustokContext context, IProductService productService, UserManager<AppUser> userManager)
        {
            _context = context;
            _productService = productService;
            _userManager = userManager;
        }

        public async Task<IActionResult >Index()
        {
            var products = _context.Products.Where(p => !p.IsDeleted).Include(pi => pi.Images);

            var headerSlider = await _context.HeaderSlider.Where(hp => !hp.IsDeleted).ToListAsync();
            var sliderProducts = new List<Product>();

            foreach (var slider in headerSlider)
            {
                var product = await products.FirstOrDefaultAsync(p => p.Id == slider.ProductId);
                if (product != null)
                {
                    sliderProducts.Add(product);
                }
            }

            return View(sliderProducts);
        }


        public async Task<IActionResult> Create()
        {
            var Products = await _productService.GetAllAsync();

            List<SliderCreateVM> vm = new List<SliderCreateVM>();

            foreach (var product in Products)
            {
                vm.Add(new SliderCreateVM
                {
                    Products = product,
                    ProductId = product.Id,
                    IsCheked = _context.HeaderSlider.Where(hs => !hs.IsDeleted).FirstOrDefault(hs => hs.ProductId == product.Id) != null ? true : false
                });
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(List<SliderCreateVM> slider)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP Address";

            var user = await _userManager.GetUserAsync(User);
            string currentUser = user?.FullName ?? "Unknown Person";

            

            List<HeaderSlider> headerSlider = new List<HeaderSlider>();

            foreach (var item in slider)
            {
                var existingHeaderSlider = await _context.HeaderSlider.FirstOrDefaultAsync(h => h.ProductId == item.ProductId);
                if (item.IsCheked)
                {

                    if (existingHeaderSlider == null)
                    {
                        headerSlider.Add(new HeaderSlider
                        {
                            ProductId = item.ProductId,
                            CreatedBy = currentUser,
                            Created = DateTime.UtcNow.AddHours(4),
                            IPAddress = ipAddress
                        });
                    }
                    else 
                    {
                        _context.HeaderSlider.Remove(existingHeaderSlider);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    if (existingHeaderSlider != null)
                    {
                        _context.HeaderSlider.Remove(existingHeaderSlider);
                        await _context.SaveChangesAsync();
                    }
                }
            }


            _context.HeaderSlider.AddRange(headerSlider);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Delete(int? id)
        {
            if (id<1||id==null)
            {
                return View("Error404");
            }
            var existingHeaderSlider = await _context.HeaderSlider.FirstOrDefaultAsync(h => h.ProductId == id);
            if (existingHeaderSlider != null)
            {
                _context.HeaderSlider.Remove(existingHeaderSlider);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }



    }
}
