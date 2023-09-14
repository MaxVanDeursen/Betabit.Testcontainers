using AutoMapper;
using Betatalks.Testcontainers.Api.TransferObjects;
using Betatalks.Testcontainers.Core.Entities;

namespace Betatalks.Testcontainers.Api.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
    }
}
