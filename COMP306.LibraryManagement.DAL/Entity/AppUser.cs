using System;
using System.Collections.Generic;

namespace COMP306.LibraryManagement.DAL.Entity;

public partial class AppUser
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime RegisteredDate { get; set; }

    public virtual ICollection<TftLocationreservation> TftLocationreservations { get; set; } = new List<TftLocationreservation>();

    public virtual ICollection<AppRole> Roles { get; set; } = new List<AppRole>();
}
