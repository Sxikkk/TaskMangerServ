using Application.Dtos.Request;
using Application.Dtos.Response;
using Application.Exceptions;
using Application.Interfaces;
using Application.RInterfaces;
using Domain.Models;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUserRepository _userRepository;

    public TaskService(ITaskRepository taskRepository, IUserRepository userRepository)
    {
        _taskRepository = taskRepository;
        _userRepository = userRepository;
    }

    public async Task<TaskResponseDto> CreateTaskAsync(TaskCreateRequestDto dto)
    {
        if (!await _userRepository.ExistUserByIdAsync(dto.UserId))
            throw new UserNotFoundException("User not found", dto.UserId);

        var task = new Domain.Models.Task
        {
            UserId = dto.UserId,
            Title = dto.Title,
            Description = dto.Description,
            Status = dto.Status,
            DueDate = dto.DueDate
        };

        var createdTask = await _taskRepository.AddTaskAsync(dto.UserId, task);
        return MapToResponseDto(createdTask);
    }

    public async Task<TaskResponseDto> DeleteTaskAsync(TaskDeleteRequestDto dto)
    {
        if (!await _taskRepository.ExistTaskById(dto.TaskId))
            throw new TaskNotFoundException($"Task with id {dto.TaskId} not found");

        var deletedTask = await _taskRepository.DeleteTaskAsync(dto.TaskId);
        return MapToResponseDto(deletedTask);
    }

    public async Task<TaskResponseDto> UpdateTaskAsync(TaskUpdateRequestDto dto)
    {
        if (!await _taskRepository.ExistTaskById(dto.TaskId))
            throw new TaskNotFoundException($"Task with id {dto.TaskId} not found");

        var existingTask = await _taskRepository.GetTaskByIdAsync(dto.TaskId);
        existingTask.Title = dto.Title ?? existingTask.Title;
        existingTask.Description = dto.Description ?? existingTask.Description;
        existingTask.Status = dto.Status ?? existingTask.Status;
        existingTask.DueDate = dto.DueDate ?? existingTask.DueDate;

        var updatedTask = await _taskRepository.UpdateTaskAsync(existingTask, dto.TaskId);
        return MapToResponseDto(updatedTask);
    }

    public async Task<TaskResponseDto> GetTaskByIdAsync(Guid taskId)
    {
        if (!await _taskRepository.ExistTaskById(taskId))
            throw new TaskNotFoundException($"Task with id {taskId} not found");

        var task = await _taskRepository.GetTaskByIdAsync(taskId);
        return MapToResponseDto(task);
    }

    public async Task<List<TaskResponseDto>> GetUserTasksAsync(Guid userId)
    {
        if (!await _userRepository.ExistUserByIdAsync(userId))
            throw new UserNotFoundException("User not found", userId);

        var tasks = await _taskRepository.GetAllUsersTasksAsync(userId);
        return tasks.Select(MapToResponseDto).ToList();
    }

    public async Task<List<TaskResponseDto>> GetTasksByStatusAsync(Guid userId, Status status)
    {
        if (!await _userRepository.ExistUserByIdAsync(userId))
            throw new UserNotFoundException("User not found", userId);

        var tasks = await _taskRepository.GetAllUsersTasksAsync(userId);
        return tasks.Where(t => t.Status == status)
                   .Select(MapToResponseDto)
                   .ToList();
    }

    public async Task<List<TaskResponseDto>> GetTasksByDueDateAsync(Guid userId, DateTime dueDate)
    {
        if (!await _userRepository.ExistUserByIdAsync(userId))
            throw new UserNotFoundException("User not found", userId);

        var tasks = await _taskRepository.GetAllUsersTasksAsync(userId);
        return tasks.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == dueDate.Date)
                   .Select(MapToResponseDto)
                   .ToList();
    }

    public async Task<List<TaskResponseDto>> GetSortedTasksAsync(Guid userId, string sortBy, bool ascending = true)
    {
        if (!await _userRepository.ExistUserByIdAsync(userId))
            throw new UserNotFoundException("User not found", userId);

        var tasks = await _taskRepository.GetAllUsersTasksAsync(userId);
        var sortedTasks = sortBy.ToLower() switch
        {
            "title" => ascending 
                ? tasks.OrderBy(t => t.Title)
                : tasks.OrderByDescending(t => t.Title),
            "duedate" => ascending
                ? tasks.OrderBy(t => t.DueDate)
                : tasks.OrderByDescending(t => t.DueDate),
            "status" => ascending
                ? tasks.OrderBy(t => t.Status)
                : tasks.OrderByDescending(t => t.Status),
            "createdat" => ascending
                ? tasks.OrderBy(t => t.CreatedAt)
                : tasks.OrderByDescending(t => t.CreatedAt),
            _ => throw new ArgumentException($"Invalid sort field: {sortBy}")
        };

        return sortedTasks.Select(MapToResponseDto).ToList();
    }

    public async Task<List<TaskResponseDto>> SearchTasksAsync(Guid userId, string searchTerm)
    {
        if (!await _userRepository.ExistUserByIdAsync(userId))
            throw new UserNotFoundException("User not found", userId);

        var tasks = await _taskRepository.GetAllUsersTasksAsync(userId);
        return tasks.Where(t => t.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                               t.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                   .Select(MapToResponseDto)
                   .ToList();
    }

    private static TaskResponseDto MapToResponseDto(Domain.Models.Task task)
    {
        return new TaskResponseDto
        {
            Id = task.Id,
            UserId = task.UserId,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            DueDate = task.DueDate
        };
    }
} 