using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace i4conn.GatewayCloudConfigurationCore.Persistence.Models
{
    public partial class Ts400InterfacceGruppi
    {
        public long Recno { get; set; }
        [Required]
        [StringLength(10)]
        public string IdGruppoInterfacce { get; set; }
        [Required]
        [StringLength(100)]
        public string DesGruppoInterfacce { get; set; }
        [Required]
        [StringLength(3)]
        public string IdGateway { get; set; }
    }
}
