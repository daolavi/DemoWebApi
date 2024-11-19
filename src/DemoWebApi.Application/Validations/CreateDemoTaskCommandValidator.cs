using DemoWebApi.Application.Commands;
using FluentValidation;

namespace DemoWebApi.Application.Validations;

public class CreateDemoTaskCommandValidator : AbstractValidator<CreateDemoTaskCommand>
{
    public CreateDemoTaskCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Name must be between 1 and 200 characters");
        
        RuleFor(x => new {x.IsDone, x.CompletionDate})
            .Must(x => HaveCompletionDateWhenDone(x.IsDone, x.CompletionDate))
            .WithMessage("CompletionDate and IsDone set to true are required for a completed task");
        
        RuleFor(x => x.DueDate).NotEmpty().WithMessage("DueDate is required");
    }

    private static bool HaveCompletionDateWhenDone(bool isDone, DateTime? completionDate)
        => isDone && completionDate.HasValue;
}