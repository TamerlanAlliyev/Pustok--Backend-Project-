using System.ComponentModel.DataAnnotations;

namespace Pustok.Areas.Admin.ViewModels.Categories;

public class CategoryCreatVM
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = null!;
    public int? CategoryId { get; set; }
}
