using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using NotificationDemo.Web.Controllers;
using NotificationDemo.Web.Helpers;
using System;

namespace NotificationDemo.Web.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class NotificationDemoExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public NotificationDemoExceptionFilterAttribute(ILogger logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled) return;

            _logger.LogError(context.Exception, context.Exception.Message);

            if (context.ActionDescriptor is ControllerActionDescriptor descriptor &&
                ApiControllerType.IsAssignableFrom(descriptor.ControllerTypeInfo.BaseType))
            {
                var message = WebErrorHelper.GetMessageFromException(context.Exception);

                context.ExceptionHandled = true;
                context.HttpContext.Response.ContentType = "application/json";
                context.Result = ApiController.InternalServerError(message);
            }

            base.OnException(context);
        }

        private readonly ILogger _logger;
        private static readonly Type ApiControllerType = typeof(ApiController);
    }
}
