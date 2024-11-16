namespace DemoWebApi.Domain.Exceptions;

public class DemoTaskDomainException : Exception
{
    public DemoTaskDomainException()
    { }

    public DemoTaskDomainException(string message)
        : base(message)
    { }

    public DemoTaskDomainException(string message, Exception innerException)
        : base(message, innerException)
    { }
}