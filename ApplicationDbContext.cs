using Microsoft.EntityFrameworkCore;
using RoofstockFullStackSample.Models;

namespace RoofstockFullStackSample
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {            
        }

        public DbSet<Property> Properties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This is required in order to store correctly the percentaje field (GrossYield)
            modelBuilder.Entity<Property>().Property(x => x.GrossYield).HasPrecision(6, 4);
        }
    }


}
