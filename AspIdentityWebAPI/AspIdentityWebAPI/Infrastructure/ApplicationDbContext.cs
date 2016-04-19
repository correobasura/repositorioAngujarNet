using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace AspIdentityWebAPI.Infrastructure
{
    /// <summary>
    /// Clase que establece la conexión con el contexto de base de datos y el mapeo de las tablas
    /// dentro de la misma
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ApplicationDbContext()
            : base("AppDBContextConn", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// Método que llama a la creación de un nuevo contexto
        /// </summary>
        /// <returns></returns>
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        /// <summary>
        /// Método que realiza la verificación de la creación del modelo de datos
        /// </summary>
        /// <param name="modelBuilder">constructor del contexto</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers", "USERIDENTITY");
            modelBuilder.Entity<IdentityUserRole>().ToTable("AspNetUserRoles", "USERIDENTITY");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("AspNetUserLogins", "USERIDENTITY");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("AspNetUserClaims", "USERIDENTITY");
            modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles", "USERIDENTITY");
        }
    }
}