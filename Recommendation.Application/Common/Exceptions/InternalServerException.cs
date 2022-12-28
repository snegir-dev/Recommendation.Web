namespace Recommendation.Application.Common.Exceptions;

public class InternalServerException : Exception
{
    public InternalServerException(string message)
        : base(message)
    {
    }

    public InternalServerException(IEnumerable<object> errors)
        : base(CreateMessageError(errors))
    {
    }

    private static string CreateMessageError(IEnumerable<object> errors)
    {
        return errors.Aggregate("", (current, error) => current + error);
    }
}