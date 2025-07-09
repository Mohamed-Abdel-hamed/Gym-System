using Gym.Api.Abstractions;
using Gym.Api.Contracts.Authentications;
using Gym.Api.Entities;
using Gym.Api.Errors;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Gym.Api.Services.Auth;

public class AuthService(UserManager<ApplicationUser> userManager,ILogger<AuthService> logger) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ILogger<AuthService> _logger = logger;

    public async Task<Result> RegisterAsync(RegisterRequest request,CancellationToken cancellation=default)
    {
        var emailIsExists = await _userManager.Users.AnyAsync(x=>x.Email==request.Email,cancellation);

        if (emailIsExists)
            return Result.Failure(UserErrors.DuplicatedEmail);

        var phoneIsExists = await _userManager.Users.AnyAsync(x => x.PhoneNumber == request.PhoneNumber, cancellation);

        if (phoneIsExists)
            return Result.Failure(UserErrors.DuplicatedPhoneNumber);

        ApplicationUser user=request.Adapt<ApplicationUser>();

       var result= await _userManager.CreateAsync(user,request.Password);
        if(result.Succeeded )
        {
            var token= await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token=WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            _logger.LogInformation("Confirmation Token,{token}",token);
            return Result.Success();
        }
        var error=result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description,StatusCodes.Status400BadRequest));
    }
    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        if(await _userManager.FindByIdAsync(request.UserId) is not { }user)
            return Result.Failure(UserErrors.InvalidCode);

        if(user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicatedConfirmation);

        var token = request.Token;

        try
        {
            token=Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        }
        catch (FormatException)
        {
            return Result.Failure(UserErrors.InvalidCode);
        }

        var result=await _userManager.ConfirmEmailAsync(user,token);

        if(result.Succeeded ) 
           return Result.Success();

        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
    public async Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Success();

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicatedConfirmation);

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        _logger.LogInformation("Confirmation Token,{token}", token);
        // send email

        return Result.Success();
    }
}
