using Microsoft.EntityFrameworkCore;
using HouseHub.Shared.Models; 

namespace HouseHub.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { }

        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserProfile>().HasKey(u => u.Id);

            modelBuilder.Entity<UserProfile>().Property(u => u.FullName).IsRequired().HasMaxLength(100);
        }
    }
}


