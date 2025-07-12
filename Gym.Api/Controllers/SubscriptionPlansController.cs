using Gym.Api.Abstractions;
using Gym.Api.Contracts.SubscriptionPlans;
using Gym.Api.Entities;
using Gym.Api.Services.SubscriptionPlans;
using Microsoft.AspNetCore.Mvc;

namespace Gym.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SubscriptionPlansController(ISubscriptionPlanService planService) : ControllerBase
{
    private readonly ISubscriptionPlanService _planService = planService;

    [HttpGet("")]
    public async Task<IActionResult> GetAll(CancellationToken cancellation)
    {
       var result= await _planService.GetAllAsync(cancellation);
        return result.IsSuccess? Ok(result.Value):result.ToProblem();

    }
    [HttpPost("")]
    public async Task<IActionResult> Add(SubscriptionPlanRequest request,CancellationToken cancellation)
    {
        var result = await _planService.AddAsync(request,cancellation);
 
        return result.IsSuccess ? Ok() : result.ToProblem();

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id,SubscriptionPlanRequest request, CancellationToken cancellation)
    {
        var result = await _planService.UpdateAsync(id,request, cancellation);

        return result.IsSuccess ? NoContent() : result.ToProblem();

    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellation)
    {
        var result = await _planService.DeleteAsync(id, cancellation);

        return result.IsSuccess ? NoContent() : result.ToProblem();

    }
}
