using Pustok.Models;

namespace Pustok.ViewModels.Pagenate
{
    public class PaginateVM
    {
        public int TotalPageCount { get; set; }
        public int CurrentPage { get; set; }
        public List<Product> Products { get; set; } = null!;
    }
}
