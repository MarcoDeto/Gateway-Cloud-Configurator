using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Helpers;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Interfaces;
using i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Security;
using i4conn.GatewayCloudConfigurationCore.Persistence;
using i4conn.GatewayCloudConfigurationCore.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace i4conn.GatewayCloudConfigurationCore.DataAccessLayer.Repositories
{
    public class UserRepository : BaseRepository<Confute>, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;
        private readonly AppSettings _appSettings;
        private readonly ConnContext _context;

        public UserRepository(
            ConnContext context,
            IOptions<AppSettings> appSettings,
            ILogger<UserRepository> logger) : base(context, logger)
        {
            _appSettings = appSettings.Value;
            _context = context;
            _logger = logger;
        }

        public Confute CheckUser(string username)
        {
            return _context.Confutes
                .AsNoTracking()
                .Where(c => c.Utente == username && c.Livello.Equals("PW"))
                .FirstOrDefault();
        }

        public bool Authenticate(string username, string password)
        {
            bool retVal = false;

            var user = CheckUser(username);

            if (user != null)
            {
                string encryptPwd = user.Oggetto;
                retVal = EncryptionPassword.Decrypt(encryptPwd).Trim().Equals(password);
            }

            return retVal;
        }

        public JwtSecurityToken GetToken(string username)
        {
            var user = CheckUser(username);
            if (user == null)
                return null;

            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            List<Claim> claimList = new List<Claim>();
            claimList.Add(new Claim(ClaimTypes.Name, user.Utente));

            var token = new JwtSecurityToken(
                claims: claimList,
                expires: DateTime.UtcNow.AddHours(_appSettings.Expiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature));

            return token;
        }

        public override bool Insert(Confute entity)
        {
            entity.Oggetto = EncryptionPassword.Crypt(entity.Oggetto, 20);
            return base.Insert(entity);
        }
    }
}
