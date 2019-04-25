using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sp.auth.app.account.commands.authenticate;
using sp.auth.app.account.commands.create;
using sp.auth.app.account.commands.renew;
using sp.auth.app.infra.config;

namespace sp.auth.service.controllers.api
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
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
            var sb = new StringBuilder();

            sb.Append("you are here\r\n");
            
            var claims = HttpContext.User.Claims;

            foreach (var i in claims)
            {
                sb.Append($"{i.Type} : {i.Value}\r\n");
            }
            
            return sb.ToString();
        }
        
        [HttpGet("SecuredAdmin")]
        [Authorize(Roles = Roles.Admin)]
        public object SecuredAdmin()
        {
            var sb = new StringBuilder();

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
            var res = await _mediator.Send(cmd);

            return Ok(res);
        }
        
        [HttpPost("renew")]
        [AllowAnonymous]
        public async Task<IActionResult> Renew([FromBody] RenewAccountSessionCommand cmd)
        {
            var res = await _mediator.Send(cmd);

            return Ok(res);
        }
    }
}