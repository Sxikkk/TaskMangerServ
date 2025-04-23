using Application.Dtos.Response;

namespace Application.Interfaces;

public interface IUserService
{
    Task<UserResponseDto> GetUserAsync(Guid userId);
    Task<List<UserResponseDto>> GetAllUserAsync();
}