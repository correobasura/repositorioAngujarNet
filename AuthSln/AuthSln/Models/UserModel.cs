using System.ComponentModel.DataAnnotations;

namespace AuthSln.Models
{
    public class UserModel
    {
        [Required]
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La {0} debe contener al menos {2} caracteres", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas nos coinciden")]
        public string ConfirmPassword { get; set; }
    }
}