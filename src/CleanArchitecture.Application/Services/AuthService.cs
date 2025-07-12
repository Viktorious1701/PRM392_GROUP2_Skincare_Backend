using CleanArchitecture.Application.DTOs.Auth;
using CleanArchitecture.Application.DTOs.Email;
using CleanArchitecture.Application.Enums;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CleanArchitecture.Application.Services;

public class AuthService : IAuthService
{
  private readonly UserManager<User> _userManager;
  private readonly IEmailService _emailService;
  private readonly IHttpClientFactory _httpClientFactory;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IValidator<LoginRequest> _loginValidator;
  private readonly IValidator<RegisterRequest> _registerValidator;
  private readonly IValidator<ForgotPasswordRequest> _forgotPasswordValidator;
  private readonly IValidator<ResetPasswordRequest> _resetPasswordValidator;
  private readonly IValidator<DTOs.Auth.RefreshTokenRequest> _refreshTokenValidator;
  private readonly ILogger<AuthService> _logger;
  private readonly IConfiguration _configuration;

  public AuthService(UserManager<User> userManager,
                     IEmailService emailService,
                     IHttpClientFactory httpClientFactory,
                     IValidator<LoginRequest> loginValidator,
                     IValidator<RegisterRequest> registerValidator,
                     IValidator<ForgotPasswordRequest> forgotPasswordValidator,
                     IValidator<ResetPasswordRequest> resetPasswordValidator,
                     IValidator<DTOs.Auth.RefreshTokenRequest> refreshTokenValidator,
                     IHttpContextAccessor httpContextAccessor,
                     ILogger<AuthService> logger,
                     IConfiguration configuration
                     )
  {
    _userManager = userManager;
    _emailService = emailService;
    _httpClientFactory = httpClientFactory;
    _loginValidator = loginValidator;
    _registerValidator = registerValidator;
    _forgotPasswordValidator = forgotPasswordValidator;
    _resetPasswordValidator = resetPasswordValidator;
    _refreshTokenValidator = refreshTokenValidator;
    _httpContextAccessor = httpContextAccessor;
    _logger = logger;
    _configuration = configuration;
  }

  private string GetAuthority()
  {
    var request = _httpContextAccessor.HttpContext?.Request;
    if (request == null)
    {
      throw new InvalidOperationException("HttpContext is not available.");
    }
    return $"{request.Scheme}://{request.Host}";
  }

  public async Task<Result<string>> ForgotPasswordAsync(ForgotPasswordRequest request)
  {
    var validationResult = await _forgotPasswordValidator.ValidateAsync(request);
    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors
          .Select(e => new Error("ValidationError", e.ErrorMessage))
          .ToList();

      return Result<string>.Failure(errors, StatusCodes.Status400BadRequest);
    }
    var user = await _userManager.FindByEmailAsync(request.Email!);
    if (user == null)
    {
      return Result<string>.Failure([AuthErrors.UserNotFound], StatusCodes.Status404NotFound);
    }

    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

    var domain = "https://web.pak160404.click/reset-password";
    var queryString = $"?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email!)}";
    var forgotPasswordLink = $"{domain}{queryString}";
    var htmlMail = $@"<!DOCTYPE html>
      <html lang=""en"">
        <head>
          <meta charset=""UTF-8"" />
          <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
          <title>Password Reset Request</title>
          <style>
            .overlay {{
              background-color: rgba(255, 255, 255, 0.7); /* White with 70% opacity */
              position: absolute;
              top: 0;
              left: 0;
              right: 0;
              bottom: 0;
              z-index: 1;
            }}
            .content-container {{
              position: relative;
              z-index: 2;
            }}
          </style>
        </head>
        <body
          style=""margin: 0; font-family: Arial, sans-serif; background-color: #f9f9f9""
        >
          <div
            style=""
              max-width: 800px;
              margin: auto;
              background-color: white;
              padding: 20px;
            ""
          >
            <div style=""text-align: center; padding: 20px; background-color: white"">
              <div
                style=""
                  display: flex;
                  justify-content: center;
                  align-items: center;
                  text-align: center;
                  height: 80px;
                ""
              >
                <span style=""font-size: 3.5rem; font-weight: 600; color: #3a4d39""
                  >De Fleur</span
                >
              </div>
            </div>
            <div class=""overlay""></div>
            <div class=""content-container"" style=""padding: 20px"">
              <h2 style=""font-size: 2rem; font-weight: bold"">
                Password Reset Request
              </h2>
              <p style=""font-size: 1rem"">Dear User,</p>
              <p style=""font-size: 1rem"">
                You requested to reset your password. Please click the button below to
                reset your password:
              </p>
              <p style=""font-size: 1rem"">
                To ensure the security of your account, please note the following:
              </p>
              <ul style=""font-size: 1rem; line-height: 1.5"">
                <li>The reset link is valid for only 24 hours.</li>
                <li>
                  If you did not request a password reset, please ignore this email or
                  contact our support team.
                </li>
                <li>Do not share this email or the reset link with anyone.</li>
              </ul>
              <div style=""text-align: center; margin: 20px 0"">
                <a
                  href='{forgotPasswordLink}'
                  style=""
                    display: inline-block;
                    background-color: #789678;
                    color: white;
                    padding: 10px 20px;
                    text-decoration: none;
                    border-radius: 4px;
                  ""
                  >Reset Password</a
                >
              </div>
              <p style=""font-size: 1rem"">
                Thank you for using our services. We value your security and are here
                to help if you need any assistance.
              </p>
              <p style=""font-size: 1rem"">
                Best regards,<br />Your Company Support Team
              </p>
            </div>
          </div>
        </body>
      </html>
      ";
    var message = new EmailMessage([user.Email!], "Reset password link", htmlMail);
    await _emailService.SendEmailAsync(message);

