using Dominio;

namespace Repository;

public class TransaccionMemoryRepository : IRepository<Transaccion>
{
    private List<Transaccion> _transaccions = new List<Transaccion>();

    public Transaccion Add(Transaccion oneElement)
    {
        oneElement.Id = _transaccions.OrderByDescending(x => x.Id)
            .Select(x => x.Id)
            .FirstOrDefault() + 1;
        _transaccions.Add(oneElement);
        return oneElement;
    }

    public Transaccion? Find(Func<Transaccion, bool> filter)
    {
        return _transaccions.Where(filter).FirstOrDefault();
    }

    public IList<Transaccion> FindAll()
    {
        return _transaccions;
    }

    public Transaccion? Update(Transaccion updatedEntity)
    {
        Transaccion findTransaccion = Find(x => x.Id == updatedEntity.Id);
        actualizar(findTransaccion, updatedEntity);
        return findTransaccion;
    }

    private void actualizar(Transaccion findTransaccion, Transaccion updatedEntity)
    {
        findTransaccion.Monto = updatedEntity.Monto;
        findTransaccion.Moneda = updatedEntity.Moneda;
    }

    public void Delete(int id)
    {
        var x = _transaccions.Find(x => x.Id == id);
        _transaccions.Remove(x);
    } 
}