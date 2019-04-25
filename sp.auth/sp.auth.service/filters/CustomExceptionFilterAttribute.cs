using System;
using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using sp.auth.domain.account.exceptions;

namespace sp.auth.service.filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException)
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new JsonResult(
                    ((ValidationException)context.Exception).Errors);

                return;
            }

            if (context.Exception is UnauthorizedException)
            {
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                context.HttpContext.Response.ContentType = "application/json";
                context.Result = new JsonResult( new {});
                return;
            }
            
            var code = HttpStatusCode.InternalServerError;

            //throw from executed command
            if (context.Exception is AccountException)
            {
                code = HttpStatusCode.BadRequest;
            }

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)code;
            context.Result = new JsonResult(new
            {
                error = new[] { context.Exception.Message },
                stackTrace = context.Exception.StackTrace
            });
        }
    }
}