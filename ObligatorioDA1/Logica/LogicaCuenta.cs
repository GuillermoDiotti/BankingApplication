using System.Text.RegularExpressions;
using Dominio;
using Repository;

namespace Logica;

public class LogicaCuenta
{
    private readonly IRepository<Cuenta> _repository;
    

    public LogicaCuenta(IRepository<Cuenta> Repository)
    {
        _repository = Repository;
    }
    
    public bool NombreUnico(Espacio space, string nombre)
    {
        Cuenta? x = ListarCuentas(space).Find(cuenta => cuenta.Nombre == nombre);
        return x == null;
    }

    public List<Cuenta> ListarCuentasMonetarias(Espacio space)
    {
        return _repository.FindAll().Where(x => x is CuentaMonetaria && x.Espacio == space).ToList();
    }
    
    public List<Cuenta> ListarTarjetas(Espacio space)
    {
        return _repository.FindAll().Where(x => x is TarjetaCredito && x.Espacio == space).ToList();
    }
    
    public List<Cuenta> ListarCuentas(Espacio space)
    {
        return _repository.FindAll().Where(x => x.Espacio == space).ToList();
    }
    
    public void AgregarTarjetaDeCredito(TarjetaCredito tarjeta, Espacio space)
    {
        _repository.Add(tarjeta);
    }

    public bool AgregarCuentaMonetaria(CuentaMonetaria cuenta, Espacio space)
    {
        ValidarInputs(cuenta.MontoInicial, space, cuenta);
        _repository.Add(cuenta);
        return true;
    }

    public void ValidarInputs(double monto, Espacio space, Cuenta cuenta)
    {
        if (!NombreUnico(space, cuenta.Nombre))
        {
            throw new LogicException("El nombre de la cuenta debe de ser unico");
        }
        if (monto < 0)
        {
            throw new LogicException("El monto inicial debe ser mayor o igual a cero");
        }
    }

    public void EliminarCuenta(Cuenta c, LogicaTransaccion logicaTransaccion)
    {
        if (TieneTransaccionAsociada(c.Espacio, c, logicaTransaccion))
        {
            throw new LogicException("La cuenta tiene transacciones asociadas, no se puede eliminar");
        }
        _repository.Delete(c.Id);
    }

    public void ModificarCuentaMonetaria(Cuenta c)
    {
        if(c.Nombre == null || c.Nombre.Equals(""))
            throw new LogicException("El nombre de la cuenta no puede ser vacio");
        _repository.Update(c);
    }
    
    public void ModificarTarjeta(Espacio space, Cuenta c)
    {
        _repository.Update(c);
    }

    public bool ChequearFormatoDigitos(string digitos)
    {
        return Regex.IsMatch(digitos, @"^\d{4}$");
    }

    public double CalcularSaldoDisponibleCuentasMonetarias(LogicaTipoDeCambio _logicaTipoDeCambio, Espacio e,
        CuentaMonetaria cuenta, LogicaTransaccion _logicaTransaccion)
    {
        double total = 0;
        double montoInicial = cuenta.MontoInicial;
        double sumatoriaIngresos = 0;
        double sumatoriaCostos = 0;
        if (cuenta.Moneda == "UYU")
        {
            return calcularSaldoCuentasUYU(_logicaTipoDeCambio, e, cuenta, sumatoriaIngresos, sumatoriaCostos, montoInicial, _logicaTransaccion);
        }
        return calcularSaldoCuentasExtranjeras(_logicaTipoDeCambio, e, cuenta, sumatoriaIngresos, sumatoriaCostos, montoInicial, _logicaTransaccion);
        
    }

    public double calcularSaldoCuentasExtranjeras(LogicaTipoDeCambio _logicaTipoDeCambio, Espacio e,
        CuentaMonetaria cuenta, double sumatoriaIngresos,
        double sumatoriaCostos, double montoInicial, LogicaTransaccion _logicaTransaccion)
    {
        double total;
        foreach (var transaccion in _logicaTransaccion.ListarTransacciones(e))
        {
            if (transaccion.TipoTransaccion == "Ingreso" && transaccion.Cuenta.Nombre.Equals(cuenta.Nombre))
            {
                double montoEnUYUs = _logicaTipoDeCambio.PasarAPesosUruguayos(e, transaccion.Moneda,
                    transaccion.Monto, transaccion.Fecha);
                double montoEnMonedaExtranjera =
                    _logicaTipoDeCambio.PasarDeUYUAMonedaDestino(e, montoEnUYUs, transaccion.Fecha, cuenta.Moneda);
                sumatoriaIngresos = sumatoriaIngresos + montoEnMonedaExtranjera;
            }
            else if (transaccion.TipoTransaccion == "Costo" && transaccion.Cuenta.Nombre.Equals(cuenta.Nombre))
            {
                double montoEnUYUs = _logicaTipoDeCambio.PasarAPesosUruguayos(e, transaccion.Moneda,
                    transaccion.Monto, transaccion.Fecha);
                double montoEnMonedaExtranjera =
                    _logicaTipoDeCambio.PasarDeUYUAMonedaDestino(e, montoEnUYUs, transaccion.Fecha, cuenta.Moneda);
                sumatoriaCostos = sumatoriaCostos + montoEnMonedaExtranjera;
            }
        }

        total = (montoInicial + sumatoriaIngresos) - sumatoriaCostos;
        return total;
    }

