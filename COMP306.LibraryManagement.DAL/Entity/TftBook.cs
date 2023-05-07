using System;
using System.Collections.Generic;

namespace COMP306.LibraryManagement.DAL.Entity;

public partial class TftBook
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    /// <summary>
    /// PublicationTitle
    /// </summary>
    public string? PublicationTitle { get; set; }

    public int CallNumber { get; set; }

    /// <summary>
    /// DOI
    /// </summary>
    public string? Doi { get; set; }

    public string? Isbn { get; set; }

    public int LanguageId { get; set; }

    public DateTime? PublicationDate { get; set; }

    public virtual TluLanguage Language { get; set; } = null!;

    public virtual ICollection<TftBookreservation> TftBookreservations { get; set; } = new List<TftBookreservation>();

    public virtual ICollection<TftAuthor> Authors { get; set; } = new List<TftAuthor>();

    public virtual ICollection<TluContent> Contents { get; set; } = new List<TluContent>();

    public virtual ICollection<TluStatetype> States { get; set; } = new List<TluStatetype>();

    public virtual ICollection<TluSubject> Subjects { get; set; } = new List<TluSubject>();
}
