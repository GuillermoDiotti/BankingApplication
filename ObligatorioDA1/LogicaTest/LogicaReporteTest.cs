using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Dominio;
using LogicaTest.Context;
using Repository;

namespace Logica.Tests
{
    [TestClass]
    public class LogicaReporteTests
    {
        private LogicaEspacio _logicaEspacio;
        private LogicaCategoria _logicaCategoria;
        private LogicaCuenta _logicaCuenta;
        private LogicaTransaccion _logicaTransaccion;
        private LogicaTipoDeCambio _logicaTipoDeCambio;
        private LogicaObjetivos _logicaObjetivos;
        private LogicaReporte _logicaReporte;
        private LogicaUsuario _logicaUsuario;
        private Usuario user;
        private Espacio espacio;
        private SqlContext _context;

        [TestInitialize]
        public void setup()
        {
            SqlContextFactory sqlContextFactory = new SqlContextFactory();
            _context = sqlContextFactory.CreateMemoryContext();

            IRepository<Categoria> catRepository = new CategoriaDatabaseRepository(_context);
            IRepository<Espacio> espacioRepository = new EspacioDatabaseRepository(_context);
            IRepository<Cuenta> accRepository = new CuentaDatabaseRepository(_context);
            IRepository<Transaccion> tranRepository = new TransaccionDatabaseRepository(_context);
            IRepository<TipoDeCambio> cambioRepository = new TiposDeCambioDatabaseRepository(_context);
            IRepository<ObjetivosGastos> objRepository = new ObjetivosGastosDatabaseRepository(_context);
            IRepository<Usuario> uRepository = new UsuarioDatabaseRepository(_context);

            _logicaEspacio = new LogicaEspacio(espacioRepository);
            _logicaCategoria = new LogicaCategoria(catRepository);
            _logicaCuenta = new LogicaCuenta(accRepository);
            _logicaTransaccion = new LogicaTransaccion(tranRepository);
            _logicaTipoDeCambio = new LogicaTipoDeCambio(cambioRepository);
            _logicaObjetivos = new LogicaObjetivos(objRepository);
            _logicaUsuario = new LogicaUsuario(uRepository);
            _logicaReporte = new LogicaReporte();
            user = new Usuario
            {
                Name = "Pedro",
                Mail = "pedro@gmail.com",
                LastName = "Rodriguez",
                Password = "123456789A",
                Address = "456 Av Italia"
            };
            _logicaUsuario.AgregarUsuario(user);
            espacio = new Espacio()
            {
                AdminEspacio = user,
                NombreEspacio = "General",
            };
            _logicaEspacio.CrearEspacio(espacio);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GenerarReporteDeObjetivos_DebeGenerarReporteDeObjetivos()
        {
            var cuenta = new CuentaMonetaria()
            {
                Nombre = "Cuenta",
                MontoInicial = 100,
                Espacio = espacio,
                Moneda = "UYU",
            };
            _logicaCuenta.AgregarCuentaMonetaria(cuenta, espacio);
            var categoria = new Categoria
            {
                Nombre = "Alimentos",
                Estatus = "Activa",
                Tipo = "Costo",
                Espacio = espacio
            };
            _logicaCategoria.AgregarCategoria(categoria);
            Transaccion t1 = new Transaccion()
            {
                Titulo = "DF5",
                TipoTransaccion = "Costo",
                Fecha = new DateTime(2023, 11, 1),
                Categoria = categoria,
                Monto = 50,
                Espacio = espacio,
                Cuenta = cuenta,
                Moneda = "UYU"
            };
            _logicaTransaccion.NuevaTransaccion(t1);
            var objetivo = new ObjetivosGastos()
            {
                URL = "1234",
                Titulo = "Obj1",
                MontoMaximo = 100,
                Espacio = espacio,
                UsuarioCreador = user,
                Categorias = new List<Categoria>() { categoria },
            };
            var lista = new List<ObjetivosGastos> { objetivo };
            _logicaObjetivos.CrearObjetivo(objetivo);


            var resultado = _logicaReporte.GenerarReporteDeObjetivos(_logicaObjetivos, lista, espacio,
                _logicaTipoDeCambio, _logicaTransaccion);


            Assert.AreEqual(1, resultado.Count);
            Assert.IsTrue(resultado[0].CumpleObjetivo);
            Assert.AreEqual(100, resultado[0].MontoDefinido);
            Assert.AreEqual(50, resultado[0].MontoGastado);
            Assert.AreEqual("Obj1", resultado[0].TituloObjetivo);
        }

        [TestMethod]
        public void GenerarReporteDeCategorias_DebeGenerarReporteDeCategorias()
        {
            var cuenta = new CuentaMonetaria()
            {
                Nombre = "Cuenta",
                MontoInicial = 100,
                Espacio = espacio,
                Moneda = "UYU",
            };
            _logicaCuenta.AgregarCuentaMonetaria(cuenta, espacio);
            var categoria = new Categoria
            {
                Nombre = "Alimentos",
                Estatus = "Activa",
                Tipo = "Costo",
                Espacio = espacio
            };
            _logicaCategoria.AgregarCategoria(categoria);
            Transaccion t1 = new Transaccion()
            {
                Titulo = "DF5",
                TipoTransaccion = "Costo",
                Fecha = new DateTime(2023, 11, 1),
                Categoria = categoria,
                Monto = 50,
                Espacio = espacio,
                Cuenta = cuenta,
                Moneda = "UYU"
            };
            _logicaTransaccion.NuevaTransaccion(t1);
            var objetivo = new ObjetivosGastos()
            {
                URL = "1234",
                Titulo = "Obj1",
                MontoMaximo = 100,
                Espacio = espacio,
                UsuarioCreador = user,
                Categorias = new List<Categoria>() { categoria },
            };
            var lista = new List<Categoria> { categoria };
            _logicaObjetivos.CrearObjetivo(objetivo);


            var resultado = _logicaReporte.GenerarReporteDeCategorias(_logicaTransaccion, _logicaCategoria, lista,
                espacio, DateTime.Now.Month);


            Assert.AreEqual(1, resultado.Count);
            Assert.AreEqual("Alimentos", resultado[0].NombreCategoria);
            Assert.AreEqual(50, resultado[0].GastoPorCategoria);
            Assert.AreEqual(100, resultado[0].PorcentajeDeLTotal);
        }

        [TestMethod]
        public void GenerarReportePorTarjeta_DebeGenerarReportePorTarjeta()
        {
            var cuenta = new TarjetaCredito()
            {
                Nombre = "Cuenta",
                Espacio = espacio,
                Moneda = "UYU",
                UltimosDigitos = "1234",
                CreditoDisponible = 1000,
                BancoEmisor = "Itau"
            };
            _logicaCuenta.AgregarTarjetaDeCredito(cuenta, espacio);
            var categoria = new Categoria
            {
                Nombre = "Alimentos",
                Estatus = "Activa",
                Tipo = "Costo",
                Espacio = espacio
            };
            _logicaCategoria.AgregarCategoria(categoria);
            Transaccion t1 = new Transaccion()
            {
                Titulo = "DF5",
                TipoTransaccion = "Costo",
                Fecha = new DateTime(2023, 11, 1),
                Categoria = categoria,
                Monto = 50,
                Espacio = espacio,
                Cuenta = cuenta,
                Moneda = "UYU"
            };
            _logicaTransaccion.NuevaTransaccion(t1);
            var objetivo = new ObjetivosGastos()
            {
                URL = "1234",
                Titulo = "Obj1",
                MontoMaximo = 100,
                Espacio = espacio,
                UsuarioCreador = user,
                Categorias = new List<Categoria>() { categoria },
            };
            _logicaObjetivos.CrearObjetivo(objetivo);

            var iDateTimeProvider = new Mock<IDateTimeProvider>();
            iDateTimeProvider.Setup(p => p.ObtenerFechaHoy()).Returns(new DateTime(2023, 11, 1));

            var resultado =
                _logicaReporte.GenerarReportePorTarjeta(espacio, cuenta, _logicaTransaccion, iDateTimeProvider.Object);


            Assert.AreEqual(1, resultado.Count);
            Assert.AreEqual("UYU", resultado[0].Moneda);
            Assert.AreEqual(50, resultado[0].Gasto);
        }

        [TestMethod]
        public void GenerarReporteCategoriasTest()
        {
            List<Categoria> lista = new List<Categoria>();
            var resp = _logicaReporte.GenerarReporteDeCategorias(_logicaTransaccion, _logicaCategoria, lista, espacio,
                7);
            Assert.AreEqual(0, resp.Count);
        }

        [TestMethod]
        public void GenerarReportePorTarjeta_DebeGenerarReporteIngresoegreso()
        {
            var cuenta = new TarjetaCredito()
            {
                Nombre = "Cuenta",
                Espacio = espacio,
                Moneda = "UYU",
                UltimosDigitos = "1234",
                CreditoDisponible = 1000,
                BancoEmisor = "Itau"
            };
            _logicaCuenta.AgregarTarjetaDeCredito(cuenta, espacio);
            var categoria = new Categoria
            {
                Nombre = "Alimentos",
                Estatus = "Activa",
                Tipo = "Costo",
                Espacio = espacio
            };
            _logicaCategoria.AgregarCategoria(categoria);
            Transaccion t1 = new Transaccion()
            {
                Titulo = "DF5",
                TipoTransaccion = "Costo",
                Fecha = new DateTime(2023, 11, 1),
                Categoria = categoria,
                Monto = 50,
                Espacio = espacio,
                Cuenta = cuenta,
                Moneda = "UYU"
            };
            _logicaTransaccion.NuevaTransaccion(t1);
            var objetivo = new ObjetivosGastos()
            {
                URL = "1234",
                Titulo = "Obj1",
                MontoMaximo = 100,
                Espacio = espacio,
                UsuarioCreador = user,
                Categorias = new List<Categoria>() { categoria },
            };
            _logicaObjetivos.CrearObjetivo(objetivo);
            var resultado =
                _logicaReporte.GenerarReporteIngresosEgresos(_logicaTransaccion, espacio, 2, _logicaTipoDeCambio, 28);


            Assert.AreEqual(28, resultado.Count);

        }
        
        [TestMethod]
        public void GenerarReportePor()
        {
            var cuenta = new TarjetaCredito()
            {
                Nombre = "Cuenta",
                Espacio = espacio,
                Moneda = "UYU",
                UltimosDigitos = "1234",
                CreditoDisponible = 1000,
                BancoEmisor = "Itau",
                FechaCierre = new DateTime(2023, 11, 1),
            };
            _logicaCuenta.AgregarTarjetaDeCredito(cuenta, espacio);
            var categoria = new Categoria
            {
                Nombre = "Alimentos",
                Estatus = "Activa",
                Tipo = "Costo",
                Espacio = espacio
            };
            _logicaCategoria.AgregarCategoria(categoria);
            Transaccion t1 = new Transaccion()
            {
                Titulo = "DF5",
                TipoTransaccion = "Costo",
                Fecha = new DateTime(2023, 11, 3),
                Categoria = categoria,
                Monto = 50,
                Espacio = espacio,
                Cuenta = cuenta,
                Moneda = "UYU",
            };
            _logicaTransaccion.NuevaTransaccion(t1);
            var objetivo = new ObjetivosGastos()
            {
                URL = "1234",
                Titulo = "Obj1",
                MontoMaximo = 100,
                Espacio = espacio,
                UsuarioCreador = user,
                Categorias = new List<Categoria>() { categoria },
            };
            _logicaObjetivos.CrearObjetivo(objetivo);
            
            var iDateTimeProvider = new Mock<IDateTimeProvider>();
            iDateTimeProvider.Setup(p => p.ObtenerFechaHoy()).Returns(new DateTime(2023, 11, 3));

            
            var resultado =
                _logicaReporte.GenerarReportePorTarjeta(espacio, cuenta, _logicaTransaccion, iDateTimeProvider.Object);


            Assert.AreEqual(1, resultado.Count);
            Assert.AreEqual("UYU", resultado[0].Moneda);
            Assert.AreEqual(50, resultado[0].Gasto);

        }
    }
    
    
    
}