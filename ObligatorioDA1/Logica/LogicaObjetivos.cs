using Dominio;
using Repository;

namespace Logica;

public class LogicaObjetivos
{
    private readonly IRepository<ObjetivosGastos> _repository;
    
    public LogicaObjetivos(IRepository<ObjetivosGastos> ObjetivosRepository)
    {
        _repository = ObjetivosRepository;
    }

    public void CrearObjetivo(ObjetivosGastos obj)
    {
        ValidarCrearObjetivo(obj);
        _repository.Add(obj);
    }

    public void ValidarCrearObjetivo(ObjetivosGastos obj)
    {
        if(obj.Categorias == null || !obj.Categorias.Any()) throw new LogicException("Debe seleccionar al menos una categoria");
        if(string.IsNullOrEmpty(obj.Titulo))throw new LogicException("Debe ingresar un titulo");
        if(obj.MontoMaximo<=0)throw new LogicException("Debe ingresar un monto maximo mayor a 0");
    }

    public List<ObjetivosGastos> ListarObjEspacio(Espacio espacio)
    {
        return _repository.FindAll().Where(x => x.Espacio.Id == espacio.Id).ToList();
    }

    public double ConseguirGastoEnElMes(ObjetivosGastos obj, Espacio e, LogicaTipoDeCambio _logicaTipoDeCambio, LogicaTransaccion _logicaTransaccion)
    {
        double total = 0;
        var transaccionesGasto = _logicaTransaccion.ListarTransacciones(e).Where(x => x.TipoTransaccion == "Costo");
        foreach (Transaccion transaccion in transaccionesGasto)
        {
            
            if (obj.Categorias.Contains(transaccion.Categoria) && transaccion.Fecha.Month.Equals(DateTime.Now.Month))
            {
                double montoConvertido = _logicaTipoDeCambio.PasarAPesosUruguayos(e, transaccion.Moneda, transaccion.Monto, transaccion.Fecha);
                total = total + montoConvertido;
            }
        }

        return total;
    }

    public double ConseguirGasto(ObjetivosGastos obj, Espacio e, LogicaTipoDeCambio _logicaTipoDeCambio, LogicaTransaccion _logicaTransaccion)
    {
        double total = 0;
        var transaccionesGasto = _logicaTransaccion.ListarTransacciones(e).Where(x => x.TipoTransaccion == "Costo");
        foreach (Transaccion transaccion in transaccionesGasto)
        {
            
            if (obj.Categorias.Contains(transaccion.Categoria))
            {
                double montoConvertido = _logicaTipoDeCambio.PasarAPesosUruguayos(e, transaccion.Moneda, transaccion.Monto, transaccion.Fecha);
                    total = total + montoConvertido;
            }
        }
        return total;
    }


    public bool CumpleObjetivo(ObjetivosGastos obj, Espacio e, LogicaTipoDeCambio _logicaTipoDeCambio, LogicaTransaccion _logicaTransaccion)
    {
        if (ConseguirGastoEnElMes(obj, e, _logicaTipoDeCambio, _logicaTransaccion) > obj.MontoMaximo) return false;
        return true;
    }
    
    public string AgregarURL(ObjetivosGastos obj)
    {
        return obj.URL = GeneradorToken.generar(7);
    }
    
    

    public string conseguirURL(ObjetivosGastos obj)
    {
            return obj.URL;
    }

    public void ActualizarURL(ObjetivosGastos obj)
    {
        _repository.Update(obj);
    }
    
    
    public static class GeneradorToken
    {
        private static readonly Random random = new Random();
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private static HashSet<string> tokensUsados = new HashSet<string>();

        public static string generar(int length)
        {
            string token;
            do
            {
                var tokenArray = new char[length];
                for (int i = 0; i < length; i++)
                {
                    tokenArray[i] = chars[random.Next(chars.Length)];
                }
                token = new string(tokenArray);
            } while (tokensUsados.Contains(token));

            tokensUsados.Add(token);
            return token;
        }
    }
    
}