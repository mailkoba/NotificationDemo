using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotificationDemo.Service;
using NotificationDemo.Service.Dto;

namespace NotificationDemo.Web.Controllers
{
    public class UserApiController : AuthApiController
    {
        public UserApiController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("[action]")]
        public async Task<UserSubscriptionDto[]> GetUserSubscriptionList()
        {
            return await _userService.GetUserSubscriptionList();
        }

        private readonly IUserService _userService;
    }
}
