
namespace NotificationDemo.Service.Dto
{
    public class SubscriptionDto
    {
        public long UserId { get; set; }

        public string Endpoint { get; set; }

        public double? ExpirationTime { get; set; }

        public string P256Dh { get; set; }

        public string Auth { get; set; }
    }
}
