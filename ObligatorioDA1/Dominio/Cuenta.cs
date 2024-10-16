namespace Dominio;

public abstract class Cuenta
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Moneda { get; set; }
    public DateTime FechaCreacion { get; set; }
    
    public Espacio Espacio { get; set; }

    
    
}