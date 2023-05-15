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

    public string CallNumber { get; set; } = null!;

    /// <summary>
    /// DOI
    /// </summary>
    public string? Doi { get; set; }

    public string? Isbn { get; set; }

    public int LanguageId { get; set; }

    public DateTime? PublicationDate { get; set; }

    public int? Edition { get; set; }

    public int? Volume { get; set; }

    public int? Issue { get; set; }

    public string Link { get; set; } = null!;

    public DateTime AddedDate { get; set; }

    public int? Rating { get; set; }

    public virtual TluLanguage Language { get; set; } = null!;

    public virtual ICollection<TftAuthor> Authors { get; set; } = new List<TftAuthor>();

    public virtual ICollection<TluContent> Contents { get; set; } = new List<TluContent>();

    public virtual ICollection<TluStatetype> States { get; set; } = new List<TluStatetype>();

    public virtual ICollection<TluSubject> Subjects { get; set; } = new List<TluSubject>();
}
