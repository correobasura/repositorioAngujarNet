using System;
using System.ComponentModel.DataAnnotations;

namespace AuthSln.Models
{
    /// <summary>
    /// Clase que contiene la información del Token
    /// </summary>
    public class RefreshToken
    {
        /// <summary>
        /// Identificador de la clase con información del token
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// Referencia al usuario al qúe pertenece el token
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Subject { get; set; }

        /// <summary>
        /// Identificador de la aplicación del cliente
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string ClientId { get; set; }

        /// <summary>
        /// Contiene la fecha de emisión del token
        /// </summary>
        public DateTime FechaEmision { get; set; }

        /// <summary>
        /// Contiene la fecha de expiración del token
        /// </summary>
        public DateTime FechaExpiracion { get; set; }

        /// <summary>
        /// Contiene la cadena hash del ticket protegido de un usuario específico
        /// </summary>
        [Required]
        public string TicketProtegido { get; set; }
    }
}