using Gym.Api.Abstractions;
using Gym.Api.Contracts.Members;
using Gym.Api.Errors;
using Gym.Api.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Gym.Api.Services.Members;

public class MemberSerive(ApplicationDbContext context) : IMemberSerive
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<MemberResponse>> GetAsync(string userId, CancellationToken cancellation = default)
    {
       var member=await _context.Members
            .AsNoTracking()
            .Where(m=>m.UserId == userId)
            .Include(m=>m.User)
            .Include(m=>m.Bookings)
            .ThenInclude(m=>m.Class)
            .ThenInclude(m=>m.Trainer)
            .ProjectToType<MemberResponse>()
            .SingleOrDefaultAsync(cancellation);

        if (member is null)
            return Result.Failure<MemberResponse>(MemberErros.NotFound);

        return Result.Success(member);
    }
}
