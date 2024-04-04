using Pustok.Models.BaseEntitys;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok.Models;

public class HeaderSlider:BaseAuditable
{
	public string Name { get; set; } = null!;
	public string Description { get; set; } = null!;
	public string ImageUrl { get; set; }=null!;
	public decimal Price { get; set; }
	public int ProductId { get; set; }
	public Product Product { get; set; }=null!;


	[NotMapped]
	public IFormFile ImageFile { get; set; } = null!;
}
