using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using sp.auth.app.account.commands.authenticate;
using sp.auth.app.account.commands.create;

namespace sp.auth.service.controllers.api
{
    [Authorize]
    public class AuthController : Controller
    {
        private ILogger<AuthController> _logger { get; set; }
        
        private readonly IMediator _mediator;

        public AuthController(ILogger<AuthController> logger,IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public object Index()
        {
            return "hello";
        }
        
        [HttpGet]
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

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateAccountCommand cmd)
        {
            var res = await _mediator.Send(cmd);

            return Ok(res);
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateAccountCommand cmd)
        {
            var res = await _mediator.Send(cmd);

            return Ok(res);
        }
    }
}