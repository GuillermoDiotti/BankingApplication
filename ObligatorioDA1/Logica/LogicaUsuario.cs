using System.Runtime.CompilerServices;
using Dominio;
using Repository;

namespace Logica;

public class LogicaUsuario
{
    private readonly IRepository<Usuario> _repository;
    
    public LogicaUsuario(IRepository<Usuario> userRepository)
    {
        _repository = userRepository;
    }
    
    public bool MailUnico(string mail)
    {
        Usuario x = _repository.Find(usu => usu.Mail == mail);
        return x is null;
    }
    
    public void IniciarSesion(string mail, string pass)
    {
        Usuario? usuarioExiste = _repository.Find(usuario => (usuario.Mail == mail && usuario.Password == pass));
        if (usuarioExiste is null) 
        { 
            throw new LogicException("Los datos ingresados son incorrectos");
        }
    }

    public Usuario ConseguirUsuarioPorId(int id)
    {
       return  _repository.Find(x => x.Id == id);
    }

    public Usuario ConseguirUsuarioPorMail(string mail)
    {
        return  _repository.Find(x => x.Mail == mail);
    }

    public void AgregarUsuario(Usuario nuevoUsuario)
    {
        if (MailUnico(nuevoUsuario.Mail))
        {
            _repository.Add(nuevoUsuario);
        }
        else
        {
            throw new LogicException("El mail ingresado ya existe");
        }
    }

    public void Update(Usuario updatedUser)
    {
        _repository.Update(updatedUser);
    }

    public Usuario FindByMail(string mail)
    {
        return _repository.Find(x => x.Mail == mail);
    }

    public IList<Usuario> FindAllUsers()
    {
        return _repository.FindAll();
    }
    
    
}