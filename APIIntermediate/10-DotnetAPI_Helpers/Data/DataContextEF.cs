using Microsoft.EntityFrameworkCore;
using DotnetAPI.Models;
using Microsoft.Extensions.Configuration;

namespace DotnetAPI.Data
{
    public class DataContextEF : DbContext
    {
        private readonly IConfiguration _config;
        public DataContextEF(IConfiguration config)
        {
            _config = config;
        }

        public virtual DbSet<Users>? Users { get; set; }
        public virtual DbSet<UserSalary>? UserSalary { get; set; }
        public virtual DbSet<UserJobInfo>? UserJobInfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                        options => options.EnableRetryOnFailure());
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TutorialAppSchema");

            modelBuilder.Entity<Users>()
                .ToTable("Users", "TutorialAppSchema")
                .HasKey(e => e.UserId);

            modelBuilder.Entity<UserSalary>()
                .ToTable("UserSalary", "TutorialAppSchema")
                .HasKey(e => e.UserId);

            modelBuilder.Entity<UserJobInfo>()
                .ToTable("UserJobInfo", "TutorialAppSchema")
                .HasKey(e => e.UserId);
        }
    }
}