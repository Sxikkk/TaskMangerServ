using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request;

public class UserUpdateRequestDto
{
    [Required]
    public Guid UserId { get; set; }
    
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [MaxLength(254, ErrorMessage = "Email cannot exceed 254 characters")]
    public string? Email { get; set; }
    
    [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    public string? FirstName { get; set; }
    
    [MaxLength(50, ErrorMessage = "Second name cannot exceed 50 characters")]
    public string? SecondName { get; set; }
} 