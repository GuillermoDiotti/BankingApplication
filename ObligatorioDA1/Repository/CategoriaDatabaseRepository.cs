using Dominio;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class CategoriaDatabaseRepository : IRepository<Categoria>
{
    private SqlContext _context;
    
    public CategoriaDatabaseRepository(SqlContext context)
    {
        _context = context;    
    }

    public Categoria Add(Categoria oneElement)
    {
        _context.Categorias.Add(oneElement);
        _context.SaveChanges();
        return oneElement;
    }

    public Categoria? Find(Func<Categoria, bool> filter)
    {
        return _context.Categorias
            .Include(categoria => categoria.Espacio)
            .Include(categoria =>categoria.ObjetivosGastosList)
            .Where(filter).FirstOrDefault();
    }

    IList<Categoria> IRepository<Categoria>.FindAll()
    {
        return _context.Categorias
            .Include(categoria => categoria.Espacio)
            .Include(categoria =>categoria.ObjetivosGastosList)
            .ToList();
    }

    public Categoria? Update(Categoria updatedEntity)
    {
        Categoria findCategory = Find(x => x.Id == updatedEntity.Id);
        actualizar(findCategory, updatedEntity);
        _context.SaveChanges();
        return findCategory;
    }
    
    private void actualizar(Categoria findCategory, Categoria updatedEntity)
    {
        findCategory.Estatus = updatedEntity.Estatus;
        findCategory.Tipo = updatedEntity.Tipo;
    }
    
    public void Delete(int id)
    {
        Categoria findSpace = Find(x => x.Id == id);
        _context.Categorias.RemoveRange(findSpace);
        _context.SaveChanges();
    }
}