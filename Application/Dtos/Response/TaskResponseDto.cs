using Domain.Enums;

namespace Application.Dtos.Response;

public record TaskResponseDto
{
    public string Title { get; init; }
    public string Description { get; init; }
    public Status Status { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public DateTime DueDate { get; init; }
};