using System;
using System.Collections.Generic;

namespace COMP306.LİbraryManagement.DAL.Entity;

public partial class TftAuthor
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<TftBook> Books { get; set; } = new List<TftBook>();
}
