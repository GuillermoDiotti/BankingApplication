using Dominio;

namespace Repository;

public class CategoriaMemoryRepository : IRepository<Categoria>
{
    private List<Categoria> _categorias = new List<Categoria>();

    public Categoria Add(Categoria oneElement)
    {
        oneElement.Id = _categorias.OrderByDescending(x => x.Id)
            .Select(x => x.Id)
            .FirstOrDefault() + 1;
        _categorias.Add(oneElement);
        return oneElement;
    }

    public Categoria? Find(Func<Categoria, bool> filter)
    {
        return _categorias.Where(filter).FirstOrDefault();
    }

    public IList<Categoria> FindAll()
    {
        return _categorias;
    }

    public Categoria? Update(Categoria updatedEntity)
    {
        Categoria findCategory = Find(x => x.Id == updatedEntity.Id);
        actualizar(findCategory, updatedEntity);
        return findCategory;
    }

    private void actualizar(Categoria findCategory, Categoria updatedEntity)
    {
        findCategory.Estatus = updatedEntity.Estatus;
        findCategory.Tipo = updatedEntity.Tipo;
    }

    public void Delete(int id)
    {
        var x = _categorias.Find(x => x.Id == id);
        _categorias.Remove(x);
    }
}