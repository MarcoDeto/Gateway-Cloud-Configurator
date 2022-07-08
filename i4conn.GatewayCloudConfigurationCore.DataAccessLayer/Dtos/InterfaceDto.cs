using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos
{
    public class InterfaceDto
    {
        [StringLength(3, ErrorMessage = "Il campo dell'id deve contenere massimo 3 caratteri")]
        public string InterfaceId { get; set; } // IdInterfaccia
        [Required(ErrorMessage = "Il campo della descrizione è obbligatorio")]
        [StringLength(80, ErrorMessage = "Il campo della descrizione deve contenere massimo 80 caratteri")]
        public string InterfaceDescription { get; set; } // DesInterfaccia
        //[Required(ErrorMessage = "Il campo dell'ip del terminale è obbligatorio")]
        [StringLength(30, ErrorMessage = "Il campo dell'ip del terminale deve contenere massimo 30 caratteri")]
        public string TerminalIp { get; set; } // IpTerminale
        //[Required(ErrorMessage = "Il campo della porta del terminale è obbligatorio")]
        [StringLength(10, ErrorMessage = "Il campo della porta del terminale deve contenere massimo 10 caratteri")]
        public string TerminalPort { get; set; } // PortaTerminale
        public int DeviceId { get; set; } // IdDispositivo
        [StringLength(16, ErrorMessage = "Il campo della router deve contenere massimo 16 caratteri")]
        public string Router { get; set; }
        public int InputChannelNumber { get; set; } // NumCanaliI
        public int OutputChannelNumber { get; set; } // NumCanaliU
        [StringLength(10, ErrorMessage = "Il campo dell'id gruppi e interfacce deve contenere massimo 10 caratteri")]
        public string InterfaceGroupId { get; set; } // IdGruppoInterfacce
        [StringLength(100, ErrorMessage = "Il campo della descrizione gruppi e interfacce deve contenere massimo 10 caratteri")]
        public string InterfaceGroupDescription { get; set; } // DesGruppoInterfacce
        public DateTime? LastInterrogation { get; set; } // UltimaInterrogazione
        [Required(ErrorMessage = "Il campo della tipologia è obbligatorio")]
        [StringLength(10, ErrorMessage = "Il campo della tipologia deve contenere massimo 10 caratteri")]
        public string TypologyInterface { get; set; } // InterfacciaContapezzi
        [StringLength(1, ErrorMessage = "Il campo del coordinatore deve contenere massimo 1 caratteri")]
        public string Coordinator { get; set; } // Coordinatore
    }
}
