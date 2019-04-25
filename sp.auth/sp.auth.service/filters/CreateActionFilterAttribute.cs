using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using sp.auth.app.account.commands.create;
using sp.auth.app.interfaces;
using sp.auth.common.constraints;
using Microsoft.Extensions.DependencyInjection;

namespace sp.auth.service.filters
{
    public class CreateActionFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContextService = context.HttpContext.RequestServices.GetService<IHttpContextService>();
            
            var cmd = context.ActionArguments.Values.SingleOrDefault(x => x is CreateAccountCommand) as CreateAccountCommand;

            if (cmd.Role == Roles.Account)
            {
                return;
            }

            var principal = httpContextService.Validate(true);

            //only authorised admin allowed to create other user types
            if (!principal.IsInRole(Roles.Admin))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = (int)HttpStatusCode.Forbidden,
                    Content = "Forbidden"
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
         
        }
    }
}