    public double calcularSaldoCuentasUYU(LogicaTipoDeCambio _logicaTipoDeCambio, Espacio e, CuentaMonetaria cuenta,
        double sumatoriaIngresos,
        double sumatoriaCostos, double montoInicial, LogicaTransaccion _logicaTransaccion)
    {
        double total;
        foreach (var transaccion in _logicaTransaccion.ListarTransacciones(e))
        {
            if (transaccion.TipoTransaccion == "Ingreso" && transaccion.Cuenta.Nombre.Equals(cuenta.Nombre))
            {
                double montoEnUYUs = _logicaTipoDeCambio.PasarAPesosUruguayos(e, transaccion.Moneda,
                    transaccion.Monto, transaccion.Fecha);
                sumatoriaIngresos = sumatoriaIngresos + montoEnUYUs;
            }
            else if (transaccion.TipoTransaccion == "Costo" && transaccion.Cuenta.Nombre.Equals(cuenta.Nombre))
            {
                double montoEnUYUs = _logicaTipoDeCambio.PasarAPesosUruguayos(e, transaccion.Moneda,
                    transaccion.Monto, transaccion.Fecha);
                sumatoriaCostos = sumatoriaCostos + montoEnUYUs;
            }
        }

        total = (montoInicial + sumatoriaIngresos) - sumatoriaCostos;
        return total;
    }


    public double CalcularSaldoDisponibleTarjeta(LogicaTipoDeCambio _logicaTipoDeCambio, Espacio e,
        TarjetaCredito tarjeta, LogicaTransaccion _logicaTransaccion)
    {
        double total = 0;
        double montoInicial = tarjeta.CreditoDisponible;
        double sumatoriaIngresos = 0;
        double sumatoriaCostos = 0;
        if (tarjeta.Moneda == "UYU")
        {
            return CalcularSaldoTarjetasUYU(_logicaTipoDeCambio, e, tarjeta, sumatoriaIngresos, sumatoriaCostos, montoInicial, _logicaTransaccion);
        }
        return CalcularSaldoTarjetasMonedaExtranjera(_logicaTipoDeCambio, e, tarjeta, sumatoriaIngresos, sumatoriaCostos, montoInicial, _logicaTransaccion);

    }

    public double CalcularSaldoTarjetasMonedaExtranjera(LogicaTipoDeCambio _logicaTipoDeCambio, Espacio e,
        TarjetaCredito tarjeta, double sumatoriaIngresos,
        double sumatoriaCostos, double montoInicial, LogicaTransaccion _logicaTransaccion)
    {
        double total;
        foreach (var transaccion in _logicaTransaccion.ListarTransacciones(e))
        {
            if (transaccion.TipoTransaccion == "Ingreso" && transaccion.Cuenta.Nombre.Equals(tarjeta.Nombre))
            {
                double montoEnUYUs = _logicaTipoDeCambio.PasarAPesosUruguayos(e, transaccion.Moneda,
                    transaccion.Monto, transaccion.Fecha);
                double montoEnMonedaExtranjera =
                    _logicaTipoDeCambio.PasarDeUYUAMonedaDestino(e, montoEnUYUs, transaccion.Fecha, tarjeta.Moneda);
                sumatoriaIngresos = sumatoriaIngresos + montoEnMonedaExtranjera;
            }
            else if (transaccion.TipoTransaccion == "Costo" && transaccion.Cuenta.Nombre.Equals(tarjeta.Nombre))
            {
                double montoEnUYUs = _logicaTipoDeCambio.PasarAPesosUruguayos(e, transaccion.Moneda,
                    transaccion.Monto, transaccion.Fecha);
                double montoEnMonedaExtranjera =
                    _logicaTipoDeCambio.PasarDeUYUAMonedaDestino(e, montoEnUYUs, transaccion.Fecha, tarjeta.Moneda);
                sumatoriaCostos = sumatoriaCostos + montoEnMonedaExtranjera;
            }
        }

        total = (montoInicial + sumatoriaIngresos) - sumatoriaCostos;
        return total;
    }

    public double CalcularSaldoTarjetasUYU(LogicaTipoDeCambio _logicaTipoDeCambio, Espacio e, TarjetaCredito tarjeta,
        double sumatoriaIngresos,
        double sumatoriaCostos, double montoInicial, LogicaTransaccion _logicaTransaccion)
    {
        double total;
        foreach (var transaccion in _logicaTransaccion.ListarTransacciones(e))
        {
            if (transaccion.TipoTransaccion == "Ingreso" && transaccion.Cuenta.Nombre.Equals(tarjeta.Nombre))
            {
                double montoEnUYUs = _logicaTipoDeCambio.PasarAPesosUruguayos(e, transaccion.Moneda,
                    transaccion.Monto, transaccion.Fecha);
                sumatoriaIngresos = sumatoriaIngresos + montoEnUYUs;
            }
            else if (transaccion.TipoTransaccion == "Costo" && transaccion.Cuenta.Nombre.Equals(tarjeta.Nombre))
            {
                double montoEnUYUs = _logicaTipoDeCambio.PasarAPesosUruguayos(e, transaccion.Moneda,
                    transaccion.Monto, transaccion.Fecha);
                sumatoriaCostos = sumatoriaCostos + montoEnUYUs;
            }
        }
        total = (montoInicial + sumatoriaIngresos) - sumatoriaCostos;
        return total;
    }

    public bool TieneTransaccionAsociada(Espacio space, Cuenta c, LogicaTransaccion _logicaTransaccion)
    {
        foreach (Transaccion transaccion in _logicaTransaccion.ListarTransacciones(space))
        {
            if (transaccion.Cuenta.Id.Equals(c.Id)) return true;
        }

        return false;
    }
    
    
}