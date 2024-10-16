using Dominio;

namespace Repository;

public class CuentaMemoryRepository : IRepository<Cuenta>
{
    private List<Cuenta> _cuentas = new List<Cuenta>();

    public Cuenta Add(Cuenta oneElement)
    {
        oneElement.Id = _cuentas.OrderByDescending(x => x.Id)
            .Select(x => x.Id)
            .FirstOrDefault() + 1;
        _cuentas.Add(oneElement);
        return oneElement;
    }

    public Cuenta? Find(Func<Cuenta, bool> filter)
    {
        return _cuentas.Where(filter).FirstOrDefault();
    }

    public IList<Cuenta> FindAll()
    {
        return _cuentas;
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
        var x = _cuentas.Find(x => x.Id == id);
        _cuentas.Remove(x);
    }
}