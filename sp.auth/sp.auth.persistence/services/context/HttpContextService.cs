using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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
        private const string TokenIssuer = "sp.auth";

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
                Subject = new ClaimsIdentity(new [] 
                {
                    new Claim(ClaimTypes.Name, accountId.ToString()),
                    new Claim(ClaimTypes.Role, role) 
                }),
                
                Expires = expired,
                Issuer = TokenIssuer,
                IssuedAt = issuedOn,
                NotBefore = issuedOn,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenEncoded = tokenHandler.WriteToken(token);
            
            _httpContextAccessor.HttpContext.Response.Headers.Add("sp.auth",tokenEncoded);
            await Task.CompletedTask;
        }

        private TokenValidationParameters GetTokenValidationParameters(bool validateLifetime = true)
        {
            var key = Encoding.ASCII.GetBytes(_hash.secret);

            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = TokenIssuer,
                RequireExpirationTime = true,
                ValidateLifetime = validateLifetime,
                RequireSignedTokens = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateAudience = false
            };
        }

        private ClaimsPrincipal ValidateToken(string authValue,bool validateLifetime = true)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.ValidateToken
                (
                    authValue,GetTokenValidationParameters(validateLifetime), 
                    out _
                );
        }

        private string GetTokenFromContext()
        {
            var authHeader = _httpContextAccessor.HttpContext?.Request?.Headers["Authorization"];

            if (!authHeader.HasValue)
            {
                throw new NullReferenceException("Not allowed.");
            }

            var authValue = authHeader.GetValueOrDefault(string.Empty).ToString();

            if (string.IsNullOrEmpty(authValue))
            {
                throw new NullReferenceException("Not allowed.");
            }

            if (!authValue.StartsWith("Bearer"))
            {
                throw new NullReferenceException("Not allowed.");
                
            }

            authValue = authValue.Substring(7);

            return authValue;
        }

        public long GetAccountIdFromContext()
        {
            var authValue = GetTokenFromContext();
            
            //try to get account id from context even it's lifetime check not passed
            var principal = ValidateToken(authValue, false);
            
            var cname = principal.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Name);
            
            if (cname == null)
            {
                throw new NullReferenceException("Claims not available.");
            }

            if (!long.TryParse(cname.Value, out var id))
            {
                throw new NullReferenceException($"Invalid claim value:{cname.Value}.");
            }

            return id;
        }

        public ClaimsPrincipal Validate(bool validateLifetime)
        {
            var authValue = GetTokenFromContext();
            return ValidateToken(authValue,validateLifetime);
        }
    }
}