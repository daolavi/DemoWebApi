using DemoWebApi.Domain.AggregatesModel.DemoTaskAggregate;
using DemoWebApi.Infrastructure;
using FluentResults;
using MediatR;

namespace DemoWebApi.Application.Commands;

public class CreateDemoTaskCommandHandler(DemoWebApiContext context) : IRequestHandler<CreateDemoTaskCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateDemoTaskCommand request, CancellationToken cancellationToken)
    {
        var createDemoTask = DemoTask.CreateTask(request.Name, request.IsDone, request.DueDate, request.CompletionDate);

        if (!createDemoTask.IsSuccess)
        {
            return Result.Fail<Guid>(createDemoTask.Errors);
        }

        await context.DemoTasks.AddAsync(createDemoTask.Value, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Ok(createDemoTask.Value.Id);

    }
}