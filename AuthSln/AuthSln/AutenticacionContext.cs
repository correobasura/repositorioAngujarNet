using AuthSln.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AuthSln.API
{
    /// <summary>
    /// IdentityDbContext<IdentityUser>
    /// Provee todas las clases necesarias para manejar la autenticación
    /// </summary>
    public class AutenticacionContext : IdentityDbContext<IdentityUser>
    {
        /// <summary>
        /// Método que inicializa el contexto
        /// </summary>
        public AutenticacionContext()
            : base("AutenticacionContext")
        {

        }

        /// <summary>
        /// Método que realiza la verificación de la creación del modelo de datos
        /// </summary>
        /// <param name="modelBuilder">constructor del contexto</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>().ToTable("AspNetUsers", "USERPRUEBA");
            modelBuilder.Entity<IdentityUserRole>().ToTable("AspNetUserRoles", "USERPRUEBA");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("AspNetUserLogins", "USERPRUEBA");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("AspNetUserClaims", "USERPRUEBA");
            modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles", "USERPRUEBA");
            modelBuilder.Entity<User>().ToTable("User", "USERPRUEBA");
            modelBuilder.Entity<RefreshToken>().ToTable("RefreshToken", "USERPRUEBA");
        }

        /// <summary>
        /// Colección de clientes
        /// </summary>
        public DbSet<User> Usuarios { get; set; }
        /// <summary>
        /// Colección de RefreshToken
        /// </summary>
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}