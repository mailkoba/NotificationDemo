using System;
using System.Text;

namespace NotificationDemo.Common
{
    public static class ExceptionHelper
    {
        public static string GetMessageFromException(this Exception ex)
        {
            if (ex == null) return "Системная ошибка";

            var sb = new StringBuilder();

            sb.AppendLine(ex.Message);

            var exception = ex.InnerException;
            while (exception != null)
            {
                sb.AppendLine(exception.Message);
                exception = exception.InnerException;
            }

            return sb.ToString();
        }
    }
}
