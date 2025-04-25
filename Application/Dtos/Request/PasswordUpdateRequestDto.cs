using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request;

public class PasswordUpdateRequestDto
{
    [Required(ErrorMessage = "Current password is required")]
    public string CurrentPassword { get; init; }
    
    [Required(ErrorMessage = "New password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
    public string NewPassword { get; init; }
} 