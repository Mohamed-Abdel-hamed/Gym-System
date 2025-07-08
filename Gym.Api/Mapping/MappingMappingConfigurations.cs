using Gym.Api.Contracts.Authentications;
using Gym.Api.Entities;
using Mapster;

namespace Gym.Api.Mapping;

public class MappingMappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest,ApplicationUser>()
            .Map(dest=>dest.UserName,src=>src.Email);
    }
}
