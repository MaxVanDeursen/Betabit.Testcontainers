using AutoMapper;
using Betatalks.Testcontainers.Api.TransferObjects;
using Betatalks.Testcontainers.Core.Requests;

namespace Betatalks.Testcontainers.Api.Profiles;

public class UserUpsertProfile : Profile
{
    public UserUpsertProfile()
    {
        CreateMap<UserUpsertDto, UpsertUserRequest>();
    }
}
