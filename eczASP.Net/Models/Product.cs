using System;
using System.Collections.Generic;

namespace eczASP.Net.Models;

public partial class Product
{
    public int IdProduct { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }

    public string? DescriptionProduct { get; set; }

    public int StockQuantity { get; set; }

    public int? CategoryId { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category? Category { get; set; }
}
public class ProductViewModel
{
    public IEnumerable<Product> Products { get; set; }
    public string Search { get; set; }
    public string Sort { get; set; }
    public string Filter { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool? InStock { get; set; }
}
