namespace Pustok.Models.BaseEntitys;

public class BaseAuditable: BaseEntity
{
    public string CreatedBy { get; set; } = null!;
    public DateTime Created { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? Modified { get; set; }
    public string? IPAddress { get; set; }
}
