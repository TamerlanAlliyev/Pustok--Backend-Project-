using Pustok.Models.BaseEntitys;

namespace Pustok.Models;

public class Category: BaseAuditable
{
    public string Name { get; set; } = null!;
    public int? ParentCategoryId { get; set; }
    public Category? ParentCategories { get; set; }
    public ICollection<Category>? ChildCategories { get; set; }
}
