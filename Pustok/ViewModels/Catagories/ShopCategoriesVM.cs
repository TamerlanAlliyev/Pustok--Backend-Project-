using Pustok.Models;

namespace Pustok.ViewModels.Catagories
{
	public class ShopCategoriesVM
	{
		public int ProductCount { get; set; }=default;
		public Category Category { get; set; } = null!;
	}
}
