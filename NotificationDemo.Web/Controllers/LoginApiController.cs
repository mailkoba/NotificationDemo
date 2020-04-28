using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationDemo.Service;
using NotificationDemo.Web.Models;

namespace NotificationDemo.Web.Controllers
{
    public class LoginApiController : AuthApiController
    {
        public LoginApiController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginInputModel loginInputModel)
        {
            if (loginInputModel == null ||
                string.IsNullOrWhiteSpace(loginInputModel.Username))
            {
                return UnauthorizedResult("Неверные учетная запись и/или пароль.");
            }

            var user = await _userService.Authenticate(loginInputModel.Username);

            if (user != null)
            {
                var principal = new NotificationDemoPrincipal(new NotificationDemoIdentity(user));

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
                        IsPersistent = true
                    });

                return Ok();
            }

            return UnauthorizedResult("Неверные учетная запись и/или пароль.");
        }

        [HttpPost("[action]")]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private readonly IUserService _userService;
    }
}
