using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace i4conn.GatewayCloudConfigurationCore.Persistence.Models
{
    public partial class Ts400EntitaTipologium
    {
        public long Recno { get; set; }
        [Required]
        [StringLength(80)]
        public string DesTipo { get; set; }
        [Required]
        [StringLength(50)]
        public string Entita { get; set; }
        [Required]
        [StringLength(80)]
        public string Par01 { get; set; }
        [Required]
        [StringLength(80)]
        public string Par02 { get; set; }
        [Required]
        [StringLength(80)]
        public string Par03 { get; set; }
        [Required]
        [StringLength(10)]
        public string IdTipo { get; set; }
        public bool AllowEditChannel { get; set; }
        public bool AllowEditVariable { get; set; }
    }
}
