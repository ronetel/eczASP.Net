using System;
using System.Collections.Generic;

namespace eczASP.Net.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string LoginUser { get; set; } = null!;

    public string PasswordUser { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int? RoleId { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Role? Role { get; set; }
}
