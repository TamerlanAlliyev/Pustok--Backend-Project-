using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pustok.Areas.Admin.Services.Interfaces;
using Pustok.Areas.Admin.ViewModels.Categories;
using Pustok.Data;
using Pustok.Models;
using System.Net;

namespace Pustok.Areas.Admin.Services.Implements
{

    public class CategoryService : ICategoryService
    {
        readonly PustokContext _context;
        readonly UserManager<AppUser> _userManager;
        public CategoryService(PustokContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        public async Task<SelectList> SelectCategoriesAsync()
        {
            var categories = await _context.Categories.Where(c => !c.IsDeleted && c.ParentCategoryId == null).ToListAsync();
            return new SelectList(categories, "Id", "Name");
        }



        public async Task<IActionResult> CreateAsync(CategoryListVM categoryVM, string currentUser, string? ipAddress)
        {
            Category category = new Category
            {
                Name = categoryVM.CreatVM.Name,
                ParentCategoryId = categoryVM.CreatVM.CategoryId,
                Created = DateTime.UtcNow,
                CreatedBy = currentUser,
                IPAddress = ipAddress,
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return new OkResult();
        }



        public async Task<Category?> GetAsync(int? id)
        {
            if (id < 1 || id == null)
                return null;

            var category = await _context.Categories
                .Where(c => !c.IsDeleted)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return null;

            return category;
        }



        public async Task<CategoryDetailVM?> DetailAsync(int? id)
        {
            var category = await GetAsync(id);
            if (category == null)
                return null;

            var sub = await _context.Categories.Where(c => !c.IsDeleted && c.ParentCategoryId == id).ToListAsync();

            var categoryDetailVM = new CategoryDetailVM
            {
                Category = category,
                ChildCategories = sub
            };

            return categoryDetailVM;
        }



        public async Task<CategoryUpdateVM?> UpdateViewAsync(int? id)
        {
            var category =await GetAsync(id);
            if (category == null)
                return null;

            CategoryUpdateVM categoryCreat = new CategoryUpdateVM
            {
                Id = category.Id,
                Name = category.Name,
                CategoryId = category.ParentCategoryId,
            };

            return categoryCreat;
        }



        public async Task UpdateAsync( CategoryUpdateVM categoryVM, int? id, string ipAddress,string currentUser)
        {
            if (id < 1 || id == null && categoryVM.CategoryId != id) throw new Exception();

            var category = await GetAsync(id);

            if (category == null) 
                throw new Exception("Category not found.");

            category.Id = categoryVM.Id;
            category.Name = categoryVM.Name;
            category.ParentCategoryId = categoryVM.CategoryId;
            category.Modified = DateTime.UtcNow;
            category.ModifiedBy = currentUser;
            category.IPAddress = ipAddress;

            await _context.SaveChangesAsync();

        }



        public async Task<IActionResult> HardDeleteAsync(int? id)
        {
            if (id < 1 || id == null)
                return new BadRequestResult();

            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return new NotFoundResult();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return new OkResult();
        }



        public async Task<IActionResult> SoftDeleteAsync(int? id, string currentUser)
        {
            if (id < 1 || id == null)
                return new BadRequestResult();

            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return new NotFoundResult();

            category.IsDeleted = true;
            category.IPAddress = "";
            category.ModifiedBy = currentUser;
            category.Modified = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new OkResult();
        }



        public async Task<IActionResult> ReturnItAsync(int? id, string currentUser)
        {
            if (id < 1 || id == null)
                return new BadRequestResult();

            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return new NotFoundResult();

            category.IsDeleted = false;
            category.IPAddress = "";
            category.ModifiedBy = currentUser;
            category.Modified = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return new OkResult();

        }
    }
}
