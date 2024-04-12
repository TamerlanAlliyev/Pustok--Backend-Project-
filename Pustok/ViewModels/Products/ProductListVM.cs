using Pustok.Models;
using Pustok.ViewModels.Basket;
using Pustok.ViewModels.Catagories;

namespace Pustok.ViewModels.Products
{
	public class ProductListVM
	{
		public int CurrentPage { get; set; }
		public int TotalPageCount { get; set; }
        public int CategoryCount { get; set; }
		public int? SelectedCategory{ get; set; }
		public int? SelectedTag{ get; set; }
		public int TagCount { get; set; }
        public int ProductCount { get; set; }
		public List<Tag>? Tags { get; set; }
		public Product? Modal { get; set; }
		public List<Product> Products { get; set; } = null!;
		public List<Category>? Categories { get; set; }

		public List<BasketItemProductVM> baskets { get; set; } = null!;
	}
}

