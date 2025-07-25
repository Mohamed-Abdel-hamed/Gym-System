﻿using Gym.Api.Abstractions;
using Gym.Api.Abstractions.Consts;
using Gym.Api.Contracts.Roles;
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
    [HttpPost("")]
    public async Task<IActionResult> Add(RoleRequest request)
    {
        var result = await _roleService.AddAsync(request);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPut("{roleId}")]
    public async Task<IActionResult> Update(string roleId,RoleRequest request)
    {
        var result = await _roleService.UpdateAsync(roleId,request);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}
