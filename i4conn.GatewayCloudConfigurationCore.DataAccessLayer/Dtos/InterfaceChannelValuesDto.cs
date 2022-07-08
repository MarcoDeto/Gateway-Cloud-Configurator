using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos
{
    public class InterfaceChannelValuesDto
    {
        [Required(ErrorMessage = "Il campo dell'id dell'interfaccia è obbligatorio")]
        [StringLength(3, ErrorMessage = "Il campo dell'id dell'interfaccia deve contenere massimo 3 caratteri")]
        public string InterfaceId { get; set; } // IdInterfaccia
        //[Required(ErrorMessage = "Il campo dell'id del canale è obbligatorio")]
        [StringLength(2, ErrorMessage = "Il campo dell'id del canale deve contenere massimo 2 caratteri")]
        public string ChannelId { get; set; } // IdCanale
        [Required(ErrorMessage = "Il campo del tipo canale è obbligatorio")]
        [StringLength(10, ErrorMessage = "Il campo del tipo canale deve contenere massimo 10 caratteri")]
        public string ChannelType { get; set; } // TipoCanale
        [Required(ErrorMessage = "Il campo della descrizione è obbligatorio")]
        [StringLength(255, ErrorMessage = "Il campo della descrizione deve contenere massimo 255 caratteri")]
        public string Description { get; set; } // Descrizione
        [Required(ErrorMessage = "Il campo della direzione è obbligatorio")]
        [StringLength(6, ErrorMessage = "l campo della direzione deve contenere massimo 6 caratteri")]
        public string Direction { get; set; } // Direzione
        public bool FlgVirtual { get; set; }
        [StringLength(10, ErrorMessage = "Il campo dell'id della regola deve contenere massimo 10 caratteri")]
        public string RuleId { get; set; } // IdRegola
        [StringLength(10, ErrorMessage = "Il campo della destinazione deve contenere massimo 10 caratteri")]
        public string Destination { get; set; }
        [StringLength(2, ErrorMessage = "Il campo dell'id del canale di origine deve contenere massimo 2 caratteri")]
        public string OriginChannelId { get; set; } // IdCanaleOrigine
        public bool AllowEditChannel { get; set; }
        public bool AllowEditVariable { get; set; }
        public List<InterfaceChannelVariablesDto> Variables { get; set; }
        public List<InterfaceChannelAssociateDto> AssociateChannels { get; set; }
    }
}
