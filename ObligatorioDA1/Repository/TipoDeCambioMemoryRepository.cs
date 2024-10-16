using Dominio;

namespace Repository;

public class TipoDeCambioMemoryRepository : IRepository<TipoDeCambio>
{
    private List<TipoDeCambio> _tipoDeCambios = new List<TipoDeCambio>();

    public TipoDeCambio Add(TipoDeCambio oneElement)
    {
        oneElement.Id = _tipoDeCambios.OrderByDescending(x => x.Id)
            .Select(x => x.Id)
            .FirstOrDefault() + 1;
        _tipoDeCambios.Add(oneElement);
        return oneElement;
    }

    public TipoDeCambio? Find(Func<TipoDeCambio, bool> filter)
    {
        return _tipoDeCambios.Where(filter).FirstOrDefault();
    }

    public IList<TipoDeCambio> FindAll()
    {
        return _tipoDeCambios;
    }

    public TipoDeCambio? Update(TipoDeCambio updatedEntity)
    {
        TipoDeCambio findCambio = Find(x => x.Id == updatedEntity.Id);
        findCambio = updatedEntity;
        return findCambio;
    }

    public void Delete(int id)
    {
        var x = _tipoDeCambios.Find(x => x.Id == id);
        _tipoDeCambios.Remove(x);
    } 
}