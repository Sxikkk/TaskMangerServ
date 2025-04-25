using Application.Dtos.Request;
using Application.Dtos.Response;
using Application.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<UserResponseDto>> GetUserAsync(Guid userId)
    {
        try
        {
            var response = await _userService.GetUserAsync(userId);
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

    [HttpGet]
    public async Task<ActionResult<List<UserResponseDto>>> GetAllUsersAsync()
    {
        try
        {
            var response = await _userService.GetAllUserAsync();
            return Ok(response);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<UserResponseDto>> GetUserByEmailAsync(string email)
    {
        try
        {
            var response = await _userService.GetUserByEmailAsync(email);
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

    [HttpPut("update")]
    public async Task<ActionResult<UserResponseDto>> UpdateUserAsync(UserUpdateRequestDto dto)
    {
        try
        {
            var response = await _userService.UpdateUserAsync(dto);
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

    [HttpDelete("{userId:guid}")]
    public async Task<ActionResult<bool>> DeleteUserAsync(Guid userId)
    {
        try
        {
            var response = await _userService.DeleteUserAsync(userId);
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

    [HttpGet("role/{roleId}")]
    public async Task<ActionResult<List<UserResponseDto>>> GetUsersByRoleAsync(int roleId)
    {
        try
        {
            var response = await _userService.GetUsersByRoleAsync(roleId);
            return Ok(response);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("{userId:guid}/role/{newRoleId}")]
    public async Task<ActionResult<bool>> ChangeUserRoleAsync(Guid userId, int newRoleId)
    {
        try
        {
            var response = await _userService.ChangeUserRoleAsync(userId, newRoleId);
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

    [HttpPut("{userId:guid}/password")]
    public async Task<ActionResult<bool>> UpdateUserPasswordAsync(
        Guid userId, 
        [FromBody] PasswordUpdateRequestDto dto)
    {
        try
        {
            var response = await _userService.UpdateUserPasswordAsync(
                userId, 
                dto.CurrentPassword, 
                dto.NewPassword);
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