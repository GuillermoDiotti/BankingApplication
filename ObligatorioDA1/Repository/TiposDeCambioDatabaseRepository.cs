using Dominio;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class TiposDeCambioDatabaseRepository : IRepository<TipoDeCambio>
{
    private SqlContext _context;
    
    public TiposDeCambioDatabaseRepository(SqlContext context)
    {
        _context = context;    
    }

    public TipoDeCambio Add(TipoDeCambio oneElement)
    {
        _context.TiposDeCambio.Add(oneElement);
        _context.SaveChanges();
        return oneElement;
    }

    public TipoDeCambio? Find(Func<TipoDeCambio, bool> filter)
    {
        return _context.TiposDeCambio.Include(t => t.Espacio).Where(filter).FirstOrDefault();
    }

    IList<TipoDeCambio> IRepository<TipoDeCambio>.FindAll()
    {
        return _context.TiposDeCambio.Include(t => t.Espacio).ToList();
    }

    public TipoDeCambio? Update(TipoDeCambio updatedEntity)
    {
        TipoDeCambio findCambio = Find(x => x.Id == updatedEntity.Id);
        actualizar(findCambio, updatedEntity);
        _context.SaveChanges();
        return findCambio;
    }
    
    private void actualizar(TipoDeCambio findCambio, TipoDeCambio updatedEntity)
    {
        findCambio.Cotizacion = updatedEntity.Cotizacion;
    }
    
    public void Delete(int id)
    {
        TipoDeCambio findCuentaMonetaria = Find(x => x.Id == id);
        _context.TiposDeCambio.RemoveRange(findCuentaMonetaria);
        _context.SaveChanges();
    }
}