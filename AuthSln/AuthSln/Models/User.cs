using System.ComponentModel.DataAnnotations;

namespace AuthSln.Models
{
    /// <summary>
    /// Clase que establece la información del cliente
    /// </summary>
    public class User
    {
        /// <summary>
        /// Atributo que referencia el identificador del usuario
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// Atributo aque almacena la información secreta
        /// </summary>
        [Required]
        public string Secret { get; set; }

        /// <summary>
        /// Atributo que almacena el nombre de la aplicación
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string NombreApp { get; set; }

        /// <summary>
        /// Estrucutra que contiene el tipo de aplicación
        /// </summary>
        public TiposAplicacion TipoAplicacion { get; set; }

        /// <summary>
        /// Atributo que indica el estado del cliente
        /// </summary>
        public bool Activo { get; set; }

        /// <summary>
        /// Atributo que almacena el tiempo en que se actualiza el Token
        /// </summary>
        public int TiempoActualizacionToken { get; set; }

        /// <summary>
        /// Atributo que referencia el origen desde el que se admite la petición
        /// </summary>
        [MaxLength(100)]
        public string OrigenAdmitido { get; set; }
    }
}