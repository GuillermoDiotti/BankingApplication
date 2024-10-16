using System.Xml;
using Dominio;
using Repository;

namespace Logica;

public class LogicaTipoDeCambio
{
    private readonly IRepository<TipoDeCambio> _repository;
    
    public LogicaTipoDeCambio(IRepository<TipoDeCambio> tipoDeCambioRepository)
    {
        _repository = tipoDeCambioRepository;
    }
    
    public void CrearCotizacion(TipoDeCambio cambio)
    {
        VaidarInputs(cambio.Fecha, cambio.Moneda, cambio.Cotizacion, cambio.Espacio);
        _repository.Add(cambio);
    }
    
    public void EliminarCambio(TipoDeCambio cambio, Espacio space, string moneda, LogicaTransaccion logicaTransaccion  )
    {
        if(TieneTransaccionAsociada(space, cambio,moneda,logicaTransaccion)) throw new LogicException("No se puede eliminar el tipo de cambio porque tiene transacciones asociadas");
        _repository.Delete(cambio.Id);
    }

    public List<TipoDeCambio> listarCambiosPorEspacio(Espacio espacio)
    {
        return _repository.FindAll().Where(x => x.Espacio == espacio).ToList();
    }

    public bool CambioUnicoParaLaFecha(DateTime fecha, Espacio space, string monedaElegida)
    {
        TipoDeCambio? cambio = ConseguirCotizacionPorFecha(fecha, monedaElegida, space);
        if (cambio == null) return true;
        return !(cambio.Moneda == monedaElegida && cambio.Espacio == space && cambio.Fecha.Date == fecha.Date);
    }

    public void VaidarInputs(DateTime Fecha, string? monedaElegida, double _valorEnUYUs, Espacio e)
    {
        try
        {
            if (monedaElegida == null)
            {
                throw new LogicException("Seleccione una moneda");

            }
            if (!CambioUnicoParaLaFecha(Fecha, e, monedaElegida))
            {
                throw new LogicException("Ya existe una cotizacion para esa fecha, debe de editar la actual");
            }
            if(string.IsNullOrEmpty(monedaElegida)|| monedaElegida == "UYU")
            {
                throw new LogicException("Debe elegir una moneda");
            }
            if(_valorEnUYUs <= 0)
            {
                throw new LogicException("Debe ingresar un valor mayor a 0");
            }
        }
        catch (LogicException ex)
        {
            throw new LogicException(ex.mensaje);
        }
    }

    public TipoDeCambio ConseguirCotizacionPorFecha(DateTime fecha, string moneda, Espacio e)
    {
        if (moneda == "UYU")
        {
            TipoDeCambio cambio = new TipoDeCambio();
            cambio.Fecha = fecha;
            cambio.Cotizacion = 1;
            cambio.Moneda = "UYU";
            return cambio;
        }
        TipoDeCambio? cotizacion = listarCambiosPorEspacio(e).Find(x => x.Fecha.Date == fecha.Date && x.Moneda==moneda);
        return cotizacion;
    }

    public void ModificarCotizacion(TipoDeCambio nuevoCambio, TipoDeCambio cambioActual, LogicaTransaccion TransactionLogic)
    {
        ValidarModificarCotizacion(nuevoCambio.Espacio, cambioActual, TransactionLogic ,nuevoCambio);
        _repository.Update(nuevoCambio);
    }

    public void ValidarModificarCotizacion(Espacio space, TipoDeCambio _cambioActual, LogicaTransaccion TransactionLogic, TipoDeCambio nuevoCambio)
    {
        if (TieneTransaccionAsociada(space, _cambioActual, _cambioActual.Moneda, TransactionLogic))
        {
            throw new LogicException("El tipo de cambio seleccionado no puede ser editado porque se encuentra asociado a una transaccion");
        }
        if(nuevoCambio.Cotizacion<= 0)
        {
            throw new LogicException("Debe ingresar un valor mayor a 0");
        }
    }

    public bool TieneTransaccionAsociada(Espacio e, TipoDeCambio cotizacion, string moneda, LogicaTransaccion _logicaTransaccion)
    {
        foreach (Transaccion transaccion in _logicaTransaccion.ListarTransacciones(e))
        {
            return ((transaccion.Moneda == moneda &&
                     transaccion.Fecha.Date == cotizacion.Fecha.Date) || (transaccion.Cuenta.Moneda == moneda && transaccion.Moneda!= moneda && transaccion.Fecha.Date == cotizacion.Fecha.Date));
        }
        return false;
    }
    
    public double PasarAPesosUruguayos(Espacio e, string monedaInicial, double monto, DateTime fecha)
    {
        var cotizacion = ConseguirCotizacionPorFecha(fecha, monedaInicial, e);
        if(cotizacion== null) throw new LogicException("No existe cotizacion para la fecha ingresada");
        return monto * cotizacion.Cotizacion;
    }
    
    public double PasarDeUYUAMonedaDestino(Espacio e, double monto, DateTime fecha, string monedaFinal)
    {
        var cotizacionFinal = ConseguirCotizacionPorFecha(fecha, monedaFinal, e);
        if( cotizacionFinal == null) throw new LogicException("No existe cotizacion para la fecha ingresada");
        return monto / cotizacionFinal.Cotizacion;
    }
    
    
    
}