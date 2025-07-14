using Gym.Api.Abstractions;
using Gym.Api.Contracts.Classes;
using Gym.Api.Entities;
using Gym.Api.Errors;
using Gym.Api.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Gym.Api.Services.Classes;

public class ClassService(ApplicationDbContext context) : IClassService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result> AddAsync(int trainerId,ClassRequest request, CancellationToken cancellation = default)
    {
        var isExistsTrainer = await _context.Trainers.AnyAsync(x => x.Id == trainerId, cancellation);
        if (!isExistsTrainer)
            return Result.Failure(TrainerErrors.NotFound);

        var isExistsClass=await _context.Classes.AnyAsync(x=>x.Title == request.Title&&x.TrainerId == trainerId,cancellation);

        if (isExistsClass)
            return Result.Failure(ClassErrors.AlreadyExists);

        Class newClass = request.Adapt<Class>();
        newClass.TrainerId = trainerId;
        await _context.Classes.AddAsync(newClass,cancellation);
        await _context.SaveChangesAsync(cancellation);

        return Result.Success();

    }
}
