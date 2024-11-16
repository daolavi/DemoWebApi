namespace DemoWebApi.Contracts.Responses;

public record DemoTaskDto(
    Guid Id,
    string Name,
    bool IsDone,
    DateTime DueDate,
    DateTime? CompletionDate);