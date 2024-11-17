using System.Net;
using AutoFixture;
using DemoWebApi.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace DemoWebApi.Api.IntegrationTests.Controllers;

public class IntegrationTestBase
{
    protected DemoWebApiApplicationFactory<Program> Factory = null!;
    protected HttpClient Sut = null!;
    protected HttpResponseMessage Response = null!;
    protected readonly Fixture Fixture = new();

    [SetUp]
    protected async Task Setup()
    {
        Factory = new DemoWebApiApplicationFactory<Program>();
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DemoWebApiContext>();
        await dbContext.Database.EnsureCreatedAsync();
        
        Sut = Factory.CreateClient();
    }
    
    [TearDown]
    protected async Task TearDown()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DemoWebApiContext>();
        await dbContext.Database.EnsureDeletedAsync();
        
        Sut.Dispose();
        await Factory.DisposeAsync();
    }

    protected void ThenReturnsOk()
    {
        Response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    protected void ThenReturnsNotFound()
    {
        Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    protected void ThenReturnsBadRequest()
    {
        Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    protected async Task ThenReturnsErrorMessage(string expectedMessage)
    {
        var message = await Response.Content.ReadAsStringAsync();
        message.Should().Be(expectedMessage);
    }
}