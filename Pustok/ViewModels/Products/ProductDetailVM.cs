using Pustok.Models;

namespace Pustok.ViewModels.Products
{
    public class ProductDetailVM
    {
        public List<Tag>? Tags { get; set; }
        public Product Product { get; set; } = null!;
        public List<Product> relatedProducts { get; set; } = null!;
    }
}
