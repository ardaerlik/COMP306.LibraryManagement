using System;
using System.Collections.Generic;

namespace COMP306.LibraryManagement.DAL.Entity;

public partial class TluStatetype
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<TftBook> Books { get; set; } = new List<TftBook>();
}
