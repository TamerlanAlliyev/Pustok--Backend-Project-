using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pustok.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok.Areas.Admin.ViewModels.ProductVM;



public class ProductCreateVM
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Author { get; set; } = null!;
    public decimal ExTax { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public string ProductCode { get; set; } = null!;
    public int? RewardPoint { get; set; }
    public int Count { get; set; }
    public int SelectedTagId { get; set; }
    public List<CheckTag>? Tags { get; set; }
    public int SelectedCategoryId { get; set; }
    public List<CheckCategory>? Categories { get; set; }


    [NotMapped]
    public IFormFile MainFile { get; set; } = null!;
    [NotMapped]
    public IFormFile? HoverFile { get; set; }
    [NotMapped]
    public List<IFormFile>? AdditionFiles { get; set; }
}