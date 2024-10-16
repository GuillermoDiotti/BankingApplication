using Microsoft.Win32.SafeHandles;

namespace Dominio;

public class Espacio
{
    public int Id { get; set; }
    public Usuario AdminEspacio { get; set; }
    public int AdminEspacioId { get; set; }
    public List<Usuario> MiembrosEspacio = new List<Usuario>();
    public string NombreEspacio { get; set; }
}