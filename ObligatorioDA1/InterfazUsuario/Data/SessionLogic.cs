using Dominio;

namespace InterfazUsuario.Data;

public class SessionLogic
{
    public Usuario UsuarioActivo { get; set; }
    public Espacio EspacioActivo { get; set; }
    
    public void Login(Usuario usuario)
    {
        UsuarioActivo = usuario;
    }
    
    public void LogOut()
    {
        UsuarioActivo = null;
    }
    
    public void IngresarAEspacio(Espacio space)
    {
        EspacioActivo = space;
    }
}