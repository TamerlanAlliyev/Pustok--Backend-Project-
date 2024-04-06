using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pustok.Areas.Admin.ViewModels.TagsVM;
using Pustok.Data;
using Pustok.Models;
using System.Net;

namespace Pustok.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        readonly PustokContext _context;
        readonly UserManager<AppUser> _userManager;
        public TagController(PustokContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(TagCreateVM createVM)
        {
            if (!ModelState.IsValid)
            {
                return View(createVM);
            }

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var user = await _userManager.GetUserAsync(User);
            string CreatedBy = user?.FullName ?? "Unknown Person";
            Tag tag = new Tag
            {
                Name = createVM.Name,
                Created = DateTime.UtcNow,
                CreatedBy = CreatedBy,
                IPAddress = ipAddress,
            };
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }




        public async Task<IActionResult> Update(int? id)
        {
            if (id < 1 && id == null) return View("Error404");
            if (!ModelState.IsValid) return View();

            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return BadRequest();


            return View(tag);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Tag tag)
        {
            

            if (tag == null)
            {
                return View("Error404");
            }

            var _tag = _context.Tags.Where(t => !t.IsDeleted).FirstOrDefault(t => t.Id == tag.Id);
            if (_tag == null) return BadRequest();

            var user = await _userManager.GetUserAsync(User);
            string Modified = user?.FullName ?? "Unknown Person";

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            _tag.Name=tag.Name;
            _tag.Modified = DateTime.UtcNow;
            _tag.ModifiedBy = Modified;
            _tag.IPAddress = ipAddress;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id < 1 && id == null) return View("Error404");
            if (!ModelState.IsValid) return View();

            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return BadRequest();

            return View(tag);
        }






        public IActionResult DeletedList()
        {
            return View();
        }

        public async Task<IActionResult> HardDelete(int? id)
        {
            if (id < 1 && id == null) return BadRequest();


            var tag = await _context.Tags.FindAsync(id);

            if (tag == null) return View("Error404");

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SoftDelete(int? id)
        {
            if (id < 1 && id == null) return BadRequest();


            var tag = await _context.Tags.FindAsync(id);

            if (tag == null) return View("Error404");

            var user = await _userManager.GetUserAsync(User);
            string Modified = user?.FullName ?? "Unknown Person";

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            tag.Modified = DateTime.UtcNow;
            tag.ModifiedBy = Modified;
            tag.IPAddress = ipAddress;
            tag.IsDeleted = true;
            
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ReturnIt(int? id)
        {
            if (id < 1 && id == null) return BadRequest();
            ;

            var tag = await _context.Tags.FindAsync(id);

            if (tag == null) return View("Error404");

            var user = await _userManager.GetUserAsync(User);
            string Modified = user?.FullName ?? "Unknown Person";

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            tag.Modified = DateTime.UtcNow;
            tag.ModifiedBy = Modified;
            tag.IPAddress = ipAddress;
            tag.IsDeleted = false;
           
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
