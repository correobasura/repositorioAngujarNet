using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace AspIdentityWebAPI.Infrastructure
{
    /// <summary>
    /// Clase encargada de manejar las peticiones de consulta sobre la clase IdentityUser
    /// </summary>
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="store">Interface que provee las apis para el objeto <ApplicationUser></param>
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        /// <summary>
        /// Método encargado de retornar una instancia de Manager de Identity
        /// </summary>
        /// <param name="options"></param>
        /// <param name="context"></param>
        /// <returns>instancia del manager ManagerUserIdentity</returns>
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var appDbContext = context.Get<ApplicationDbContext>();
            var appUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(appDbContext));

            return appUserManager;
        }
    }
}