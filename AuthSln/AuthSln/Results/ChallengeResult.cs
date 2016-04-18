using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace AuthSln.Results
{
    /// <summary>
    /// Clase que gestiona las peticiones de conexion a traves de otras cuentas
    /// </summary>
    public class ChallengeResult : IHttpActionResult
    {
        /// <summary>
        /// Atributo que establece el proveedor externo de login
        /// </summary>
        public string LoginProvider { get; set; }

        /// <summary>
        /// Atributo que relacion la petición
        /// </summary>
        public HttpRequestMessage Request { get; set; }

        /// <summary>
        /// Constructor de la clase que inicializa los datos necesarios de la clase
        /// </summary>
        /// <param name="loginProvider">Contiene el nombre del proveedor de login</param>
        /// <param name="controller">Objeto que relaciona el controlador</param>
        public ChallengeResult(string loginProvider, ApiController controller)
        {
            LoginProvider = loginProvider;
            Request = controller.Request;
        }

        /// <summary>
        /// Método que realiza el llamado a la ejecución de login
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            Request.GetOwinContext().Authentication.Challenge(LoginProvider);
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.RequestMessage = Request;
            return Task.FromResult(response);
        }
    }
}