using Newtonsoft.Json;

namespace NotificationDemo.Service.Dto
{
    /// <summary>
    ///     <see href="https://notifications.spec.whatwg.org/#dictdef-notificationaction">Notification API Standard</see>
    /// </summary>
    public class NotificationActionDto
    {

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
