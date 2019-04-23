using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using sp.auth.app.infra.config;
using sp.auth.app.interfaces;

namespace sp.auth.persistence.services.context
{
    public class HttpContextService : IHttpContextService
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly HashConfig _hash;

        public HttpContextService(IHttpContextAccessor httpContextAccessor, HashConfig hash)
        {
            _httpContextAccessor = httpContextAccessor;
            _hash = hash;
        }

        public async Task UpdateContext(long accountId, string role,DateTime expired, DateTime issuedOn)
        {
            var key = Encoding.ASCII.GetBytes(_hash.secret);
            
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, accountId.ToString()),
                    new Claim(ClaimTypes.Role, role) 
                }),
                
                Expires = expired,
                Issuer = "sp.auth",
                IssuedAt = issuedOn,
                NotBefore = issuedOn,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenEncoded = tokenHandler.WriteToken(token);
            
            _httpContextAccessor.HttpContext.Response.Headers.Add("sp.auth",tokenEncoded);
            await Task.CompletedTask;
        }

        public long GetAccountIdFromContext()
        {
            var claims = _httpContextAccessor.HttpContext?.User?.Claims;

            if (claims == null)
            {
                throw new NullReferenceException("Claims not available.");
            }

            var cname = claims.SingleOrDefault(x => x.Type == ClaimTypes.Name);
            
            if (cname == null)
            {
                throw new NullReferenceException("Claims not available.");
            }

            long id = 0;

            if (!long.TryParse(cname.Value, out id))
            {
                throw new NullReferenceException($"Invalid claim value:{cname.Value}.");
            }

            return id;
        }
    }
}