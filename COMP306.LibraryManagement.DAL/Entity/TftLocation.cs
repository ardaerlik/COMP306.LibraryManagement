using System;
using System.Collections.Generic;

namespace COMP306.LibraryManagement.DAL.Entity;

public partial class TftLocation
{
    public int Id { get; set; }

    public int LocationTypeId { get; set; }

    public string Name { get; set; } = null!;

    public int Capacity { get; set; }

    public virtual ICollection<TftLocationreservation> TftLocationreservations { get; set; } = new List<TftLocationreservation>();
}
