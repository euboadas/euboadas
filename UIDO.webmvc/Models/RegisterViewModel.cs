using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UIDO.webmvc.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Email obligatorio"), EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nombre obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Contranseña obligatoria")]
        [StringLength(50, ErrorMessage ="El {0} debe tener {2} de largo", MinimumLength =5)]
        [DataType(DataType.Password)]
        [Display(Name ="Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirmación contraseña obligatoria")]
        [Compare("Password",ErrorMessage ="Contraseña y confirmación de contraseña no coinciden")]
        [StringLength(50, ErrorMessage = "El {0} debe tener {2} de largo", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        public string ConfirmPassword { get; set; }
        [Display(Name ="Seleccionar Rol")]
        public IEnumerable<SelectListItem> ListaRol { get; set; }
        public string RolSeleccionado { get; set; }
    }
}
