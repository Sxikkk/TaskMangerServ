using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.Dtos.Request;

public record TaskCreateRequestDto
{
    [Required]
    public Guid UserId { get; init; }
    [Required]
    [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
    public string Title { get; init; }
    
    [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string Description { get; init; }
    
    [Required]
    public Status Status { get; init; } = Status.Pending;
    
    public DateTime? DueDate { get; init; }
}