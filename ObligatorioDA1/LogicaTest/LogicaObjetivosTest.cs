using System.Xml;
using Dominio;
using Logica;
using LogicaTest.Context;
using Moq;
using Repository;

namespace LogicaTest;
[TestClass]
public class LogicaObjetivosTest
{
    private LogicaObjetivos logicaObjetivos;
    private LogicaEspacio logicaEspacio;
    private LogicaTipoDeCambio logicaTipoDeCambio;
    private LogicaTransaccion logicaTransaccion;
    private LogicaCategoria logicaCategoria;
    private LogicaUsuario logicaUsuario;
    private LogicaCuenta logicaCuenta;
    private Espacio espacio;
    private Usuario user;
    private SqlContext _context;
    
    [TestInitialize]
    public void setup(){
        SqlContextFactory sqlContextFactory = new SqlContextFactory();
        _context = sqlContextFactory.CreateMemoryContext();
        
        IRepository<ObjetivosGastos> accRepository = new ObjetivosGastosDatabaseRepository(_context);
        IRepository<Espacio> espRepository = new EspacioDatabaseRepository(_context);
        IRepository<TipoDeCambio> tcRepository = new TiposDeCambioDatabaseRepository(_context);
        IRepository<Transaccion> traRepository = new TransaccionDatabaseRepository(_context);
        IRepository<Categoria> catRepository = new CategoriaDatabaseRepository(_context);
        IRepository<Usuario> userRepository = new UsuarioDatabaseRepository(_context);
        IRepository<Cuenta> cuentaRepository = new CuentaDatabaseRepository(_context);
        logicaEspacio = new LogicaEspacio(espRepository);
        logicaObjetivos = new LogicaObjetivos(accRepository);
        logicaTipoDeCambio = new LogicaTipoDeCambio(tcRepository);
        logicaTransaccion = new LogicaTransaccion(traRepository);
        logicaCategoria = new LogicaCategoria(catRepository);
        logicaUsuario = new LogicaUsuario(userRepository);
        logicaCuenta = new LogicaCuenta(cuentaRepository);
        user = new Usuario()
        {
            Address = null,
            Mail = "hola@outlook.com",
            Password = "123456789A",
            LastName = "Cueva",
            Name = "Pedro",
        };
        espacio = new Espacio()
        {
            AdminEspacio = user,
            NombreEspacio = "General",
        };
        logicaUsuario.AgregarUsuario(user);

        logicaEspacio.CrearEspacio(espacio);
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }
    
    [TestMethod]
    public void CrearObjetivo_DeberiaAgregarObjetivoAlRepositorio()
    {
        var cat = new Categoria()
        {
            Nombre = "cat1",
            Espacio = espacio,
            Tipo = "Ingreso",
            Estatus = "Activa"
        };
        logicaCategoria.AgregarCategoria(cat);
        var objetivo = new ObjetivosGastos()
        {
            URL = "1234",
            Titulo = "Obj1",
            MontoMaximo = 100,
            Espacio = espacio,
            UsuarioCreador = user,
            Categorias = new List<Categoria>() { cat },
        };
        logicaObjetivos.CrearObjetivo(objetivo);
        Assert.AreEqual(1, logicaObjetivos.ListarObjEspacio(espacio).Count);
    }
    
