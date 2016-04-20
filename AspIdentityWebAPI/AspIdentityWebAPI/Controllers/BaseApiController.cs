using AspIdentityWebAPI.Infraestructura;
using AspIdentityWebAPI.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace AspIdentityWebAPI.Controllers
{
    /// <summary>
    /// Clase que sirve como plantilla para heredar los objetos que serán usados en las otras clases que
    /// hereden de la misma
    /// </summary>
    public class BaseApiController : ApiController
    {
        /// <summary>
        /// Instancia de ModelFactory, objeto que da forma y controla los datos retornados
        /// </summary>
        private ModelFactory _modelFactory;
        /// <summary>
        /// Instancia de ManagerUserIdentity
        /// </summary>
        private ManagerUserIdentity _AppUserManager = null;

        protected ManagerUserIdentity AppUserManager
        {
            get
            {
                return _AppUserManager ?? Request.GetOwinContext().GetUserManager<ManagerUserIdentity>();
            }
        }

        public BaseApiController()
        {
        }

        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(this.Request, this.AppUserManager);
                }
                return _modelFactory;
            }
        }

        /// <summary>
        /// Objeto que verifica los errores que se presentan en una petición
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected IHttpActionResult ObtenerErrorResult(IdentityResult result)
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
    }
}