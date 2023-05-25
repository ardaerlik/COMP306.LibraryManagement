using System;
using System.Collections.Generic;

namespace COMP306.LibraryManagement.DAL.Entity;

public partial class TftLocationreservation
{
    public int Id { get; set; }

    public int LocationId { get; set; }

    public int UserId { get; set; }

    public DateTime ReservationStartDate { get; set; }

    public DateTime ReservationEndDate { get; set; }

    public int ReservationStatus { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual TftLocation Location { get; set; } = null!;

    public virtual TluReservationstatus ReservationStatusNavigation { get; set; } = null!;

    public virtual AppUser User { get; set; } = null!;
}
