using System.Collections;
using Application.Dtos.Request;
using Task = Domain.Models.Task;

namespace Application.RInterfaces;

public interface ITaskRepository
{
    Task<List<Task>> GetAllUsersTasksAsync(Guid userId);
    Task<Task> GetTaskByIdAsync(Guid taskId);
    Task<Task> AddTaskAsync(Guid userId, Task task);
    Task<Task> UpdateTaskAsync(Task task, Guid taskId);
    Task<Task> DeleteTaskAsync(Guid taskId);
    Task<bool> ExistTaskById(Guid taskId);
}