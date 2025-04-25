using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Request;

public record TaskDeleteRequestDto
{
    [Required]
    public Guid TaskId { get; init; }
}