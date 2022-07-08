using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos
{
    public class InterfaceChannelAssociateDto
    {
        [Required(ErrorMessage = "Il campo dell'id dell'interfaccia è obbligatorio")]
        [StringLength(3, ErrorMessage = "Il campo dell'id dell'interfaccia deve contenere massimo 3 caratteri")]
        public string InterfaceId { get; set; } // IdInterfaccia
        [Required(ErrorMessage = "Il campo dell'id del canale virtuale è obbligatorio")]
        [StringLength(2, ErrorMessage = "Il campo dell'id del canale virtuale deve contenere massimo 2 caratteri")]
        public string VirtualChannelId { get; set; } // IdCanaleVirtuale
        [Required(ErrorMessage = "Il campo dell'id del canale è obbligatorio")]
        [StringLength(2, ErrorMessage = "Il campo dell'id del canale deve contenere massimo 2 caratteri")]
        public string ChannelId { get; set; } // IdCanale
        [Required(ErrorMessage = "Il campo della direzione è obbligatorio")]
        [StringLength(6, ErrorMessage = "l campo della direzione deve contenere massimo 6 caratteri")]
        public string Direction { get; set; } // Direzione
        public bool FlgAbilitaOriginale { get; set; }
        public string chDescription { get; set; }
        public string virtualChDescription { get; set; }
        public string chType { get; set; }
        public string virtualChType { get; set; }
    }
}
