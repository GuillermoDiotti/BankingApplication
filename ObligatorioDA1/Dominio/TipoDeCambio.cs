namespace Dominio;

public class TipoDeCambio
{
    public int Id { get; set; }
    public string Moneda { get; set; }
    public double Cotizacion { get; set; }
    public DateTime Fecha { get; set; }
    public Espacio Espacio { get; set; }

}