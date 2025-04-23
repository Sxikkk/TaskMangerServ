using Application.Exceptions;
using Application.RInterfaces;
using Domain.Models;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class UserRepository: IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        var normalizedEmail = email.Trim().ToLower();
        var user = await _context.Users
            .Include(u => u.Token)
            .Include(u => u.Tasks)
            .FirstOrDefaultAsync(u => u.Email.Trim().ToLower() == normalizedEmail);
    
        if (user == null)
            throw new UserNotFoundException($"User with email '{email}' not found", email);
    
        return user;
    }
    
    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        try
        {
            if (!await ExistUserByIdAsync(userId))
                throw new UserNotFoundException("User not found", userId);

            var user = await _context.Users
                .Include(u => u.Token)
                .Include(u => u.Tasks)                
                .FirstOrDefaultAsync(u => u.Id == userId);
            return user!;
        }
        catch (UserNotFoundException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<User>> GetUsersAsync(int skip, int take)
    {
        return await _context.Users
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<User> AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> DeleteUserAsync(Guid userId)
    {
        try
        {
            if (!await ExistUserByIdAsync(userId))
                throw new UserNotFoundException("User not found", userId);

            var userNeedToDelete = await GetUserByIdAsync(userId);
            _context.Users.Remove(userNeedToDelete);
            await _context.SaveChangesAsync();
            return userNeedToDelete;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> ExistUserByIdAsync(Guid userId)
    {
        return await _context.Users.AnyAsync(u => u.Id == userId);
    }

    public async Task<bool> ExistUserByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;
        
        var normalizedEmail = email.Trim().ToLower();
        return await _context.Users
            .AnyAsync(u => u.Email.Trim().ToLower() == normalizedEmail);
    }
}