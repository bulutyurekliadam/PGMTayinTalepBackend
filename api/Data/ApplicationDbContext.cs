using Microsoft.EntityFrameworkCore;
using TayinTalepAPI.Models;

namespace TayinTalepAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TayinTalebi> TayinTalepleri { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.SicilNo)
                .IsUnique();

            modelBuilder.Entity<TayinTalebi>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.SicilNo)
                .HasPrincipalKey(u => u.SicilNo);
        }
    }
} 