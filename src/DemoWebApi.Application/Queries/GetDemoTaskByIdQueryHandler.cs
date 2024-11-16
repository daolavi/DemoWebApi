using DemoWebApi.Application.Converters;
using DemoWebApi.Contracts.Responses;
using DemoWebApi.Infrastructure;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApi.Application.Queries;

public class GetDemoTaskByIdQueryHandler(IDemoWebApiContext context) : IRequestHandler<GetDemoTaskByIdQuery, Result<DemoTaskDto>>
{
    public async Task<Result<DemoTaskDto>> Handle(GetDemoTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var demoTask = await context.DemoTasks
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.DemoTaskId, cancellationToken);
        
        return demoTask == null ? Result.Fail<DemoTaskDto>("Demo task not found") : Result.Ok(demoTask.ToContract());
    }
}