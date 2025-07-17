using Gym.Api.Abstractions;
using Gym.Api.Abstractions.Consts;
using Gym.Api.Services.Trainers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gym.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles =AppRoles.Trainer)]
public class TrainersController(ITrainerService trainerService) : ControllerBase
{
    private readonly ITrainerService _trainerService = trainerService;
    [HttpGet("")]
    public async Task<IActionResult> Get(CancellationToken cancellation = default)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        var result = await _trainerService.GetAsync(userId,cancellation);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
