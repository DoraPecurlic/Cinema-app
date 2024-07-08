using Cinema.Model;
using DTO.UserModel;

namespace Cinema.Repository.Common;

public interface IUserRepository
{
    Task<User> RegisterUserAsync(User user);
    Task<TokenData> GetUserByEmailAsync(string email);
    Task<Guid?> GetRoleIdByNameAsync(string roleName);
}