using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace i4conn.GatewayCloudConfigurationCore.Persistence.Models
{
    // Qui sono racchiusi i parametri di specifiche interfacce, gateway, vabiabili e canali
    public partial class Ts400ParamEntValori
    {
        public long Recno { get; set; }
        [Required]
        [StringLength(50)]
        public string Entita { get; set; }
        [Required]
        [StringLength(50)]
        public string IdEntita { get; set; }
        [Required]
        [StringLength(100)]
        public string ParamNome { get; set; }
        [Required]
        [StringLength(255)]
        public string ParamValore { get; set; }
    }
}
