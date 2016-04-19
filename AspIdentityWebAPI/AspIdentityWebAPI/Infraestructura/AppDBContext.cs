using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AspIdentityWebAPI.Infraestructura
{
    public class AppDBContext : IdentityDbContext<IdentityUser>
    {
        public AppDBContext()
            : base("AppDBContextConn", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static AppDBContext Create()
        {
            return new AppDBContext();
        }

        /// <summary>
        /// Método que realiza la verificación de la creación del modelo de datos
        /// </summary>
        /// <param name="modelBuilder">constructor del contexto</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>().ToTable("AspNetUsers", "USERIDENTITY");
            modelBuilder.Entity<IdentityUserRole>().ToTable("AspNetUserRoles", "USERIDENTITY");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("AspNetUserLogins", "USERIDENTITY");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("AspNetUserClaims", "USERIDENTITY");
            modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles", "USERIDENTITY");
        }
    }
}