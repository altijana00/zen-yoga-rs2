using ZEN_Yoga.Models.Responses;
using ZEN_Yoga.Services.Interfaces.Base;

namespace ZEN_Yoga.Services.Interfaces.User
{
    public interface IGetUserService : IGetService<Models.User, UserResponse>
    {
        Task<UserResponse> GetByEmail(string email);

        Task<UserResponse> GetByEmailandPassword(string email, string password);
    }
}
