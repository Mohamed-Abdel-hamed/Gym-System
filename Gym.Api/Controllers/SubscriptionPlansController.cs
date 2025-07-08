using Gym.Api.Services.SubscriptionPlans;
using Microsoft.AspNetCore.Mvc;

namespace Gym.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SubscriptionPlansController(ISubscriptionPlanService planService) : ControllerBase
{
    private readonly ISubscriptionPlanService _planService = planService;

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
       var result= await _planService.GetAllAsync();
        return result.IsSuccess? Ok(result.Value):
            Problem(statusCode:StatusCodes.Status404NotFound,title:result.Error.Code,detail:result.Error.Description);
    }
}
