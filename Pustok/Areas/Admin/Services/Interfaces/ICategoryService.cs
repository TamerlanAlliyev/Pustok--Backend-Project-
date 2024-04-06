using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pustok.Areas.Admin.ViewModels.Categories;
using Pustok.Models;

namespace Pustok.Areas.Admin.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<Category?> GetAsync(int? id);
        public Task<SelectList> SelectCategoriesAsync();
        public Task<CategoryDetailVM?> DetailAsync(int? id);
        public Task<CategoryUpdateVM?> UpdateViewAsync(int? id);
        public Task UpdateAsync(CategoryUpdateVM categoryVM, int? id, string ipAddress, string currentUser);
        public Task<IActionResult> CreateAsync(CategoryListVM categoryVM, string currentUser, string ipAddress);
        public Task<IActionResult> HardDeleteAsync(int? id);
        public Task<IActionResult> SoftDeleteAsync(int? id, string currentUser);
        public Task<IActionResult> ReturnItAsync(int? id, string currentUser);

    }
}
