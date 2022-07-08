using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos
{
    public class InterfaceChannelVariablesDto
    {
        [Required(ErrorMessage = "Il campo dell'id dell'interfaccia è obbligatorio")]
        [StringLength(3, ErrorMessage = "Il campo dell'id dell'interfaccia deve contenere massimo 3 caratteri")]
        public string InterfaceId { get; set; } // IdInterfaccia
        [Required(ErrorMessage = "Il campo dell'id del canale è obbligatorio")]
        [StringLength(2, ErrorMessage = "Il campo dell'id del canale deve contenere massimo 2 caratteri")]
        public string ChannelId { get; set; } // IdCanale
        [Required(ErrorMessage = "Il campo della direzione è obbligatorio")]
        [StringLength(6, ErrorMessage = "l campo della direzione deve contenere massimo 6 caratteri")]
        public string Direction { get; set; } // Direzione
        [Required(ErrorMessage = "Il campo del nome è obbligatorio")]
        [StringLength(30, ErrorMessage = "Il campo del nome deve contenere massimo 30 caratteri")]
        public string Name { get; set; } // Nome
        //[Required(ErrorMessage = "Il campo del nome è obbligatorio")]
        [StringLength(30, ErrorMessage = "Il campo del gruppo deve contenere massimo 30 caratteri")]
        public string Group { get; set; } // Gruppo
        [Required(ErrorMessage = "Il campo della chiave è obbligatorio")]
        [StringLength(500, ErrorMessage = "Il campo della chiave deve contenere massimo 500 caratteri")]
        public string Key { get; set; } // Chiave
    }
}
