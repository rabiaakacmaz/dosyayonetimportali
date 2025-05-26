using System.Security.Cryptography;
using DosyaYonetim.API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DosyaYonetim.API.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }


        public DbSet<UserFile> UserFiles { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<UserFile>()
                .HasOne(f => f.AppUser)
                .WithMany() 
                .HasForeignKey(f => f.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}