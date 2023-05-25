using System;
using System.Collections.Generic;
using COMP306.LibraryManagement.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace COMP306.LibraryManagement.DAL.Context;

public partial class ApplicationContext : DbContext
{
    public ApplicationContext()
    {
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppAction> AppActions { get; set; }

    public virtual DbSet<AppPermission> AppPermissions { get; set; }

    public virtual DbSet<AppRole> AppRoles { get; set; }

    public virtual DbSet<AppSection> AppSections { get; set; }

    public virtual DbSet<AppUser> AppUsers { get; set; }

    public virtual DbSet<TftAuthor> TftAuthors { get; set; }

    public virtual DbSet<TftBook> TftBooks { get; set; }

    public virtual DbSet<TftLocation> TftLocations { get; set; }

    public virtual DbSet<TftLocationreservation> TftLocationreservations { get; set; }

    public virtual DbSet<TluContent> TluContents { get; set; }

    public virtual DbSet<TluLanguage> TluLanguages { get; set; }

    public virtual DbSet<TluLocationtype> TluLocationtypes { get; set; }

    public virtual DbSet<TluReservationstatus> TluReservationstatuses { get; set; }

    public virtual DbSet<TluStatetype> TluStatetypes { get; set; }

    public virtual DbSet<TluSubject> TluSubjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySQL("Server=comp-306-library-management.mysql.database.azure.com;Database=preprod;User ID=comp306;Password=test1234!");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppAction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("app_action");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<AppPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("app_permission");

            entity.HasIndex(e => e.ActionId, "app_permission_app_action_Id_fk");

            entity.HasIndex(e => e.SectionId, "app_permission_app_section_Id_fk");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.ActionId).HasColumnType("int(11)");
            entity.Property(e => e.SectionId).HasColumnType("int(11)");

            entity.HasOne(d => d.Action).WithMany(p => p.AppPermissions)
                .HasForeignKey(d => d.ActionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("app_permission_app_action_Id_fk");

            entity.HasOne(d => d.Section).WithMany(p => p.AppPermissions)
                .HasForeignKey(d => d.SectionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("app_permission_app_section_Id_fk");
        });

