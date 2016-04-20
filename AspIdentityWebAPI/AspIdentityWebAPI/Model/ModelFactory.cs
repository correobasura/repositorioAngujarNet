using AspIdentityWebAPI.Infraestructura;
using AspIdentityWebAPI.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net.Http;
using System.Web.Http.Routing;

namespace AspIdentityWebAPI.Model
{
    public class ModelFactory
    {
        private UrlHelper _UrlHelper;
        private ManagerUserIdentity _AppUserManager;

        public ModelFactory(HttpRequestMessage request, ManagerUserIdentity appUserManager)
        {
            _UrlHelper = new UrlHelper(request);
            _AppUserManager = appUserManager;
        }

        public UsuarioModel Create(IdentityUser appUser)
        {
            return new UsuarioModel
            {
                Url = _UrlHelper.Link("GetUserById", new { id = appUser.Id }),
                Id = appUser.Id,
                UserName = appUser.UserName,
                Email = appUser.Email,
                EmailConfirmed = appUser.EmailConfirmed,
                Roles = _AppUserManager.GetRolesAsync(appUser.Id).Result,
                Claims = _AppUserManager.GetClaimsAsync(appUser.Id).Result
            };
        }
    }
}