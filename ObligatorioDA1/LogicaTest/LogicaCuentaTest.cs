using Dominio;
using Moq;
using Repository;
using Logica;
using LogicaTest.Context;

[TestClass]
public class LogicaCuentaTest
{
    private Usuario user;
    private LogicaCuenta _logicaCuenta;
    private LogicaTransaccion _logicaTransaccion;
    private LogicaCategoria _logicaCategoria;
    private LogicaTipoDeCambio _logicaTipoDeCambio;
    private LogicaUsuario _logicaUsuario;
    private LogicaEspacio _logicaEspacio;
    private Espacio espacio;
    private SqlContext _context;
    private TipoDeCambio tipoDeCambio;

    [TestInitialize]
    public void setup()
    {
        
        SqlContextFactory sqlContextFactory = new SqlContextFactory();
        _context = sqlContextFactory.CreateMemoryContext();
        user = new Usuario()
        {
            Mail = "Pedro@outlook.com",
            Password = "123456789A",
            LastName = "Cueva",
            Name = "Pedro",
        };
        espacio = new Espacio()
        {
            AdminEspacio = user,
            NombreEspacio = "General",
        };
        tipoDeCambio = new TipoDeCambio()
        {
            Espacio = espacio,
            Moneda = "USD",
            Cotizacion = 40,
            Fecha = DateTime.Today

        };
        IRepository<Cuenta> accRepository = new CuentaMemoryRepository();
        IRepository<Transaccion> traRepository = new TransaccionMemoryRepository();
        IRepository<Categoria> catRepository = new CategoriaDatabaseRepository(_context);
        IRepository<TipoDeCambio> tcRepository = new TiposDeCambioDatabaseRepository(_context);
        IRepository<Usuario> uRepository = new UsuarioDatabaseRepository(_context);
        IRepository<Espacio> eRepository = new EspacioDatabaseRepository(_context);

        _logicaCuenta = new LogicaCuenta(accRepository);
        _logicaTransaccion = new LogicaTransaccion(traRepository);
        _logicaCategoria = new LogicaCategoria(catRepository);
        _logicaTipoDeCambio = new LogicaTipoDeCambio(tcRepository);
        _logicaUsuario = new LogicaUsuario(uRepository);
        _logicaEspacio = new LogicaEspacio(eRepository);
        _logicaUsuario.AgregarUsuario(user);
        _logicaEspacio.CrearEspacio(espacio);
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }
    
    [TestMethod]
    public void NombreUnico_DebeVerificarSiElNombreEsÚnico()
    {
        var nombre = "Cuenta1";
        CuentaMonetaria c1 = new CuentaMonetaria
        {
            Nombre = "Cuenta1",
            Espacio = espacio
        };
        CuentaMonetaria c2 = new CuentaMonetaria
        {
            Nombre = "Cuenta2",
            Espacio = espacio
        };
        _logicaCuenta.AgregarCuentaMonetaria(c1, espacio);
        _logicaCuenta.AgregarCuentaMonetaria(c2, espacio);
        
        var resultado = _logicaCuenta.NombreUnico(espacio, nombre);
        
        Assert.IsFalse(resultado);
    }

    [TestMethod]
    public void ListarCuentasMonetarias_DebeRetornarListaDeCuentasMonetarias()
    {
        
        CuentaMonetaria c1 = new CuentaMonetaria
        {
            Nombre = "Cuenta1",
            Espacio = espacio,
            Moneda = "UYU",
            MontoInicial = 100
        };
        CuentaMonetaria c2 = new CuentaMonetaria
        {
            Nombre = "Cuenta2",
            Espacio = espacio,
            Moneda = "UYU",
            MontoInicial = 100
        };
        _logicaCuenta.AgregarCuentaMonetaria(c1, espacio);
        _logicaCuenta.AgregarCuentaMonetaria(c2, espacio);
        
        var resultado = _logicaCuenta.ListarCuentasMonetarias(espacio);

        
        Assert.AreEqual(2, resultado.Count);
    }

    [TestMethod]
    public void ListarTarjetas_DebeRetornarListaDeTarjetas()
    {
        _logicaCuenta.AgregarTarjetaDeCredito(new TarjetaCredito { Nombre = "Tarjeta1", Espacio = espacio }, espacio);
        
        var resultado = _logicaCuenta.ListarTarjetas(espacio);

        
        Assert.AreEqual(1, resultado.Count());
    }

