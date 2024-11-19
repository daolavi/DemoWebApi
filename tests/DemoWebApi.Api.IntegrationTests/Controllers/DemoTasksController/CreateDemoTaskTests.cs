using System.Net.Http.Json;
using AutoFixture;
using DemoWebApi.Application.Converters;
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
public class CreateDemoTaskTests : IntegrationTestBase
{
    [Test]
    public async Task Create_WhenNameIsNull_ReturnsBadRequest()
    {
        var request = GivenARequest(null!, Fixture.Create<bool>(), Fixture.Create<DateTime>(),
            Fixture.Create<DateTime>());
        await WhenCallingEndpoint(request);
        ThenReturnsBadRequest();
        await ThenReturnsErrorMessage($"The Name field is required.");
    }
    
    [Test]
    public async Task Create_WhenTaskCompletedAndCompletionDateIsNull_ReturnsBadRequest()
    {
        var request = GivenARequest(Fixture.Create<string>(), true, Fixture.Create<DateTime>(),
            null);
        await WhenCallingEndpoint(request);
        ThenReturnsBadRequest();
        await ThenReturnsErrorMessage($"CompletionDate and IsDone set to true are required for a completed task");
    }
    
    [Test]
    public async Task Create_WhenDueDateIsNull_ReturnsBadRequest()
    {
        var request = GivenARequest(Fixture.Create<string>(), true, null,Fixture.Create<DateTime>());
        await WhenCallingEndpoint(request);
        ThenReturnsBadRequest();
        await ThenReturnsErrorMessage($"DueDate is required");
    }
    
    [Test]
    public async Task Create_WhenValid_ReturnsOk()
    {
        var request = GivenARequest(Fixture.Create<string>(), true, Fixture.Create<DateTime>(),
            Fixture.Create<DateTime>());
        await WhenCallingEndpoint(request);
        ThenReturnsOk();
        await ThenReturnsDemoTaskId();
    }

    private static CreateDemoTaskRequest GivenARequest(string name, bool isDone, DateTime? dueDate, DateTime? completionDate)
    {
        var request = new CreateDemoTaskRequest(name, isDone, dueDate, completionDate);
        return request;
    }

    private async Task WhenCallingEndpoint(CreateDemoTaskRequest request)
    {
        var content = JsonContent.Create(request);
        Response = await Sut.PostAsync($"/api/DemoTasks", content);
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