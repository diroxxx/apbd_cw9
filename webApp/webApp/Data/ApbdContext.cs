using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using webApp.Models;

namespace webApp.Data;

public partial class ApbdContext : DbContext
{
    public ApbdContext()
    {
    }

    public ApbdContext(DbContextOptions<ApbdContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Animal> Animals { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientTrip> ClientTrips { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Medicament> Medicaments { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Owner> Owners { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    public virtual DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

    public virtual DbSet<Procedure> Procedures { get; set; }

    public virtual DbSet<ProcedureAnimal> ProcedureAnimals { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductWarehouse> ProductWarehouses { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Default");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Animal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Animal_pk");

            entity.ToTable("Animal");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.OwnerId).HasColumnName("Owner_ID");
            entity.Property(e => e.Type).HasMaxLength(100);

            entity.HasOne(d => d.Owner).WithMany(p => p.Animals)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Animal_Owner");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("Client_pk");

            entity.ToTable("Client");

            entity.Property(e => e.Email).HasMaxLength(120);
            entity.Property(e => e.FirstName).HasMaxLength(120);
            entity.Property(e => e.LastName).HasMaxLength(120);
            entity.Property(e => e.Pesel).HasMaxLength(120);
            entity.Property(e => e.Telephone).HasMaxLength(120);
        });

        modelBuilder.Entity<ClientTrip>(entity =>
        {
            entity.HasKey(e => new { e.IdClient, e.IdTrip }).HasName("Client_Trip_pk");

            entity.ToTable("Client_Trip");

            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.RegisteredAt).HasColumnType("datetime");

            entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.ClientTrips)
                .HasForeignKey(d => d.IdClient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Table_5_Client");

            entity.HasOne(d => d.IdTripNavigation).WithMany(p => p.ClientTrips)
                .HasForeignKey(d => d.IdTrip)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Table_5_Trip");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.IdCountry).HasName("Country_pk");

            entity.ToTable("Country");

            entity.Property(e => e.Name).HasMaxLength(120);

            entity.HasMany(d => d.IdTrips).WithMany(p => p.IdCountries)
                .UsingEntity<Dictionary<string, object>>(
                    "CountryTrip",
                    r => r.HasOne<Trip>().WithMany()
                        .HasForeignKey("IdTrip")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Country_Trip_Trip"),
                    l => l.HasOne<Country>().WithMany()
                        .HasForeignKey("IdCountry")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Country_Trip_Country"),
                    j =>
                    {
                        j.HasKey("IdCountry", "IdTrip").HasName("Country_Trip_pk");
                        j.ToTable("Country_Trip");
                    });
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.IdDoctor).HasName("Doctor_pk");

            entity.ToTable("Doctor");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
        });

        modelBuilder.Entity<Medicament>(entity =>
        {
            entity.HasKey(e => e.IdMedicament).HasName("Medicament_pk");

            entity.ToTable("Medicament");

            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(100);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder).HasName("Order_pk");

            entity.ToTable("Order");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.FulfilledAt).HasColumnType("datetime");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Receipt_Product");
        });

        modelBuilder.Entity<Owner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Owner_pk");

            entity.ToTable("Owner");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.IdPatient).HasName("Patient_pk");

            entity.ToTable("Patient");

            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(e => e.IdPrescription).HasName("Prescription_pk");

            entity.ToTable("Prescription");

            entity.HasOne(d => d.IdDoctorNavigation).WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.IdDoctor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Prescription_Doctor");

            entity.HasOne(d => d.IdPatientNavigation).WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.IdPatient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Prescription_Patient");
        });

        modelBuilder.Entity<PrescriptionMedicament>(entity =>
        {
            entity.HasKey(e => new { e.IdMedicament, e.IdPrescription }).HasName("Prescription_Medicament_pk");

            entity.ToTable("Prescription_Medicament");

            entity.Property(e => e.Details).HasMaxLength(100);

            entity.HasOne(d => d.IdMedicamentNavigation).WithMany(p => p.PrescriptionMedicaments)
                .HasForeignKey(d => d.IdMedicament)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Prescription_Medicament_Medicament");

            entity.HasOne(d => d.IdPrescriptionNavigation).WithMany(p => p.PrescriptionMedicaments)
                .HasForeignKey(d => d.IdPrescription)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Prescription_Medicament_Prescription");
        });

        modelBuilder.Entity<Procedure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Procedure_pk");

            entity.ToTable("Procedure");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<ProcedureAnimal>(entity =>
        {
            entity.HasKey(e => new { e.ProcedureId, e.AnimalId, e.Date }).HasName("Procedure_Animal_pk");

            entity.ToTable("Procedure_Animal");

            entity.Property(e => e.ProcedureId).HasColumnName("Procedure_ID");
            entity.Property(e => e.AnimalId).HasColumnName("Animal_ID");

            entity.HasOne(d => d.Animal).WithMany(p => p.ProcedureAnimals)
                .HasForeignKey(d => d.AnimalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Procedure_Animal_Animal");

            entity.HasOne(d => d.Procedure).WithMany(p => p.ProcedureAnimals)
                .HasForeignKey(d => d.ProcedureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Procedure_Animal_Procedure");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("Product_pk");

            entity.ToTable("Product");

            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("numeric(25, 2)");
        });

        modelBuilder.Entity<ProductWarehouse>(entity =>
        {
            entity.HasKey(e => e.IdProductWarehouse).HasName("Product_Warehouse_pk");

            entity.ToTable("Product_Warehouse");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("numeric(25, 2)");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.ProductWarehouses)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Product_Warehouse_Order");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.ProductWarehouses)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("_Product");

            entity.HasOne(d => d.IdWarehouseNavigation).WithMany(p => p.ProductWarehouses)
                .HasForeignKey(d => d.IdWarehouse)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("_Warehouse");
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.IdTrip).HasName("Trip_pk");

            entity.ToTable("Trip");

            entity.Property(e => e.DateFrom).HasColumnType("datetime");
            entity.Property(e => e.DateTo).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(220);
            entity.Property(e => e.Name).HasMaxLength(120);
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.IdWarehouse).HasName("Warehouse_pk");

            entity.ToTable("Warehouse");

            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
