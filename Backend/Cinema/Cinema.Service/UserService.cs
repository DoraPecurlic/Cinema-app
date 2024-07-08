using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cinema.Model;
using Cinema.Repository.Common;
using Cinema.Service.Common;
using DTO.UserModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

namespace Cinema.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository; 
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<User> RegisterUserAsync(User user)
        {
            user.Id = Guid.NewGuid();
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.IsActive = true;
            user.DateCreated = DateTime.UtcNow;
            user.DateUpdated = user.DateCreated;
            var roleName = "User";
            user.RoleId = (Guid)await _userRepository.GetRoleIdByNameAsync(roleName);
            
            return await _userRepository.RegisterUserAsync(user);
        }

        public async Task<string> LoginUserAsync(UserLogin userLogin)
        {
            var tokenData = await _userRepository.GetUserByEmailAsync(userLogin.Email);

            if (tokenData == null)
            {
                throw new Exception("User not found.");
            }
            
            if (!BCrypt.Net.BCrypt.Verify(userLogin.Password, tokenData.Password))
            {
                throw new Exception("Wrong password.");
            }

            string token = CreateToken(tokenData);
            return token;
        }

        private string CreateToken(TokenData user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim("Name", user.Email),
                new Claim("Email", user.Email),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("Role", user.Role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));
            
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
            
    }
}