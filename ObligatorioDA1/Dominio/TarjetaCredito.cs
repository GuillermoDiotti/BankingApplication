namespace Dominio;

public class TarjetaCredito:Cuenta
{
    public string UltimosDigitos { get; set; }
    public string BancoEmisor { get; set; }
    public DateTime FechaCierre { get; set; }
    
    public double CreditoDisponible { get; set; }
    
   
}