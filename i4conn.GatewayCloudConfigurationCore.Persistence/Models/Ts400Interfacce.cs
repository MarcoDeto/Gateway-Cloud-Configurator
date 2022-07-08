using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace i4conn.GatewayCloudConfigurationCore.Persistence.Models
{
    public partial class Ts400Interfacce
    {
        public long Recno { get; set; }
        [Required]
        [StringLength(3)]
        public string IdInterfaccia { get; set; }
        [Required]
        [StringLength(80)]
        public string DesInterfaccia { get; set; }
        [Required]
        [StringLength(30)]
        public string IpTerminale { get; set; }
        [Required]
        [StringLength(10)]
        public string PortaTerminale { get; set; }
        public int IdDispositivo { get; set; }
        [Required]
        [StringLength(16)]
        public string Router { get; set; }
        public int NumCanaliI { get; set; }
        public int NumCanaliU { get; set; }
        [Required]
        [StringLength(10)]
        public string IdGruppoInterfacce { get; set; }
        [Required]
        [StringLength(100)]
        public string DesGruppoInterfacce { get; set; }
        public DateTime? UltimaInterrogazione { get; set; }
        [Required]
        [StringLength(10)]
        public string InterfacciaContapezzi { get; set; }
        [Required]
        [StringLength(1)]
        public string Coordinatore { get; set; }
    }
}