        modelBuilder.Entity<AppRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("app_role");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasMany(d => d.Permissions).WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "AppRolepermission",
                    r => r.HasOne<AppPermission>().WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("app_rolepermission_app_permission_Id_fk"),
                    l => l.HasOne<AppRole>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("app_rolepermission_app_role_Id_fk"),
                    j =>
                    {
                        j.HasKey("RoleId", "PermissionId").HasName("PRIMARY");
                        j.ToTable("app_rolepermission");
                        j.HasIndex(new[] { "PermissionId" }, "app_rolepermission_app_permission_Id_fk");
                        j.IndexerProperty<int>("RoleId").HasColumnType("int(11)");
                        j.IndexerProperty<int>("PermissionId").HasColumnType("int(11)");
                    });
        });

        modelBuilder.Entity<AppSection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("app_section");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("app_user");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.RegisteredDate).HasColumnType("datetime");
            entity.Property(e => e.Surname).HasMaxLength(100);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AppUserrole",
                    r => r.HasOne<AppRole>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("app_userrole_app_role_Id_fk"),
                    l => l.HasOne<AppUser>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("app_userrole_app_user_Id_fk"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("PRIMARY");
                        j.ToTable("app_userrole");
                        j.HasIndex(new[] { "RoleId" }, "app_userrole_app_role_Id_fk");
                        j.IndexerProperty<int>("UserId").HasColumnType("int(11)");
                        j.IndexerProperty<int>("RoleId").HasColumnType("int(11)");
                    });
        });

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
            entity.Property(e => e.AddedDate).HasColumnType("datetime");
            entity.Property(e => e.CallNumber).HasMaxLength(100);
            entity.Property(e => e.Doi)
                .HasMaxLength(100)
                .HasComment("DOI")
                .HasColumnName("DOI");
            entity.Property(e => e.Edition).HasColumnType("int(11)");
            entity.Property(e => e.Isbn)
                .HasMaxLength(100)
                .HasColumnName("ISBN");
            entity.Property(e => e.Issue).HasColumnType("int(11)");
            entity.Property(e => e.LanguageId).HasColumnType("int(11)");
            entity.Property(e => e.Link).HasMaxLength(2000);
            entity.Property(e => e.PublicationDate)
                .HasColumnType("date")
                .HasColumnName("publicationDate");
            entity.Property(e => e.PublicationTitle)
                .HasMaxLength(500)
                .HasComment("PublicationTitle");
            entity.Property(e => e.Rating).HasColumnType("int(11)");
            entity.Property(e => e.Title).HasMaxLength(500);
            entity.Property(e => e.Volume).HasColumnType("int(11)");
                .HasComment("PublicationTitle");
            entity.Property(e => e.Rating).HasColumnType("int(11)");
            entity.Property(e => e.Title).HasMaxLength(500);
            entity.Property(e => e.Volume).HasColumnType("int(11)");

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

            entity.HasMany(d => d.Contents).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "TftBookcontent",
                    r => r.HasOne<TluContent>().WithMany()
                        .HasForeignKey("ContentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("tft_bookcontent_tlu_content_Id_fk"),
                    l => l.HasOne<TftBook>().WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("tft_bookcontent_tft_book_Id_fk"),
                    j =>
                    {
                        j.HasKey("BookId", "ContentId").HasName("PRIMARY");
                        j.ToTable("tft_bookcontent");
                        j.HasIndex(new[] { "ContentId" }, "tft_bookcontent_tlu_content_Id_fk");
                        j.IndexerProperty<int>("BookId")
                            .HasColumnType("int(11)")
                            .HasColumnName("bookId");
                        j.IndexerProperty<int>("ContentId")
                            .HasColumnType("int(11)")
                            .HasColumnName("contentId");
                    });

            entity.HasMany(d => d.States).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "TftBookstate",
                    r => r.HasOne<TluStatetype>().WithMany()
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("tft_bookstate_tlu_statetype_Id_fk"),
                    l => l.HasOne<TftBook>().WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("tft_bookstate_tft_book_Id_fk"),
                    j =>
                    {
                        j.HasKey("BookId", "StateId").HasName("PRIMARY");
                        j.ToTable("tft_bookstate");
                        j.HasIndex(new[] { "StateId" }, "tft_bookstate_tlu_statetype_Id_fk");
                        j.IndexerProperty<int>("BookId").HasColumnType("int(11)");
                        j.IndexerProperty<int>("StateId").HasColumnType("int(11)");
                    });

            entity.HasMany(d => d.Subjects).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "TftBooksubject",
                    r => r.HasOne<TluSubject>().WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("tft_booksubject_tlu_subject_Id_fk"),
                    l => l.HasOne<TftBook>().WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("tft_booksubject_tft_book_Id_fk"),
                    j =>
                    {
                        j.HasKey("BookId", "SubjectId").HasName("PRIMARY");
                        j.ToTable("tft_booksubject");
                        j.HasIndex(new[] { "SubjectId" }, "tft_booksubject_tlu_subject_Id_fk");
                        j.IndexerProperty<int>("BookId").HasColumnType("int(11)");
                        j.IndexerProperty<int>("SubjectId").HasColumnType("int(11)");
                    });
        });

        modelBuilder.Entity<TftLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tft_location");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Capacity).HasColumnType("int(11)");
            entity.Property(e => e.LocationTypeId).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<TftLocationreservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tft_locationreservations");

            entity.HasIndex(e => e.LocationId, "tft_locationreservations___fk");

            entity.HasIndex(e => e.UserId, "tft_locationreservations_app_user_Id_fk");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.LocationId).HasColumnType("int(11)");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.ReservationEndDate).HasColumnType("datetime");
            entity.Property(e => e.ReservationStartDate).HasColumnType("datetime");
            entity.Property(e => e.ReservationStatus).HasColumnType("int(11)");
            entity.Property(e => e.UserId).HasColumnType("int(11)");

            entity.HasOne(d => d.Location).WithMany(p => p.TftLocationreservations)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("tft_locationreservations___fk");

            entity.HasOne(d => d.User).WithMany(p => p.TftLocationreservations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("tft_locationreservations_app_user_Id_fk");
        });

        modelBuilder.Entity<TluContent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tlu_content");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<TluLanguage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tlu_language");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<TluLocationtype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tlu_locationtype");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.CanBeReserved)
                .HasDefaultValueSql("b'0'")
                .HasColumnType("bit(1)");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<TluReservationstatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tlu_reservationstatus");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<TluStatetype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tlu_statetype");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<TluSubject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tlu_subject");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