    [TestMethod]
    public void EliminarCuenta_DebeEliminarCuenta()
    {
        
        var cuenta = new CuentaMonetaria()
        {
            Nombre = "Cuenta2",
            Espacio = espacio,
            Moneda = "UYU",
            MontoInicial = 100
        };
        _logicaCuenta.AgregarCuentaMonetaria(cuenta, espacio);
        _logicaCuenta.EliminarCuenta(cuenta, _logicaTransaccion);
        var resultado = _logicaCuenta.ListarCuentasMonetarias(espacio);

        Assert.AreEqual(0, resultado.Count());
    }

    [TestMethod]
    public void ModificarCuenta_DebeModificarCuenta()
    {
        var cuenta = new CuentaMonetaria()
        {
            Nombre = "Cuenta1",
            Espacio = espacio,
            Moneda = "UYU",
            MontoInicial = 100
        };
        _logicaCuenta.AgregarCuentaMonetaria(cuenta, espacio);
        var cuenta2 = new CuentaMonetaria()
        {
            Nombre = "Cuenta2",
            Id = cuenta.Id,
        };
        _logicaCuenta.ModificarCuentaMonetaria(cuenta2);

        var resultado = _logicaCuenta.ListarCuentasMonetarias(espacio).Find(x => x.Nombre == "Cuenta1");
        Assert.IsTrue(resultado == null);
    }

    [TestMethod]
    public void ChequearFormatoDigitos_DebeValidarElFormatoDeDigitos()
    {
        var resultadoValido = _logicaCuenta.ChequearFormatoDigitos("1234");
        var resultadoInvalido = _logicaCuenta.ChequearFormatoDigitos("12A4");

        
        Assert.IsTrue(resultadoValido);
        Assert.IsFalse(resultadoInvalido);
    }

