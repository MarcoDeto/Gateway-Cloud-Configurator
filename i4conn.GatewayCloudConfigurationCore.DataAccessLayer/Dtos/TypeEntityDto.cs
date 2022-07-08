using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos
{
    public class TypeEntityDto
    {
        [Required(ErrorMessage = "Il campo dell'id è obbligatorio")]
        [StringLength(10, ErrorMessage = "Il campo dell'id deve contenere massimo 10 caratteri")]
        public string Id { get; set; } // IdTipo
        [Required(ErrorMessage = "Il campo della descrizione è obbligatorio")]
        [StringLength(80, ErrorMessage = "Il campo della descrizione deve contenere massimo 80 caratteri")]
        public string Description { get; set; } // DesTipo
        [Required(ErrorMessage = "Il campo dell'entità è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il campo dell'entità deve contenere massimo 50 caratteri")]
        public string Entity { get; set; } // Entita
        [StringLength(80, ErrorMessage = "Il campo del primo parametro deve contenere massimo 80 caratteri")]
        public string Param1 { get; set; } // Par01
        [StringLength(80, ErrorMessage = "Il campo del secondo parametro deve contenere massimo 80 caratteri")]
        public string Param2 { get; set; } // Par02
        [StringLength(80, ErrorMessage = "Il campo del terzo parametro deve contenere massimo 80 caratteri")]
        public string Param3 { get; set; } // Par03
        public bool AllowEditChannel { get; set; }
        public bool AllowEditVariable { get; set; }
    }
}
