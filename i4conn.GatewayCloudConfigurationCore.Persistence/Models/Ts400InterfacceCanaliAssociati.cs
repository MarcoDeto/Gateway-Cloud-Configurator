using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace i4conn.GatewayCloudConfigurationCore.Persistence.Models
{
    public partial class Ts400InterfacceCanaliAssociati
    {
        public long Recno { get; set; }
        [Required]
        [StringLength(3)]
        public string IdInterfaccia { get; set; }
        [Required]
        [StringLength(2)]
        public string IdCanaleVirtuale { get; set; }
        [Required]
        [StringLength(2)]
        public string IdCanale { get; set; }
        [Required]
        [StringLength(6)]
        public string Direzione { get; set; }
        public bool FlgAbilitaOriginale { get; set; }
    }
}
