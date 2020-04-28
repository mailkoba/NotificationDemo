using System.Security.Claims;

namespace NotificationDemo.Web.Models
{
    public class NotificationDemoPrincipal : ClaimsPrincipal
    {
        public NotificationDemoPrincipal(NotificationDemoIdentity identity) : base(identity)
        {
        }
    }
}
