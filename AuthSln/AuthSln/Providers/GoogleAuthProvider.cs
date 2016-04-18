using Microsoft.Owin.Security.Google;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthSln.Providers
{
    /// <summary>
    /// Clase que gestiona los llamados a logueo del proveedor de login de Google
    /// </summary>
    public class GoogleAuthProvider : IGoogleOAuth2AuthenticationProvider
    {
        /// <summary>
        /// Método que gestiona la redirección a a una Uri
        /// </summary>
        /// <param name="context">Objeto obtenido del proveedor</param>
        public void ApplyRedirect(GoogleOAuth2ApplyRedirectContext context)
        {
            context.Response.Redirect(context.RedirectUri);
        }

        /// <summary>
        /// Método que valida la información de autenticación
        /// </summary>
        /// <param name="context">Objeto obtenido del proveedor</param>
        /// <returns></returns>
        public Task Authenticated(GoogleOAuth2AuthenticatedContext context)
        {
            context.Identity.AddClaim(new Claim("ExternalAccessToken", context.AccessToken));
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Método que gestiona la redirección 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task ReturnEndpoint(GoogleOAuth2ReturnEndpointContext context)
        {
            return Task.FromResult<object>(null);
        }
    }
}