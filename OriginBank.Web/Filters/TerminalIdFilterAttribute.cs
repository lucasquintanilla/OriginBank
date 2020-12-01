using OriginBank.Web.Utilities;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace OriginBank.Web.Filters
{
    public class TerminalIdFilterAttribute : ActionFilterAttribute
    {
        private readonly ITerminalSessionManager _sessionManager = new BasicTerminalSessionManager();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!_sessionManager.GetSessionCardId(context.HttpContext).HasValue)
            {
                //throw new InvalidOperationException("Can't access this area with no valid card selected");

                //context.Result = new RedirectToRouteResult(new RouteValueDictionary())

                context.Result = new RedirectResult("Index");
            }

            base.OnActionExecuting(context);
        }
    }
}
