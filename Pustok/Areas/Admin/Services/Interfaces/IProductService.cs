using Microsoft.AspNetCore.Mvc;
using Pustok.Models;

namespace Pustok.Areas.Admin.Services.Interfaces
{
    public interface IProductService
    {
        public Task<List<Product>> GetAllAsync();
        public Task<List<Product>> GetAllDeletedAsync();
        public Task<Product?> GetAsync(int? id);
        public  Task<IActionResult> HardDeleteAsync(int? id);
        public Task<IActionResult> SoftDeleteAsync(int? id, string currentUser);
        public Task<IActionResult> ReturnItAsync(int? id, string currentUser);

    }
}
