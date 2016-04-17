using AuthSln.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AuthSln.Controllers
{
    /// <summary>
    /// Controlador de RefreshToken
    /// </summary>
    [RoutePrefix("api/RefreshTokens")]
    public class RefreshTokensController : ApiController
    {
        /// <summary>
        /// Atributo que referencia al repositorio
        /// </summary>
        private AutenticacionRepository _repositorio = null;

        /// <summary>
        /// Constructor de la clase, inicializa los objetos requeridos
        /// </summary>
        public RefreshTokensController()
        {
            _repositorio = new AutenticacionRepository();
        }

        [Authorize(Users = "Admin")]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(_repositorio.ObtenerTodosRefreshTokens());
        }

        [Authorize(Users = "Admin")]
        [AllowAnonymous]
        [Route("")]
        public async Task<IHttpActionResult> BorrarToken(string tokenId)
        {
            var result = await _repositorio.RemoverRefreshToken(tokenId);
            if (result)
            {
                return Ok();
            }
            return BadRequest("El token no existe");

        }

        /// <summary>
        /// Método encargado de cerrar lo conexión del repositorio
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repositorio.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}