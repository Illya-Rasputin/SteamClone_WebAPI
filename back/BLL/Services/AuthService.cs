using BLL.DTO.Auth;
using BLL.Settings;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AuthService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly JwtService _jwtService;
        private readonly EmailService _emailService;
        private readonly EmailSettings _emailSettings;

        public AuthService(
            UserManager<UserEntity> userManager,
            JwtService jwtService,
            EmailService emailService,
            IOptions<EmailSettings> emailOptions)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _emailService = emailService;
            _emailSettings = emailOptions.Value;
        }

        public async Task<ServiceResponse> LoginAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);

            if (user == null)
            {
                return ServiceResponse.Error($"Користувача з іменем '{dto.UserName}' не існує");
            }

            bool passwordResult = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!passwordResult)
            {
                return ServiceResponse.Error("Невірний пароль");
            }

            if (!user.EmailConfirmed)
            {
                return ServiceResponse.Error("Please confirm your email before signing in.");
            }

            // Jwt token
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GetAcessToken(user, roles);

            return ServiceResponse.Success("Успішний вхід", token);
        }

        public async Task<ServiceResponse> RegisterAsync(RegisterDTO dto)
        {
            if (!Uri.TryCreate(
                    _emailSettings.PublicApiBaseUrl,
                    UriKind.Absolute,
                    out var publicApiUri)
                || (publicApiUri.Scheme != Uri.UriSchemeHttp
                    && publicApiUri.Scheme != Uri.UriSchemeHttps))
            {
                return ServiceResponse.Error(
                    "The public API URL for email confirmation is not configured correctly.");
            }

            var user = new UserEntity
            {
                UserName = dto.UserName,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return ServiceResponse.Error(
                    string.Join("; ", result.Errors.Select(error => error.Description)));
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "user");
            if (!roleResult.Succeeded)
            {
                return await DeleteUserAfterFailedRegistrationAsync(
                    user,
                    string.Join("; ", roleResult.Errors.Select(error => error.Description)));
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationEndpoint = $"{_emailSettings.PublicApiBaseUrl.TrimEnd('/')}/api/auth/confirm-email";
            var confirmationUrl = $"{confirmationEndpoint}?userId={Uri.EscapeDataString(user.Id)}&token={Uri.EscapeDataString(token)}";
            var emailResult = await _emailService.SendConfirmationEmailAsync(
                dto.Email,
                confirmationUrl);

            if (!emailResult.IsSuccess)
            {
                return await DeleteUserAfterFailedRegistrationAsync(user, emailResult.Message);
            }

            return ServiceResponse.Success(
                "Registration successful. Check your email to confirm your account.");
        }

        public async Task<ServiceResponse> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ServiceResponse.Error("The confirmation link is invalid or has expired.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return ServiceResponse.Error("The confirmation link is invalid or has expired.");
            }

            return ServiceResponse.Success("Your email address has been confirmed.");
        }

        private async Task<ServiceResponse> DeleteUserAfterFailedRegistrationAsync(
            UserEntity user,
            string reason)
        {
            var deleteResult = await _userManager.DeleteAsync(user);
            if (!deleteResult.Succeeded)
            {
                var deletionErrors = string.Join(
                    "; ",
                    deleteResult.Errors.Select(error => error.Description));
                return ServiceResponse.Error(
                    $"{reason} The unconfirmed account could not be removed: {deletionErrors}");
            }

            return ServiceResponse.Error(reason);
        }
    }
}
