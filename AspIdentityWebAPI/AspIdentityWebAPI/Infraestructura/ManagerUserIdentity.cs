using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspIdentityWebAPI.Infraestructura
{
    /// <summary>
    /// Clase encargada de manejar las peticiones de consulta sobre la clase IdentityUser
    /// </summary>
    public class ManagerUserIdentity : UserManager<IdentityUser>
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="store">Interface que provee las apis para el objeto <IdentityUSer></param>
        public ManagerUserIdentity(IUserStore<IdentityUser> store)
            : base(store)
        {
        }

        /// <summary>
        /// Método encargado de retornar una instancia de Manager de Identity
        /// </summary>
        /// <param name="opciones"></param>
        /// <param name="contexto"></param>
        /// <returns>instancia del manager ManagerUserIdentity</returns>
        public static ManagerUserIdentity Create(IdentityFactoryOptions<ManagerUserIdentity> opciones, IOwinContext contexto)
        {
            var appDbContext = contexto.Get<AppDBContext>();
            var appUserManager = new ManagerUserIdentity(new UserStore<IdentityUser>(appDbContext));
            return appUserManager;
        }
    }
}