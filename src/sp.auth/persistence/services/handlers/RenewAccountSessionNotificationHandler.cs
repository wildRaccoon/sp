using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using sp.auth.app.infra.config;
using sp.auth.domain.account.events;

namespace sp.auth.persistence.account.handlers
{
    public class RenewAccountSessionNotificationHandler : INotificationHandler<RenewAccountSessionSuccessDomainEvent>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HashConfig _hash;

        public RenewAccountSessionNotificationHandler(IHttpContextAccessor httpContextAccessor, HashConfig hash)
        {
            _httpContextAccessor = httpContextAccessor;
            _hash = hash;
        }

        public async Task Handle(RenewAccountSessionSuccessDomainEvent notification, CancellationToken cancellationToken)
        {
            var key = Encoding.ASCII.GetBytes(_hash.secret);
            
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, notification.AccId.ToString()),
                    new Claim(CustomClaims.RenewToken, notification.RenewToken),
                    new Claim(ClaimTypes.Role, notification.Role) 
                }),
                
                Expires = notification.SessionExpired,
                Issuer = "sp.auth",
                IssuedAt = notification.IssuedOn,
                NotBefore = notification.IssuedOn,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenEncoded = tokenHandler.WriteToken(token);
            
            _httpContextAccessor.HttpContext.Response.Headers.Add("sp.auth",tokenEncoded);
            
            await Task.CompletedTask;
        }
    }
}