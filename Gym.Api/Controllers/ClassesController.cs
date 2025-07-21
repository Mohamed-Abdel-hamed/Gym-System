using Gym.Api.Abstractions;
using Gym.Api.Abstractions.Consts;
using Gym.Api.Contracts.Classes;
using Gym.Api.Services.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Gym.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles =AppRoles.Staff)]
public class ClassesController(IClassService classService) : ControllerBase
{
    private readonly IClassService _classService = classService;

    [HttpPost("trainer/{trainerId}")]
    [EnableRateLimiting("token")]
    public async Task<IActionResult> Add([FromRoute]int trainerId,[FromBody] ClassRequest request,CancellationToken cancellation=default)
    {
        var result = await _classService.AddAsync(trainerId, request, cancellation);

        return result.IsSuccess ? Ok() : result.ToProblem();

    }
}
