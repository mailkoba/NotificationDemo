using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NotificationDemo.Service.Dto
{
    /// <summary>
    ///     <see href="https://notifications.spec.whatwg.org/#dictdef-notificationoptions">Notification API Standard</see>
    /// </summary>
    public class NotificationDto
    {
        public NotificationDto() { }

        public NotificationDto(string text)
        {
            Body = text;
        }

        [JsonProperty("title")]
        public string Title { get; set; } = "Push Demo";

        [JsonProperty("lang")]
        public string Lang { get; set; } = "ru";

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("badge")]
        public string Badge { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        [JsonProperty("requireInteraction")]
        public bool RequireInteraction { get; set; }

        [JsonProperty("actions")]
        public List<NotificationActionDto> Actions { get; set; } = new List<NotificationActionDto>();
    }
}
