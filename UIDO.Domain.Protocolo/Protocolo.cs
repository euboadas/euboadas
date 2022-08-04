using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIDO.Domain.Protocolo
{
    public  class Protocolo
    {
        [Key]
        public string Nombre { get; set; }
        [Required]
        public string Sponsor { get; set; }
        
        public string CRO { get; set; }
        public string ContactoCRA { get; set; }
        public string GrupoDiagnostico { get; set; }
        public string FaseProtocolo { get; set; }
        [Required]
        public string InvestigadorPrincipal { get; set; }
        public string Regulatorio { get; set; }
        [Required]
        public string Status { get; set; }
        public string CoordinadorClinico { get; set; }
        public string AnalistaFinanciero { get; set; }
        public string CoordinadorBotiquin { get; set; }
    }
}
