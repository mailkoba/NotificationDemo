
namespace NotificationDemo.Service.Dto.Push
{
    /// <summary>
    /// Representation of the Web Standard Push API's <see href="https://developer.mozilla.org/en-US/docs/Web/API/PushSubscription">PushSubscription</see>
    /// </summary>
    public class PushSubscriptionDto
    {
        /// <summary>
        /// The endpoint associated with the push subscription.
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// The subscription expiration time associated with the push subscription, if there is one, or null otherwise.
        /// </summary>
        public double? ExpirationTime { get; set; }

        /// <inheritdoc cref="Keys"/>
        public PushKeysDto Keys { get; set; }

        ///// <summary>
        ///// Converts the push subscription to the format of the library WebPush
        ///// </summary>
        ///// <returns>WebPush subscription</returns>
        //public WebPush.PushSubscription ToWebPushSubscription() => new WebPush.PushSubscription(Endpoint, Keys.P256Dh, Keys.Auth);
    }
}
