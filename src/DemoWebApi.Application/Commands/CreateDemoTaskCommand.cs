using DemoWebApi.Contracts.Responses;
using FluentResults;
using MediatR;

namespace DemoWebApi.Application.Commands;

public record CreateDemoTaskCommand(
    string Name,
    DateTime? DueDate,
    DateTime? CompletionDate) : IRequest<Result<CreateDemoTaskResponse>>;