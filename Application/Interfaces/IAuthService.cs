using Application.Dtos.Request;
using Application.Dtos.Response;
using Domain.Models;
using Microsoft.AspNetCore.Identity.Data;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegistrationAsync(RegistrationRequestDto dto);
    Task<AuthResponseDto> LoginUserAsync(LoginRequestDto dto);
    Task<AuthResponseDto> RefreshAsync(RefreshRequestDto dto);
}