using System.Linq;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using NotificationDemo.Service.Dto;

namespace NotificationDemo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        public static IActionResult NeedPasswordChangeResult()
        {
            return new ObjectResult(new ContainerDto<string>("Необходима смена пароля!"))
            {
                StatusCode = (int)HttpStatusCode.PaymentRequired
            };
        }

        public static IActionResult InternalServerError(string value)
        {
            return new ObjectResult(new ContainerDto<string>(value))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }

        public static IActionResult UnauthorizedResult(string value)
        {
            return new ObjectResult(new ContainerDto<string>(value))
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };
        }

        public new IActionResult Ok()
        {
            return base.Ok(new { });
        }

        public IActionResult Ok<T>(T value)
        {
            var type = typeof(T);
            if (type.GetTypeInfo().IsPrimitive || type == StringType)
            {
                return new OkObjectResult(new ContainerDto<T>(value));
            }

            return base.Ok(value);
        }

        protected string GetForwardedParam()
        {
            return Request.Headers.TryGetValue(XForwardedFor, out var forwardedVal)
                ? forwardedVal.First()?.Trim()
                : null;
        }

        private const string XForwardedFor = "HTTP_X_FORWARDED_FOR";

        private static readonly TypeInfo StringType = typeof(string).GetTypeInfo();
    }
}