    return Result<string>.Success(token, StatusCodes.Status200OK);
  }

  public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request)
  {
    var validationResult = await _loginValidator.ValidateAsync(request);
    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors
          .Select(e => new Error("ValidationError", e.ErrorMessage))
          .ToList();
      return Result<AuthResponse>.Failure(errors, StatusCodes.Status400BadRequest);
    }

    var user = await _userManager.FindByNameAsync(request.UserName!);
    if (user == null)
    {
      return Result<AuthResponse>.Failure([AuthErrors.InvalidCredentials], StatusCodes.Status400BadRequest);
    }

    var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password!);
    if (!passwordValid)
    {
      await _userManager.AccessFailedAsync(user);
      return Result<AuthResponse>.Failure([AuthErrors.InvalidCredentials], StatusCodes.Status400BadRequest);
    }

    if (await _userManager.IsLockedOutAsync(user))
    {
      return Result<AuthResponse>.Failure([AuthErrors.UserNotFound], StatusCodes.Status423Locked);
    }

    await _userManager.ResetAccessFailedCountAsync(user);

    var claims = await GetClaims(user);
    var accessToken = GenerateAccessToken(claims);
    var refreshToken = GenerateRefreshToken();

    user.RefreshToken = refreshToken;
    user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["JwtSettings:RefreshTokenExpirationDays"]));
    await _userManager.UpdateAsync(user);

    return Result<AuthResponse>.Success(new AuthResponse
    {
      UserName = user.UserName,
      Email = user.Email,
      AccessToken = accessToken,
      AccessTokenExpiration = Convert.ToInt32(_configuration["JwtSettings:AccessTokenExpirationMinutes"]) * 60,
      RefreshToken = refreshToken,
      RefreshTokenExpiration = Convert.ToInt32(_configuration["JwtSettings:RefreshTokenExpirationDays"]) * 24 * 60 * 60
    }, StatusCodes.Status200OK);
  }

  private async Task<List<Claim>> GetClaims(User user)
  {
    var claims = new List<Claim>
      {
          new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
          new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
          new Claim(JwtRegisteredClaimNames.Email, user.Email!),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
      };

    var roles = await _userManager.GetRolesAsync(user);
    claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
    return claims;
  }

  private string GenerateAccessToken(IEnumerable<Claim> claims)
  {
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:AccessTokenExpirationMinutes"]));

    var token = new JwtSecurityToken(
        issuer: _configuration["JwtSettings:Issuer"],
        audience: _configuration["JwtSettings:Audience"],
        claims: claims,
        expires: expires,
        signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  private string GenerateRefreshToken()
  {
    var randomNumber = new byte[32];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);
    return Convert.ToBase64String(randomNumber);
  }

  public async Task<Result<AuthResponse>> RefreshTokenAsync(DTOs.Auth.RefreshTokenRequest request)
  {
    var validationResult = await _refreshTokenValidator.ValidateAsync(request);
    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors
          .Select(e => new Error("ValidationError", e.ErrorMessage))
          .ToList();

      return Result<AuthResponse>.Failure(errors, StatusCodes.Status400BadRequest);
    }

    var authorizedUser = _httpContextAccessor.HttpContext?.User;
    if (authorizedUser == null || !authorizedUser.Identity!.IsAuthenticated)
    {
      return Result<AuthResponse>.Failure([UserErrors.UnauthorizedUser], StatusCodes.Status401Unauthorized);
    }
    var id = authorizedUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;
    var user = await _userManager.FindByIdAsync(id);

    var client = _httpClientFactory.CreateClient();
    var authority = GetAuthority();
    var disco = await client.GetDiscoveryDocumentAsync(authority);
    if (disco.IsError)
    {
      _logger.LogError("Discovery document error: {Error}", disco.Error);
      return Result<AuthResponse>.Failure([AuthErrors.IdentityServerFailed], StatusCodes.Status500InternalServerError);
    }

    var tokenResponse = await client.RequestRefreshTokenAsync(new IdentityModel.Client.RefreshTokenRequest
    {
      Address = disco.TokenEndpoint,
      GrantType = "refresh_token",

      ClientId = "api_client",
      ClientSecret = "secret",

      RefreshToken = request.RefreshToken!,

      //Scope = "openid profile email roles API offline_access"
    });

    if (tokenResponse.IsError)
      return Result<AuthResponse>.Failure([AuthErrors.TokenResponseError(tokenResponse.ErrorDescription!)], StatusCodes.Status400BadRequest);

    user!.RefreshToken = tokenResponse.RefreshToken;
    user.RefreshTokenExpiration = DateTime.UtcNow + TimeSpan.FromDays(30);
    await _userManager.UpdateAsync(user);

    return Result<AuthResponse>.Success(new AuthResponse
    {
      AccessToken = tokenResponse.AccessToken!,
      AccessTokenExpiration = tokenResponse.ExpiresIn,
      RefreshToken = tokenResponse.RefreshToken!,
      RefreshTokenExpiration = 2592000,
      Email = user.Email!,
      UserName = user.UserName!,
    }, StatusCodes.Status200OK);
  }

  public async Task<Result<string>> RegisterAsync(RegisterRequest request)
  {
    if (await _userManager.FindByEmailAsync(request.Email!) != null)
    {
      return Result<string>.Failure([AuthErrors.AlreadyRegistered], StatusCodes.Status409Conflict);
    }
    if (await _userManager.FindByNameAsync(request.UserName!) != null)
    {
      return Result<string>.Failure([AuthErrors.DuplicateUserName], StatusCodes.Status409Conflict);
    }
    var validationResult = await _registerValidator.ValidateAsync(request);
    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors
          .Select(e => new Error("ValidationError", e.ErrorMessage))
          .ToList();

      return Result<string>.Failure(errors, StatusCodes.Status400BadRequest);
    }

    var user = new User
    {
      UserName = request.UserName,
      Email = request.Email,
      Gender = request.Gender,
      FirstName = request.FirstName,
      LastName = request.LastName,
      PhoneNumber = request.PhoneNumber
    };

    var result = await _userManager.CreateAsync(user, request.Password!);
    if (!result.Succeeded)
    {
      var errors = result.Errors.Select(e => new Error(e.Code, e.Description)).ToList();
      return Result<string>.Failure(errors, StatusCodes.Status400BadRequest);
    }
    var roleResult = await _userManager.AddToRolesAsync(user, [Roles.Customer]);
    if (!roleResult.Succeeded)
    {
      var errors = roleResult.Errors.Select(e => new Error(e.Code, e.Description)).ToList();
      return Result<string>.Failure(errors, StatusCodes.Status500InternalServerError);
    }

    return Result<string>.Success(default!, StatusCodes.Status200OK);
  }

  public async Task<Result<string>> ResetPasswordAsync(ResetPasswordRequest request)
  {
    var validationResult = await _resetPasswordValidator.ValidateAsync(request);
    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors
          .Select(e => new Error("ValidationError", e.ErrorMessage))
          .ToList();

      return Result<string>.Failure(errors, StatusCodes.Status400BadRequest);
    }
    var user = await _userManager.FindByEmailAsync(request.Email!);
    if (user == null)
    {
      return Result<string>.Failure([AuthErrors.UserNotFound], StatusCodes.Status404NotFound);
    }

    var result = await _userManager.ResetPasswordAsync(user, request.AccessToken!, request.Password!);
    if (!result.Succeeded)
    {
      var errors = result.Errors.Select(e => new Error(e.Code, e.Description)).ToList();
      return Result<string>.Failure(errors, StatusCodes.Status500InternalServerError);
    }

    return Result<string>.Success(default!, StatusCodes.Status200OK);
  }
}