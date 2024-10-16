namespace Logica;

public class LogicException : Exception
{
    public string mensaje;
    
    public LogicException(string? message) : base (message)
    {
        mensaje = message;
    }
}