using CafeBooking.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace CafeBooking.DataAccess.Context
{
    public class CafeDbContext : DbContext
    {
        public CafeDbContext(DbContextOptions<CafeDbContext> options)
            : base(options) { }

        public DbSet<Table> Tables { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Table>()
                .HasMany(t => t.Reservations)
                .WithOne(r => r.Table)
                .HasForeignKey(r => r.TableId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasIndex(r => r.PhoneNumber);

            modelBuilder.Entity<Reservation>()
                .HasIndex(r => r.StartDateTime);
        }
    }
}
