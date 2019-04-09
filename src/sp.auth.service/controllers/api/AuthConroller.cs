using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using sp.auth.app.account.commands.authenticate;
using sp.auth.app.account.commands.create;

namespace sp.auth.service.controllers.api
{
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public object Index()
        {
            return "hello";
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccountCommand cmd)
        {
            var res = await _mediator.Send(cmd);

            return Ok(res);
        }
        
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateAccountCommand cmd)
        {
            var res = await _mediator.Send(cmd);

            return Ok(res);
        }
    }
}