using AutoFixture;
using DemoWebApi.Tests.Common;
using FluentAssertions;

namespace DemoWebApi.Domain.UnitTests;

[TestFixture]
public class DemoTaskTests
{
    private readonly Fixture _fixture = new();
    
    [Test]
    public void Create_WhenNameIsNull_ReturnsError()
    {
        var builder = new DemoTaskBuilder()
            .WithName(null!)
            .WithDueDate(_fixture.Create<DateTime>())
            .WithCompletedDate(_fixture.Create<DateTime>())
            .Build();

        builder.IsFailed.Should().BeTrue();
        builder.Errors.Should().Contain(e => e.Message.Contains("Name is required."));
    }

    [Test]
    public void Create_WhenDueDateIsNull_ReturnsError()
    {
        var builder = new DemoTaskBuilder()
            .WithName("demo")
            .WithDueDate(null)
            .WithCompletedDate(_fixture.Create<DateTime>())
            .Build();

        builder.IsFailed.Should().BeTrue();
        builder.Errors.Should().Contain(e => e.Message.Contains("DueDate is required."));
    }
    
    [Test]
    public void Create_WhenDueDateAndNameAreNull_ReturnsError()
    {
        var builder = new DemoTaskBuilder()
            .WithName(null)
            .WithDueDate(null)
            .WithCompletedDate(_fixture.Create<DateTime>())
            .Build();

        builder.IsFailed.Should().BeTrue();
        builder.Errors.Should().Contain(e => e.Message.Contains("Name is required."));
        builder.Errors.Should().Contain(e => e.Message.Contains("DueDate is required."));
    }
    
    [Test]
    public void Create_WhenValid_ReturnsDemoTask()
    {
        var name = "demo";
        var completedDate = _fixture.Create<DateTime>();
        var dueDate = _fixture.Create<DateTime>();
        
        var builder = new DemoTaskBuilder()
            .WithName(name)
            .WithDueDate(dueDate)
            .WithCompletedDate(completedDate)
            .Build();
        
        builder.IsSuccess.Should().BeTrue();
        var demoTask = builder.Value;
        demoTask.Name.Should().Be(name);
        demoTask.CompletionDate.Should().Be(completedDate);
        demoTask.DueDate.Should().Be(dueDate);
    }
    
    [Test]
    public void Complete_WhenTaskCompleted_ReturnsError()
    {
        var builder = new DemoTaskBuilder()
            .WithName(_fixture.Create<string>())
            .WithDueDate(_fixture.Create<DateTime>())
            .WithCompletedDate(_fixture.Create<DateTime>())
            .Build();
        
        builder.IsSuccess.Should().BeTrue();
        var demoTask = builder.Value;

        var completeTask = demoTask.Complete(DateTime.UtcNow);
        completeTask.IsFailed.Should().BeTrue();
        completeTask.Errors.Should().Contain(e => e.Message.Contains("Task is already completed."));
    }
    
    [Test]
    public void Complete_WhenTaskNotCompleted_ReturnsOk()
    {
        var builder = new DemoTaskBuilder()
            .WithName(_fixture.Create<string>())
            .WithDueDate(_fixture.Create<DateTime>())
            .WithCompletedDate(null)
            .Build();
        
        builder.IsSuccess.Should().BeTrue();
        var demoTask = builder.Value;

        var completeTask = demoTask.Complete(DateTime.UtcNow);
        completeTask.IsSuccess.Should().BeTrue();
    }
}