    [TestMethod]
    public void ListarObjEspacio_DeberiaRetornarListaFiltradaPorEspacio()
    {
        var objetivosList = new List<ObjetivosGastos>
        {
            new ObjetivosGastos { Espacio = new Espacio { Id = 1 } },
            new ObjetivosGastos { Espacio = new Espacio { Id = 2 } },
        };
                
        var result = logicaObjetivos.ListarObjEspacio(espacio);
    
        Assert.AreEqual(0, result.Count);
    }
    
    
    [TestMethod]
    public void ActualizarURL_DeberiaActualizarElObjetivoEnElRepositorio()
    {
        var cat = new Categoria()
        {
            Nombre = "cat1",
            Espacio = espacio,
            Tipo = "Ingreso",
            Estatus = "Activa"
        };
        logicaCategoria.AgregarCategoria(cat);
        var objetivo = new ObjetivosGastos()
        {
            URL = "1234",
            Titulo = "Obj1",
            MontoMaximo = 100,
            Espacio = espacio,
            UsuarioCreador = user,
            Categorias = new List<Categoria>() { cat },
        };
        logicaObjetivos.CrearObjetivo(objetivo);
        var objetivoNuevo = new ObjetivosGastos()
        {
            Id = objetivo.Id,
            URL = "2222",
        };
        logicaObjetivos.ActualizarURL(objetivoNuevo);
        var resultado = logicaObjetivos.ListarObjEspacio(espacio).Find(x => x.Id == objetivo.Id);
    
        Assert.AreEqual("2222", resultado.URL);
    }
    
    [TestMethod]
    public void AgregarURL_DeberiaGenerarUnTokenYAsignarloAlObjetivo()
    {
                
        var objetivo = new ObjetivosGastos();
                
        var result = logicaObjetivos.AgregarURL(objetivo);
                
        Assert.IsNotNull(result);
        Assert.AreEqual(result.Length, 7);
        Assert.AreEqual(objetivo.URL, result);
    }

    [TestMethod]
    public void ConseguirGasto()
    {
        var cuenta = new CuentaMonetaria()
        {
            Nombre = "Cuenta",
            MontoInicial = 100,
            Espacio = espacio,
            Moneda = "USD",
        };
        logicaCuenta.AgregarCuentaMonetaria(cuenta, espacio);
        var categoria = new Categoria
        {
            Nombre = "Alimentos",
            Estatus = "Activa", 
            Tipo = "Costo", 
            Espacio = espacio
        };
        logicaCategoria.AgregarCategoria(categoria);
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
        logicaTransaccion.NuevaTransaccion(t1);
        var objetivo = new ObjetivosGastos()
        {
            URL = "1234",
            Titulo = "Obj1",
            MontoMaximo = 100,
            Espacio = espacio,
            UsuarioCreador = user,
            Categorias = new List<Categoria>() { categoria },
        };       
        logicaObjetivos.CrearObjetivo(objetivo);
                
        Assert.AreEqual(50, logicaObjetivos.ConseguirGasto(objetivo, espacio, logicaTipoDeCambio, logicaTransaccion));
    }
    
    [TestMethod]
    public void CumpleObj()
    {
        var cuenta = new CuentaMonetaria()
        {
            Nombre = "Cuenta",
            MontoInicial = 100,
            Espacio = espacio,
            Moneda = "USD",
        };
        logicaCuenta.AgregarCuentaMonetaria(cuenta, espacio);
        var categoria = new Categoria
        {
            Nombre = "Alimentos",
            Estatus = "Activa", 
            Tipo = "Costo", 
            Espacio = espacio
        };
        logicaCategoria.AgregarCategoria(categoria);
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
        logicaTransaccion.NuevaTransaccion(t1);
        var objetivo = new ObjetivosGastos()
        {
            URL = "1234",
            Titulo = "Obj1",
            MontoMaximo = 100,
            Espacio = espacio,
            UsuarioCreador = user,
            Categorias = new List<Categoria>() { categoria },
        };       
        logicaObjetivos.CrearObjetivo(objetivo);
        var resultado = logicaObjetivos.CumpleObjetivo( objetivo, espacio, logicaTipoDeCambio, logicaTransaccion);
                
        Assert.AreEqual(true, resultado);
    }
            
    [TestMethod]
    public void ConseguirURL(){
        ObjetivosGastos og = new ObjetivosGastos(){ URL = "1234" };
        Assert.AreEqual(og.URL, logicaObjetivos.conseguirURL(og));
    }
}