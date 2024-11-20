using System.Net.Http.Json;
using AutoFixture;
using DemoWebApi.Contracts.Requests;
using DemoWebApi.Contracts.Responses;
using DemoWebApi.Infrastructure;
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
        var request = GivenARequest(null!, Fixture.Create<DateTime>(),
            Fixture.Create<DateTime>());
        await WhenCallingEndpoint(request);
        ThenReturnsBadRequest();
        await ThenReturnsErrorMessage($"The Name field is required.");
    }
    
    [Test]
    public async Task Create_WhenDueDateIsNull_ReturnsBadRequest()
    {
        var request = GivenARequest(Fixture.Create<string>(), null,Fixture.Create<DateTime>());
        await WhenCallingEndpoint(request);
        ThenReturnsBadRequest();
        await ThenReturnsErrorMessage($"DueDate is required");
    }
    
    [Test]
    public async Task Create_WhenValid_ReturnsOk()
    {
        var request = GivenARequest(Fixture.Create<string>(), Fixture.Create<DateTime>(),
            Fixture.Create<DateTime>());
        await WhenCallingEndpoint(request);
        ThenReturnsOk();
        await ThenReturnsDemoTaskId();
    }

    private static CreateDemoTaskRequest GivenARequest(string name, DateTime? dueDate, DateTime? completionDate)
    {
        var request = new CreateDemoTaskRequest(name, dueDate, completionDate);
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