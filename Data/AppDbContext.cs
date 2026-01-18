using Microsoft.EntityFrameworkCore;
using ProjekTrashVision.Models;

namespace ProjekTrashVision.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<RiwayatDeteksi> RiwayatDeteksis { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Kita biarkan simpel seperti ini agar tidak bentrok dengan versi MySQL Laragon
            modelBuilder.Entity<RiwayatDeteksi>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.LabelHasil).IsRequired().HasMaxLength(50);
            });
        }
    }
}