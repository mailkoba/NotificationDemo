using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NotificationDemo.Common;
using NotificationDemo.Service;
using NotificationDemo.Service.Dto;
using NotificationDemo.Web.Attributes;

namespace NotificationDemo.Web.Controllers
{
    public class HomeApiController : AuthApiController
    {
        [HttpPost("[action]")]
        [NoCache]
        public IActionResult CheckAuthorization()
        {
            return Ok();
        }

        [HttpPost("[action]")]
        public ActionResult<UserContextDto> GetUserContext()
        {
            var user = Request.HttpContext
                .RequestServices
                .GetRequiredService<IUserContextService>()
                .GetCurrentUser();

            if (user == null)
            {
                throw new NotificationDemoException("Пользователь не авторизован");
            }

            return new UserContextDto
            {
                Login = user.Login,
                Name = user.Name
            };
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<UserContextDto[]> GetLoginList()
        {
            return await Request.HttpContext
                .RequestServices
                .GetRequiredService<IUserService>()
                .GetLoginList();
        }
    }
}
