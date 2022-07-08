using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace i4conn.GatewayCloudConfigurationCore.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// Action di test per autorizzazione
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
                return identity.FindFirst(ClaimTypes.Name).Value.Trim();
            return string.Empty;
        }
    }
}
