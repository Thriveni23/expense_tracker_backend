using ExpenseTrackerCrudWebAPI.DTOs;
using ExpenseTrackerCrudWebAPI.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ExpenseTrackerCrudWebAPI.Services
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterDto dto);
        Task<TokenResponseDto> LoginUserAsync(LoginDto dto);
        Task<TokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);
        Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordDTO dto);
        Task<IdentityResult> DeleteAccountAsync(string userId);
    }
}

