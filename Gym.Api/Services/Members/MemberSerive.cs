using DocumentFormat.OpenXml.Spreadsheet;
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
            .ProjectToType<MemberResponse>()
            .SingleOrDefaultAsync(cancellation);

        if (member is null)
            return Result.Failure<MemberResponse>(MemberErros.NotFound);

        return Result.Success(member);
    }

    public async Task<Result<MemberResponse>> DetailsAsync(int id, CancellationToken cancellation = default)
    {
        var member = await _context.Members
             .AsNoTracking()
            .Where(m => m.Id == id)
            .ProjectToType<MemberResponse>()
            .SingleOrDefaultAsync(cancellation);

        if (member is null)
            return Result.Failure<MemberResponse>(MemberErros.NotFound);

        return Result.Success(member);
    }
}
