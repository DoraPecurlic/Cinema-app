using AutoMapper;
using Cinema.Model;
using Cinema.Repository.Common;
using DTO.UserModel;
using Npgsql;

namespace Cinema.Repository;

public class UserRepository: IUserRepository
{
    private readonly string _connectionString;
    private readonly IMapper _mapper;

    public UserRepository(string connectionString, IMapper mapper)
    {
        _connectionString = connectionString;
        _mapper = mapper;

    }
    
    public async Task<User> RegisterUserAsync(User user)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var commandText = @"
                INSERT INTO ""User"" 
                (""Id"", ""Email"", ""Password"", ""FirstName"", ""LastName"", ""RoleId"", ""IsActive"", ""DateCreated"", ""DateUpdated"") 
                VALUES 
                (@Id, @Email, @Password, @FirstName, @LastName, @RoleId, @IsActive, @DateCreated, @DateUpdated) 
                RETURNING *;";

            await using var command = new NpgsqlCommand(commandText, connection);
            command.Parameters.AddWithValue("@Id", user.Id);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Password", user.Password);
            command.Parameters.AddWithValue("@FirstName", user.FirstName);
            command.Parameters.AddWithValue("@LastName", user.LastName);
            command.Parameters.AddWithValue("@RoleId", user.RoleId);
            command.Parameters.AddWithValue("@IsActive", user.IsActive);
            command.Parameters.AddWithValue("@DateCreated", user.DateCreated);
            command.Parameters.AddWithValue("@DateUpdated", user.DateUpdated);

            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                user.Id = reader.GetGuid(reader.GetOrdinal("Id"));
                user.Email = reader.GetString(reader.GetOrdinal("Email"));
                user.Password = reader.GetString(reader.GetOrdinal("Password"));
                user.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                user.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                user.RoleId = reader.GetGuid(reader.GetOrdinal("RoleId"));
                user.IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                user.DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated"));
                user.DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated"));
            }

            return user;
        
    }

   
    public async Task<Guid?> GetRoleIdByNameAsync(string roleName)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var query = @"
            SELECT ""Id""
            FROM ""Role""
            WHERE ""RoleName"" = @RoleName;";

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@RoleName", roleName);

        var roleId = await command.ExecuteScalarAsync() as Guid?;
        return roleId;
    }
    
    public async Task<TokenData> GetUserByEmailAsync(string email)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var query = @"
            SELECT u.*, r.""RoleName""
            FROM ""User"" u
            LEFT JOIN ""Role"" r ON u.""RoleId"" = r.""Id""
            WHERE u.""Email"" = @Email;";

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@Email", email);

        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            var user =  new User
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Password = reader.GetString(reader.GetOrdinal("Password")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                RoleId = reader.GetGuid(reader.GetOrdinal("RoleId")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated"))
            };
            var tokenData = _mapper.Map<TokenData>(user);
            tokenData.Role = reader.GetString(reader.GetOrdinal("RoleName")); 
            return tokenData;
        }

        return null; 
    }
    
    
    
    
    
}