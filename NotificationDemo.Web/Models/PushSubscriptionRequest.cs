using NotificationDemo.Service.Dto.Push;

namespace NotificationDemo.Web.Models
{
    public class PushSubscriptionRequest
    {
        /// <inheritdoc cref="Subscription"/>
        public PushSubscriptionDto Subscription { get; set; }

        /// <summary>
        /// Other attributes, like device id for example.
        /// </summary>
        public string DeviceId { get; set; }
    }
}
