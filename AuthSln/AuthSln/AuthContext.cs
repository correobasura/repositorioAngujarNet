using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}