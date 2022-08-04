using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc.Rendering;

namespace UIDO.Domain.Indentity
{
    public class ApplicationUser: IdentityUser
    {
        public string Nombre { get; set; }
        public string Rut { get; set; }
        public string GrupoCoordinacion { get; set; }
        public string Profesion { get; set; }

        [NotMapped]
        [Display(Name="Rol para el Usuario")]
        public string IdRol { get; set; }
        [NotMapped]
        public string Rol { get; set; }
        [NotMapped]
        public IEnumerable<string> listaRol { get; set; }
        //public IEnumerable<SelectListItem> listaRol { get; set; }
    }
}
