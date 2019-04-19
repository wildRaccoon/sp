using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sp.auth.app.account.commands.authenticate;
using sp.auth.app.account.commands.create;
using sp.auth.app.account.commands.renew;
using sp.auth.app.account.queries.GetRenewToken;
using sp.auth.app.infra.config;

namespace sp.auth.service.controllers.api
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private ILogger<AuthController> _logger { get; set; }
        
        private readonly IMediator _mediator;

        public AuthController(ILogger<AuthController> logger,IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("index")]
        [AllowAnonymous]
        public object Index()
        {
            return "hello";
        }
        
        [HttpGet("secured")]
        [Authorize]
        public object Secured()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("you are here\r\n");
            
            var claims = HttpContext.User.Claims;

            foreach (var i in claims)
            {
                sb.Append($"{i.Type} : {i.Value}\r\n");
            }
            
            return sb.ToString();
        }
        
        [HttpGet("securedadmin")]
        [Authorize(Roles = Roles.Admin)]
        public object SecuredAdmin()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("you are here\r\n");
            
            var claims = HttpContext.User.Claims;

            foreach (var i in claims)
            {
                sb.Append($"{i.Type} : {i.Value}\r\n");
            }
            
            return sb.ToString();
        }

        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateAccountCommand cmd)
        {
            var res = await _mediator.Send(cmd);

            return Ok(res);
        }
        
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateAccountCommand cmd)
        {
            await _mediator.Send(cmd);

            return Ok();
        }
        
        [HttpPost("renew")]
        [AllowAnonymous]
        public async Task<IActionResult> Renew([FromBody] RenewAccountSessionCommand cmd)
        {
            await _mediator.Send(cmd);

            return Ok();
        }
        
        [HttpGet("getrenew")]
        public async Task<IActionResult> GetRenew()
        {
            var claim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            if (claim == null )
            {
                return BadRequest();
            }

            var cmd = new GetRenewTokenQuery()
            {
                AccountId = long.Parse(claim.Value)
            };

            var res = await _mediator.Send(cmd);

            return Ok(res);
        }
        
    }
}