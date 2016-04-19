using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;

namespace AspIdentityWebAPI.Infrastructure
{
    /// <summary>
    /// Clase que hereda los datos de Identity User y extiende los atributos que para adicionar las columnas requeridas
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Columna que referencia los nombres del usuario
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        /// <summary>
        /// Columna que referencia los apellidos de un usuario
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public byte Level { get; set; }

        /// <summary>
        /// Columna que referencia la fecha que se registró en el sistema
        /// </summary>
        [Required]
        public DateTime JoinDate { get; set; }

    }
}