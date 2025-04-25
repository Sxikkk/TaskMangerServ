using Application.Dtos.Request;
using Application.Dtos.Response;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost("create")]
    public async Task<ActionResult<TaskResponseDto>> CreateTaskAsync(TaskCreateRequestDto dto)
    {
        try
        {
            var response = await _taskService.CreateTaskAsync(dto);
            return Ok(response);
        }
        catch (UserNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("delete")]
    public async Task<ActionResult<TaskResponseDto>> DeleteTaskAsync(TaskDeleteRequestDto dto)
    {
        try
        {
            var response = await _taskService.DeleteTaskAsync(dto);
            return Ok(response);
        }
        catch (TaskNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("update")]
    public async Task<ActionResult<TaskResponseDto>> UpdateTaskAsync(TaskUpdateRequestDto dto)
    {
        try
        {
            var response = await _taskService.UpdateTaskAsync(dto);
            return Ok(response);
        }
        catch (TaskNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{taskId:guid}")]
    public async Task<ActionResult<TaskResponseDto>> GetTaskByIdAsync(Guid taskId)
    {
        try
        {
            var response = await _taskService.GetTaskByIdAsync(taskId);
            return Ok(response);
        }
        catch (TaskNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult<List<TaskResponseDto>>> GetUserTasksAsync(Guid userId)
    {
        try
        {
            var response = await _taskService.GetUserTasksAsync(userId);
            return Ok(response);
        }
        catch (UserNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("user/{userId:guid}/status/{status}")]
    public async Task<ActionResult<List<TaskResponseDto>>> GetTasksByStatusAsync(Guid userId, Status status)
    {
        try
        {
            var response = await _taskService.GetTasksByStatusAsync(userId, status);
            return Ok(response);
        }
        catch (UserNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("user/{userId:guid}/due-date")]
    public async Task<ActionResult<List<TaskResponseDto>>> GetTasksByDueDateAsync(Guid userId, [FromQuery] DateTime dueDate)
    {
        try
        {
            var response = await _taskService.GetTasksByDueDateAsync(userId, dueDate);
            return Ok(response);
        }
        catch (UserNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("user/{userId:guid}/sorted")]
    public async Task<ActionResult<List<TaskResponseDto>>> GetSortedTasksAsync(
        Guid userId, 
        [FromQuery] string sortBy, 
        [FromQuery] bool ascending = true)
    {
        try
        {
            var response = await _taskService.GetSortedTasksAsync(userId, sortBy, ascending);
            return Ok(response);
        }
        catch (UserNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("user/{userId:guid}/search")]
    public async Task<ActionResult<List<TaskResponseDto>>> SearchTasksAsync(
        Guid userId, 
        [FromQuery] string searchTerm)
    {
        try
        {
            var response = await _taskService.SearchTasksAsync(userId, searchTerm);
            return Ok(response);
        }
        catch (UserNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
} 