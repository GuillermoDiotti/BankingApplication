using Dominio;

namespace Repository;

public class ObjetivosGastosMemoryRepository : IRepository<ObjetivosGastos>
{
    private List<ObjetivosGastos> _objetivosGastos = new List<ObjetivosGastos>();

    public ObjetivosGastos Add(ObjetivosGastos oneElement)
    {
        oneElement.Id = _objetivosGastos.OrderByDescending(x => x.Id)
            .Select(x => x.Id)
            .FirstOrDefault() + 1;
        _objetivosGastos.Add(oneElement);
        return oneElement;
    }

    public ObjetivosGastos? Find(Func<ObjetivosGastos, bool> filter)
    {
        return _objetivosGastos.Where(filter).FirstOrDefault();
    }

    public IList<ObjetivosGastos> FindAll()
    {
        return _objetivosGastos.ToList();
    }

    public ObjetivosGastos? Update(ObjetivosGastos updatedEntity)
    {
        ObjetivosGastos findObj = Find(x => x.Id == updatedEntity.Id);
        findObj = updatedEntity;
        return findObj;
    }

    public void Delete(int id)
    {
        var x = _objetivosGastos.Find(x => x.Id == id);
        _objetivosGastos.Remove(x);
    } 
}