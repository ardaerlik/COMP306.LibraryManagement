using System;
using System.Collections.Generic;
using COMP306.LİbraryManagement.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace COMP306.LİbraryManagement.DAL.Context;

public partial class ApplicationContext : DbContext
{
    public ApplicationContext()
    {
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TftAuthor> TftAuthors { get; set; }

    public virtual DbSet<TftBook> TftBooks { get; set; }

    public virtual DbSet<TluLanguage> TluLanguages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySQL("Server=comp-306-library-management.mysql.database.azure.com;Database=preprod;User ID=comp306;Password=test1234!");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TftAuthor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tft_author");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Surname).HasMaxLength(200);
        });

        modelBuilder.Entity<TftBook>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tft_book");

            entity.HasIndex(e => e.LanguageId, "tft_book_tlu_language_Id_fk");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.CallNumber).HasColumnType("int(11)");
            entity.Property(e => e.Doi)
                .HasMaxLength(100)
                .HasComment("DOI")
                .HasColumnName("DOI");
            entity.Property(e => e.Isbn)
                .HasMaxLength(100)
                .HasColumnName("ISBN");
            entity.Property(e => e.LanguageId).HasColumnType("int(11)");
            entity.Property(e => e.PublicationTitle)
                .HasMaxLength(500)
                .HasComment("PublicationTitle");
            entity.Property(e => e.Title).HasMaxLength(500);

            entity.HasOne(d => d.Language).WithMany(p => p.TftBooks)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("tft_book_tlu_language_Id_fk");

            entity.HasMany(d => d.Authors).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "TftBookauthor",
                    r => r.HasOne<TftAuthor>().WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("tft_bookauthor_tft_author_Id_fk"),
                    l => l.HasOne<TftBook>().WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("tft_bookauthor_tft_book_Id_fk"),
                    j =>
                    {
                        j.HasKey("BookId", "AuthorId").HasName("PRIMARY");
                        j.ToTable("tft_bookauthor");
                        j.HasIndex(new[] { "AuthorId" }, "tft_bookauthor_tft_author_Id_fk");
                        j.IndexerProperty<int>("BookId").HasColumnType("int(11)");
                        j.IndexerProperty<int>("AuthorId").HasColumnType("int(11)");
                    });
        });

        modelBuilder.Entity<TluLanguage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tlu_language");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
