using AuthSln.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace AuthSln
{
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext()
            : base("AuthContext")
        {            

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>().ToTable("AspNetUsers", "USERPRUEBA");
            modelBuilder.Entity<IdentityUserRole>().ToTable("AspNetUserRoles", "USERPRUEBA");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("AspNetUserLogins", "USERPRUEBA");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("AspNetUserClaims", "USERPRUEBA");
            modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles", "USERPRUEBA");
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}