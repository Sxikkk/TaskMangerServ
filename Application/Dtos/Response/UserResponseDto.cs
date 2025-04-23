using Domain.Models;

namespace Application.Dtos.Response;

public record UserResponseDto
{
    public Guid UserId { get; init; }
    public string Email { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public Role Role { get; init; }
};