using System;
using System.Collections.Generic;

namespace COMP306.LibraryManagement.DAL.Entity;

public partial class TluLanguage
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public virtual ICollection<TftBook> TftBooks { get; set; } = new List<TftBook>();
}
