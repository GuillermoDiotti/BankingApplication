namespace Dominio;

public class ObjetivosGastos
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public double MontoMaximo { get; set; }
    
    public List<Categoria>? Categorias = new List<Categoria>();
    public double GastoActual { get; set; }
    
    public string? URL { get; set; }
    
    public bool URLHabilitada { get; set; }
    
    public Usuario? UsuarioCreador { get; set; }
    
    public Espacio? Espacio { get; set; }
    
}