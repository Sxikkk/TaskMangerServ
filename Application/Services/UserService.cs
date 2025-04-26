using Application.Dtos.Request;
using Application.Dtos.Response;
using Application.Exceptions;
using Application.Interfaces;
using Application.RInterfaces;
using Domain.Models;
using DevOne.Security.Cryptography.BCrypt;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponseDto> GetUserAsync(Guid userId)
    {
        if (!await _userRepository.ExistUserByIdAsync(userId))
            throw new UserNotFoundException("User not found", userId);

        var user = await _userRepository.GetUserByIdAsync(userId);
        return MapToResponseDto(user);
    }

    public async Task<List<UserResponseDto>> GetAllUserAsync()
    {
        var users = await _userRepository.GetUsersAsync(0, int.MaxValue);
        return users.Select(MapToResponseDto).ToList();
    }

    public async Task<UserResponseDto> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        var user = await _userRepository.GetUserByEmailAsync(email);
        return MapToResponseDto(user);
    }

    public async Task<UserResponseDto> UpdateUserAsync(UserUpdateRequestDto dto)
    {
        if (!await _userRepository.ExistUserByIdAsync(dto.UserId))
            throw new UserNotFoundException("User not found", dto.UserId);

        var existingUser = await _userRepository.GetUserByIdAsync(dto.UserId);
        
        // Update only non-null properties
        if (!string.IsNullOrWhiteSpace(dto.FirstName))
            existingUser.FirstName = dto.FirstName;
        if (!string.IsNullOrWhiteSpace(dto.SecondName))
            existingUser.SecondName = dto.SecondName;
        if (!string.IsNullOrWhiteSpace(dto.Email))
            existingUser.Email = dto.Email;

        var updatedUser = await _userRepository.UpdateUserAsync(existingUser);
        return MapToResponseDto(updatedUser);
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        if (!await _userRepository.ExistUserByIdAsync(userId))
            throw new UserNotFoundException("User not found", userId);

        await _userRepository.DeleteUserAsync(userId);
        return true;
    }

    public async Task<List<UserResponseDto>> GetUsersByRoleAsync(int roleId)
    {
        var users = await _userRepository.GetUsersAsync(0, int.MaxValue);
        return users.Where(u => u.RoleId == roleId)
                   .Select(MapToResponseDto)
                   .ToList();
    }

    public async Task<bool> ChangeUserRoleAsync(Guid userId, int newRoleId)
    {
        if (!await _userRepository.ExistUserByIdAsync(userId))
            throw new UserNotFoundException("User not found", userId);

        var user = await _userRepository.GetUserByIdAsync(userId);
        user.RoleId = newRoleId;
        await _userRepository.UpdateUserAsync(user);
        return true;
    }

    public async Task<bool> UpdateUserPasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        if (!await _userRepository.ExistUserByIdAsync(userId))
            throw new UserNotFoundException("User not found", userId);

        var user = await _userRepository.GetUserByIdAsync(userId);

        if (!BCryptHelper.CheckPassword(currentPassword, user.PasswordHash))
            throw new UnauthorizedAccessException("Current password is incorrect");

        user.PasswordHash = BCryptHelper.HashPassword(newPassword, BCryptHelper.GenerateSalt());
        await _userRepository.UpdateUserAsync(user);
        return true;
    }

    private static UserResponseDto MapToResponseDto(User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            SecondName = user.SecondName,
            RoleId = user.RoleId,
            CreatedAt = user.CreatedAt
        };
    }
} 