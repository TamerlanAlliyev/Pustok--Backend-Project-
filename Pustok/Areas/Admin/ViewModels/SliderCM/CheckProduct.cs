using Pustok.Models;

namespace Pustok.Areas.Admin.ViewModels.SliderCM
{
    public class CheckProduct
    {
        public bool IsCheked { get; set; }
        public int Id { get; set; }

        public Product Products { get; set; } = null!;

    }
}
