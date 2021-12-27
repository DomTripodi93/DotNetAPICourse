using Microsoft.EntityFrameworkCore;
using HelloWorld.Models;

namespace HelloWorld.Data
{
    public class DataContextEF : DbContext
    {
        public virtual DbSet<Computer>? Computer { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer("Server=localhost;Database=TestDatabase;Trusted_Connection=true;TrustServerCertificate=true;",
                        options => options.EnableRetryOnFailure());
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TestAppSchema");

            modelBuilder.Entity<Computer>()
                .ToTable("ComputerForTestApp", "TestAppSchema")
                .HasKey(e => e.ComputerId);
        }
    }
}