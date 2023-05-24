using System;
using System.Collections.Generic;

namespace COMP306.LibraryManagement.DAL.Entity;

public partial class TluReservationstatus
{
    public int Id { get; set; }

    public string? StatusName { get; set; }

    public virtual ICollection<TftLocationreservation> TftLocationreservations { get; set; } = new List<TftLocationreservation>();
}
