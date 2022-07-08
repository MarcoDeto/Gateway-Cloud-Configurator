using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos
{
    public class GatewayDto
    {
        [Required(ErrorMessage = "Il campo dell'id è obbligatorio")]
        [StringLength(3, ErrorMessage = "Il campo dell'id deve contenere massimo 3 caratteri")]
        public string GatewayId { get; set; } // IdGateway
        [Required(ErrorMessage = "Il campo della descrizione è obbligatorio")]
        [StringLength(255, ErrorMessage = "Il campo della descrizione deve contenere massimo 255 caratteri")]
        public string Description { get; set; } // DesGateway
        [Required(ErrorMessage = "Il campo del numero seriale è obbligatorio")]
        [StringLength(15, ErrorMessage = "Il campo del numero seriale deve contenere massimo 15 caratteri")]
        public string SerialNumber { get; set; } // Serialnr
        [Required(ErrorMessage = "Il campo del nome è obbligatorio")]
        [StringLength(30, ErrorMessage = "Il campo del nome deve contenere massimo 30 caratteri")]
        public string GatewayName { get; set; } // NomeGateway
        [Required(ErrorMessage = "Il campo della location è obbligatorio")]
        [StringLength(15, ErrorMessage = "Il campo della location deve contenere massimo 15 caratteri")]
        public string Location { get; set; } // Loc
        [Required(ErrorMessage = "Il campo dell'ip di destinazione è obbligatorio")]
        [StringLength(15, ErrorMessage = "Il campo dell'ip di destinazione deve contenere massimo 15 caratteri")]
        public string DestinationIp { get; set; } // DestIp
        [Required(ErrorMessage = "Il campo della porta di destinazione è obbligatorio")]
        [StringLength(10, ErrorMessage = "Il campo della porta di destinazione deve contenere massimo 10 caratteri")]
        public string DestinationPort { get; set; } // DestPorta
        //[Required(ErrorMessage = "Il campo dell'utente di destinazione è obbligatorio")]
        [StringLength(20, ErrorMessage = "Il campo dell'utente di destinazione deve contenere massimo 20 caratteri")]
        public string DestinationUser { get; set; } // DestUser
        //[Required(ErrorMessage = "Il campo della password di destinazione è obbligatorio")]
        [StringLength(20, ErrorMessage = "Il campo della password di destinazione deve contenere massimo 20 caratteri")]
        public string DestinationPassword { get; set; } // DestPwd
        [StringLength(255, ErrorMessage = "Il campo della versione del supervisor deve contenere massimo 255 caratteri")]
        public string VersionSupervisor { get; set; } // VerSupervisor
        [StringLength(255, ErrorMessage = "Il campo della versione del device deve contenere massimo 255 caratteri")]
        public string VersionDevice { get; set; } // VerDevice
        [StringLength(255, ErrorMessage = "Il campo della versione del router deve contenere massimo 255 caratteri")]
        public string VersionRouter { get; set; } // VerRouter
        [StringLength(255, ErrorMessage = "Il campo delle regole di versione deve contenere massimo 255 caratteri")]
        public string VersionRules { get; set; } // VerRules
        [StringLength(255, ErrorMessage = "Il campo della root del firmware deve contenere massimo 255 caratteri")]
        public string FirmwareRoot { get; set; }
        [StringLength(255, ErrorMessage = "Il campo della versione della web app deve contenere massimo 255 caratteri")]
        public string VersionWebapp { get; set; } // VerWebapp
        [StringLength(15, ErrorMessage = "Il campo della versione dell'ip di destinazione secondario deve contenere massimo 15 caratteri")]
        public string DestinationSecondaryIp { get; set; } // DestIpSec
        [StringLength(10, ErrorMessage = "Il campo della versione della porta di destinazione secondaria deve contenere massimo 15 caratteri")]
        public string DestinationSecondaryPort { get; set; } // DestPortaSec
        [StringLength(20, ErrorMessage = "Il campo della versione dell'utente di destinazione secondario deve contenere massimo 15 caratteri")]
        public string DestinationSecondaryUser { get; set; } // DestUserSec
        [StringLength(20, ErrorMessage = "Il campo della versione della password di destinazione secondaria deve contenere massimo 15 caratteri")]
        public string DestinationSecondaryPassword { get; set; } // DestPwdSec
        public int CounterAdapters { get; set; }
        public int CounterDevices { get; set; }
    }
}
