using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AspIdentityWebAPI.Controllers
{
    /// <summary>
    /// Controlador de usuarios
    /// </summary>
    [RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {

        [Route("usuarios")]
        public IHttpActionResult GetUsers()
        {
            return Ok(this.AppUserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));
        }

        [Route("usuario/{id:guid}", Name = "ObtenerUsuarioById")]
        public async Task<IHttpActionResult> ObtenerUsuario(string Id)
        {
            var user = await this.AppUserManager.FindByIdAsync(Id);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        [Route("usuario/{username}")]
        public async Task<IHttpActionResult> ObtenerUsuarioPorNombre(string username)
        {
            var user = await this.AppUserManager.FindByNameAsync(username);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }
    }
}