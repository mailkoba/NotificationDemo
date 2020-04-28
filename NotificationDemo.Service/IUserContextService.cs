using NotificationDemo.Service.Dto;

namespace NotificationDemo.Service
{
    public interface IUserContextService
    {
        UserDto GetCurrentUser();
    }
}
