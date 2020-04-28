using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NotificationDemo.DbContext;
using NotificationDemo.Domain;
using NotificationDemo.Service.Dto;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebPush;

namespace NotificationDemo.Service.Impls
{
    public class PushService : IPushService
    {
        public PushService(NotificationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _client = new WebPushClient();

            var vapidSubject = configuration.GetValue<string>("Vapid:Subject");
            var vapidPublicKey = configuration.GetValue<string>("Vapid:PublicKey");
            var vapidPrivateKey = configuration.GetValue<string>("Vapid:PrivateKey");

            CheckOrGenerateVapidDetails(vapidSubject, vapidPublicKey, vapidPrivateKey);

            _vapidDetails = new VapidDetails(vapidSubject, vapidPublicKey, vapidPrivateKey);
        }

        /// <inheritdoc />
        public void CheckOrGenerateVapidDetails(string vapidSubject, string vapidPublicKey, string vapidPrivateKey)
        {
            if (string.IsNullOrEmpty(vapidSubject) ||
                string.IsNullOrEmpty(vapidPublicKey) ||
                string.IsNullOrEmpty(vapidPrivateKey))
            {
                var vapidKeys = VapidHelper.GenerateVapidKeys();

                // Prints 2 URL Safe Base64 Encoded Strings
                Debug.WriteLine($"Public {vapidKeys.PublicKey}");
                Debug.WriteLine($"Private {vapidKeys.PrivateKey}");

                throw new Exception(
                    "You must set the Vapid:Subject, Vapid:PublicKey and Vapid:PrivateKey application settings or pass them to the service in the constructor. You can use the ones just printed to the debug console.");
            }
        }

        /// <inheritdoc />
        public string GetVapidPublicKey() => _vapidDetails.PublicKey;

        /// <inheritdoc />
        public async Task<SubscriptionDto> Subscribe(SubscriptionDto subscription, long userId)
        {
            subscription.UserId = userId;

            if (await _context.Subscriptions.AnyAsync(x => x.UserId == userId &&
                                                           x.P256Dh == subscription.P256Dh))
            {
                return subscription;
            }

            await _context.Subscriptions.AddAsync(
                new Subscription(userId, subscription.Endpoint, subscription.P256Dh, subscription.Auth)
                {
                    ExpirationTime = subscription.ExpirationTime
                });
            await _context.SaveChangesAsync();

            return subscription;
        }

        /// <inheritdoc />
        public async Task Unsubscribe(SubscriptionDto subscription, long userId)
        {
            var id = await _context.Subscriptions
                .Where(x => x.UserId == userId && x.P256Dh == subscription.P256Dh)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            if (id == 0)
            {
                return;
            }

            _context.Subscriptions.Remove(new Subscription
            {
                Id = id
            });
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task Send(long[] userIds, NotificationDto notification)
        {
            foreach (var subscription in await GetUserSubscriptions(userIds))
            {
                try
                {
                    await _client.SendNotificationAsync(subscription.ToWebPushSubscription(),
                        JsonConvert.SerializeObject(notification),
                        _vapidDetails);
                }
                catch (WebPushException e)
                {
                    if (e.Message != "Subscription no longer valid") continue;

                    _context.Subscriptions.RemoveRange(
                        _context.Subscriptions.Where(x => x.UserId == subscription.UserId &&
                                                          x.P256Dh == subscription.P256Dh));
                    await _context.SaveChangesAsync();
                }
            }
        }

        /// <inheritdoc />
        public async Task Send(long[] userIds, string text)
        {
            await Send(userIds, new NotificationDto(text));
        }

        /// <summary>
        /// Loads a list of user subscriptions from the database
        /// </summary>
        /// <param name="userIds">users id</param>
        /// <returns>List of subscriptions</returns>
        private async Task<SubscriptionDto[]> GetUserSubscriptions(long[] userIds)
        {
            return await _context.Subscriptions
                .Where(x => userIds.Contains(x.UserId))
                .Select(x => new SubscriptionDto
                {
                    UserId = x.UserId,
                    P256Dh = x.P256Dh,
                    ExpirationTime = x.ExpirationTime,
                    Auth = x.Auth,
                    Endpoint = x.Endpoint
                })
                .ToArrayAsync();
        }

        private readonly NotificationDbContext _context;
        private readonly WebPushClient _client;
        private readonly VapidDetails _vapidDetails;
    }
}
