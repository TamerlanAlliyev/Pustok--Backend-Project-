using Pustok.Models;

namespace Pustok.Areas.Admin.ViewModels.SliderCM;

public class SliderCreateVM
{
    public bool IsCheked { get; set; }
    public int ProductId { get; set; }

    public Product? Products { get; set; }
}