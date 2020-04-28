using System.Threading.Tasks;
using NotificationDemo.Service.Dto;

namespace NotificationDemo.Service
{
    public interface IUserService
    {
        Task<UserDto> Authenticate(string login);

        Task<UserContextDto[]> GetLoginList();

        Task<UserSubscriptionDto[]> GetUserSubscriptionList();
    }
}
