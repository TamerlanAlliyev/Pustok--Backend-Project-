using Pustok.Models;

namespace Pustok.ViewModels.Home
{
    public class HomeHeadProductsVM
    {
        public List<Product> DiscountedProducts { get; set; } = null!;
        public List<Product> NewArrivals { get; set; } = null!;
        public List<Product> MostProducts { get; set; } = null!;
        public List<Product> SliderProducts { get; set; } = null!;
        
        public List<Product> ChildrenProduct { get; set; } = null!;
        public List<Product> ArtsPhotographyProduct { get; set; } = null!;
    }
}


