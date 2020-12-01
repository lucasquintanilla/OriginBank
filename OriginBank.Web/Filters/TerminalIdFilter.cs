using OriginBank.Web.Utilities;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace OriginBank.Web.Filters
{
    public class TerminalIdFilter : ActionFilterAttribute
    {
        private readonly ITerminalSessionManager _sessionManager = new BasicTerminalSessionManager();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!_sessionManager.GetSessionCardId(context.HttpContext).HasValue)
            {
                //throw new InvalidOperationException("Can't access this area with no valid card selected");

                context.Result = new RedirectResult("Error");
            }

            base.OnActionExecuting(context);
        }
    }
}
