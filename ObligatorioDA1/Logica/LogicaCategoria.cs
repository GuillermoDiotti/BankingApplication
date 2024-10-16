using System.Dynamic;
using Dominio;
using Repository;

namespace Logica;

public class LogicaCategoria
{
    private readonly IRepository<Categoria> _repository;
    
    public LogicaCategoria(IRepository<Categoria> categoryRepository)
    {
        _repository = categoryRepository;
    }

    public Categoria AgregarCategoria(Categoria unaCategoria)
    {
        _repository.Add(unaCategoria);
        return unaCategoria;
    }

    public List<Categoria> ObtenerCategoriasDeEspacio(Espacio space)
    {
        var categorias = FindCategories();
        List<Categoria> lista = categorias.Where(categoria => categoria.Espacio.Id == space.Id).ToList();
        return lista;
    }
    
    public Categoria FindById(int id)
    {
        return _repository.Find(x => x.Id == id);
    }

    public void EliminarCategoria(Categoria c,LogicaTransaccion logicaTransaccion, LogicaObjetivos logicaObjetivos)
    {
        ValidarEliminarCategoria(c, logicaTransaccion, logicaObjetivos);
        _repository.Delete(c.Id);
    }
    
    public IList<Categoria> FindCategories()
    {
        return _repository.FindAll();
    }

    public void EditarCategoria(Categoria updatedCategory, LogicaTransaccion logicaTransaccion, string tipo, LogicaObjetivos logicaObjetivos )
    {
        ValidarEditarCategoria(updatedCategory, logicaTransaccion, tipo, logicaObjetivos);
        _repository.Update(updatedCategory);
    }

    public void ValidarEditarCategoria(Categoria c, LogicaTransaccion logicaTransaccion, string? tipoActual, LogicaObjetivos logicaObjetivos)
    {

        if (c.Tipo==null || string.IsNullOrEmpty(c.Tipo))
        {
            throw new Exception("El tipo de categoria no puede ser vacio");
        }
        if(TieneTransaccionAsociada(c.Espacio,c ,logicaTransaccion) && c.Tipo!=tipoActual)
        {
            throw new Exception("No se puede editar el tipo de categoria porque tiene transacciones asociadas");
        }

        if (TieneObjetivoAsociado(c.Espacio, c, logicaObjetivos) && c.Tipo!=tipoActual)
        {
            throw new Exception("No se puede editar el tipo de categoria porque tiene objetivos asociados");
        }
    }

    public void ValidarEliminarCategoria(Categoria c, LogicaTransaccion logicaTransaccion,
        LogicaObjetivos logicaObjetivos)
    {
        if (TieneTransaccionAsociada(c.Espacio, c, logicaTransaccion))
        {
            throw new Exception("No se puede eliminar la categoria porque tiene transacciones asociadas");
        }
        if (TieneObjetivoAsociado(c.Espacio, c, logicaObjetivos))
        {
            throw new Exception("No se puede eliminar la categoria porque tiene objetivos asociados");
        }
    }

    public double TotalGastadoSegunCategoria(Categoria c, Espacio space, int mes, LogicaTransaccion _logicaTransaccion)
    {
        double gastadoEnCategoria = 0;
        var TransaccionesGasto = _logicaTransaccion.ListarTransacciones(space).Where(x => x.TipoTransaccion == "Costo").ToList();
        foreach (Transaccion transaccion in TransaccionesGasto)
        {
            if (transaccion.Fecha.Month.Equals(mes) && transaccion.Categoria.Nombre == c.Nombre)
            {
                gastadoEnCategoria = gastadoEnCategoria + transaccion.Monto;
            }
        }

        return gastadoEnCategoria;
    }

    public int PorcentajeSobreElTotal(LogicaTransaccion _logicaTransaccion, Categoria c, Espacio space, int mes)
    {
        double TotalGastado = _logicaTransaccion.TotalGastadoPorMes(space, mes);
        double gastoEnCatgoria = TotalGastadoSegunCategoria(c, space, mes, _logicaTransaccion);
        int calculo = (int)((gastoEnCatgoria * 100) / TotalGastado);
        return calculo;
    }

    public bool TieneTransaccionAsociada(Espacio space, Categoria c, LogicaTransaccion _logicaTransaccion)
    {
        foreach (Transaccion transaccion in _logicaTransaccion.ListarTransacciones(space))
        {
            if (transaccion.Categoria.Equals(c)) return true;
        }

        return false;
    }
    
    public bool TieneObjetivoAsociado(Espacio space, Categoria c, LogicaObjetivos _logicaObjetivos)
    {
        foreach (ObjetivosGastos obj in _logicaObjetivos.ListarObjEspacio(space))
        {
            if (obj.Categorias.Contains(c)) return true;
        }
        return false;
    }
}