using System.ComponentModel.DataAnnotations;

namespace UIDO.Services.API.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email obligatorio"), EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contranseña obligatoria")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Recuerdame")]
        public bool RememberMe { get; set; }

    }
}
