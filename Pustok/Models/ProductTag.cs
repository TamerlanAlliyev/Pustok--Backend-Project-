﻿using Pustok.Models.BaseEntitys;

namespace Pustok.Models;

public class ProductTag
{
	public int Id { get; set; }
	public int Count { get; set; }
	public int ProductId { get; set; }
	public Product Product { get; set; } = null!;
	public int TagId { get; set; }
	public Tag Tag { get; set; } = null!;
}