using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace i4conn.GatewayCloudConfigurationCore.Persistence.Models
{
    public partial class Ts400TipiCanale
    {
        public long Recno { get; set; }
        [Required]
        [StringLength(2)]
        public string TipoCanale { get; set; }
        [Required]
        [StringLength(40)]
        public string DesCanale { get; set; }
        [Required]
        [StringLength(100)]
        public string Topic { get; set; }
        public bool FlgStandard { get; set; }
    }
}
