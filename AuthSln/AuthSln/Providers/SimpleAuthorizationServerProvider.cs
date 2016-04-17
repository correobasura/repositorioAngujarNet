using AuthSln.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthSln.API
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        /// <summary>
        /// Método que valida el cliente
        /// </summary>
        /// <param name="oauthContext"></param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext oauthContext)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            User client = null;

            if (!oauthContext.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                oauthContext.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (oauthContext.ClientId == null)
            {
                //Remove the comments from the below line oauthContext.SetError, and invalidate oauthContext 
                //if you want to force sending clientId/secrects once obtain access tokens. 
                oauthContext.Validated();
                //oauthContext.SetError("invalid_clientId", "ClientId should be sent.");
                return Task.FromResult<object>(null);
            }

            using (AutenticacionRepository _repo = new AutenticacionRepository())
            {
                client = _repo.BuscarCliente(oauthContext.ClientId);
            }

            //Se valida la existencia del cliente
            if (client == null)
            {
                //Si el cliente no existe, se envía la notificación invalidando la petición
                oauthContext.SetError("invalid_clientId", string.Format("El usuario '{0}' no está refistrado en el sistema", oauthContext.ClientId));
                return Task.FromResult<object>(null);
            }

            //Se valida la aplicación del cliente, si es nativa, se valida la informacióin secreta
            if (client.TipoAplicacion == TiposAplicacion.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    oauthContext.SetError("clientId_requerido", "El usuario debe ser enviado");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    //Se valida que el cliente enviado coincida con el almacenado
                    if (client.Secret != Helper.GetHash(clientSecret))
                    {
                        oauthContext.SetError("clientId_Invalido", "El usuario no es válido");
                        return Task.FromResult<object>(null);
                    }
                }
            }

            if (!client.Activo)
            {
                //Se verifica que el cliente se encuentre activo
                oauthContext.SetError("cliente_inactivo", "El usuario no se encuentra activo");
                return Task.FromResult<object>(null);
            }

            oauthContext.OwinContext.Set<string>("as:clientAllowedOrigin", client.OrigenAdmitido);
            oauthContext.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.TiempoActualizacionToken.ToString());
            //Se valida el contexto si todo se encuentra bien
            oauthContext.Validated();
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Método que valida las credenciales recibidas
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            if (allowedOrigin == null) allowedOrigin = "*";
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
            using (AutenticacionRepository _repo = new AutenticacionRepository())
            {
                IdentityUser user = await _repo.BuscarUsuario(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));
            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "as:client_id", (context.ClientId == null) ? string.Empty : context.ClientId
                    },
                    {
                        "userName", context.UserName
                    }
                });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            //Se lee la información original del cliente
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            //Se compara el cliente firmado con el cliente recibido como parámetro
            if (originalClient != currentClient)
            {
                context.SetError("clientId_invalido", "El token emitido no concide con el cliente recibido");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }
    }
}