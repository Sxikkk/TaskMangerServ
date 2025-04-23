using Application.Dtos.Request;
using Application.Dtos.Response;
using Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace Application.Interfaces;

public interface ITaskService
{
     Task<TaskResponseDto> CreateTaskAsync(TaskRequestDto dto);
     Task<TaskResponseDto> DeleteTaskAsync(TaskRequestDto dto);
     Task<TaskResponseDto> UpdateTaskAsync(TaskUpdateRequestDto dto);
} 