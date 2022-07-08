using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Models
{
    public class EntityParam
    {
        [Required(ErrorMessage = "Il campo dell'entità è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il campo dell'entità deve contenere massimo 50 caratteri")]
        public string Entity { get; set;  }
        [Required(ErrorMessage = "Il campo del nome del parametro è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il campo del nome del parametro deve contenere massimo 100 caratteri")]
        public string ParamName { get; set; }
        public string ParamDefaultValue { get; set; }
        public string Type { get; set; }
        [Required(ErrorMessage = "Il campo del valore del parametro è obbligatorio")]
        [StringLength(255, ErrorMessage = "Il campo del valore del parametro deve contenere massimo 255 caratteri")]
        public string ParamValue { get; set; }
        //[Required(ErrorMessage = "Il campo dell'id è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il campo dell'id deve contenere massimo 50 caratteri")]
        public string EntityId { get; set; }
        public bool UseDefault { get; set; }
    }
}
