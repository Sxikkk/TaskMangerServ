using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request;

public record RegistrationRequestDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid Email")]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    public string SecondName { get; set; }
}