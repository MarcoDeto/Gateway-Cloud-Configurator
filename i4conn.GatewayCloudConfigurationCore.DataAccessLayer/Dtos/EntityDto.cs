using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos
{
    public class EntityDto
    {
        [Required(ErrorMessage = "Il campo del nome è obbligatorio")]
        [StringLength(50, ErrorMessage = "Il nome dell'entità deve contenere massimo 50 caratteri")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Il campo della descrizione è obbligatorio")]
        [StringLength(80, ErrorMessage = "Il nome della descrizione deve contenere massimo 80 caratteri")]
        public string Description { get; set; }
    }
}
