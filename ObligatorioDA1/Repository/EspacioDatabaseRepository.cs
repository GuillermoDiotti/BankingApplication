using Dominio;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class EspacioDatabaseRepository : IRepository<Espacio>
{
    private SqlContext _context;
    
    public EspacioDatabaseRepository(SqlContext context)
    {
        _context = context;    
    }

    public Espacio Add(Espacio oneElement)
    {
        _context.Espacios.Add(oneElement);
        _context.SaveChanges();
        return oneElement;
    }

    public Espacio? Find(Func<Espacio, bool> filter)
    {
        return _context.Espacios.Include(x => x.AdminEspacio).Include(x => x.MiembrosEspacio).Where(filter).FirstOrDefault();
    }

    IList<Espacio> IRepository<Espacio>.FindAll()
    {
        return _context.Espacios.Include(x=>x.AdminEspacio).Include(x=>x.MiembrosEspacio).ToList();
    }

    public Espacio? Update(Espacio updatedEntity)
    {
        Espacio findSpace = Find(x => x.Id == updatedEntity.Id);
        actualizar(findSpace, updatedEntity);
        _context.SaveChanges();
        return findSpace;
    }
    
    private void actualizar(Espacio finSpace, Espacio updatedEntity)
    {
        finSpace.NombreEspacio = updatedEntity.NombreEspacio;
        finSpace.MiembrosEspacio = updatedEntity.MiembrosEspacio;
    }
    
    public void Delete(int id)
    {
        Espacio findSpace = Find(x => x.Id == id);
        _context.Espacios.RemoveRange(findSpace);
        _context.SaveChanges();
    }
    
}