using Microsoft.AspNetCore.Mvc;
using Pustok.Areas.Admin.ViewModels.ProductVM;
using Pustok.Models;

namespace Pustok.Areas.Admin.Services.Interfaces
{
    public interface IProductService
    {
        public Task<List<CheckCategory>?> CategorySelectAsync();
        public Task<List<CheckTag>?> TagSelectAsync();
        public Task<List<Product>> GetAllAsync();
        public Task<List<Product>> GetAllDeletedAsync();
        public Task<Product?> GetAsync(int? id);
        public Task<IActionResult> HardDeleteAsync(int? id);
        public Task<IActionResult> SoftDeleteAsync(int? id, string currentUser);
        public Task<IActionResult> ReturnItAsync(int? id, string currentUser);
        public ProductImages CreatImage(string Url, bool Main, bool Hover, string currentUser, string ipAddress, Product product);
        public Task<List<CheckCategory>> ProductSelectedCategories(Product product);
        public Task<List<CheckTag>> ProductSelectedTags(Product product);
        public Task<Product?> ProductUpdateGet(int id);
        public string? GetMainImageUrl(Product product);
        public string? GetHoverImageUrl(Product product);
        public List<string>? GetAdditionalImageUrls(Product product);
    }
}
