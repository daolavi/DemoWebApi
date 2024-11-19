using System.Net.Http.Json;
using AutoFixture;
using DemoWebApi.Application.Converters;
using DemoWebApi.Contracts.Responses;
using DemoWebApi.Domain.AggregatesModel.DemoTaskAggregate;
using DemoWebApi.Infrastructure;
using DemoWebApi.Tests.Common;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace DemoWebApi.Api.IntegrationTests.Controllers.DemoTasksController;

[TestFixture]
public class GetDemoTaskByIdTests : IntegrationTestBase
{
    [Test]
    public async Task Get_WhenInvalidTaskId_ReturnsBadRequest()
    {
        var demoTaskId = "123";
        await WhenCallingEndpoint(demoTaskId);
        ThenReturnsBadRequest();
        await ThenReturnsErrorMessage($"TaskId {demoTaskId} not a Guid");
    }
    
    [Test]
    public async Task Get_WhenDemoTaskDoesNotExist_ReturnsNotFound()
    {
        var demoTaskId = Guid.NewGuid();
        await WhenCallingEndpoint(demoTaskId.ToString());
        ThenReturnsNotFound();
        await ThenReturnsErrorMessage($"TaskId {demoTaskId} not found");
    }
    
    [Test]
    public async Task Get_WhenDemoTaskExists_ReturnsOk()
    {
        var demoTask = await GivenADemoTask();
        await WhenCallingEndpoint(demoTask.Id.ToString());
        ThenReturnsOk();
        await ThenReturnsDemoTaskDto(demoTask.ToContract());
    }

    private async Task<DemoTask> GivenADemoTask()
    {
        var builder = new DemoTaskBuilder()
            .WithName(Fixture.Create<string>())
            .WithDueDate(Fixture.Create<DateTime>())
            .WithCompletedDate(Fixture.Create<DateTime>())
            .Build();

        var demoTask = builder.Value;
        
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DemoWebApiContext>();
        await dbContext.DemoTasks.AddAsync(demoTask);
        await dbContext.SaveChangesAsync();
        
        return demoTask;
    }

    private async Task WhenCallingEndpoint(string demoTaskId)
    {
        Response = await Sut.GetAsync($"/api/DemoTasks/{demoTaskId}");
    }

    private async Task ThenReturnsDemoTaskDto(DemoTaskDto expectedDemoTaskDto)
    {
        var demoTask = await Response.Content.ReadFromJsonAsync<DemoTaskDto>();
        demoTask.Should().BeEquivalentTo(expectedDemoTaskDto);
    }
}