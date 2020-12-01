using OriginBank.Web.Utilities;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Microsoft.AspNetCore.Mvc;

namespace OriginBank.Web.Filters
{
    public class TerminalAuthorizationFilter : ActionFilterAttribute
    {
        private readonly ITerminalSessionManager _sessionManager = new BasicTerminalSessionManager();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!_sessionManager.IsAuthorized(context.HttpContext))
            {
                //throw new InvalidOperationException("Can't access this area without providing a valid number-pin combination");

                context.Result = new RedirectResult("SessionExpired");
            }
            base.OnActionExecuting(context);
        }
    }
}
