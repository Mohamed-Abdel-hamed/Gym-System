using DocumentFormat.OpenXml.Spreadsheet;
using Gym.Api.Abstractions;
using Gym.Api.Contracts.Members;
using Gym.Api.Errors;
using Gym.Api.Persistence;
using Mapster;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace Gym.Api.Services.Members;

public class MemberSerive(ApplicationDbContext context, IDataProtectionProvider dataProtector) : IMemberSerive
{
    private readonly ApplicationDbContext _context = context;
    private readonly IDataProtector _protector = dataProtector.CreateProtector("_key");
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

   public async Task<Result<string>> SearchAsync(string name, CancellationToken cancellation = default)
    {
        var member= await _context.Members
            .Include(m=>m.User)
            .SingleOrDefaultAsync(m=>m.User.FirstName.ToLower().Contains(name.ToLower()),cancellation);

        if (member is null)
            return Result.Failure<string>(MemberErros.NotFound);

        var key = _protector.Protect(member.Id.ToString());

        return Result.Success(key);
    }

    public async Task<Result<MemberResponse>> DetailsAsync(string key, CancellationToken cancellation = default)
    {
        var id= int.Parse(_protector.Unprotect(key));
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
