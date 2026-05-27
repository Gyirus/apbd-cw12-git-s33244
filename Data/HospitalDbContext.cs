using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using apbd_cw12.Models;

namespace apbd_cw12.Data;

public partial class HospitalDbContext : DbContext
{
    public HospitalDbContext()
    {
    }

    public HospitalDbContext(DbContextOptions<HospitalDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admission> Admissions { get; set; }

    public virtual DbSet<Bed> Beds { get; set; }

    public virtual DbSet<BedAssignment> BedAssignments { get; set; }

    public virtual DbSet<BedType> BedTypes { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Ward> Wards { get; set; }

//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//         => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=HospitalDb;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Admissions_pk");

            entity.Property(e => e.PatientPesel).IsFixedLength();

            entity.HasOne(d => d.PatientPeselNavigation).WithMany(p => p.Admissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Admissions_Patients");

            entity.HasOne(d => d.Ward).WithMany(p => p.Admissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Admissions_Wards");
        });

        modelBuilder.Entity<Bed>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Beds_pk");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.BedType).WithMany(p => p.Beds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Beds_BedTypes");

            entity.HasOne(d => d.Room).WithMany(p => p.Beds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Beds_Rooms");
        });

        modelBuilder.Entity<BedAssignment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("BedAssignments_pk");

            entity.Property(e => e.PatientPesel).IsFixedLength();

            entity.HasOne(d => d.Bed).WithMany(p => p.BedAssignments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BedAssignments_Beds");

            entity.HasOne(d => d.PatientPeselNavigation).WithMany(p => p.BedAssignments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BedAssignments_Patients");
        });

        modelBuilder.Entity<BedType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("BedTypes_pk");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Pesel).HasName("Patients_pk");

            entity.Property(e => e.Pesel).IsFixedLength();
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Rooms_pk");

            entity.HasOne(d => d.Ward).WithMany(p => p.Rooms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Room_Ward");
        });

        modelBuilder.Entity<Ward>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Wards_pk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
