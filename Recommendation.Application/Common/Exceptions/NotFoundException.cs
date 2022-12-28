namespace Recommendation.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string name, object? key)
        : base($"Entity '{name}' ({key ?? "null"}) not found.")
    {
    }
    
    public NotFoundException(string message)
        : base(message)
    {
    }
}