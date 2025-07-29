using ExpenseTrackerCrudWebAPI.DTOs;
using ExpenseTrackerCrudWebAPI.Services;
using ExpenseTrackerCrudWebAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTrackerCrudWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            _logger.LogInformation("Registration attempt for email:{Email} with role:{Role}",dto.Email,dto.Role);
            var result = await _authService.RegisterUserAsync(dto);
            if (!result.Succeeded) {
                _logger.LogWarning("Registration failed for email: {Email}. Errors: {Errors}", dto.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

            }

            _logger.LogInformation("Registration successful for email: {Email} with role: {Role}", dto.Email, dto.Role);
            return Ok(new { message = $"Registered as {dto.Role}" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            _logger.LogInformation("Login attempt for user: {Email}", dto.Email);
            var response = await _authService.LoginUserAsync(dto);

            if (response == null)
            {
                _logger.LogWarning("Login failed for user: {Email}", dto.Email);
                return Unauthorized(new { message = "Invalid email or password." });
            }
            _logger.LogInformation("Login successful for user: {Email}", dto.Email);
            return Ok(new
            {
                token = response.AccessToken,
                role = response.Role,
                refreshToken = response.RefreshToken
            });
        }


        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            _logger.LogInformation("ChangePassword request received for userId: {UserId}", userId);
            var result = await _authService.ChangePasswordAsync(userId, dto);

            if (!result.Succeeded)
            {
                _logger.LogWarning("Failed to change password for userId: {UserId}. Errors: {Errors}",
                 userId, string.Join(", ", result.Errors.Select(e => e.Description)));
                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
            }

            _logger.LogInformation("Password changed successfully for userId: {UserId}", userId);

            return Ok(new { message = "Password changed successfully" });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto);
            if (result == null)
                return Unauthorized(new { message = "Invalid refresh token" });

            return Ok(result);
        }


        [Authorize]
        [HttpDelete("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _logger.LogInformation("DeleteAccount request received for userId: {UserId}", userId);
            var result = await _authService.DeleteAccountAsync(userId);

            if (!result.Succeeded) {
                _logger.LogWarning("Failed to delete account for userId: {UserId}. Errors: {Errors}",
    userId, string.Join(", ", result.Errors.Select(e => e.Description)));
                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
            }

            _logger.LogInformation("Account deleted successfully for userId: {UserId}", userId);

            return Ok(new { message = "Account deleted successfully" });
        }
    }
}
