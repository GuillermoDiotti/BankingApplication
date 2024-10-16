using Dominio;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class CuentaDatabaseRepository : IRepository<Cuenta>
{
    private SqlContext _context;
    
    public CuentaDatabaseRepository(SqlContext context)
    {
        _context = context;    
    }
    
    public Cuenta Add(Cuenta oneElement)
    {
        _context.Cuentas.Add(oneElement);
        _context.SaveChanges();
        return oneElement;
    }

    public Cuenta? Find(Func<Cuenta, bool> filter)
    {
        return _context.Cuentas.Include(cuenta => cuenta.Espacio).Where(filter).FirstOrDefault();
    }

    public IList<Cuenta> FindAll()
    {
        return _context.Cuentas.Include(cuenta => cuenta.Espacio).ToList();
    }

    public Cuenta? Update(Cuenta updatedEntity)
    {
        Cuenta findCuenta = Find(x => x.Id == updatedEntity.Id);
        if (findCuenta.GetType() == typeof(CuentaMonetaria))
        {
            actualizarCuentaMonetaria(findCuenta as CuentaMonetaria, updatedEntity as CuentaMonetaria);
        }
        else
        {
            actualizarTarjeta(findCuenta as TarjetaCredito, updatedEntity as TarjetaCredito);
        }
        _context.SaveChanges();
        return findCuenta;
    }

    private void actualizarCuentaMonetaria(CuentaMonetaria findCuenta, CuentaMonetaria updatedEntity)
    {
        findCuenta.Nombre = updatedEntity.Nombre;
    }
    
    private void actualizarTarjeta(TarjetaCredito findCuenta, TarjetaCredito updatedEntity)
    {
        findCuenta.BancoEmisor = updatedEntity.BancoEmisor;
        findCuenta.UltimosDigitos = updatedEntity.UltimosDigitos;
        findCuenta.FechaCierre = updatedEntity.FechaCierre;
    }


    public void Delete(int id)
    {
        Cuenta findCuentaMonetaria = Find(x => x.Id == id);
        _context.Cuentas.RemoveRange(findCuentaMonetaria);
        _context.SaveChanges();
    }
}