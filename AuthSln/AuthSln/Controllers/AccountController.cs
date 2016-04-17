using AuthSln.API;
using AuthSln.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Http;

namespace AuthSln.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        /// <summary>
        /// Objeto que referencia al repositorio de autenticación
        /// </summary>
        private AutenticacionRepository _repositorio = null;

        /// <summary>
        /// Se inicializa el controlador y los objetos requeridos por el mismo
        /// </summary>
        public AccountController()
        {
            _repositorio = new AutenticacionRepository();
        }

        // POST api/Account/Registrar
        [AllowAnonymous]
        [Route("Registrar")]
        public async Task<IHttpActionResult> Registrar(UsuarioModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Se hace el llamado al método que realiza el registro de usuarios y se obtiene su resultado
            IdentityResult result = await _repositorio.RegistrarUsuario(userModel);
            //Se verifica si existe un error en el objeto guardado obtenido
            IHttpActionResult errorResult = ValidarErroresResultado(result);
            if (errorResult != null)
            {
                return errorResult;
            }
            //Si el proceso se lleva a cabo, se devuelve un estatus de éxito
            return Ok();
        }

        /// <summary>
        /// Método que realiza el llamado a cerrar el repositorio
        /// </summary>
        /// <param name="disposing">bandera que indica que debe ser cerrado</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repositorio.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Método que realiza la validación de errores que se puedan presentar durante el proceso
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private IHttpActionResult ValidarErroresResultado(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }
            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
                if (ModelState.IsValid)
                {
                    // Se retorna simplemente un bad request en caso de que no se encuentre un state valido
                    return BadRequest();
                }
                //Se retorna la información con el error
                return BadRequest(ModelState);
            }
            return null;
        }
    }
}