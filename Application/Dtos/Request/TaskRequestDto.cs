using Domain.Enums;

namespace Application.Dtos.Request;

public record TaskRequestDto
{
    public Status Status { get; init; } = Status.Pending;
    public Guid TaskId { get; init; }
};