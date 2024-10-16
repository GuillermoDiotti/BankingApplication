using Dominio;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class TransaccionDatabaseRepository : IRepository<Transaccion>
{
    private SqlContext _context;
    
    public TransaccionDatabaseRepository(SqlContext context)
    {
        _context = context;    
    }

    public Transaccion Add(Transaccion oneElement)
    {
        _context.Transacciones.Add(oneElement);
        _context.SaveChanges();
        return oneElement;
    }

    public Transaccion? Find(Func<Transaccion, bool> filter)
    {
        return _context.Transacciones
            .Include(t => t.Categoria)
            .Include(t => t.Cuenta)
            .Include(t => t.Espacio)
            .Where(filter).FirstOrDefault();
    }

    IList<Transaccion> IRepository<Transaccion>.FindAll()
    {
        return _context.Transacciones
            .Include(t => t.Categoria)
            .Include(t => t.Cuenta)
            .Include(t => t.Espacio)
            .ToList();
    }

    public Transaccion? Update(Transaccion updatedEntity)
    {
        Transaccion findTransaction = Find(x => x.Id == updatedEntity.Id);
        actualizar(findTransaction, updatedEntity);
        _context.SaveChanges();
        return findTransaction;
    }
    
    private void actualizar(Transaccion findTransaccion, Transaccion updatedEntity)
    {
        findTransaccion.Monto = updatedEntity.Monto;
        findTransaccion.Moneda = updatedEntity.Moneda;
    }
    
    public void Delete(int id)
    {
        Transaccion findTransaction = Find(x => x.Id == id);
        _context.Transacciones.RemoveRange(findTransaction);
        _context.SaveChanges();
    }
}