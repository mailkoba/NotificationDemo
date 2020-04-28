using System;

namespace NotificationDemo.Common
{
    public class NotificationDemoException : Exception
    {
        public NotificationDemoException(string message)
            : base(message)
        {
        }

        public NotificationDemoException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public NotificationDemoException(string messageFormat, params object[] values)
            : base(string.Format(messageFormat, values))
        {
        }

        public bool IsWarning { get; set; }
    }
}
