using FluentResults;
using MediatR;

namespace DemoWebApi.Application.Commands;

public record CompleteDemoTaskCommand(Guid DemoTaskId, DateTime CompletionDate): IRequest<Result<bool>>;