using System;
using System.Collections.Generic;
using Pustok.Models;

namespace Pustok.Areas.Admin.ViewModels.ProductVM
{
    public class ProductDetailVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal ExTax { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string ProductCode { get; set; } = null!;
        public int? RewardPoint { get; set; }
        public int Count { get; set; }
        public string CreatedBy { get; set; }=null!;
        public DateTime Created { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string IPAddress { get; set; } = null!;
        public string MainImage { get; set; } = null!;
        public string HoverImage { get; set; } = null!;
        public List<string>? AdditionalImages { get; set; }
        public HeaderSlider? HeaderSlider { get; set; }
        public ICollection<ProductTag> ProductTag { get; set; } = null!;
        public ICollection<ProductCategory> ProductCategory { get; set; } = null!;
        //public List<ProductImages> Images { get; set; } = null!;


        ////Common relationships with user
        public ICollection<ProductRating>? Ratings { get; set; }
        public ICollection<Wish>? WishList { get; set; }
        public ICollection<Basket>? BasketList { get; set; }
    }
}
