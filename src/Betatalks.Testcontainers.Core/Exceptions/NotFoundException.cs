namespace Betatalks.Testcontainers.Core.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(Guid id, string entityName) : this($"No {entityName} with id {id} can be found.") { }

    public NotFoundException() : base() { }

    public NotFoundException(string message) : base(message) { }

    public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
