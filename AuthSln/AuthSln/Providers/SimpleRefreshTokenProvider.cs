using AuthSln.API;
using AuthSln.Models;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AuthSln.Providers
{
    public class SimpleRefreshTokenProvider : IAuthenticationTokenProvider
    {
        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Método encargado de gestionar la información de tokens
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }
            //Se genera un identificador único para el token
            var refreshTokenId = Guid.NewGuid().ToString("n");

            using (AutenticacionRepository _repo = new AutenticacionRepository())
            {
                var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

                //Se asignan las propiedades del token
                var token = new RefreshToken()
                {
                    Id = Helper.GetHash(refreshTokenId),
                    ClientId = clientid,
                    Subject = context.Ticket.Identity.Name,
                    FechaEmision = DateTime.UtcNow,
                    FechaExpiracion = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
                };

                context.Ticket.Properties.IssuedUtc = token.FechaEmision;
                context.Ticket.Properties.ExpiresUtc = token.FechaExpiracion;

                token.TicketProtegido = context.SerializeTicket();

                var result = await _repo.AgregarRefreshToken(token);

                if (result)
                {
                    context.SetToken(refreshTokenId);
                }

            }
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {

            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string hashedTokenId = Helper.GetHash(context.Token);

            using (AutenticacionRepository _repo = new AutenticacionRepository())
            {
                var refreshToken = await _repo.BuscarRefreshToken(hashedTokenId);

                if (refreshToken != null)
                {
                    //Get protectedTicket from refreshToken class
                    context.DeserializeTicket(refreshToken.TicketProtegido);
                    var result = await _repo.RemoverRefreshToken(hashedTokenId);
                }
            }
        }
    }
}