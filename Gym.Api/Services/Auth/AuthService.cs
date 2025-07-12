using Azure.Core;
using Gym.Api.Abstractions;
using Gym.Api.Abstractions.Consts;
using Gym.Api.Authentications;
using Gym.Api.Contracts.Authentications;
using Gym.Api.Contracts.Staffs;
using Gym.Api.Contracts.Trainers;
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
    , ILogger<AuthService> logger,
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
        if (await _userManager.FindByEmailAsync(email) is not { } user)
            return Result.Failure<AuthResponse>(UserErrors.UserNotFound);

        var result = await _signInManager.PasswordSignInAsync(user, password, false, true);

        if (result.Succeeded)
        {
            var (token, expiresIn) = await _jwtProvider.GenerateToken(user);

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
    public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellation = default)
    {
        var checkUserUniqueness= await CheckUserUniquenessAsync(request.Email, request.PhoneNumber,cancellation);

        if (!checkUserUniqueness.IsSuccess)

            return checkUserUniqueness;

        ApplicationUser user = request.Adapt<ApplicationUser>();

        var result = await CreateUserAsync(user, request.Password, AppRoles.Member, cancellation);

        if (!result.IsSuccess)
            return result;

        Member member = new()
            {
                UserId = user.Id
            };

            _context.Members.Add(member);
            _context.SaveChanges();

        return Result.Success();
    }
    public async Task<Result> RegisterTrainerAsync(RegisterTrainerRequest request, CancellationToken cancellation = default)
    {
        var checkUserUniqueness = await CheckUserUniquenessAsync(request.Info.Email, request.Info.PhoneNumber, cancellation);

        if (!checkUserUniqueness.IsSuccess)

            return checkUserUniqueness;

        ApplicationUser user = request.Info.Adapt<ApplicationUser>();

        var result = await CreateUserAsync(user, request.Info.Password, AppRoles.Trainer, cancellation);

        if (!result.IsSuccess)
            return result;

        Trainer trainer = new()
        {
            HireDate = request.HireDate,
            UserId = user.Id
        };

        _context.Trainers.Add(trainer);
        _context.SaveChanges();
        return Result.Success();
    }

    public async Task<Result> RegisterStaffAsync(RegisterStaffRequest request, CancellationToken cancellation = default)
    {
        var checkUserUniqueness = await CheckUserUniquenessAsync(request.Info.Email, request.Info.PhoneNumber, cancellation);

        if (!checkUserUniqueness.IsSuccess)

            return checkUserUniqueness;

        ApplicationUser user = request.Info.Adapt<ApplicationUser>();

        var result = await CreateUserAsync(user, request.Info.Password, AppRoles.Staff, cancellation);

        if (!result.IsSuccess)
            return result;

       Staff staff = new()
        {
            HireDate = request.HireDate,
            UserId = user.Id
        };

        _context.Staffs.Add(staff);
        _context.SaveChanges();
        return Result.Success();
    }
    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
            return Result.Failure(UserErrors.InvalidCode);

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicatedConfirmation);

        var token = request.Token;

        try
        {
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        }
        catch (FormatException)
        {
            return Result.Failure(UserErrors.InvalidCode);
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (result.Succeeded)
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


    private async Task<Result> CheckUserUniquenessAsync(string email, string phoneNumber, CancellationToken cancellation)
    {
        if (await _userManager.Users.AnyAsync(x => x.Email == email, cancellation))
            return Result.Failure(UserErrors.DuplicatedEmail);

        if (await _userManager.Users.AnyAsync(x => x.PhoneNumber == phoneNumber, cancellation))
            return Result.Failure(UserErrors.DuplicatedPhoneNumber);

        return Result.Success();
    }
    private async Task<Result> CreateUserAsync(ApplicationUser user, string password, string role, CancellationToken cancellation)
    {
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        _logger.LogInformation("Confirmation Token: {token}", token);

        await _userManager.AddToRoleAsync(user, role);
        await SendConfirmationEmail(user, token);

        return Result.Success();
    }
}