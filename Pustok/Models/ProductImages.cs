using Pustok.Models.BaseEntitys;

namespace Pustok.Models;

public class ProductImages:BaseAuditable
{
    public string Url { get; set; } = null!;
    public bool IsMain { get; set; }
    public bool IsHover { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }=null!;
}
