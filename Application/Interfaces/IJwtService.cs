using Application.Dtos.Response;
using Domain.Models;
using Microsoft.IdentityModel.Tokens;

namespace Application.Interfaces;

public interface IJwtService
{
    SymmetricSecurityKey GetSymmetricSecurityKey();
    public string GenerateRefreshToken(User user);
    public string GenerateAccessToken(User user);
    public Task<AuthResponseDto> RefreshTokenAsync(RefreshToken token, User user);
}