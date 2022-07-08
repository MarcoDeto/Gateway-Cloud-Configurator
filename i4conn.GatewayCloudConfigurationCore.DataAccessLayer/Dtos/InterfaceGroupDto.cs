using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos
{
    public class InterfaceGroupDto
    {
        [Required(ErrorMessage = "Il campo dell'id è obbligatorio")]
        [StringLength(10, ErrorMessage = "Il campo dell'id deve contenere massimo 10 caratteri")]
        public string Id { get; set; } // IdGruppoInterfacce
        [Required(ErrorMessage = "Il campo della descrizione è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il campo della descrizione deve contenere massimo 100 caratteri")]
        public string Description { get; set; } // DesGruppoInterfacce
        //[Required(ErrorMessage = "Il campo dell'id del gateway è obbligatorio")]
        [StringLength(3, ErrorMessage = "Il campo dell'id del gateway deve contenere massimo tre caratteri")]
        public string GatewayId { get; set; } // IdGateway
        public List<InterfaceDto> Interfaces { get; set; }
    }
}
