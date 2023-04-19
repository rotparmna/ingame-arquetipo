namespace ServicioUno.API.Infrastructure.Exceptions;
public class ServicioUnoDomainException : Exception
{
    public ServicioUnoDomainException()
    { }

    public ServicioUnoDomainException(string message)
        : base(message)
    { }

    public ServicioUnoDomainException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
