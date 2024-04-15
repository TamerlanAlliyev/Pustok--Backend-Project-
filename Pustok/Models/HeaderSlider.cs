using Pustok.Models.BaseEntitys;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok.Models;

public class HeaderSlider:BaseAuditable
{
	public int ProductId { get; set; }
	public Product Product { get; set; }=null!;
	[NotMapped]
	public IFormFile ImageFile { get; set; } = null!;
}
