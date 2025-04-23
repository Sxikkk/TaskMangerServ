using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Dtos.Response;
using Application.Interfaces;
using Application.RInterfaces;
using Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class JwtService: IJwtService
{

    private readonly IOptions<JwtSettings> _jwtSettings;
    private readonly ITokenRepository _tokenRepository;

    public JwtService(IOptions<JwtSettings> jwtSettings, ITokenRepository tokenRepository)
    {
        _jwtSettings = jwtSettings;
        _tokenRepository = tokenRepository;
    }

    public SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings?.Value.SecretKey!));

    public string GenerateRefreshToken(User user)
    {
        try
        {
            var refreshToken = new JwtSecurityToken(
                issuer: _jwtSettings.Value.Issuer,
                audience: _jwtSettings.Value.Audience,
                claims: new List<Claim>()
                {
                    new Claim("email", user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                },
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(refreshToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public string GenerateAccessToken(User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        
        var claims = new List<Claim>
        {
            new Claim("userId", user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
        };

        var accessToken = new JwtSecurityToken(
            issuer: _jwtSettings.Value.Issuer,
            audience: _jwtSettings.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(2),
            signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(accessToken);
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshToken token, User user)
    {
        var dToken = await _tokenRepository.UpdateTokenAsync(token.Id, GenerateRefreshToken(user));
        
        return new AuthResponseDto
        {
            AccessToken = GenerateAccessToken(user),
            RefreshToken = dToken.Token
        };
    }
}