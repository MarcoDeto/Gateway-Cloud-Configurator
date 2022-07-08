using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace i4conn.GatewayCloudConfigurationCore.Persistence.Models
{
    public partial class Confute
    {
        public long Recno { get; set; }
        [Required]
        [StringLength(10)]
        public string Utente { get; set; }
        [Required]
        [StringLength(20)]
        public string Oggetto { get; set; }
        [Required]
        [StringLength(30)]
        public string Adw { get; set; }
        [Required]
        [StringLength(2)]
        public string Livello { get; set; }
        public bool FlgProfilo { get; set; }
    }
}
