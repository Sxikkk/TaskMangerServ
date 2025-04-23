using Domain.Models;

namespace Application.RInterfaces;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(Guid userId);
    Task<User> GetUserByEmailAsync(string email);
    Task<List<User>> GetUsersAsync(int skip, int take);
    Task<User> AddUserAsync(User user);
    Task<User> DeleteUserAsync(Guid userId);
    Task<bool> ExistUserByIdAsync(Guid userId);
    Task<bool> ExistUserByEmailAsync(string email);
}