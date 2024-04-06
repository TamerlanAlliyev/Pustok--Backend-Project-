using System.Xml.Linq;

namespace Pustok.Areas.Admin.ViewModels.ProductVM
{
    public class ProductListVM
    {
        public string Name { get; set; } = null!;
        public decimal ExTax { get; set; } 
        public decimal Price { get; set; } 
        public decimal DiscountPrice { get; set; }
        public int Count{ get; set; }
        public string Image { get; set; } = null!;
    }
}

