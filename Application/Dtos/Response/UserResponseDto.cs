using Domain.Models;

namespace Application.Dtos.Response;

public record UserResponseDto
{
    public Guid Id { get; init; }
    public string Email { get; init; }
    public string FirstName { get; init; }
    public string? SecondName { get; init; }
    public int RoleId { get; init; }
    public DateTime CreatedAt { get; init; }
};