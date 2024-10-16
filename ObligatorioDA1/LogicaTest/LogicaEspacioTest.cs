using Dominio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Repository;
using System;
using System.Collections.Generic;
using Logica;
using LogicaTest.Context;

[TestClass]
public class LogicaEspacioTest
{
    private LogicaEspacio _logicaEspacio;
    private LogicaCategoria _logicaCategoria;
    private LogicaCuenta _logicaCuenta;
    private LogicaTransaccion _logicaTransaccion;
    private LogicaTipoDeCambio _logicaTipoDeCambio;
    private LogicaUsuario _logicaUsuario;
    private Usuario user;
    private SqlContext _context;
    
    [TestInitialize]
    public void setup()
    {
        
        SqlContextFactory sqlContextFactory = new SqlContextFactory();
        _context = sqlContextFactory.CreateMemoryContext();
        
        user = new Usuario()
        {
            Id = 4,
            Name = "Arturo",
            Mail = "arturo@fff.com",
            LastName = "Rodriguez",
            Password = "123456789A",
        };
        IRepository<Categoria> catRepository = new CategoriaDatabaseRepository(_context);
        IRepository<Espacio> espacioRepository = new EspacioDatabaseRepository(_context);
        IRepository<Cuenta> accRepository = new CuentaDatabaseRepository(_context);
        IRepository<Transaccion> tranRepository = new TransaccionDatabaseRepository(_context);
        IRepository<TipoDeCambio> cambioRepository = new TiposDeCambioDatabaseRepository(_context);
        IRepository<Usuario> userRepository = new UsuarioDatabaseRepository(_context);
        _logicaEspacio = new LogicaEspacio(espacioRepository);
        _logicaCategoria = new LogicaCategoria(catRepository);
        _logicaCuenta = new LogicaCuenta(accRepository);
        _logicaTransaccion = new LogicaTransaccion(tranRepository);
        _logicaTipoDeCambio = new LogicaTipoDeCambio(cambioRepository);
        _logicaUsuario = new LogicaUsuario(userRepository);
        user = new Usuario
        {
            Name = "Pedro",
            Mail = "pedro@gmail.com",
            LastName = "Rodriguez",
            Password = "123456789A",
            Address = "456 Av Italia"
        };
    }

    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }
    
    [TestMethod]
    public void CrearEspacio_DebeCrearUnEspacioNuevo()
    {
        
        var espacioNuevo = new Espacio { NombreEspacio = "NuevoEspacio", AdminEspacio = user };
        var resultado = _logicaEspacio.CrearEspacio(espacioNuevo);
        
        Assert.AreEqual(espacioNuevo, resultado);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicException))]
    public void AgregarMiembro_DebeLanzarExcepcionSiElUsuarioYaEsMiembro()
    {
        
        var espacio = new Espacio();
        var usuario = new Usuario();
        espacio.MiembrosEspacio.Add(usuario);
        var logicaEspacio = new LogicaEspacio(new Mock<IRepository<Espacio>>().Object);

        
        logicaEspacio.AgregarMiembro(usuario, espacio);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicException))]
    public void EliminarMiembro_DebeLanzarExcepcionSiIntentaEliminarAlAdministrador()
    {
        
        var espacio = new Espacio { AdminEspacio = new Usuario() };
        var logicaEspacio = new LogicaEspacio(new Mock<IRepository<Espacio>>().Object);

        
        logicaEspacio.EliminarMiembro(espacio.AdminEspacio, espacio);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicException))]
    public void EliminarMiembro_DebeLanzarExcepcionSiElUsuarioNoEsMiembro()
    {
        
        var espacio = new Espacio();
        var usuario = new Usuario();
        var logicaEspacio = new LogicaEspacio(new Mock<IRepository<Espacio>>().Object);

        
        logicaEspacio.EliminarMiembro(usuario, espacio);
    }

    [TestMethod]
    public void ObtenerListaEspacios_DebeRetornarListaDeEspacios()
    {
        
        var espacios = new List<Espacio>
        {
            new Espacio(),
            new Espacio(),
        };
        var mockRepository = new Mock<IRepository<Espacio>>();
        mockRepository.Setup(repo => repo.FindAll()).Returns(espacios);
        var logicaEspacio = new LogicaEspacio(mockRepository.Object);

        
        var resultado = logicaEspacio.ObtenerListaEspacios();

        
        CollectionAssert.AreEqual(espacios, resultado);
    }

    [TestMethod]
    public void ObtenerEspaciosPorUsuario_DebeRetornarEspaciosDelUsuario()
    {
        
        var usuario = new Usuario();
        var espacios = new List<Espacio>
        {
            new Espacio { AdminEspacio = usuario },
            new Espacio { MiembrosEspacio = new List<Usuario> { usuario } },
            new Espacio(),
        };
        var mockRepository = new Mock<IRepository<Espacio>>();
        mockRepository.Setup(repo => repo.FindAll()).Returns(espacios);
        var logicaEspacio = new LogicaEspacio(mockRepository.Object);

        
        var resultado = logicaEspacio.ObtenerEspaciosPorUsuario(usuario);

        
        Assert.AreEqual(2, resultado.Count);
    }

    [TestMethod]
    public void NombreEspacioDeUsuarioUnico_DebeVerificarNombreEspacioUnico()
    {
        var espacio = new Espacio { NombreEspacio = "Espacio1", AdminEspacio = user };
        _logicaEspacio.CrearEspacio(espacio);
        
        var resultado = _logicaEspacio.NombreEspacioDeUsuarioUnico("Espacio1", user);

        
        Assert.IsFalse(resultado);
    }

    [TestMethod]
    public void ObtenerMiembrosEspacio_DebeRetornarMiembrosDelEspacio()
    {
        
        var espacio = new Espacio();
        var usuario1 = new Usuario();
        var usuario2 = new Usuario();
        espacio.MiembrosEspacio.Add(usuario1);
        espacio.MiembrosEspacio.Add(usuario2);
        var logicaEspacio = new LogicaEspacio(new Mock<IRepository<Espacio>>().Object);

        
        var resultado = logicaEspacio.ObtenerMiembrosEspacio(espacio);

        
        Assert.AreEqual(2, resultado.Count);
    }

    [TestMethod]
    public void ObtenerMiembrosFueraDelEspacio_DebeRetornarMiembrosFueraDelEspacio()
    {
        
        var espacio = new Espacio { NombreEspacio = "Espacio1", AdminEspacio = user };
        _logicaEspacio.CrearEspacio(espacio);
        var usuario2 = new Usuario()
        {
            Name = "Pedro",
            Mail = "a@gmail.com",
            LastName = "Rodriguez",
            Password = "123456789A",
        };
        _logicaUsuario.AgregarUsuario(usuario2);
        
        var resultado = _logicaEspacio.ObtenerMiembrosFueraDelEspacio(espacio, _logicaUsuario);

        
        Assert.AreEqual(1, resultado.Count);
    }
    
    [TestMethod]
    public void ObtenerPorIdEspacio_DebeRetornarEspacioPorId()
    {
        
        var espacio = new Espacio { NombreEspacio = "Espacio1", AdminEspacio = user };
        _logicaEspacio.CrearEspacio(espacio);
        
        var resultado = _logicaEspacio.ObtenerEspacioPorId(espacio.Id);

        
        Assert.AreEqual(espacio, resultado);
    }

    [TestMethod]
    public void ObtenerCategoriasDeEspacio_DebeRetornarCategoriasDelEspacio()
    {
        var espacio = new Espacio()
        {
            AdminEspacio = user,
            NombreEspacio = "General",
        };
        _logicaEspacio.CrearEspacio(espacio);
        var categorias = new Categoria()
        {
            Nombre = "Categoria1",
            Espacio = espacio,
            Tipo = "Costo",
            Estatus = "Activa"
        };
        _logicaCategoria.AgregarCategoria(categorias);
        
        var resultado = _logicaCategoria.ObtenerCategoriasDeEspacio(espacio);

        
        Assert.AreEqual(1, resultado.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicException))]
    public void ActualizarEspacioTiraExcepcion()
    {
        Espacio espacio = new Espacio()
        {
            AdminEspacio = user,
            NombreEspacio = "General",
        };
        _logicaEspacio.CrearEspacio(espacio);
        var espacio2 = new Espacio()
        {
            Id = espacio.Id,
            AdminEspacio = user,
        };
        _logicaEspacio.ActualiarEspacio(espacio2);

    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicException))]
    public void CrearEspacioTiraExcepcion()
    {
        Espacio espacio = new Espacio()
        {
            AdminEspacio = user,
        };
        _logicaEspacio.CrearEspacio(espacio);

    }

    [TestMethod]
    public void ObtenerCuentasDeEspacio_DebeRetornarCuentasDelEspacio()
    {
        
        var espacio = new Espacio();
        var mockCuentaRepository = new Mock<IRepository<Cuenta>>();
        var cuentas = new List<Cuenta>
        {
            new CuentaMonetaria { Espacio = espacio },
            new CuentaMonetaria(),
            new CuentaMonetaria { Espacio = espacio },
        };
        mockCuentaRepository.Setup(repo => repo.FindAll()).Returns(cuentas);

        var logicaCuenta = new LogicaCuenta(mockCuentaRepository.Object);

        var mockRepository = new Mock<IRepository<Espacio>>();
        var logicaEspacio = new LogicaEspacio(mockRepository.Object);

        
        var resultado = logicaCuenta.ListarCuentas(espacio);

        
        Assert.AreEqual(2, resultado.Count);
    }


    [TestMethod]
    public void ObtenerTransaccionesDeEspacio_DebeRetornarTransaccionesDelEspacio()
    {
        Espacio espacio = new Espacio()
        {
            AdminEspacio = user,
            NombreEspacio = "General",
        };
        _logicaEspacio.CrearEspacio(espacio);
        Categoria cat = new Categoria()
        {
            Nombre = "Categoria1",
            Espacio = espacio,
            Tipo = "Costo",
            Estatus = "Activa"
        };
        CuentaMonetaria cu = new CuentaMonetaria()
        {
            Nombre = "Cuenta1",
            Espacio = espacio,
            MontoInicial = 100,
            Moneda = "UYU",
        };
        _logicaCategoria.AgregarCategoria(cat);
        _logicaCuenta.AgregarCuentaMonetaria(cu, espacio);
        var transaccin = new Transaccion()
        {
            Espacio = espacio,
            Titulo = "Transaccion1",
            Monto = 100,
            Fecha = DateTime.Now,
            TipoTransaccion = "Ingreso",
            Moneda = "UYU",
            Cuenta = cu,
            Categoria = cat
        };
        _logicaTransaccion.NuevaTransaccion(transaccin);

        var resultado = _logicaTransaccion.ListarTransacciones(espacio);

        
        Assert.AreEqual(1, resultado.Count);
    }

    [TestMethod]
    public void ObtenerCotizacionesDeEspacio_DebeRetornarCotizacionesDelEspacio()
    {
        Espacio espacio = new Espacio()
        {
            AdminEspacio = user,
            NombreEspacio = "General",
        };
        _logicaEspacio.CrearEspacio(espacio);

        var cotizaciones = new TipoDeCambio()
        {
            
            Espacio = espacio,
            Moneda = "USD",
            Cotizacion = 10,
            Fecha = DateTime.Now,
        };
        _logicaTipoDeCambio.CrearCotizacion(cotizaciones);
        
        var resultado = _logicaTipoDeCambio.listarCambiosPorEspacio(espacio);

        
        Assert.AreEqual(1, resultado.Count);
    }

}