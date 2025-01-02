using HotelBookingApp.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelAppDb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Room> Room { get; set; }
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.;Database=HotelAppDb;Trusted_Connection=True;TrustServerCertificate=true;");
            }
        }
       
    }
}