using Application.Dtos.Request;
using Application.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api")]
public class AuthController: ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult> LoginAsync(LoginRequestDto dto)
    {
        try
        {
            var response = await _authService.LoginUserAsync(dto);
            Console.WriteLine(response);
            return Ok(response);
        }
        catch (UserNotFoundException e)
        {
            Console.WriteLine(e);
            return StatusCode(203, e.Message);
        }
    }

    [HttpPost("registration")]
    public async Task<ActionResult> RegistrationAsync(RegistrationRequestDto dto)
    {
        try
        {
            var response = await _authService.RegistrationAsync(dto);
            return Ok(response);
        }
        catch (AlreadyExistException e)
        {
            Console.WriteLine(e);
            return StatusCode(203, e.Message);
        }
    }

    [HttpPost("refresh")]
    public async Task<ActionResult> RefreshAsync(RefreshRequestDto dto)
    {
        try
        {
            var response = await _authService.RefreshAsync(dto);
            return Ok(response);
        }
        catch (AlreadyExistException e)
        {
            Console.WriteLine(e);
            return StatusCode(203, e.Message);
        }
    }
}