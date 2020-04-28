using System.Threading.Tasks;
using NotificationDemo.Service.Dto;

namespace NotificationDemo.Service
{
    public interface IPushService
    {
        /// <summary>
        /// Checks VAPID info and if invalid generates new keys and throws exception
        /// </summary>
        /// <param name="subject">This should be a URL or a 'mailto:' email address</param>
        /// <param name="vapidPublicKey">The VAPID public key as a base64 encoded string</param>
        /// <param name="vapidPrivateKey">The VAPID private key as a base64 encoded string</param>
        void CheckOrGenerateVapidDetails(string subject, string vapidPublicKey, string vapidPrivateKey);

        /// <summary>
        /// Get the server's saved VAPID public key
        /// </summary>
        /// <returns>VAPID public key</returns>
        string GetVapidPublicKey();

        /// <summary>
        /// Register a push subscription (save to the database for later use)
        /// </summary>
        /// <param name="subscription">push subscription</param>
        /// <param name="userId">user id</param>
        Task<SubscriptionDto> Subscribe(SubscriptionDto subscription, long userId);

        /// <summary>
        /// Un-register a push subscription (delete from the database)
        /// </summary>
        /// <param name="subscription">push subscription</param>
        /// /// <param name="userId">user id</param>
        Task Unsubscribe(SubscriptionDto subscription, long userId);

        /// <summary>
        /// Send a plain text push notification to a user without any special option
        /// </summary>
        /// <param name="userIds">user id the push should be sent to</param>
        /// <param name="text">text of the notification</param>
        Task Send(long[] userIds, string text);

        /// <summary>
        /// Send a push notification to a user
        /// </summary>
        /// <param name="userIds">users id the push should be sent to</param>
        /// <param name="notification">the notification to be sent</param>
        Task Send(long[] userIds, NotificationDto notification);
    }
}
