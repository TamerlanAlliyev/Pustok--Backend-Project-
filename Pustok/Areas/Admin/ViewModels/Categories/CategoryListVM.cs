using Pustok.Models;

namespace Pustok.Areas.Admin.ViewModels.Categories
{
    public class CategoryListVM
    {
        public List<Category> Categories { get; set; } = null!;
        public CategoryCreatVM CreatVM { get; set; } = null!;
    }
}
