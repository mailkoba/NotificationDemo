using NotificationDemo.Service.Dto;
using WebPush;

namespace NotificationDemo.Service.Impls
{
    public static class SubscriptionDtoExtensions
    {
        public static PushSubscription ToWebPushSubscription(this SubscriptionDto dto)
        {
            return new PushSubscription(dto.Endpoint, dto.P256Dh, dto.Auth);
        }
    }
}
