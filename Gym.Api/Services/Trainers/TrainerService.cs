using Gym.Api.Abstractions;
using Gym.Api.Contracts.Trainers;
using Gym.Api.Errors;
using Gym.Api.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Gym.Api.Services.Trainers;

public class TrainerService(ApplicationDbContext context) : ITrainerService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<TrainerResponse>> GetAsync(string userId, CancellationToken cancellation = default)
    {
       var trainer= await _context.Trainers
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Include(t=>t.User)
            .Include(t=>t.ClassesTaught)
            .ThenInclude(c=>c.Bookings)
            .ThenInclude(b=>b.Member)
            .ThenInclude(m=>m.User)
            .ProjectToType<TrainerResponse>()
            .SingleOrDefaultAsync(cancellation);

        if(trainer is null)
            return Result.Failure<TrainerResponse>(TrainerErrors.NotFound);

        return Result.Success(trainer);
    }
}
