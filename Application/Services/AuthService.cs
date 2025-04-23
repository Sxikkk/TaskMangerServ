using Application.Dtos.Request;
using Application.Dtos.Response;
using Application.Exceptions;
using Application.Interfaces;
using Application.RInterfaces;
using DevOne.Security.Cryptography.BCrypt;
using Domain.Models;
using Microsoft.AspNetCore.Identity.Data;

namespace Application.Services;

public class AuthService: IAuthService
{
    private readonly IJwtService _jwtService;
    private readonly IUserRepository _userRepository;
    private readonly ITokenRepository _tokenRepository;

    public AuthService(IJwtService jwtService, IUserRepository userRepository, ITokenRepository tokenRepository)
    {
        _jwtService = jwtService;
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
    }
    
    public async Task<AuthResponseDto> RegistrationAsync(RegistrationRequestDto dto)
    {
        if (await _userRepository.ExistUserByEmailAsync(dto.Email))
            throw new AlreadyExistException($"User with {dto.Email} already exist");
        
        var user = new User
        {
            Email = dto.Email,
            PasswordHash = BCryptHelper.HashPassword(dto.Password, BCryptHelper.GenerateSalt()),
            FirstName = dto.FirstName,
            SecondName = dto.SecondName,
            RoleId = 1
        };

        var addedUSer = await _userRepository.AddUserAsync(user);
        var token = new RefreshToken
        {
            UserId = addedUSer.Id,
            Token = _jwtService.GenerateRefreshToken(addedUSer),
            ExpiresAt = DateTime.UtcNow.AddDays(30)
        };
        var refreshToken = await _tokenRepository.AddTokenAsync(token);
        return new AuthResponseDto
        {
            RefreshToken = refreshToken.Token,
            AccessToken = _jwtService.GenerateAccessToken(addedUSer)
        };
    }

    public async Task<AuthResponseDto> LoginUserAsync(LoginRequestDto dto)
    {
        if (!await _userRepository.ExistUserByEmailAsync(dto.Email))
            throw new UserNotFoundException($"User with {dto.Email} not found", dto.Email);

        var user = await _userRepository.GetUserByEmailAsync(dto.Email);

        if (!BCryptHelper.CheckPassword(dto.Password, user.PasswordHash))
            throw new Exception("Password incorrect");
        
        var newRefreshToken =
            await _tokenRepository.UpdateTokenAsync(user.Token.Id, _jwtService.GenerateRefreshToken(user));

        return new AuthResponseDto
        {
            AccessToken = _jwtService.GenerateAccessToken(user),
            RefreshToken = newRefreshToken.Token
        };
    }

    public async Task<AuthResponseDto> RefreshAsync(RefreshRequestDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        
        if (!await _userRepository.ExistUserByEmailAsync(dto.Email))
            throw new UserNotFoundException($"User with {dto.Email} not found", dto.Email);

        var user = await _userRepository.GetUserByEmailAsync(dto.Email);

        if (dto.RefreshToken != user.Token.Token)
            throw new Exception("Invalid Token");
        
        var newRefreshToken =
            await _tokenRepository.UpdateTokenAsync(user.Token.Id, _jwtService.GenerateRefreshToken(user));
        
        return new AuthResponseDto
        {
            AccessToken = _jwtService.GenerateAccessToken(user),
            RefreshToken = newRefreshToken.Token
        };
    }
}