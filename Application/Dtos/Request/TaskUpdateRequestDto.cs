using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Application.Dtos.Request;

public record TaskUpdateRequestDto
{
    [Required]
    public Guid TaskId { get; init; }
    
    [MaxLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
    public string? Title { get; init; }
    
    [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string? Description { get; init; }
    
    public Status? Status { get; init; }
    
    public DateTime? DueDate { get; init; }
}