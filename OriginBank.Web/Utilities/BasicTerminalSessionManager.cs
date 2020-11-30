using Microsoft.AspNetCore.Http;

namespace OriginBank.Web.Utilities
{
    public class BasicTerminalSessionManager : ITerminalSessionManager
    {
        private static readonly string SESSION_KEY_CARD_ID = "cardId";
        private static readonly string SESSION_KEY_AUTHORIZED = "isAuthorized";
        private static readonly string SESSION_PIN_ATTEMPTS = "pinAttempts";

        public void ClearSession(HttpContext httpContext)
        {
            httpContext.Session.Clear();
        }

        public int? GetSessionCardId(HttpContext httpContext)
        {
            return httpContext.Session.GetInt32(SESSION_KEY_CARD_ID);
        }

        public void SetSessionCardId(HttpContext httpContext, int id)
        {
            httpContext.Session.SetInt32(SESSION_KEY_CARD_ID, id);
        }

        public void Authorize(HttpContext httpContext)
        {
            httpContext.Session.SetInt32(SESSION_KEY_AUTHORIZED, 1);
        }

        public void Unauthorize(HttpContext httpContext)
        {
            httpContext.Session.SetInt32(SESSION_KEY_AUTHORIZED, 0);
        }

        public bool IsAuthorized(HttpContext httpContext)
        {
            int? val = httpContext.Session.GetInt32(SESSION_KEY_AUTHORIZED);
            return val != null && val.HasValue && val == 1;
        }

        public void SetPinAttempts(HttpContext httpContext, int id)
        {
            httpContext.Session.SetInt32(SESSION_PIN_ATTEMPTS, id);
        }
        public int GetPinAttempts(HttpContext httpContext)
        {
            var value = httpContext.Session.GetInt32(SESSION_PIN_ATTEMPTS);

            if (value == null)
            {
                return 0;
            }

            return (int)value;
        }
    }
}
