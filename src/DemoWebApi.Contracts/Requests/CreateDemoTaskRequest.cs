namespace DemoWebApi.Contracts.Requests;

public record CreateDemoTaskRequest(
    string Name,
    bool IsDone,
    DateTime? DueDate,
    DateTime? CompletionDate);