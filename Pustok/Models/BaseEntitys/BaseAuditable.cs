namespace Pustok.Models.BaseEntitys;

public class BaseAuditable: BaseEntity
{
    public int CreatedBy { get; set; }
    public DateTime Created { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? Modified { get; set; }
    public string? IPAddress { get; set; }
}
