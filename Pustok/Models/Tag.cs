using Pustok.Models.BaseEntitys;

namespace Pustok.Models;

public class Tag:BaseAuditable
{
	public string Name { get; set; } = null!;
	public ICollection<ProductTag> ProductTag { get; set; } = null!;

}
