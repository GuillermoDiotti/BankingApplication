using Dominio;

namespace Repository;

public class UserMemoryRepository : IRepository<Usuario>
{
    private List<Usuario> _usuarios= new List<Usuario>();

    public Usuario Add(Usuario oneElement)
    {
        oneElement.Id = _usuarios.OrderByDescending(x => x.Id)
            .Select(x => x.Id)
            .FirstOrDefault() + 1;
        _usuarios.Add(oneElement);
        return oneElement;
    }

    public Usuario? Find(Func<Usuario, bool> filter)
    {
        return _usuarios.Where(filter).FirstOrDefault();

    }

    public IList<Usuario> FindAll()
    {
        return _usuarios;
    }

    public Usuario? Update(Usuario updatedEntity)
    {
        Usuario findUser = Find(x => x.Mail == updatedEntity.Mail);
        findUser = updatedEntity;
        return findUser;
    }

    public void Delete(int id)
    {
        var x = _usuarios.Find(x => x.Id == id);
        _usuarios.Remove(x);
    }
}