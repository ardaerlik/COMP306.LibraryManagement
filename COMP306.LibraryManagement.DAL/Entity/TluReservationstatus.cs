using System;
using System.Collections.Generic;

namespace COMP306.LibraryManagement.DAL.Entity;

public partial class TluReservationstatus
{
    public int Id { get; set; }

    public string? StatusName { get; set; }

    public int? NextStatusId { get; set; }

    public int? PreviousStatusId { get; set; }
}
