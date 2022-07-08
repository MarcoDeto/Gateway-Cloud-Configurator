using System.ComponentModel.DataAnnotations;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
