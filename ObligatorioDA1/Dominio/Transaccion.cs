namespace Dominio;

public class Transaccion
{
    public string Titulo { get; set; }
    
    public int Id { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now.Date;
    public double Monto { get; set; }
    public string Moneda { get; set; } = "UYU";
    public Cuenta Cuenta { get; set; }
    public Categoria Categoria { get; set; }
    public string TipoTransaccion { get; set; }
    
    public Espacio Espacio { get; set; }

}