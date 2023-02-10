namespace Recommendation.Application.Common.Exceptions;

public class RecordExistsException : Exception
{
    public RecordExistsException(object record)
        : base($"Record '{record}' already exists")
    {
    }
}