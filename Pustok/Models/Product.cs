using Pustok.Models.BaseEntitys;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok.Models
{
    public class Product : BaseAuditable
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Author { get; set; } = null!;
        public decimal ExTax { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string ProductCode { get; set; } = null!;
        public int? RewardPoint { get; set; }
        public int Count { get; set; }
        //public bool Availability { get; set; }
        public HeaderSlider? HeaderSlider { get; set; }
        public ICollection<ProductTag> ProductTag { get; set; } = null!;
        public ICollection<ProductCategory> ProductCategory { get; set; } = null!;
        public List<ProductImages> Images { get; set; } = null!;


        ////Common relationships with user
        public ICollection<ProductRating>? Ratings { get; set; }
        public ICollection<Wish>? WishList { get; set; }
        public ICollection<Basket>? BasketList { get; set; }


        [NotMapped]
        public IFormFile MainFile { get; set; } = null!;
        [NotMapped]
        public IFormFile HoverFile { get; set; } = null!;
        [NotMapped]
        public List<IFormFile>? AditionFiles { get; set; }
    }
}
