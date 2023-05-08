using System;
using System.Collections.Generic;

namespace COMP306.LibraryManagement.DAL.Entity;

public partial class AppRole
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<AppPermission> Permissions { get; set; } = new List<AppPermission>();

    public virtual ICollection<AppUser> Users { get; set; } = new List<AppUser>();
}
