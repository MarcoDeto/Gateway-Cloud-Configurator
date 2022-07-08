using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos
{
    public class ChannelTypeDto
    {
        [Required(ErrorMessage = "Il campo del tipo canale è obbligatorio")]
        [StringLength(2, ErrorMessage = "Il campo del tipo canale deve contenere massimo 2 caratteri")]
        public string Type { get; set; } // TipoCanale
        [Required(ErrorMessage = "Il campo della descrizione è obbligatorio")]
        [StringLength(40, ErrorMessage = "Il campo della descrizione deve contenere massimo 40 caratteri")]
        public string Description { get; set; } // DesCanale
        [Required(ErrorMessage = "Il campo del topic è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il campo del topic deve contenere massimo 100 caratteri")]
        public string Topic { get; set; }
        public bool FlgStandard { get; set; }
    }
}
