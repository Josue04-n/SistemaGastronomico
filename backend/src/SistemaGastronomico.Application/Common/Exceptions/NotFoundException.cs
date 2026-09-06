namespace SistemaGastronomico.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException()
        : base()
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public NotFoundException(string name, object key)
        : base($"La entidad \"{name}\" con identificador ({key}) no fue encontrada.")
    {
    }
}
