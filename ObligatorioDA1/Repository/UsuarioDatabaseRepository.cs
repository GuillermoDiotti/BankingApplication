using Dominio;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class UsuarioDatabaseRepository : IRepository<Usuario>
{

    private SqlContext _context;
    
    public UsuarioDatabaseRepository(SqlContext context)
    {
        _context = context;    
    }
    
    public Usuario Add(Usuario oneElement)
    {
        _context.Usuarios.Add(oneElement);
        _context.SaveChanges();
        return oneElement;
    }

    public Usuario? Find(Func<Usuario, bool> filter)
    {
        return _context.Usuarios
            .Include(u => u.espacios)
            .Include(u => u.espaciosAdministrados)
            .Where(filter).FirstOrDefault();

    }

    public IList<Usuario> FindAll()
    {
        return _context.Usuarios
            .Include(u => u.espacios)
            .Include(u => u.espaciosAdministrados)
            .ToList();
    }

    public Usuario? Update(Usuario updatedEntity)
    {
        Usuario findUser = _context.Usuarios.FirstOrDefault(x => x.Id == updatedEntity.Id);
        actualizar(findUser, updatedEntity);
        _context.SaveChanges();
        return findUser;
    }

    private void actualizar(Usuario findUser, Usuario updatedEntity)
    {
        findUser.Name = updatedEntity.Name;
        findUser.LastName = updatedEntity.LastName;
        findUser.Password = updatedEntity.Password;
        findUser.Address = updatedEntity.Address;
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }
}