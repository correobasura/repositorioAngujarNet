using System.ComponentModel.DataAnnotations;

namespace AuthSln.Models
{
    /// <summary>
    /// Clase que contiene los datos del login externo
    /// </summary>
    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    /// <summary>
    /// Clase que registra los datos del login externo
    /// </summary>
    public class RegisterExternalBindingModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Provider { get; set; }

        [Required]
        public string ExternalAccessToken { get; set; }

    }

    /// <summary>
    /// Clase que contiene los datos asignados del token externo
    /// </summary>
    public class ParsedExternalAccessToken
    {
        public string user_id { get; set; }
        public string app_id { get; set; }
    }
}