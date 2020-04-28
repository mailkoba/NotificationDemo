using Microsoft.AspNetCore.Http;
using NotificationDemo.Service.Dto;
using System.Linq;
using System.Security.Claims;

namespace NotificationDemo.Service.Impls
{
    public class UserContextService : IUserContextService
    {
        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public UserDto GetCurrentUser()
        {
            var user = _httpContextAccessor?.HttpContext?.User;

            if (user == null ||
                !user.HasClaim(x => x.Type == IdentityClaims.Id) ||
                !user.HasClaim(x => x.Type == IdentityClaims.Login) ||
                !user.HasClaim(x => x.Type == ClaimTypes.Name))
            {
                return null;
            }

            try
            {
                return new UserDto
                {
                    Id = long.Parse(user.Claims.First(x => x.Type == IdentityClaims.Id).Value),
                    Login = user.Claims.First(x => x.Type == IdentityClaims.Login).Value,
                    Name = user.Claims.First(x => x.Type == ClaimTypes.Name).Value
                };
            }
            catch
            {
                return null;
            }
        }

        private readonly IHttpContextAccessor _httpContextAccessor;
    }
}
