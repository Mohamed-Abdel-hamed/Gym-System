using Gym.Api.Abstractions;
using Gym.Api.Abstractions.Consts;
using Gym.Api.Services.Bookings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gym.Api.Controllers;
[Route("api/class/{classId}/[controller]")]
[ApiController]
[Authorize(Roles =AppRoles.Member)]
public class BookingsController(IBookingService bookingService) : ControllerBase
{
    private readonly IBookingService _bookingService = bookingService;
    [HttpPost("")]
    public async Task<IActionResult> Book([FromRoute]int classId,CancellationToken cancellation=default)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        var result = await _bookingService.Book(userId,classId, cancellation);

        return result.IsSuccess ? Ok(result) : result.ToProblem();
    }
}
