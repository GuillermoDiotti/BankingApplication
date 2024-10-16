using Dominio;

namespace Repository;

public class EspacioMemoryRepository : IRepository<Espacio>
{
    private List<Espacio> _espacios = new List<Espacio>();

    public Espacio Add(Espacio oneElement)
    {
        oneElement.Id = _espacios.OrderByDescending(x => x.Id)
            .Select(x => x.Id)
            .FirstOrDefault() + 1;
        _espacios.Add(oneElement);
        return oneElement;
    }

    public Espacio? Find(Func<Espacio, bool> filter)
    {
        return _espacios.Where(filter).FirstOrDefault();
    }

    public IList<Espacio> FindAll()
    {
        return _espacios;
    }

    public Espacio? Update(Espacio updatedEntity)
    {
        Espacio findEspacio = Find(x => x.Id == updatedEntity.Id);
        findEspacio = updatedEntity;
        return findEspacio;
    }

    public void Delete(int id)
    {
        var x = _espacios.Find(x => x.Id == id);
        _espacios.Remove(x);
    }
}