using Cinema.Model;

namespace Cinema.Service.Common;

public interface IUserService
{
    Task<User> RegisterUserAsync(User user);
    Task<string> LoginUserAsync(UserLogin userLogin);
}