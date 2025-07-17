using Gym.Api.Contracts.Authentications;
using Gym.Api.Contracts.Classes;
using Gym.Api.Contracts.Staffs;
using Gym.Api.Contracts.Trainers;
using Gym.Api.Entities;
using Mapster;

namespace Gym.Api.Mapping;

public class MappingMappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest,ApplicationUser>()
            .Map(dest=>dest.UserName,src=>src.Email);

        config.NewConfig<RegisterTrainerRequest, ApplicationUser>()
           .Map(dest => dest.UserName, src => src.Info.Email);

        config.NewConfig<RegisterStaffRequest, ApplicationUser>()
           .Map(dest => dest.UserName, src => src.Info.Email);


        config.NewConfig<ClassRequest, Class>()
       .Map(dest => dest.Duration, src => TimeSpan.FromMinutes(src.Duration))
       .Map(dest => dest.Capacity, src => src.Capacity ?? 20);
        config.NewConfig<Class, ClassResponse>()
            .Map(dest => dest.TrainerName, src => $"{src.Trainer.User.FirstName} {src.Trainer.User.LastName}");


        config.NewConfig<Trainer, TrainerResponse>()
            .Map(dest => dest.Classes, src => src.ClassesTaught);

    }
}
