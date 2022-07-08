using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace i4conn.GatewayCloudConfigurationCore.Persistence.Models
{
    public partial class Ts400ParamEntAnagr
    {
        public long Recno { get; set; }
        [Required]
        [StringLength(50)]
        public string Entita { get; set; }
        [Required]
        [StringLength(100)]
        public string ParamNome { get; set; }
        [Required]
        [StringLength(255)]
        public string ParamValoreDefault { get; set; }
        [Required]
        [StringLength(10)]
        public string Tipologia { get; set; }
    }
}
