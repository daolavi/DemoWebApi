using System.Net.Http.Json;
using AutoFixture;
using DemoWebApi.Contracts.Requests;
using DemoWebApi.Contracts.Responses;
using DemoWebApi.Domain.AggregatesModel.DemoTaskAggregate;
using DemoWebApi.Infrastructure;
using DemoWebApi.Tests.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DemoWebApi.Api.IntegrationTests.Controllers.DemoTasksController;

[TestFixture]
public class CompleteDemoTaskTests : IntegrationTestBase
{
    [Test]
    public async Task Complete_WhenInvalidTaskId_ReturnsBadRequest()
    {
        await GivenADemoTask(null);
        var request = GivenARequest(DateTime.UtcNow);
        await WhenCallingEndpoint("taskId", request);
        ThenReturnsBadRequest();
        await ThenReturnsErrorMessage($"TaskId taskId not a Guid");
    }
    
    [Test]
    public async Task Complete_WhenTaskCompleted_ReturnsBadRequest()
    {
        var demoTask = await GivenADemoTask(Fixture.Create<DateTime>());
        var request = GivenARequest(DateTime.UtcNow);
        await WhenCallingEndpoint(demoTask.Id.ToString(), request);
        ThenReturnsBadRequest();
        await ThenReturnsErrorMessage($"Task is already completed.");
    }
    
    [Test]
    public async Task Complete_WhenTaskNotCompleted_ReturnsOk()
    {
        var demoTask = await GivenADemoTask(null);
        var request = GivenARequest(DateTime.UtcNow);
        await WhenCallingEndpoint(demoTask.Id.ToString(), request);
        ThenReturnsOk();
    }

    private static CompleteDemoTaskRequest GivenARequest(DateTime completionDate)
    {
        var request = new CompleteDemoTaskRequest(completionDate);
        return request;
    }
    
    private async Task<DemoTask> GivenADemoTask(DateTime? completionDate)
    {
        var builder = new DemoTaskBuilder()
            .WithName(Fixture.Create<string>())
            .WithDueDate(Fixture.Create<DateTime>())
            .WithCompletedDate(completionDate)
            .Build();

        var demoTask = builder.Value;
        
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DemoWebApiContext>();
        await dbContext.DemoTasks.AddAsync(demoTask);
        await dbContext.SaveChangesAsync();
        
        return demoTask;
    }

    private async Task WhenCallingEndpoint(string demoTaskId, CompleteDemoTaskRequest request)
    {
        var content = JsonContent.Create(request);
        Response = await Sut.PatchAsync($"/api/DemoTasks/{demoTaskId}", content);
    }

    private async Task ThenReturnsDemoTaskId()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DemoWebApiContext>();
        var demoTask = await dbContext.DemoTasks.FirstOrDefaultAsync();
        
        var createDemoTaskResponse = await Response.Content.ReadFromJsonAsync<CreateDemoTaskResponse>();
        createDemoTaskResponse.Should().BeEquivalentTo(new CreateDemoTaskResponse(demoTask!.Id));
    }
}