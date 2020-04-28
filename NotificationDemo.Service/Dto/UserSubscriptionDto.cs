
namespace NotificationDemo.Service.Dto
{
    public class UserSubscriptionDto
    {
        public UserDto User { get; set; }

        public SubscriptionDto[] Subscriptions { get; set; }
    }
}
