using Gym.Api.Abstractions;
using Gym.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gym.Api.Services.MembershipFreezes;

public class MembershipFreezeService(ApplicationDbContext context) : IMembershipFreezeService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result> AddAsync(int memberId, int membershipId,CancellationToken cancellation=default)
    {
         var isExistsMembership=await _context.Memberships.SingleOrDefaultAsync()
    }
}
