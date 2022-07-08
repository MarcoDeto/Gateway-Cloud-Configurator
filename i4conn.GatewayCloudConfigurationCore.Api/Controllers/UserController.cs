using i4conn.GatewayCloudConfigurationCore.Api.Helpers;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Dtos;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Helpers;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(400, Type = typeof(InfoMsg))]
    [ProducesResponseType(500, Type = typeof(InfoMsg))]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository, ILogger<UserController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Login (user: TestAdmin password: SecretPwd)
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(AuthenticateResponse))]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] AuthenticateRequest req)
        {
            // TestAdmin
            // SecretPwd
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            bool isOk = _repository.Authenticate(req.Username, req.Password);

            var tokenJWT = _repository.GetToken(req.Username);
            if (tokenJWT == null || !isOk)
                return BadRequest(
                    new InfoMsg(
                        StatusCodes.Status400BadRequest, "Username e/o password non corretti!"));

            return Ok(new AuthenticateResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(tokenJWT),
                Expiration = tokenJWT.ValidTo
            });
        }

        /// <summary>
        /// Per aggiungere un utente di test
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoMsg))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(InfoMsg))]
        public ActionResult<InfoMsg> SaveUser([FromBody] Confute user)
        {
            _logger.LogDebug(LoggerHelper.GetActualMethodName());
            if (user == null)
            {
                return BadRequest(new InfoMsg(
                    StatusCodes.Status400BadRequest, "E' necessario inserire i dati dell'utente"));
            }

            // Verifichiamo che i dati siano corretti
            if (!ModelState.IsValid)
            {
                StringBuilder ErrVal = new StringBuilder(string.Empty);
                string errore = (this.HttpContext == null) ? "400" : this.HttpContext.Response.StatusCode.ToString();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        ErrVal.Append(modelError.ErrorMessage);
                        ErrVal.Append(" - ");
                    }
                }
                return BadRequest(new InfoMsg(StatusCodes.Status400BadRequest, ErrVal.ToString()));
            }

            // Controlliamo se l'utente è presente
            var isPresent = _repository.CheckUser(user.Utente);
            if (isPresent != null)
            {
                return StatusCode(422, new InfoMsg(
                    StatusCodes.Status422UnprocessableEntity,
                    $"Utente {user.Utente} presente in uso! Impossibile inserire!"));
            }

            if (!_repository.Insert(user))
            {
                return StatusCode(500, new InfoMsg(
                    StatusCodes.Status500InternalServerError,
                    $"Ci sono stati problemi nell'inserimento dell'Utente {user.Utente}."));
            }

            return Ok(new InfoMsg(
                StatusCodes.Status200OK, $"Inserimento Utente {user.Utente} eseguito con successo!"));
        }
    }
}
