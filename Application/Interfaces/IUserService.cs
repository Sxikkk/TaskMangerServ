using Application.Dtos.Response;
using Application.Dtos.Request;
using Task = System.Threading.Tasks.Task;

namespace Application.Interfaces;

public interface IUserService
{
    Task<UserResponseDto> GetUserAsync(Guid userId);
    Task<List<UserResponseDto>> GetAllUserAsync();
    Task<UserResponseDto> GetUserByEmailAsync(string email);
    Task<UserResponseDto> UpdateUserAsync(UserUpdateRequestDto dto);
    Task<bool> DeleteUserAsync(Guid userId);
    Task<List<UserResponseDto>> GetUsersByRoleAsync(int roleId);
    Task<bool> ChangeUserRoleAsync(Guid userId, int newRoleId);
    Task<bool> UpdateUserPasswordAsync(Guid userId, string currentPassword, string newPassword);
}