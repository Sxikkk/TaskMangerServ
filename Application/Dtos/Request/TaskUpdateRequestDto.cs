using Domain.Enums;

namespace Application.Dtos.Request;

public record TaskUpdateRequestDto
{
    public Status Status { get; init; } = Status.Pending;
    public string? Title { get; init; } = null;
    public string? Description { get; init; } = null;
    public DateTime? DueDate { get; init; } = null;
};