using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace i4conn.GatewayCloudConfigurationCore.Persistence.Models
{
    public partial class Ts400Gateway
    {
        public long Recno { get; set; }
        [Required]
        [StringLength(3)]
        public string IdGateway { get; set; }
        [Required]
        [StringLength(255)]
        public string DesGateway { get; set; }
        [Required]
        [StringLength(15)]
        public string Serialnr { get; set; }
        [Required]
        [StringLength(30)]
        public string NomeGateway { get; set; }
        [Required]
        [StringLength(15)]
        public string Loc { get; set; }
        [Required]
        [StringLength(15)]
        public string DestIp { get; set; }
        [Required]
        [StringLength(10)]
        public string DestPorta { get; set; }
        [Required]
        [StringLength(20)]
        public string DestUser { get; set; }
        [Required]
        [StringLength(20)]
        public string DestPwd { get; set; }
        [Required]
        [StringLength(255)]
        public string VerSupervisor { get; set; }
        [Required]
        [StringLength(255)]
        public string VerDevice { get; set; }
        [Required]
        [StringLength(255)]
        public string VerRouter { get; set; }
        [Required]
        [StringLength(255)]
        public string VerRules { get; set; }
        [Required]
        [StringLength(255)]
        public string FirmwareRoot { get; set; }
        [Required]
        [StringLength(255)]
        public string VerWebapp { get; set; }
        [Required]
        [StringLength(15)]
        public string DestIpSec { get; set; }
        [Required]
        [StringLength(10)]
        public string DestPortaSec { get; set; }
        [Required]
        [StringLength(20)]
        public string DestUserSec { get; set; }
        [Required]
        [StringLength(20)]
        public string DestPwdSec { get; set; }
    }
}
