using DemoWebApi.Contracts.Responses;
using DemoWebApi.Domain.AggregatesModel.DemoTaskAggregate;
using DemoWebApi.Infrastructure;
using FluentResults;
using MediatR;

namespace DemoWebApi.Application.Commands;

public class CreateDemoTaskCommandHandler(DemoWebApiContext context) : IRequestHandler<CreateDemoTaskCommand, Result<CreateDemoTaskResponse>>
{
    public async Task<Result<CreateDemoTaskResponse>> Handle(CreateDemoTaskCommand request, CancellationToken cancellationToken)
    {
        var createDemoTask = DemoTask.CreateTask(request.Name, request.DueDate, request.CompletionDate);

        if (!createDemoTask.IsSuccess)
        {
            return Result.Fail<CreateDemoTaskResponse>(createDemoTask.Errors);
        }

        await context.DemoTasks.AddAsync(createDemoTask.Value, cancellationToken);
        await context.SaveEntitiesAsync(cancellationToken);
        return Result.Ok(new CreateDemoTaskResponse(createDemoTask.Value.Id));
    }
}