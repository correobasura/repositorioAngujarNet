using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspIdentityWebAPI.Model
{
    /// <summary>
    /// Clase que estructura los datos del usuario que van a ser mostrados
    /// </summary>
    public class UsuarioModel
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public int Level { get; set; }
        public DateTime JoinDate { get; set; }
        public IList<string> Roles { get; set; }
        public IList<System.Security.Claims.Claim> Claims { get; set; }
    }
}