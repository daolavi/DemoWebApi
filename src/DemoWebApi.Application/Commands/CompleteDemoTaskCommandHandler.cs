using DemoWebApi.Infrastructure;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApi.Application.Commands;

public class CompleteDemoTaskCommandHandler(DemoWebApiContext context): IRequestHandler<CompleteDemoTaskCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CompleteDemoTaskCommand request, CancellationToken cancellationToken)
    {
        var demoTask = await context.DemoTasks.FirstOrDefaultAsync(t => t.Id == request.DemoTaskId, cancellationToken);
        if (demoTask == null)
        {
            return Result.Fail<bool>($"DemoTask with id {request.DemoTaskId} not found");
        }

        var completeDemoTask = demoTask.Complete(request.CompletionDate);
        if (completeDemoTask.IsFailed)
        {
            return Result.Fail<bool>(completeDemoTask.Errors);
        }
        context.DemoTasks.Update(demoTask);
        await context.SaveEntitiesAsync(cancellationToken);
        return Result.Ok(completeDemoTask.Value);
    }
}