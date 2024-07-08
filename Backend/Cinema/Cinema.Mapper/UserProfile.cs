using AutoMapper;
using Cinema.Model;
using DTO.UserModel;

namespace Cinema.Mapper;

public class UserProfile: Profile
{
    public UserProfile()
    {
        CreateMap<RegisterPost, User>();
        CreateMap<LoginPost, UserLogin>();
        CreateMap<User, TokenData>();
    }

}