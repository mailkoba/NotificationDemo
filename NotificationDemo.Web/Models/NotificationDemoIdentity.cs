using System;
using System.Security.Claims;
using NotificationDemo.Service;
using NotificationDemo.Service.Dto;

namespace NotificationDemo.Web.Models
{
    public class NotificationDemoIdentity : ClaimsIdentity
    {
        public NotificationDemoIdentity(UserDto userDto)
        {
            if (userDto == null)
            {
                throw new Exception(nameof(userDto));
            }

            if (string.IsNullOrWhiteSpace(userDto.Login))
            {
                throw new Exception(userDto.Login);
            }

            // ReSharper disable VirtualMemberCallInConstructor
            AddClaim(new Claim(IdentityClaims.Id, userDto.Id.ToString()));
            AddClaim(new Claim(IdentityClaims.Login, userDto.Login));
            AddClaim(new Claim(ClaimTypes.Name, userDto.Name));
            // ReSharper restore VirtualMemberCallInConstructor
        }

        public override string AuthenticationType => "NotificationDemo";

        public override bool IsAuthenticated => true;
    }
}
