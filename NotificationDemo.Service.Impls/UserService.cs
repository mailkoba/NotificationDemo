using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NotificationDemo.DbContext;
using NotificationDemo.Service.Dto;

namespace NotificationDemo.Service.Impls
{
    public class UserService : IUserService
    {
        public UserService(NotificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserDto> Authenticate(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                throw new Exception(nameof(login));
            }

            return await _dbContext.Users
                .AsNoTracking()
                .Where(x => x.Login == login)
                .Select(x => new UserDto
                {
                    Id = x.Id,
                    Login = x.Login,
                    Name = x.Name
                })
                .SingleOrDefaultAsync();
        }

        public async Task<UserContextDto[]> GetLoginList()
        {
            return await _dbContext.Users.Select(x => new UserContextDto
            {
                Login = x.Login,
                Name = x.Name
            }).ToArrayAsync();
        }

        public async Task<UserSubscriptionDto[]> GetUserSubscriptionList()
        {
            return await _dbContext.Users.Select(x => new UserSubscriptionDto
                {
                    User = new UserDto
                    {
                        Id = x.Id,
                        Login = x.Login,
                        Name = x.Name
                    },
                    Subscriptions = x.Subscriptions.Select(s => new SubscriptionDto
                    {
                        UserId = s.UserId,
                        P256Dh = s.P256Dh,
                        Auth = s.Auth,
                        ExpirationTime = s.ExpirationTime,
                        Endpoint = s.Endpoint
                    }).ToArray()
                })
                .ToArrayAsync();
        }

        private readonly NotificationDbContext _dbContext;
    }
}
