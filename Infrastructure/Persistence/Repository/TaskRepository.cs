using Application.Dtos.Request;
using Application.Exceptions;
using Application.RInterfaces;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Task = Domain.Models.Task;

namespace Infrastructure.Persistence.Repository;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;
    private readonly IUserRepository _userRepository;

    public TaskRepository(AppDbContext context, IUserRepository userRepository)
    {
        _context = context;
        _userRepository = userRepository;
    }
    
    public async Task<List<Task>> GetAllUsersTasksAsync(Guid userId)
    {
        try
        {
            if (!await _userRepository.ExistUserByIdAsync(userId))
                throw new UserNotFoundException("User not found", userId);
            
            var user = await _context.Users.Include(user => user.Tasks).FirstOrDefaultAsync(u => u.Id == userId);
            var tasks = user!.Tasks.ToList();
            return tasks;
        }
        catch (UserNotFoundException e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<Task> GetTaskByIdAsync(Guid taskId)
    {
        try
        {
            if (!await ExistTaskById(taskId))
                throw new TaskNotFoundException($"task with {taskId} not found");                
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
            return task;
        }
        catch (TaskNotFoundException e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<Task> AddTaskAsync(Guid userId, Task task)
    {
        try
        {
            if (!await _userRepository.ExistUserByIdAsync(userId))
                throw new UserNotFoundException("Пользователь не найден", userId);
            
            task.UserId = userId;

            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
        }
        catch (UserNotFoundException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Task> UpdateTaskAsync(Task task, Guid taskId)
    {
        var oldTask = await GetTaskByIdAsync(taskId);
        oldTask.Description = task.Description;
        oldTask.Status = task.Status;
        oldTask.Title = task.Title;
        oldTask.DueDate = task.DueDate;
        oldTask.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return oldTask;
    }

    public async Task<Task> DeleteTaskAsync(Guid taskId)
    {
        var needToDeleteTask = await GetTaskByIdAsync(taskId);
        _context.Tasks.Remove(needToDeleteTask);
        return needToDeleteTask;
    }

    public async Task<bool> ExistTaskById(Guid taskId)
    {
        return await _context.Tasks.AnyAsync(t => t.Id == taskId);
    }
}