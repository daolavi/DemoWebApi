namespace DemoWebApi.Contracts.Requests;

public record CreateDemoTaskRequest(
    string Name,
    DateTime? DueDate,
    DateTime? CompletionDate);