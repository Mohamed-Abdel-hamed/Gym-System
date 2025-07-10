using Gym.Api.Abstractions;
using Gym.Api.Abstractions.Consts;
using Gym.Api.Authentications;
using Gym.Api.Contracts.Authentications;
using Gym.Api.Entities;
using Gym.Api.Errors;
using Gym.Api.Helper;
using Gym.Api.Persistence;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Gym.Api.Services.Auth;

public class AuthService(ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IJwtProvider jwtProvider
    ,ILogger<AuthService> logger,
    IEmailSender emailSender,
    IHttpContextAccessor httpContextAccessor) : IAuthService
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly ILogger<AuthService> _logger = logger;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellation = default)
    {
        if(await _userManager.FindByEmailAsync(email) is not { } user)
            return Result.Failure<AuthResponse>(UserErrors.UserNotFound);

        var result = await _signInManager.PasswordSignInAsync(user, password,false,true);

        if(result.Succeeded)
        {
            var(token,expiresIn)=_jwtProvider.GenerateToken(user);

            var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn);

            return Result.Success(response);
        }
        var error = result.IsNotAllowed
                    ? UserErrors.EmailNotConfirmed
                    : result.IsLockedOut
                    ? UserErrors.LockedUser
                    : UserErrors.InvalidCredentials;

        return Result.Failure<AuthResponse>(error);

    }
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

            await _userManager.AddToRoleAsync(user, AppRoles.Member);

            await SendConfirmationEmail(user, token);

            Member member = new()
            {
                UserId=user.Id
            };
            _context.Members.Add(member);
            _context.SaveChanges();
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

        await SendConfirmationEmail(user, token);

        return Result.Success();
    }
    private async Task SendConfirmationEmail(ApplicationUser user, string token)
    {
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
            templateModel: new Dictionary<string, string>
            {
                { "{{name}}", user.FirstName },
                    { "{{action_url}}", $"{origin}/auth/emailConfirmation?userId={user.Id}&code={token}" }
            }
        );
        await _emailSender.SendEmailAsync(user.Email!, "Gym System Confirmation Email", emailBody);
        await Task.CompletedTask;
    }
}
