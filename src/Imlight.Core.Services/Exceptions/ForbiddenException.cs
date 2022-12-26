namespace Imlight.Core.Services.Exceptions;

public class ForbiddenException : ApiException
{
    public ForbiddenException()
    {
    }

    public ForbiddenException(string message) : base(message)
    {
    }
}