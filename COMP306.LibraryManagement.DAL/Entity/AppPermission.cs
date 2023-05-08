using System;
using System.Collections.Generic;

namespace COMP306.LibraryManagement.DAL.Entity;

public partial class AppPermission
{
    public int Id { get; set; }

    public int SectionId { get; set; }

    public int ActionId { get; set; }

    public virtual AppAction Action { get; set; } = null!;

    public virtual AppSection Section { get; set; } = null!;

    public virtual ICollection<AppRole> Roles { get; set; } = new List<AppRole>();
}
