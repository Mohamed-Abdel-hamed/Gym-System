using Gym.Api.Abstractions;
using Gym.Api.Abstractions.Consts;
using Gym.Api.Services.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles =AppRoles.Admin)]  
public class RolesController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;
    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var result=await _roleService.GetAllAsync();
        
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("{roleId}")]
    public async Task<IActionResult> Get(string roleId)
    {
        var result = await _roleService.GetAsync(roleId);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
