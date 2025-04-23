using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request;

public record RefreshRequestDto
{
    public string RefreshToken { get; init; }
    [EmailAddress(ErrorMessage = "Invalid email")]
    public string Email { get; init; }
};