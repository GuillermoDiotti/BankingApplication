using Dominio;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class ObjetivosGastosDatabaseRepository : IRepository<ObjetivosGastos>
{
    private SqlContext _context;
    
    public ObjetivosGastosDatabaseRepository(SqlContext context)
    {
        _context = context;    
    }

    public ObjetivosGastos Add(ObjetivosGastos oneElement)
    {
        _context.Objetivos.Add(oneElement);
        _context.SaveChanges();
        return oneElement;
    }

    public ObjetivosGastos? Find(Func<ObjetivosGastos, bool> filter)
    {
        return _context.Objetivos.
            Include(obj => obj.Categorias).
            Include(obj => obj.Espacio).
            Include(obj => obj.UsuarioCreador).
            Where(filter).FirstOrDefault();
    }

    IList<ObjetivosGastos> IRepository<ObjetivosGastos>.FindAll()
    {
        return _context.Objetivos.
            Include(obj => obj.Categorias).
            Include(obj => obj.Espacio).
            Include(obj => obj.UsuarioCreador).
            ToList();
    }

    public ObjetivosGastos? Update(ObjetivosGastos updatedEntity)
    {
        ObjetivosGastos findObj = Find(x => x.Id == updatedEntity.Id);
        actualizar(findObj,updatedEntity );
        _context.SaveChanges();
        return findObj;
    }
    
    private void actualizar(ObjetivosGastos finSpace, ObjetivosGastos updatedEntity)
    {
        finSpace.URL = updatedEntity.URL;
    }
    
    public void Delete(int id)
    {
        ObjetivosGastos findObj = Find(x => x.Id == id);
        _context.Objetivos.RemoveRange(findObj);
        _context.SaveChanges();
    }
}