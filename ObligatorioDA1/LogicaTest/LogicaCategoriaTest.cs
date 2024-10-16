using System;
using System.Collections.Generic;
using Dominio;
using Logica;
using LogicaTest.Context;
using Moq;
using Repository;

[TestClass]
public class LogicaCategoriaTest
{
    private LogicaObjetivos _logicaObjetivos;
    private LogicaCategoria _logicaCategoria;
    private LogicaTransaccion _logicaTransaccion;
    private LogicaEspacio _logicaEspacio;
    private LogicaCuenta _logicaCuenta;
    private LogicaUsuario _logicaUsuario;
    private Usuario usuario;
    private SqlContext _context;
    private Espacio espacio;
    private CuentaMonetaria cuenta;
    
    [TestInitialize]
    public void setup()
    {
        SqlContextFactory sqlContextFactory = new SqlContextFactory();
        _context = sqlContextFactory.CreateMemoryContext();
        
        usuario = new Usuario()
        {
            Name = "Arturo",
            Mail = "arturo@fff.com",
            LastName = "Rodriguez",
            Password = "123456789A",
        };
        espacio = new Espacio()
        {
            AdminEspacio = usuario,
            NombreEspacio = "General",
        };
        cuenta = new CuentaMonetaria()
        {
            Nombre = "Cuenta",
            MontoInicial = 100,
            Espacio = espacio,
            Moneda = "USD",
        };
        
        
        IRepository<ObjetivosGastos> respository = new ObjetivosGastosDatabaseRepository(_context);
        IRepository<Categoria> catRespository = new CategoriaDatabaseRepository(_context);
        IRepository<Transaccion> traRespository = new TransaccionDatabaseRepository(_context);
        IRepository<Espacio> espRespository = new EspacioDatabaseRepository(_context);
        IRepository<Usuario> usRespository = new UsuarioDatabaseRepository(_context);
        IRepository<Cuenta> cRespository = new CuentaDatabaseRepository(_context);

        _logicaObjetivos = new LogicaObjetivos(respository);
        _logicaCategoria = new LogicaCategoria(catRespository);
        _logicaTransaccion = new LogicaTransaccion(traRespository);
        _logicaEspacio = new LogicaEspacio(espRespository);
        _logicaUsuario = new LogicaUsuario(usRespository);
        _logicaCuenta = new LogicaCuenta(cRespository);

        
        _logicaUsuario.AgregarUsuario(usuario);
        _logicaEspacio.CrearEspacio(espacio);
        _logicaCuenta.AgregarCuentaMonetaria(cuenta, espacio);


    }
    
    
    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }
    
    [TestMethod]
    public void AgregarCategoria_DebeAgregarCategoria()
    {
        var categoria = new Categoria()
        {
            Estatus = "Activa",
            Nombre = "dad",
            Tipo = "Costo",
            Espacio = espacio
        };
        var resultado = _logicaCategoria.AgregarCategoria(categoria);
        Assert.AreEqual(categoria, resultado);
    }

    [TestMethod]
    public void EliminarCategoria_DebeEliminarCategoria()
    {
        var categoria = new Categoria { 
            Estatus = "Activa",
            Nombre = "dad",
            Tipo = "Costo",
            Espacio = espacio
        };
        _logicaCategoria.AgregarCategoria(categoria);
        _logicaCategoria.EliminarCategoria(categoria, _logicaTransaccion, _logicaObjetivos);
        
        Assert.IsTrue(_logicaCategoria.FindCategories().Count == 0);
    }
    
    [TestMethod]
    public void CategoriasEspacioa_DebeDevolverCategoriasEspacio()
    {
        var categoria = new Categoria { 
            Estatus = "Activa",
            Nombre = "dad",
            Tipo = "Costo",
            Espacio = espacio
        };
        _logicaCategoria.AgregarCategoria(categoria);
        var lista = _logicaCategoria.ObtenerCategoriasDeEspacio(espacio);
        Assert.IsTrue(lista.Contains(categoria));
    }

    [TestMethod]
    public void FindCategories_DebeRetornarListaDeCategorias()
    {
        _logicaCategoria.AgregarCategoria(new Categoria()
        {
            Estatus = "Activa",
            Nombre = "dad",
            Tipo = "Costo",
            Espacio = espacio
        });
        _logicaCategoria.AgregarCategoria(new Categoria()
        {
            Estatus = "Activa",
            Nombre = "dad",
            Tipo = "Costo",
            Espacio = espacio
        });
        var lista = _logicaCategoria.FindCategories();
        
        Assert.AreEqual(2, lista.Count);
    }
    
    [TestMethod]
    public void TotalGastadoSegunCategoria_DebeCalcularTotalGastado()
    {
        var categoria = new Categoria
        {
            Nombre = "Alimentos",
            Estatus = "Activa", 
            Tipo = "Costo", 
            Espacio = espacio
        };
        var mes = 11;
        _logicaCategoria.AgregarCategoria(categoria);
        Transaccion t1 = new  Transaccion()
        {
            Titulo = "DF5", 
            TipoTransaccion = "Costo", 
            Fecha = new DateTime(2023, 11, 1), 
            Categoria = categoria, 
            Monto = 50, 
            Espacio = espacio,
            Cuenta = cuenta
        };
        Transaccion t2 = new  Transaccion()
        {
            Titulo = "GT9", 
            TipoTransaccion = "Costo", 
            Fecha = new DateTime(2023, 11, 2), 
            Categoria = categoria, 
            Monto = 30, 
            Espacio = espacio,
            Cuenta = cuenta
        };
        _logicaTransaccion.NuevaTransaccion(t1);
        _logicaTransaccion.NuevaTransaccion(t2);
        
        var resultado = _logicaCategoria.TotalGastadoSegunCategoria(categoria, espacio, mes, _logicaTransaccion);
        
        Assert.AreEqual(80, resultado);
    }

    [TestMethod]
    public void PorcentajeSobreElTotal_DebeCalcularPorcentaje()
    {
        
        var categoria = new Categoria 
        { 
            Estatus = "Activa",
            Nombre = "dad",
            Tipo = "Costo", 
            Espacio = espacio
        };
        var mes = 11;
        _logicaCategoria.AgregarCategoria(categoria);
        Transaccion t2 = new  Transaccion()
        {
            Titulo = "GT9", 
            TipoTransaccion = "Costo", 
            Fecha = new DateTime(2023, 11, 2), 
            Categoria = categoria, 
            Monto = 40, 
            Espacio = espacio,
            Cuenta = cuenta
        };
        _logicaTransaccion.NuevaTransaccion(t2);
        var resultado = _logicaCategoria.PorcentajeSobreElTotal(_logicaTransaccion, categoria, espacio, mes);
        
        Assert.AreEqual(100, resultado);
    }

    [TestMethod]
    public void TieneTransaccionAsociada_DebeVerificarTransaccionesAsociadas()
    {
        var categoria = new Categoria 
        { 
            Estatus = "Activa",
            Nombre = "zzzzzz",
            Tipo = "Costo", 
            Espacio = espacio
        };
        _logicaCategoria.AgregarCategoria(categoria);
        var transaccion = new Transaccion
        {
            Categoria = categoria, 
            TipoTransaccion = "Costo", 
            Titulo = "TTT",
            Espacio = espacio,
            Cuenta = cuenta
        };
        
        _logicaTransaccion.NuevaTransaccion(transaccion);
        var resultado = _logicaCategoria.TieneTransaccionAsociada(espacio, categoria, _logicaTransaccion);

        
        Assert.IsTrue(resultado);
    }

    [TestMethod]
    public void TieneObjetivoAsociado_DebeVerificarObjetivosAsociados()
    {
        
        var categoria = new Categoria 
        { 
            Estatus = "Activa",
            Nombre = "dad",
            Tipo = "Costo", 
            Espacio = espacio
        };
        _logicaCategoria.AgregarCategoria(categoria);

        var objetivo = new ObjetivosGastos
        {
            Categorias = new List<Categoria> { categoria }, 
            Titulo = "dddddada", 
            Espacio = espacio,
            UsuarioCreador = usuario,
            MontoMaximo = 100,
            GastoActual = 0,
            URLHabilitada = false
        };

        _logicaObjetivos.CrearObjetivo(objetivo);
        
        
        var resultado = _logicaCategoria.TieneObjetivoAsociado(espacio, categoria, _logicaObjetivos);

        
        Assert.IsTrue(resultado);
    }
    
    [TestMethod]
    public void EditarCategoria_DebeActualizarLaCategoria()
    {

        var categoria = new Categoria
        {
            Estatus = "Activa",
            Nombre = "dad",
            Tipo = "Costo",
            Espacio = espacio
        };
        _logicaCategoria.AgregarCategoria(categoria);
        var categoriaEditada = new Categoria() 
        { 
            Id = categoria.Id, 
            Estatus = "Inactiva",
            Tipo = "Ingreso",
        };
        _logicaCategoria.EditarCategoria(categoriaEditada, _logicaTransaccion, categoriaEditada.Tipo, _logicaObjetivos);
        var categoriaActualizada = _logicaCategoria.FindById(categoria.Id);

        Assert.AreEqual("Inactiva", categoriaActualizada.Estatus);
        Assert.AreEqual("Ingreso", categoriaActualizada.Tipo);
    }
    
    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void EditarCategoriaException_TiraExcepcion()
    {
        var categoria = new Categoria
        {
            Estatus = "Activa",
            Nombre = "dad",
            Tipo = "Costo",
            Espacio = espacio
        };
        _logicaCategoria.AgregarCategoria(categoria);
        var categoriaEditada = new Categoria() 
        { 
            Id = categoria.Id, 
            Estatus = "Inactiva",
        };
        _logicaCategoria.EditarCategoria(categoriaEditada, _logicaTransaccion, categoriaEditada.Tipo, _logicaObjetivos);

    }

    [TestMethod]
    public void TieneTransaccionAsociada_DebeVerificarTransaccionesAsociadas2()
    {
        
        var categoria = new Categoria 
        { 
            Nombre = "Alimentos",
            Estatus = "Activa",
            Tipo = "Costo", 
            Espacio = espacio
        };
        _logicaCategoria.AgregarCategoria(categoria);

        var transaccion = new Transaccion
        {
            Categoria = categoria,
            TipoTransaccion = "Costo",
            Titulo = "TTT",
            Espacio = espacio,
            Cuenta = cuenta
        };
        _logicaTransaccion.NuevaTransaccion(transaccion);
        
        var resultado = _logicaCategoria.TieneTransaccionAsociada(espacio, categoria, _logicaTransaccion);

        
        Assert.IsTrue(resultado);
    }
    
    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void EliminarCategoriaConTransaccionAsociada_DebeVerificarTransaccionesAsociadas()
    {
        var categoria = new Categoria 
        { 
            Nombre = "Alimentos",
            Estatus = "Activa",
            Tipo = "Costo", 
            Espacio = espacio
        };
        _logicaCategoria.AgregarCategoria(categoria);

        var transaccion = new Transaccion
        {
            Categoria = categoria,
            TipoTransaccion = "Costo",
            Titulo = "TTT",
            Espacio = espacio,
            Cuenta = cuenta
        };
        _logicaTransaccion.NuevaTransaccion(transaccion);
        
        _logicaCategoria.EliminarCategoria(categoria, _logicaTransaccion, _logicaObjetivos);
    }

    [TestMethod]
    public void TieneObjetivoAsociado_DebeVerificarObjetivosAsociados2()
    {
        Espacio e = new Espacio()
        {
            AdminEspacio = usuario,
            NombreEspacio = "hola",
        };
        _logicaEspacio.CrearEspacio(e);
        var categoria = new Categoria 
        { 
            Nombre = "Alimentos",
            Estatus = "Activa",
            Tipo = "Costo", 
            Espacio = e
        };
        _logicaCategoria.AgregarCategoria(categoria);
        var objetivo = new ObjetivosGastos
        {
            Categorias = new List<Categoria> { categoria }, 
            Titulo = "dddddada", 
            Espacio = espacio,
            UsuarioCreador = usuario,
            MontoMaximo = 100,
            GastoActual = 0,
            URLHabilitada = false
        };
        
        _logicaObjetivos.CrearObjetivo(objetivo);
        
        var resultado = _logicaCategoria.TieneObjetivoAsociado(e, categoria, _logicaObjetivos);
        
        Assert.IsFalse(resultado);
    }
}