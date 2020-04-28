using NotificationDemo.Common;
using System;
using System.Reflection;
using System.Security;

namespace NotificationDemo.Web.Helpers
{
    public static class WebErrorHelper
    {
        public static string GetMessageFromException(Exception ex)
        {
            var message = ex.GetMessageFromException();

            return (ex.InnerException ?? ex) switch
            {
                NotificationDemoException _ => message,
                SecurityException _ => message,
                TargetInvocationException _ => message,
                _ => $"Системная ошибка: {message}"
            };
        }

        public static string GetDetailedMessageFromException(Exception ex)
        {
            var message = ex.GetMessageFromException();

            return (ex.InnerException ?? ex) switch
            {
                NotificationDemoException _ => message,
                SecurityException _ => message,
                TargetInvocationException _ => message,
                _ => $"Системная ошибка: {message}"
            };
        }
    }
}
