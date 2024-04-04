using Microsoft.AspNetCore.Identity;

namespace Pustok.Models;

public class AppUser:IdentityUser
{
	public string Name { get; set; } = null!;
	public string Surname { get; set; } = null!;
	public string FullName { get; set; } = null!;


	//Common relationships with Product
	public ICollection<ProductRating> Ratings { get; set; } = null!;
	public ICollection<Wish> WishList { get; set; } = null!;
	public ICollection<Basket> BasketList { get; set; } = null!;
}
