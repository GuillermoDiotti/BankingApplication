namespace Logica;

public class DateTimeProvider:IDateTimeProvider
{
    public DateTime ObtenerFechaHoy()
    {
        return DateTime.Now;
    }
}