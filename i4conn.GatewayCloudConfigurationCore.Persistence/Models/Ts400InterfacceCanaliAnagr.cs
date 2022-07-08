using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace i4conn.GatewayCloudConfigurationCore.Persistence.Models
{
    public partial class Ts400InterfacceCanaliAnagr
    {
        public long Recno { get; set; }
        [Required]
        [StringLength(2)]
        public string IdCanale { get; set; }
        [Required]
        [StringLength(10)]
        public string TipologiaInterfaccia { get; set; }
        [Required]
        [StringLength(10)]
        public string TipoCanale { get; set; }
        [Required]
        [StringLength(255)]
        public string Descrizione { get; set; }
        [Required]
        [StringLength(6)]
        public string Direzione { get; set; }
    }
}
