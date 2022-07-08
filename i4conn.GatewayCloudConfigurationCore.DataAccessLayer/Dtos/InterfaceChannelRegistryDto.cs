using System.ComponentModel.DataAnnotations;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos
{
    public class InterfaceChannelRegistryDto
    {
        [Required(ErrorMessage = "Il campo dell'id del canale è obbligatorio")]
        [StringLength(2, ErrorMessage = "Il campo dell'id del canale deve contenere massimo 2 caratteri")]
        public string ChannelId { get; set; } // IdCanale
        [Required(ErrorMessage = "Il campo della tipologia è obbligatorio")]
        [StringLength(10, ErrorMessage = "Il campo della tipologia deve contenere massimo 10 caratteri")]
        public string TypologyInterface { get; set; } // TipologiaInterfaccia
        [Required(ErrorMessage = "Il campo del tipo canale è obbligatorio")]
        [StringLength(10, ErrorMessage = "Il campo del tipo canale deve contenere massimo 10 caratteri")]
        public string ChannelType { get; set; } // TipoCanale
        [Required(ErrorMessage = "Il campo della descrizione è obbligatorio")]
        [StringLength(255, ErrorMessage = "Il campo della descrizione deve contenere massimo 255 caratteri")]
        public string Description { get; set; } // Descrizione
        [Required(ErrorMessage = "Il campo della direzione è obbligatorio")]
        [StringLength(6, ErrorMessage = "l campo della direzione deve contenere massimo 6 caratteri")]
        public string Direction { get; set; } // Direzione
    }
}
