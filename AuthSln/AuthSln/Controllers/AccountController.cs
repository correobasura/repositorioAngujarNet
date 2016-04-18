using AuthSln.API;
using AuthSln.Models;
using AuthSln.Results;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace AuthSln.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

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

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            string redirectUri = string.Empty;

            if (error != null)
            {
                return BadRequest(Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            var redirectUriValidationResult = ValidateClientAndRedirectUri(this.Request, ref redirectUri);

            if (!string.IsNullOrWhiteSpace(redirectUriValidationResult))
            {
                return BadRequest(redirectUriValidationResult);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            IdentityUser user = await _repositorio.BuscarAsync(new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            redirectUri = string.Format("{0}#external_access_token={1}&provider={2}&haslocalaccount={3}&external_user_name={4}",
                                            redirectUri,
                                            externalLogin.ExternalAccessToken,
                                            externalLogin.LoginProvider,
                                            hasRegistered.ToString(),
                                            externalLogin.UserName);

            return Redirect(redirectUri);

        }

        /// <summary>
        /// Método encargado de validar la información del cliente y realizar la redireccion
        /// </summary>
        /// <param name="request"></param>
        /// <param name="redirectUriOutput"></param>
        /// <returns></returns>
        private string ValidateClientAndRedirectUri(HttpRequestMessage request, ref string redirectUriOutput)
        {
            Uri redirectUri;
            var redirectUriString = GetQueryString(Request, "redirect_uri");
            //Se valida que se tenga un URI a direccionar
            if (string.IsNullOrWhiteSpace(redirectUriString))
            {
                return "redirect_uri es requerido";
            }

            bool validUri = Uri.TryCreate(redirectUriString, UriKind.Absolute, out redirectUri);
            //Se valida que el URI creado sea válido
            if (!validUri)
            {
                return "redirect_uri es inválido";
            }

            var clientId = GetQueryString(Request, "client_id");
            //Se valida que se tenga la información del cliente
            if (string.IsNullOrWhiteSpace(clientId))
            {
                return "client_Id es requerido";
            }

            var client = _repositorio.BuscarCliente(clientId);
            //Se valida que el cliente ingresado, esté registrado dentro del sistema
            if (client == null)
            {
                return string.Format("El cliente '{0}' no está registrado en el sistema", clientId);
            }
            //Se valida que el usuario tenga acceso a el URI que se especifica
            if (!string.Equals(client.OrigenAdmitido, redirectUri.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase))
            {
                return string.Format("La URL dada no está permitida para el usuario '{0}'", clientId);
            }

            redirectUriOutput = redirectUri.AbsoluteUri;
            return string.Empty;
        }

        /// <summary>
        /// Método encargado de validar la información obtenida en el queryString
        /// </summary>
        /// <param name="request">Objeto con información de la petición</param>
        /// <param name="key">clave a buscarse dentro del queryString</param>
        /// <returns></returns>
        private string GetQueryString(HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();

            if (queryStrings == null) return null;

            var match = queryStrings.FirstOrDefault(keyValue => string.Compare(keyValue.Key, key, true) == 0);

            if (string.IsNullOrEmpty(match.Value)) return null;

            return match.Value;
        }

        /// <summary>
        /// Método encargado de validar el acceso de un token obtenido por un proveedor externo
        /// </summary>
        /// <param name="provider">Proveedor del servicio de acceso</param>
        /// <param name="accessToken">Token con información de acceso</param>
        /// <returns></returns>
        private async Task<ParsedExternalAccessToken> VerificarAccesoTokenExterno(string provider, string accessToken)
        {
            ParsedExternalAccessToken parsedToken = null;
            var verifyTokenEndPoint = "";
            if (provider == "Google")
            {
                verifyTokenEndPoint = string.Format("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}", accessToken);
            }
            else
            {
                return null;
            }

            var client = new HttpClient();
            var uri = new Uri(verifyTokenEndPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                dynamic jObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content);
                parsedToken = new ParsedExternalAccessToken();
                if (provider == "Google")
                {
                    parsedToken.user_id = jObj["user_id"];
                    parsedToken.app_id = jObj["audience"];

                    if (!string.Equals(Startup.googleAuthOptions.ClientId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                }
            }
            return parsedToken;
        }

        /// <summary>
        /// Método encargado de generar el token local de acceso
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private JObject GenerarTokenAccesoLocalResponse(string userName)
        {

            var tokenExpiration = TimeSpan.FromDays(1);

            ClaimsIdentity identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Name, userName));
            identity.AddClaim(new Claim("role", "user"));

            var props = new AuthenticationProperties()
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration),
            };

            var ticket = new AuthenticationTicket(identity, props);

            var accessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

            JObject tokenResponse = new JObject(
                                        new JProperty("userName", userName),
                                        new JProperty("access_token", accessToken),
                                        new JProperty("token_type", "bearer"),
                                        new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
                                        new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
                                        new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
                );

            return tokenResponse;
        }

        // POST api/Account/RegisterExternal
        [AllowAnonymous]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Se verifica la validez del token externo recibido
            var verifiedAccessToken = await VerificarAccesoTokenExterno(model.Provider, model.ExternalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("El proveedor de inicio de sesión ó Token de Acceso no válido");
            }

            IdentityUser user = await _repositorio.BuscarAsync(new UserLoginInfo(model.Provider, verifiedAccessToken.user_id));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                return BadRequest("El usuario ya se encuentra registrado");
            }

            user = new IdentityUser() { UserName = model.UserName };
            //Se realiza el registro del usuario sin contraseña
            IdentityResult result = await _repositorio.CrearAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            var info = new ExternalLoginInfo()
            {
                DefaultUserName = model.UserName,
                Login = new UserLoginInfo(model.Provider, verifiedAccessToken.user_id)
            };

            result = await _repositorio.AgregarLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            //Se realiza el llamado al método que realiza la cración del token de acceso
            var accessTokenResponse = GenerarTokenAccesoLocalResponse(model.UserName);

            return Ok(accessTokenResponse);
        }

        /// <summary>
        /// Método que retornar un error en la información obtenida
        /// </summary>
        /// <param name="result">Objeto con información de error</param>
        /// <returns>Retorna acción con error</returns>
        private IHttpActionResult GetErrorResult(IdentityResult result)
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
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ObtenerTokenAccesoLocal")]
        public async Task<IHttpActionResult> ObtenerTokenAccesoLocal(string provider, string externalAccessToken)
        {

            if (string.IsNullOrWhiteSpace(provider) || string.IsNullOrWhiteSpace(externalAccessToken))
            {
                return BadRequest("Proveedor o token de acceso no fueron enviados");
            }

            var verifiedAccessToken = await VerificarAccesoTokenExterno(provider, externalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Proveedor o token externo de acceso no válidos");
            }

            IdentityUser user = await _repositorio.BuscarAsync(new UserLoginInfo(provider, verifiedAccessToken.user_id));

            bool hasRegistered = user != null;

            if (!hasRegistered)
            {
                return BadRequest("Usuario externo no registrado");
            }

            //generate access token response
            var accessTokenResponse = GenerarTokenAccesoLocalResponse(user.UserName);
            return Ok(accessTokenResponse);
        }

        /// <summary>
        /// Clase que establece la información del login externa
        /// </summary>
        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }
            public string ExternalAccessToken { get; set; }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer) || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name),
                    ExternalAccessToken = identity.FindFirstValue("ExternalAccessToken"),
                };
            }
        }
    }
}