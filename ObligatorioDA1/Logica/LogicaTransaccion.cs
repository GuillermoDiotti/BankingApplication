using System.Collections;
using Dominio;
using Repository;

namespace Logica;

public class LogicaTransaccion
{
    private readonly IRepository<Transaccion> _repository;
    
    public LogicaTransaccion(IRepository<Transaccion> TransactionRepository)
    {
        _repository = TransactionRepository;
    }
    
    public void NuevaTransaccion(Transaccion t)
    {
        _repository.Add(t);
    }
    
    public void ModificarTransaccion(Transaccion? t)
    {
        _repository.Update(t);
    }
    public List<Transaccion> ListarTransacciones(Espacio espacio)
    {
        return _repository.FindAll().Where(x => x.Espacio == espacio).ToList();
    }

    public void ValidarInputs(Categoria? categoria, Cuenta? cuenta, double? monto)
    {
        if (monto==null || monto <= 0)
        {
            throw new LogicException("ERROR: El monto debe ser mayor a 0");
        }
        if (categoria == null)
        {
            throw new LogicException("ERROR: Seleccione una categoria");
        }
        if (cuenta == null)
        {
            throw new LogicException("ERROR: Seleccione una cuenta");
        }
    }

    public double TotalGastadoPorMes(Espacio space, int mes)
    {
        double TotalGastado = 0;
        var TransaccionesCosto = ListarTransacciones(space).Where(x => x.TipoTransaccion == "Costo");
        foreach (var transaccion in TransaccionesCosto)
        {
            if (transaccion.Fecha.Month.Equals(mes))
            {
                TotalGastado += transaccion.Monto;
            }
        }
        return TotalGastado;
    }

    public double TotalIngresosPorDiaEnElAño(Espacio space, int i, int mes, LogicaTipoDeCambio _logicaTipoDeCambio)
    {
        double TotalIngresos = 0;
        var TransaccionesIngreso = ListarTransacciones(space).Where(x => x.TipoTransaccion == "Ingreso");
        foreach (var transaccion in TransaccionesIngreso)
        {
            if (transaccion.Fecha.Day.Equals(i) && transaccion.Fecha.Year.Equals(DateTime.Now.Year) && transaccion.Fecha.Month.Equals(mes))
            {
                double montoEnPesosUruguayos = _logicaTipoDeCambio.PasarAPesosUruguayos(space, transaccion.Moneda, transaccion.Monto, transaccion.Fecha);
                
                TotalIngresos += montoEnPesosUruguayos;
            }
        }
        return TotalIngresos;
    }


    public double TotalEgresosPorDiaEnElAño(Espacio space, int i, int mes, LogicaTipoDeCambio _logicaTipoDeCambio)
    {
        double TotalEgresos = 0;
        var TransaccionesEgreso = ListarTransacciones(space).Where(x => x.TipoTransaccion == "Costo");
        foreach (var transaccion in TransaccionesEgreso)
        {
            if (transaccion.Fecha.Day.Equals(i) && transaccion.Fecha.Year.Equals(DateTime.Now.Year) && transaccion.Fecha.Month.Equals(mes))
            {
                double montoEnPesosUruguayos = _logicaTipoDeCambio.PasarAPesosUruguayos(space, transaccion.Moneda, transaccion.Monto, transaccion.Fecha);
                TotalEgresos += montoEnPesosUruguayos;
            }
        }
        return TotalEgresos;
    }
}