using DemoWebApi.Contracts.Responses;
using FluentResults;
using MediatR;

namespace DemoWebApi.Application.Queries;

public record GetDemoTaskByIdQuery(Guid DemoTaskId) : IRequest<Result<DemoTaskDto>>;