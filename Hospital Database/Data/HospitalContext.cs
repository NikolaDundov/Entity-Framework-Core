using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext(DbContextOptions options)
            : base(options)
        {

        }

        public HospitalContext()
        {

        }

        public DbSet<Diagnose> Diagnoses { get; set; }

        public DbSet<Medicament> Medicaments { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Visitation> Visitations { get; set; }

        public DbSet<PatientMedicament> PatientMedicaments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.connectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Diagnose>(entity =>
            {
                entity.HasKey(d => d.DiagnoseId);

                entity.Property(d => d.Name)
                .HasMaxLength(50)
                .IsRequired(true)
                .IsUnicode(true);

                entity.Property(d => d.Comments)
                .HasMaxLength(250)
                .IsUnicode(true)
                .IsRequired(false);

                entity
                .HasOne(p => p.Patient)
                .WithMany(d => d.Diagnoses)
                .HasForeignKey(p => p.PatientId);
            });

            modelBuilder.Entity<Medicament>(entity =>
            {
                entity.HasKey(m => m.MedicamentId);

                entity.Property(m => m.Name)
                .HasMaxLength(50)
                .IsUnicode(true)
                .IsRequired(true);

                entity
                .HasMany(p => p.Prescriptions)
                .WithOne(m => m.Medicament)
                .HasForeignKey(p => p.MedicmentId);

            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(p => p.PatientId);

                entity.Property(p => p.FirstName)
                .HasMaxLength(50)
                .IsUnicode(true)
                .IsRequired(true);

                entity.Property(p => p.LastName)
                .HasMaxLength(50)
                .IsUnicode(true)
                .IsRequired(true);

                entity.Property(p => p.Address)
                .HasMaxLength(250)
                .IsUnicode(true)
                .IsRequired(false);

                entity.Property(p => p.HasInsurance)
                .IsRequired(true);

                entity.HasMany(m => m.Prescriptions)
                .WithOne(p => p.Patient)
                .HasForeignKey(p => p.PatientId);

                entity.HasMany(d => d.Diagnoses)
                .WithOne(p => p.Patient)
                .HasForeignKey(p => p.DiagnoseId);

                entity.HasMany(v => v.Visitations)
                .WithOne(p => p.Patient)
                .HasForeignKey(p => p.VisitationId);

            });

            modelBuilder.Entity<Visitation>(entity =>
            {
                entity.HasKey(v => v.VisitationId);

                entity.Property(v => v.Comments)
                .HasMaxLength(250)
                .IsUnicode(true);

                entity.Property(v => v.Date)
                .IsRequired(true);

                entity.HasOne(p => p.Patient)
                .WithMany(v => v.Visitations)
                .HasForeignKey(p => p.PatientId);

            });

            modelBuilder.Entity<PatientMedicament>(entity =>
            {
                entity.HasKey(pm => new { pm.PatientId, pm.MedicmentId });

                entity.HasOne(p => p.Patient)
                .WithMany(pm => pm.Prescriptions)
                .HasForeignKey(p => p.PatientId);

                entity.HasOne(m => m.Medicament)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(m => m.MedicmentId);

            });

        }
    }
}
