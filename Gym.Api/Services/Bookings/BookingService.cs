using FluentValidation.Validators;
using Gym.Api.Abstractions;
using Gym.Api.Abstractions.Consts;
using Gym.Api.Contracts.Classes;
using Gym.Api.Entities;
using Gym.Api.Errors;
using Gym.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gym.Api.Services.Bookings;

public class BookingService(ApplicationDbContext context) : IBookingService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result> Book(string userId, int classId, CancellationToken cancellation = default)
    {
        var member = await _context.Members.SingleOrDefaultAsync(x => x.UserId == userId, cancellation);

        if (member is null)

            return Result.Failure(UserErrors.UserNotFound);

        var targetClass = await _context.Classes

            .SingleOrDefaultAsync(x=>x.Id==classId, cancellation);

        if (targetClass is null)

            return Result.Failure(ClassErrors.NotFound);

        if (targetClass.StartDate <= DateTime.Now)
            return Result.Failure(ClassErrors.AlreadyStarted);


        var isExistsBooking= await _context.Bookings.AnyAsync(x=>x.MemberId==member.Id&&x.ClassId==classId, cancellation);

        if(isExistsBooking)

            return Result.Failure(BookingErrors.AlreadyExists);

        var bookingsCount = await _context.Bookings.CountAsync(x => x.ClassId == classId, cancellation);


        if (bookingsCount>= targetClass.Capacity)

            return Result.Failure(ClassErrors.CompleteClass);

        Booking booking = new()
        {
            MemberId = member.Id,
            ClassId = classId,
            Status= BookingStatus.Confirmed
        };

        await _context.Bookings.AddAsync(booking,cancellation);

        await _context.SaveChangesAsync(cancellation);
        return Result.Success();
    }

    public async Task<IEnumerable<DashboardClassBookingResponse>> GetClassesWithHighestNumberOfBookings()
    {
        var startDate = DateTime.Today.AddDays(-29);

        var totalDays = (DateTime.Today - startDate).TotalDays;

        var classes = await _context.Bookings

            .AsNoTracking()

            .Where(b => b.CreatedDate >= startDate && b.CreatedDate <= DateTime.Today)

            .GroupBy(b => new { b.ClassId })

            .Select(g => new
            {
                g.Key.ClassId,
                BookingsAverage = Math.Round(g.Count() / totalDays,2)
            })

             .OrderByDescending(g => g.BookingsAverage) 

            .Take(5)

            .Join(_context.Classes,

            b => b.ClassId,

            c => c.Id,

            (b, c) => new DashboardClassBookingResponse(
                c.Id,
                c.Title,
                c.StartDate,
                c.Capacity,
                b.BookingsAverage
            ))
            .ToListAsync();

        return classes;
    }

}
