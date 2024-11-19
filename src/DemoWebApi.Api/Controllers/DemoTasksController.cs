using DemoWebApi.Api.Extensions;
using DemoWebApi.Application.Commands;
using DemoWebApi.Application.Queries;
using DemoWebApi.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DemoTasksController(IMediator mediator) : ControllerBase
{
    [HttpGet("{demoTaskId}")]
    public async Task<IActionResult> GetDemoTaskById(string demoTaskId, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(demoTaskId, out Guid guid))
        {
            return BadRequest($"TaskId {demoTaskId} not a Guid");
        }
        
        var result = await mediator.Send(new GetDemoTaskByIdQuery(guid), cancellationToken);
        if (result.IsFailed)
        {
            return NotFound($"TaskId {demoTaskId} not found");
        }
        
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDemoTask(CreateDemoTaskRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateDemoTaskCommand(
            request.Name,
            request.DueDate,
            request.CompletionDate
            ), cancellationToken);

        if (result.IsFailed)
        {
            return BadRequest(result.ToProblemDetails(Request.Path));
        }
        
        return Ok(result.Value);
    }
}