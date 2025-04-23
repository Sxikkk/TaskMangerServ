using Application.RInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController: ControllerBase
{

    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("{id:guid}")]
    public async Task<ActionResult> GetUserInfoAsync(Guid id)
    {
        try
        {
            var response = await _userRepository.GetUserByIdAsync(id);
            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}