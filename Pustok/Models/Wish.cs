using Microsoft.AspNetCore.Identity;
using Pustok.Models.BaseEntitys;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok.Models
{
	public class Wish : BaseAuditable
	{
		public int Count { get; set; }
		public int ProductId { get; set; }
		public Product Product { get; set; } = null!;
		public string UserId { get; set; } = null!;
		[ForeignKey("UserId")]
		public AppUser User { get; set; } = null!;
	}
}
