using Application.Dtos.Request;
using Application.Dtos.Response;
using Domain.Models;
using Domain.Enums;
using Task = System.Threading.Tasks.Task;

namespace Application.Interfaces;

public interface ITaskService
{
    Task<TaskResponseDto> CreateTaskAsync(TaskCreateRequestDto dto);
    Task<TaskResponseDto> DeleteTaskAsync(TaskDeleteRequestDto dto);
    Task<TaskResponseDto> UpdateTaskAsync(TaskUpdateRequestDto dto);
    Task<TaskResponseDto> GetTaskByIdAsync(Guid taskId);
    Task<List<TaskResponseDto>> GetUserTasksAsync(Guid userId);
    Task<List<TaskResponseDto>> GetTasksByStatusAsync(Guid userId, Status status);
    Task<List<TaskResponseDto>> GetTasksByDueDateAsync(Guid userId, DateTime dueDate);
    Task<List<TaskResponseDto>> GetSortedTasksAsync(Guid userId, string sortBy, bool ascending = true);
    Task<List<TaskResponseDto>> SearchTasksAsync(Guid userId, string searchTerm);
} 