using System;
using System.Collections.Generic;

namespace eczASP.Net.Models;

public partial class CartItem
{
    public int IdCartItem { get; set; }

    public int? ProductId { get; set; }

    public int Quantity { get; set; }

    public int? UserId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
