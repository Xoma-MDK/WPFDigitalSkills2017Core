using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WPFDigitalSkills2017Core.Models;

public partial class Session106Context : DbContext
{
    public Session106Context()
    {
    }

    public Session106Context(DbContextOptions<Session106Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Activityofuse> Activityofuses { get; set; } = null!;
    public virtual DbSet<Country> Countries { get; set; } = null!;
    public virtual DbSet<Office> Offices { get; set; } = null!;
    public virtual DbSet<Role> Roles { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql("server=176.126.103.27;database=session1_06;uid=xomic;pwd=06Qrb05MDK", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.30-mysql"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Activityofuse>(entity =>
        {
            entity.ToTable("activityofuses");

            entity.HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_bin");

            entity.HasIndex(e => e.UserId, "FK_ActivityOfUses_Users");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.Property(e => e.LoginTime).HasColumnType("datetime");

            entity.Property(e => e.LogoutTime).HasColumnType("datetime");

            entity.Property(e => e.Reason).HasColumnType("text");

            entity.Property(e => e.TimeSpentOnSystem).HasColumnType("time");

            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Activityofuses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActivityOfUses_Users");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("countries");

            entity.HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_bin");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Office>(entity =>
        {
            entity.ToTable("offices");

            entity.HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_bin");

            entity.HasIndex(e => e.CountryId, "FK_Office_Country");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.Property(e => e.Contact).HasMaxLength(250);

            entity.Property(e => e.CountryId).HasColumnName("CountryID");

            entity.Property(e => e.Phone).HasMaxLength(50);

            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Country)
                .WithMany(p => p.Offices)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Office_Country");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("roles");

            entity.HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_bin");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");

            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_bin");

            entity.HasIndex(e => e.OfficeId, "FK_Users_Offices");

            entity.HasIndex(e => e.RoleId, "FK_Users_Roles");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.Property(e => e.Email).HasMaxLength(150);

            entity.Property(e => e.FirstName).HasMaxLength(50);

            entity.Property(e => e.LastName).HasMaxLength(50);

            entity.Property(e => e.OfficeId).HasColumnName("OfficeID");

            entity.Property(e => e.Password).HasMaxLength(50);

            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Office)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.OfficeId)
                .HasConstraintName("FK_Users_Offices");

            entity.HasOne(d => d.Role)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}