using System;
using System.Collections.Generic;

namespace COMP306.LibraryManagement.DAL.Entity;

public partial class TluLocationtype
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public ulong CanBeReserved { get; set; }
}
