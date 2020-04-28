using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NotificationDemo.Web.Filters
{
    public class NotificationDemoAuthorizeFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.Any(x => x is AllowAnonymousAttribute);
            if (allowAnonymous) return;

            // пользователь уже авторизован, ничего делать не нужно
            if (context.HttpContext.IsUserAuthenticated()) return;

            if (!context.HttpContext.IsIdentityExists())
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }

    public static class HttpContextExtensions
    {
        public static bool IsUserAuthenticated(this Microsoft.AspNetCore.Http.HttpContext context)
        {
            return context.IsIdentityExists();
        }

        public static bool IsIdentityExists(this Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            var identityAuthenticated = httpContext.User?.Identity?.IsAuthenticated;
            return identityAuthenticated.HasValue && identityAuthenticated.Value;
        }
    }
}
