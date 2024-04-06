using System.ComponentModel.DataAnnotations;

namespace Pustok.Areas.Admin.ViewModels.Categories
{
    public class CategoryUpdateVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;
        public int? CategoryId { get; set; }
    }
}
