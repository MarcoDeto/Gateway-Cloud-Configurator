using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos
{
    public class EntityParamRegistryDto
    {
        [Required(ErrorMessage = "Il campo dell'entità è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il campo dell'entità deve contenere massimo 50 caratteri")]
        public string Entity { get; set; } // Entita
        [Required(ErrorMessage = "Il campo del nome è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il campo del nome deve contenere massimo 100 caratteri")]
        public string ParamName { get; set; } // ParamNome
        //[Required(ErrorMessage = "Il campo del valore di default è obbligatorio")]
        [StringLength(255, ErrorMessage = "Il campo del valore di default è obbligatorio")]
        public string ParamDefaultValue { get; set; } // ParamValoreDefault
        // [Required(ErrorMessage = "Il campo della tipologia è obbligatorio")]
        [StringLength(10, ErrorMessage = "Il campo della tipologia deve contenere massimo 10 caratteri")]
        public string Type { get; set; } // Tipologia
    }
}
