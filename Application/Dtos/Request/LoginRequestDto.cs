using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request;

public record LoginRequestDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email")]
    public string Email { get; set; }
    public string Password { get; set; }
};