    [TestMethod]
    public void CalcularSaldoDisponibleCuentasMonetarias_DebeCalcularSaldo()
    {
        var cuentaMonetaria = new CuentaMonetaria()
        {
            Nombre = "Cuenta1",
            Espacio = espacio,
            Moneda = "UYU",
            MontoInicial = 100
        };
        IRepository<TipoDeCambio> IRepository = new TipoDeCambioMemoryRepository();
        var logicaTipoDeCambio = new LogicaTipoDeCambio(IRepository);
        
        var resultado = _logicaCuenta.CalcularSaldoDisponibleCuentasMonetarias(logicaTipoDeCambio, espacio, cuentaMonetaria, _logicaTransaccion);

        
        Assert.AreEqual(100, resultado);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicException))]
    public void ValidarInputs_DebeTirarException()
    {
        var cuentaMonetaria = new CuentaMonetaria()
        {
            Nombre = "Cuenta1",
            Espacio = espacio,
            Moneda = "UYU",
            MontoInicial = -10
        };
        
        _logicaCuenta.ValidarInputs(cuentaMonetaria.MontoInicial, espacio, cuentaMonetaria);
        
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicException))]
    public void ValidarInputs_DebeTirarException2()
    {
        var cuentaMonetaria1 = new CuentaMonetaria()
        {
            Nombre = "Cuenta1",
            Espacio = espacio,
            Moneda = "UYU",
            MontoInicial = 100
        };
        _logicaCuenta.AgregarCuentaMonetaria(cuentaMonetaria1, espacio);
        var cuentaMonetaria2 = new CuentaMonetaria()
        {
            Nombre = "Cuenta1",
            Espacio = espacio,
            Moneda = "UYU",
            MontoInicial = 100
        };
        
        _logicaCuenta.ValidarInputs(cuentaMonetaria2.MontoInicial, espacio, cuentaMonetaria2);
        
    }
    
    [TestMethod]
    public void CalcularSaldoDisponibleCuentaMonetaria_DebeCalcularSaldo()
    {
        var categoria = new Categoria()
        {
            Nombre = "Categoria1",
            Espacio = espacio,
            Tipo = "Costo",
            Estatus = "Activa",
            FechaCreacion = DateTime.Today
        };
        _logicaCategoria.AgregarCategoria(categoria);
        var cuentaMonetaria1 = new CuentaMonetaria()
        {
            Nombre = "Cuenta1",
            Espacio = espacio,
            Moneda = "UYU",
            MontoInicial = 100
        };
        _logicaCuenta.AgregarCuentaMonetaria(cuentaMonetaria1, espacio);
        _logicaTransaccion.NuevaTransaccion(new Transaccion
        {
            Titulo = "DF5", 
            TipoTransaccion = "Costo", 
            Fecha = new DateTime(2023, 11, 1), 
            Categoria = categoria, 
            Monto = 50, 
            Espacio = espacio,
            Cuenta = cuentaMonetaria1,
            Moneda = "UYU"
            
        });
        
        var resultado = _logicaCuenta.calcularSaldoCuentasUYU(_logicaTipoDeCambio, espacio, cuentaMonetaria1, 0, 50, 100, _logicaTransaccion);
        
        Assert.AreEqual(0, resultado);
    }
    
    [TestMethod]
    public void CalcularSaldoDisponibleCuentaMonetaria_DebeCalcularSaldo2()
    {
        var categoria = new Categoria()
        {
            Nombre = "Categoria1",
            Espacio = espacio,
            Tipo = "Ingreso",
            Estatus = "Activa",
            FechaCreacion = DateTime.Today
        };
        _logicaCategoria.AgregarCategoria(categoria);
        var cuentaMonetaria1 = new CuentaMonetaria()
        {
            Nombre = "Cuenta1",
            Espacio = espacio,
            Moneda = "UYU",
            MontoInicial = 100
        };
        _logicaCuenta.AgregarCuentaMonetaria(cuentaMonetaria1, espacio);
        _logicaTransaccion.NuevaTransaccion(new Transaccion
        {
            Titulo = "DF5", 
            TipoTransaccion = "Ingreso", 
            Fecha = new DateTime(2023, 11, 1), 
            Categoria = categoria, 
            Monto = 50, 
            Espacio = espacio,
            Cuenta = cuentaMonetaria1,
            Moneda = "UYU"
            
        });
        
        var resultado = _logicaCuenta.calcularSaldoCuentasUYU(_logicaTipoDeCambio, espacio, cuentaMonetaria1, 50, 0, 100, _logicaTransaccion);
        
        Assert.AreEqual(200, resultado);
    }
    
    [TestMethod]
    public void CalcularSaldoDisponibleTarjeta_DebeCalcularSaldo()
    {
        var categoria = new Categoria()
        {
            Nombre = "Categoria1",
            Espacio = espacio,
            Tipo = "Ingreso",
            Estatus = "Activa",
            FechaCreacion = DateTime.Today
        };
        _logicaCategoria.AgregarCategoria(categoria);
        var tarjetaCredito = new TarjetaCredito()
        {
            Nombre = "Cuenta1",
            Espacio = espacio,
            Moneda = "UYU",
            BancoEmisor = "Santander",
            UltimosDigitos = "1234",
            FechaCierre = DateTime.Today,
            CreditoDisponible = 100
        };
        _logicaCuenta.AgregarTarjetaDeCredito(tarjetaCredito, espacio);
        _logicaTransaccion.NuevaTransaccion(new Transaccion
        {
            Titulo = "DF5", 
            TipoTransaccion = "Ingreso", 
            Fecha = new DateTime(2023, 11, 1), 
            Categoria = categoria, 
            Monto = 50, 
            Espacio = espacio,
            Cuenta = tarjetaCredito,
            Moneda = "UYU"
            
        });
        
        double resultado = _logicaCuenta.CalcularSaldoTarjetasUYU(_logicaTipoDeCambio, espacio, tarjetaCredito, 50, 0, 100, _logicaTransaccion);
        
        Assert.AreEqual(200, resultado);
    }

    [TestMethod]
    public void CalcularSaldoDisponibleTarjeta_DebeCalcularSaldo2()
    {
        var tarjetaCredito = new TarjetaCredito()
        {
            Nombre = "Cuenta1",
            Espacio = espacio,
            Moneda = "UYU",
            BancoEmisor = "Santander",
            UltimosDigitos = "1234",
            FechaCierre = DateTime.Today,
            CreditoDisponible = 100
        };
        
        var resultado = _logicaCuenta.CalcularSaldoDisponibleTarjeta(_logicaTipoDeCambio, espacio, tarjetaCredito, _logicaTransaccion);
        
        Assert.AreEqual(100, resultado);
    }
    
    [TestMethod]
    public void ModificarTarjeta_DebeModificarLaTarjeta()
    {
        var tarjetaCredito = new TarjetaCredito()
        {
            Nombre = "Cuenta1",
            Espacio = espacio,
            Moneda = "UYU",
            BancoEmisor = "Santander",
            UltimosDigitos = "1234",
            FechaCierre = DateTime.Today,
            CreditoDisponible = 100
        };
        _logicaCuenta.AgregarTarjetaDeCredito(tarjetaCredito, espacio);
        var tarjetaCreditoNueva = new TarjetaCredito()
        {
            Id = tarjetaCredito.Id,
            BancoEmisor = "Itau",
            UltimosDigitos = "9999",
            FechaCierre = DateTime.Today,
        };
        _logicaCuenta.ModificarTarjeta(espacio, tarjetaCreditoNueva);
        
        var tarCambiada =(TarjetaCredito) _logicaCuenta.ListarTarjetas(espacio).Find(x => x.Id == tarjetaCredito.Id);
        
        Assert.IsTrue(tarCambiada.BancoEmisor == "Itau" && tarCambiada.UltimosDigitos == "9999");
    }

    [TestMethod]
    public void TieneTransaccionAsociada_DebeVerificarTransaccionesAsociadas()
    {
        var cuentaMonetaria = new CuentaMonetaria()
        {
            Nombre = "Cuenta1",
            Espacio = espacio,
            Moneda = "UYU",
            MontoInicial = 100
        };
        var categoria = new Categoria()
        {
            Nombre = "Categoria1",
            Espacio = espacio,
            Tipo = "Costo",
            Estatus = "Activa",
            FechaCreacion = DateTime.Today
        };
        _logicaCategoria.AgregarCategoria(categoria);
        _logicaCuenta.AgregarCuentaMonetaria(cuentaMonetaria, espacio);
        _logicaTransaccion.NuevaTransaccion(new Transaccion
        {
            Titulo = "DF5", 
            TipoTransaccion = "Costo", 
            Fecha = new DateTime(2023, 11, 1), 
            Categoria = categoria, 
            Monto = 50, 
            Espacio = espacio,
            Cuenta = cuentaMonetaria
            
        });
        
        var resultado = _logicaCuenta.TieneTransaccionAsociada(espacio, cuentaMonetaria, _logicaTransaccion);
        
        
        Assert.IsTrue(resultado);
    }

    [TestMethod]
    [ExpectedException(typeof(LogicException))]
    public void CalcularSaldoTarjetasMonedaExtranjera()
    {
        Categoria cat = new Categoria()
        {
            Espacio = espacio,
            Nombre = "Gas",
            Estatus = "Activa",
            Tipo = "Costo",
            FechaCreacion = DateTime.Today
        };
        TarjetaCredito tar = new TarjetaCredito()
        {
            Espacio = espacio,
            CreditoDisponible = 1000,
            Moneda = "USD",
            Nombre = "SAN"
        };
        Transaccion tra = new Transaccion()
        {
            Espacio = espacio,
            Categoria = cat,
            TipoTransaccion = "Costo",
            Cuenta = tar,
            Fecha = DateTime.Today,
            Moneda = "USD",
            Monto = 200,
            Titulo = "Pagar gas"
        };
        _logicaTransaccion.NuevaTransaccion(tra);
        _logicaCuenta.AgregarTarjetaDeCredito(tar, espacio);
        _logicaCategoria.AgregarCategoria(cat);

        var res = _logicaCuenta.CalcularSaldoTarjetasMonedaExtranjera(_logicaTipoDeCambio, espacio,tar, 200, 200, 0,_logicaTransaccion );
    }
    
    [TestMethod]
    public void CalcularSaldoTarjetasMonedaExtranjera2()
    {
        _logicaTipoDeCambio.CrearCotizacion(tipoDeCambio);

        Categoria cat = new Categoria()
        {
            Espacio = espacio,
            Nombre = "Gas",
            Estatus = "Activa",
            Tipo = "Costo",
            FechaCreacion = DateTime.Today
        };
        TarjetaCredito tar = new TarjetaCredito()
        {
            Espacio = espacio,
            CreditoDisponible = 1000,
            Moneda = "USD",
            Nombre = "SAN"
        };
        Transaccion tra = new Transaccion()
        {
            Espacio = espacio,
            Categoria = cat,
            TipoTransaccion = "Costo",
            Cuenta = tar,
            Fecha = DateTime.Today,
            Moneda = "USD",
            Monto = 200,
            Titulo = "Pagar gas"
        };
        _logicaTransaccion.NuevaTransaccion(tra);
        _logicaCuenta.AgregarTarjetaDeCredito(tar, espacio);
        _logicaCategoria.AgregarCategoria(cat);

        var res = _logicaCuenta.CalcularSaldoTarjetasMonedaExtranjera(_logicaTipoDeCambio, espacio,tar, 200, 200, 1000,_logicaTransaccion );
        
        Assert.AreEqual(800, res);
    }
    
    [TestMethod]
    public void CalcularSaldoTarjetasMonedaExtranjera3()
    {
        _logicaTipoDeCambio.CrearCotizacion(tipoDeCambio);

        Categoria cat = new Categoria()
        {
            Espacio = espacio,
            Nombre = "Gas",
            Estatus = "Activa",
            Tipo = "Ingreso",
            FechaCreacion = DateTime.Today
        };
        TarjetaCredito tar = new TarjetaCredito()
        {
            Espacio = espacio,
            CreditoDisponible = 1000,
            Moneda = "USD",
            Nombre = "SAN"
        };
        Transaccion tra = new Transaccion()
        {
            Espacio = espacio,
            Categoria = cat,
            TipoTransaccion = "Ingreso",
            Cuenta = tar,
            Fecha = DateTime.Today,
            Moneda = "USD",
            Monto = 200,
            Titulo = "Pagar gas"
        };
        _logicaTransaccion.NuevaTransaccion(tra);
        _logicaCuenta.AgregarTarjetaDeCredito(tar, espacio);
        _logicaCategoria.AgregarCategoria(cat);

        var res = _logicaCuenta.CalcularSaldoTarjetasMonedaExtranjera(_logicaTipoDeCambio, espacio,tar, 200, 200, 1000,_logicaTransaccion );
        
        Assert.AreEqual(1200, res);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicException))]
    public void CalcularSaldoCuentaMonedaExtranjera()
    {
        Categoria cat = new Categoria()
        {
            Espacio = espacio,
            Nombre = "Gas",
            Estatus = "Activa",
            Tipo = "Costo",
            FechaCreacion = DateTime.Today
        };
        var cuentaMonetaria = new CuentaMonetaria()
        {
            Nombre = "Cuenta1",
            Espacio = espacio,
            Moneda = "USD",
            MontoInicial = 100
        };
        Transaccion tra = new Transaccion()
        {
            Espacio = espacio,
            Categoria = cat,
            TipoTransaccion = "Costo",
            Cuenta = cuentaMonetaria,
            Fecha = DateTime.Today,
            Moneda = "USD",
            Monto = 200,
            Titulo = "Pagar gas"
        };
        _logicaTransaccion.NuevaTransaccion(tra);
        _logicaCuenta.AgregarCuentaMonetaria(cuentaMonetaria, espacio);
        _logicaCategoria.AgregarCategoria(cat);

        var res = _logicaCuenta.calcularSaldoCuentasExtranjeras(_logicaTipoDeCambio, espacio,cuentaMonetaria, 200, 200, 0,_logicaTransaccion );
    }
    
    [TestMethod]
    public void CalcularSaldoCuentaMonedaExtranjera2()
    {
        _logicaTipoDeCambio.CrearCotizacion(tipoDeCambio);

        Categoria cat = new Categoria()
        {
            Espacio = espacio,
            Nombre = "Gas",
            Estatus = "Activa",
            Tipo = "Costo",
            FechaCreacion = DateTime.Today
        };
        var cuentaMonetaria = new CuentaMonetaria()
        {
            Nombre = "Cuenta1",
            Espacio = espacio,
            Moneda = "USD",
            MontoInicial = 200
        };
        Transaccion tra = new Transaccion()
        {
            Espacio = espacio,
            Categoria = cat,
            TipoTransaccion = "Costo",
            Cuenta = cuentaMonetaria,
            Fecha = DateTime.Today,
            Moneda = "USD",
            Monto = 50,
            Titulo = "Pagar gas"
        };
        _logicaTransaccion.NuevaTransaccion(tra);
        _logicaCuenta.AgregarCuentaMonetaria(cuentaMonetaria, espacio);
        _logicaCategoria.AgregarCategoria(cat);

        var res = _logicaCuenta.calcularSaldoCuentasExtranjeras(_logicaTipoDeCambio, espacio,cuentaMonetaria, 0, 50, 200,_logicaTransaccion );
        
        Assert.AreEqual(100, res);
    }
}
