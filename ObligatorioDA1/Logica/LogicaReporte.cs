using Dominio;
using Repository;

namespace Logica;

public class LogicaReporte
{
    
    public List<ReporteDeobjetivosDeGastos> GenerarReporteDeObjetivos(LogicaObjetivos _logicaObjetivos,
        List<ObjetivosGastos> lista, Espacio e, LogicaTipoDeCambio _logicaTipoDeCambio, LogicaTransaccion _logicaTransaccion)
    {
        List<ReporteDeobjetivosDeGastos> respuesta = new List<ReporteDeobjetivosDeGastos>();
        foreach (ObjetivosGastos objetivo in lista)
        {
            ReporteDeobjetivosDeGastos reporte = new ReporteDeobjetivosDeGastos();
            reporte.CumpleObjetivo = _logicaObjetivos.CumpleObjetivo(objetivo, e, _logicaTipoDeCambio, _logicaTransaccion);
            reporte.MontoDefinido = objetivo.MontoMaximo;
            reporte.MontoGastado = _logicaObjetivos.ConseguirGastoEnElMes(objetivo, e, _logicaTipoDeCambio, _logicaTransaccion);
            reporte.TituloObjetivo = objetivo.Titulo;
            respuesta.Add(reporte);
        }
        return respuesta;
    }
    
    public List<ReporteDeCategoria> GenerarReporteDeCategorias(LogicaTransaccion _logicaTransaccion,
        LogicaCategoria _logicaCategoria, List<Categoria> lista, Espacio space, int mes)
    {
        List<ReporteDeCategoria> respuesta = new List<ReporteDeCategoria>();
        foreach (Categoria cate in lista)
        {
            ReporteDeCategoria reporte = new ReporteDeCategoria();
            reporte.GastoPorCategoria = _logicaCategoria.TotalGastadoSegunCategoria(cate, space, mes, _logicaTransaccion);
            reporte.PorcentajeDeLTotal = _logicaCategoria.PorcentajeSobreElTotal(_logicaTransaccion, cate, space, mes);
            reporte.NombreCategoria = cate.Nombre;
            if (reporte.PorcentajeDeLTotal > 0)
            {
                respuesta.Add(reporte);
            }
        }
        return respuesta;
    }

    public List<ReporteDeTarjeta> GenerarReportePorTarjeta(Espacio space, TarjetaCredito t,
        LogicaTransaccion _logicaTransaccion, IDateTimeProvider _iDateTimeProvider)
    {
        List<ReporteDeTarjeta> respuesta = new List<ReporteDeTarjeta>();
        DateTime fechaCierreInicial = calcularFechaCierreInicial(t, _iDateTimeProvider);
        DateTime fechaCierreFinal = calcularFechaCierreFin(t, _iDateTimeProvider);
        foreach (Transaccion transaccion in _logicaTransaccion.ListarTransacciones(space))
        {
            ReporteDeTarjeta reporte = new ReporteDeTarjeta();
            if (transaccion.Cuenta.GetType() == typeof(TarjetaCredito) && transaccion.Cuenta.Nombre == t.Nombre &&
                transaccion.Fecha>fechaCierreInicial && transaccion.Fecha.Date<=fechaCierreFinal.Date &&
                transaccion.TipoTransaccion=="Costo")
            {
                reporte.Moneda = transaccion.Moneda;
                reporte.Fecha = transaccion.Fecha;
                reporte.Titulo = transaccion.Titulo;
                reporte.Gasto = transaccion.Monto;
                respuesta.Add(reporte);
            }
        }
        return respuesta;
    }

    public DateTime calcularFechaCierreInicial(TarjetaCredito t, IDateTimeProvider _iDateTimeProvider)
    {
        int year, month, day;
        if (t.FechaCierre.Day < _iDateTimeProvider.ObtenerFechaHoy().Day)
        {
            month = _iDateTimeProvider.ObtenerFechaHoy().Month;
            day = t.FechaCierre.Day;
            year = _iDateTimeProvider.ObtenerFechaHoy().Year;
        }
        else
        {
            month = _iDateTimeProvider.ObtenerFechaHoy().AddMonths(-1).Month;
            day = t.FechaCierre.Day;
            if (_iDateTimeProvider.ObtenerFechaHoy().Month == 1)
            {
                year = _iDateTimeProvider.ObtenerFechaHoy().AddYears(-1).Year;
            }
            else
            {
                year = _iDateTimeProvider.ObtenerFechaHoy().Year;
            }
        }
        return new DateTime(year, month, day);
    }
    
    public DateTime calcularFechaCierreFin(TarjetaCredito t, IDateTimeProvider _iDateTimeProvider)
    {
        int year, month, day;
        if (t.FechaCierre.Day < _iDateTimeProvider.ObtenerFechaHoy().Day)
        {
            month = _iDateTimeProvider.ObtenerFechaHoy().AddMonths(1).Month;
            day = t.FechaCierre.Day;
            if (_iDateTimeProvider.ObtenerFechaHoy().Month == 12)
            {
                year = _iDateTimeProvider.ObtenerFechaHoy().AddYears(1).Year;
            }
            else
            {
                year = _iDateTimeProvider.ObtenerFechaHoy().Year;
            }
        }
        else
        {
            month = _iDateTimeProvider.ObtenerFechaHoy().Month;
            day = t.FechaCierre.Day;
            year = _iDateTimeProvider.ObtenerFechaHoy().Year;
        }
        return new DateTime(year, month, day);
    }
    
    public List<ReporteIngresosEgresos> GenerarReporteIngresosEgresos(LogicaTransaccion _logicaTransaccion,
        Espacio space, int mes, LogicaTipoDeCambio _logicaTipoDeCambio, int maxDiasMes)
    {
        List<ReporteIngresosEgresos> respuesta = new List<ReporteIngresosEgresos>();
        for (int i = 1; i <= maxDiasMes; i++)
        {
            ReporteIngresosEgresos reporte = new ReporteIngresosEgresos();
            reporte.DiaDelMes = i;
            reporte.Ingreso = _logicaTransaccion.TotalIngresosPorDiaEnElAño(space, i, mes, _logicaTipoDeCambio);
            reporte.Egreso = _logicaTransaccion.TotalEgresosPorDiaEnElAño(space, i, mes, _logicaTipoDeCambio);
            respuesta.Add(reporte);
        }
        return respuesta;
    }
    
}