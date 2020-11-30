using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OriginBank.Web.Utilities
{
    public interface ITerminalSessionManager
    {
        void ClearSession(HttpContext httpContext);
        void SetSessionCardId(HttpContext httpContext, int id);
        int? GetSessionCardId(HttpContext httpContext);
        void SetPinAttempts(HttpContext httpContext, int id);
        int GetPinAttempts(HttpContext httpContext);
        void Authorize(HttpContext httpContext);
        void Unauthorize(HttpContext httpContext);
        bool IsAuthorized(HttpContext httpContext);
    }
}
