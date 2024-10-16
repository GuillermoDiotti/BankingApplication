using System.Xml;
using Dominio;
using Repository;

namespace Logica;

public class LogicaEspacio
{
    private readonly IRepository<Espacio> _repository;
    
    public LogicaEspacio(IRepository<Espacio> espacioRepository)
    {
        _repository = espacioRepository;
    }
    
    public Espacio? CrearEspacio(Espacio espacioNuevo)
    {
        if(string.IsNullOrEmpty(espacioNuevo.NombreEspacio))
        {
            throw new LogicException("El nombre del espacio no puede estar vacio");
        }
        if (!NombreEspacioDeUsuarioUnico(espacioNuevo.NombreEspacio, espacioNuevo.AdminEspacio))
        {
            throw new LogicException("El nombre del espacio ya existe");
        }
        _repository.Add(espacioNuevo);
        AgregarMiembro(espacioNuevo.AdminEspacio, espacioNuevo);
        return espacioNuevo;
    }
    
    public void AgregarMiembro(Usuario u, Espacio space)
    {
        if (ObtenerMiembrosEspacio(space).Contains(u))
        {
            throw new LogicException("El usuario ya es miembro del espacio");
        }   
        space.MiembrosEspacio.Add(u);
        ActualiarEspacio(space);
    }
    
    public void ActualiarEspacio(Espacio espacio)
    {
        if (string.IsNullOrEmpty(espacio.NombreEspacio))
        {
            throw new LogicException("El nombre del espacio no puede estar vacio");
        }
        _repository.Update(espacio);
    }
    
    public void EliminarMiembro(Usuario? u, Espacio space)
    {
        if (u == null)
        {
            throw new LogicException("Ingrese un usuario valido");
        }
        if (u == space.AdminEspacio)
        {
            throw new LogicException("No se puede eliminar al administrador del espacio");
        }
        if (space.MiembrosEspacio.Contains(u))
        {
            space.MiembrosEspacio.RemoveAll(x => x.Mail == u.Mail);
            ActualiarEspacio(space);
        }
        else
        {
            throw new LogicException("El usuario no forma parte del espacio");
        }
    }
    
    public List<Espacio> ObtenerListaEspacios()
    {
        return _repository.FindAll().ToList();
    }
    
    public Espacio? ObtenerEspacioPorId(int id)
    {
        return _repository.Find(x => x.Id == id);
    }

    public List<Espacio> ObtenerEspaciosPorUsuario(Usuario usuario)
    {
        List<Espacio> espaciosUsuario = new List<Espacio>();
        foreach (var space in _repository.FindAll())
        {
            if(space.AdminEspacio == usuario)
            {
                espaciosUsuario.Add(space);
            }
            else
            {
                foreach (var member in space.MiembrosEspacio)
                {
                    if (member == usuario)
                    {
                        espaciosUsuario.Add(space);
                    }
                }
            }
        }
        return espaciosUsuario;
    }
    
    public bool NombreEspacioDeUsuarioUnico(string nombre, Usuario user)
    {
        foreach (var space in ObtenerEspaciosPorUsuario(user))
        {
            if (space.NombreEspacio == nombre && space.AdminEspacio == user)
            {
                return false;
            }
        }
        return true;
    }
    
    public List<Usuario> ObtenerMiembrosEspacio(Espacio space)
    {
        return space.MiembrosEspacio;
    }
    
    public List<Usuario> ObtenerMiembrosFueraDelEspacio(Espacio space, LogicaUsuario _logicaUsuario)
    {
        List<Usuario> listaNueva = new List<Usuario>();
        List<Usuario> miembros = ObtenerMiembrosEspacio(space);
        foreach (var user in _logicaUsuario.FindAllUsers())
        {
            if(!miembros.Contains(user))
            {
                listaNueva.Add(user);
            }
        }
        return listaNueva;
    }

}