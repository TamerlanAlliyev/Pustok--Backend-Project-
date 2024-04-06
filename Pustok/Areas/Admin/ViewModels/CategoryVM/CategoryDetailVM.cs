using Pustok.Models;

namespace Pustok.Areas.Admin.ViewModels.Categories
{
    public class CategoryDetailVM
    {
        public Category Category { get; set; } = null!;
        //public List<Category>? ChildCategories { get; set; }
        public ICollection<Category>? ChildCategories { get; set; }

    }
}
