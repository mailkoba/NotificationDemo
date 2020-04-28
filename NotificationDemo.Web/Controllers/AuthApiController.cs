using Microsoft.AspNetCore.Authorization;

namespace NotificationDemo.Web.Controllers
{
    [Authorize]
    public class AuthApiController : ApiController
    {
    }
}
