using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace i4conn.GatewayCloudConfigurationCore.Persistence.Models
{
    public partial class Ts400Entitum
    {
        public long Recno { get; set; }
        [Required]
        [StringLength(50)]
        public string Entita { get; set; }
        [Required]
        [StringLength(80)]
        public string DesEntita { get; set; }
    }
}
