using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dominio;
using Logica;
using Repository;
using LogicaTest.Context;

namespace LogicaTest
{
    [TestClass]
    public class LogicaTipoDeCambioTests
    {
        private IRepository<TipoDeCambio> _TipoDeCambioRepository;
        private LogicaTipoDeCambio _logicaTipoDeCambio;
        private LogicaTransaccion _logicaTransaccion;
        private LogicaUsuario _logicaUsuario;
        private LogicaEspacio _logicaEspacio;
        private Usuario user;
        private Espacio espacio;
        private SqlContext _context;

        [TestInitialize]
        public void Setup()
        {
            SqlContextFactory sqlContextFactory = new SqlContextFactory();
            _context = sqlContextFactory.CreateMemoryContext();
            
            user = new Usuario()
            {
                Address = null,
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
            IRepository<Transaccion> traRepository = new TransaccionDatabaseRepository(_context);
            IRepository<Usuario> uRepository = new UsuarioDatabaseRepository(_context);
            IRepository<Espacio> eRepository = new EspacioDatabaseRepository(_context);

            _TipoDeCambioRepository = new TiposDeCambioDatabaseRepository(_context);
            _logicaTipoDeCambio = new LogicaTipoDeCambio(_TipoDeCambioRepository);
            _logicaTransaccion = new LogicaTransaccion(traRepository);
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
        public void CrearCotizacion_ShouldAddExchangeRateToRepository()
        {
            
            var tipoDeCambio = new TipoDeCambio
            {
                Espacio = espacio,
                Fecha = DateTime.Now,
                Cotizacion = 1.0,
                Moneda = "USD",
            };

            
            _logicaTipoDeCambio.CrearCotizacion(tipoDeCambio);

            
            var listaTipoDeCambio = _TipoDeCambioRepository.FindAll().ToList();
            Assert.AreEqual(1, listaTipoDeCambio.Count);
            Assert.AreEqual(1.0, listaTipoDeCambio[0].Cotizacion);
            Assert.AreEqual("USD", listaTipoDeCambio[0].Moneda);
        }

        [TestMethod]
        public void ConseguirCotizacionPorFecha_ShouldReturnExchangeRateForDate()
        {
            var fecha = new DateTime(2023, 10, 1);
            var usd = 1.0;
            var tipoDeCambio = new TipoDeCambio
            {
                Espacio = espacio,
                Fecha = fecha,
                Cotizacion = usd,
                Moneda = "USD",
            };
            _logicaTipoDeCambio.CrearCotizacion(tipoDeCambio);
            
            var cotizacion = _logicaTipoDeCambio.ConseguirCotizacionPorFecha(fecha, "USD", espacio);

            
            Assert.IsNotNull(cotizacion);
            Assert.AreEqual(cotizacion.Fecha, fecha);
            Assert.AreEqual(usd, cotizacion.Cotizacion);
            Assert.AreEqual("USD", cotizacion.Moneda);
        }

        [TestMethod]
        public void ModificarCotizacion_ShouldModifyExchangeRateForDate()
        {
            
            var fecha = new DateTime(2023, 10, 1);
            var usd = 1.0;
            var tipoDeCambio = new TipoDeCambio
            {
                Espacio = espacio,
                Fecha = fecha,
                Cotizacion = usd,
                Moneda = "USD",
            };
            _logicaTipoDeCambio.CrearCotizacion(tipoDeCambio);
            var tipoDeCambioNuevo = new TipoDeCambio
            {
                Id = tipoDeCambio.Id,
                Cotizacion = usd,
            };
            
            _logicaTipoDeCambio.ModificarCotizacion(tipoDeCambio, tipoDeCambioNuevo,_logicaTransaccion);

            
            var listaTipoDeCambio = _TipoDeCambioRepository.FindAll().ToList();
            var cotizacion = listaTipoDeCambio.FirstOrDefault(c => c.Fecha == fecha.Date && c.Moneda == "USD");
            Assert.IsNotNull(cotizacion);
            Assert.AreEqual(1.0, cotizacion.Cotizacion);
        }

        [TestMethod]
        public void TieneTransaccionAsociada_ShouldReturnFalseWhenNoTransactionAssociated()
        {
            
            var cotizacion = new TipoDeCambio
            {
                Fecha = DateTime.Now,
                Cotizacion = 200,
                Moneda = "USD",
            };
            IRepository<Transaccion> IRepository = new TransaccionMemoryRepository();
            var logicaTransaccion = new LogicaTransaccion(IRepository);

            
            var result = _logicaTipoDeCambio.TieneTransaccionAsociada(null, cotizacion, "USD", logicaTransaccion);

            
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TieneTransaccionAsociada_ShouldReturnTrueWhenTransactionIsAssociated()
        {
            
            var fecha = DateTime.Now;
            var cuenta = new CuentaMonetaria
            {
                Nombre = "c",
                FechaCreacion = fecha,
                MontoInicial = 200,
                Moneda = "USD",
            };
            var categoria = new Categoria
            {
                Nombre = "cat",
                FechaCreacion = fecha,
                Estatus = "Activa",
                Tipo = "Ingreso"
            };
            var transaccion = new Transaccion
            {
                Fecha = fecha,
                Cuenta = cuenta,
                Moneda = "USD",
                TipoTransaccion = "Ingreso",
                Monto = 300,
                Categoria = categoria,
                Titulo = "titulo",
            };
            var tipoDeCambio = new TipoDeCambio
            {
                Fecha = fecha,
                Cotizacion = 200,
                Moneda = "USD",
            };
            IRepository<Transaccion> IRepository = new TransaccionMemoryRepository();
            var logicaTransaccion = new LogicaTransaccion(IRepository);
            logicaTransaccion.NuevaTransaccion(transaccion);

            
            var result = _logicaTipoDeCambio.TieneTransaccionAsociada(null, tipoDeCambio, "USD", logicaTransaccion);

            
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EliminarCambioTest1()
        {
            TipoDeCambio t = new TipoDeCambio(){ Id = 44, Cotizacion = 2, Espacio = espacio,Fecha = DateTime.Today, Moneda = "EUR"};
            _logicaTipoDeCambio.CrearCotizacion(t);
            _logicaTipoDeCambio.EliminarCambio(t, espacio, "USD", _logicaTransaccion);
            Assert.AreEqual(0, _logicaTipoDeCambio.listarCambiosPorEspacio(espacio).Count);
        }
        
        [TestMethod]
        public void EliminarCambioTest2()
        {
            TipoDeCambio t = new TipoDeCambio()
            { 
                Cotizacion = 2, 
                Espacio = espacio,
                Fecha = DateTime.Today, 
                Moneda = "EUR"
                
            };
            _logicaTipoDeCambio.CrearCotizacion(t);
            Transaccion tra = new Transaccion()
            { 
                Moneda = "USD", 
                Fecha = DateTime.Today, 
                Titulo = "gtry", 
                TipoTransaccion = "Costo",
                Espacio = espacio,
            };
            _logicaTransaccion.NuevaTransaccion(tra);
            _logicaTipoDeCambio.EliminarCambio(t, espacio, "USD", _logicaTransaccion);
            Assert.AreEqual(0, _logicaTipoDeCambio.listarCambiosPorEspacio(espacio).Count);
        }

        [TestMethod]
        public void CambioUnicoFechaTest()
        {
            DateTime f = DateTime.Now;
            TipoDeCambio tc = new TipoDeCambio()
            {
                Cotizacion = 1,
                Espacio = espacio,
                Fecha = f,
                Moneda = "USD",
            };
            _logicaTipoDeCambio.CrearCotizacion(tc);
            Assert.IsNotNull(_logicaTipoDeCambio.CambioUnicoParaLaFecha(f,espacio, "UYU"));
        }
        
        [TestMethod]
        public void CambioUnicoFechaTest2()
        {
            DateTime f = DateTime.Now;
            Assert.IsNotNull(_logicaTipoDeCambio.CambioUnicoParaLaFecha(f,espacio, "UYU"));
        }

        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void ValidarInputsTests()
        {
            DateTime f = DateTime.Now;
            string str = null;
            double dou = 1.01;
            Espacio e = espacio;
            
            _logicaTipoDeCambio.VaidarInputs(f,str,dou,e);
        }
        
        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void ValidarInputsTests2()
        {
            DateTime f = DateTime.Now;
            string str = "UYU";
            double dou = 1.01;
            Espacio e = espacio;
            
            _logicaTipoDeCambio.VaidarInputs(f,str,dou,e);
        }
        
        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void ValidarInputsTests3()
        {
            DateTime f = DateTime.Now;
            string str = "";
            double dou = 1.01;
            Espacio e = espacio;
            
            _logicaTipoDeCambio.VaidarInputs(f,str,dou,e);
        }
        
        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void ValidarInputsTests4()
        {
            DateTime f = DateTime.Now;
            string str = "";
            double dou = -10.01;
            Espacio e = espacio;
            
            
            
            _logicaTipoDeCambio.VaidarInputs(f,str,dou,e);
        }

        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void ValidarInputsTests5()
        {
            DateTime f = DateTime.Now;
            string str = "";
            double dou = -10.01;
            Espacio e = espacio;
            TipoDeCambio tc = new TipoDeCambio()
            {
                Id = 2,
                Cotizacion = 1,
                Espacio = espacio,
                Fecha = f,
                Moneda = "UYU",
            };
            _logicaTipoDeCambio.CrearCotizacion(tc);
            _logicaTipoDeCambio.VaidarInputs(f,str,dou,e);
        }
        
        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void ValidarInputsTests6()
        {
            DateTime f = DateTime.Now;
            string str = "UYU";
            double dou = 10.01;
            Espacio e = espacio;
            _logicaTipoDeCambio.VaidarInputs(f,str,dou,e);
        }

        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void PasarAPesosUruguayosTest()
        {
            DateTime f = DateTime.Now;
            string str = "USD";
            double dou = 10.01;
            Espacio e = espacio;
            _logicaTipoDeCambio.PasarAPesosUruguayos(e, str, dou, f);
        }
        
        [TestMethod]
        public void PasarAPesosUruguayosTest2()
        {
            DateTime f = DateTime.Today;
            string str = "USD";
            double dou = 10.01;
            Espacio e = espacio;
            TipoDeCambio tc = new TipoDeCambio()
            {
                Id = 2,
                Cotizacion = 1,
                Espacio = espacio,
                Fecha = f,
                Moneda = "USD",
            };
            _logicaTipoDeCambio.CrearCotizacion(tc);
            Assert.IsNotNull(_logicaTipoDeCambio.PasarAPesosUruguayos(e, str, dou, f));
        }
        
        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void PasarUYUADestino()
        {
            DateTime f = DateTime.Now;
            string str = "USD";
            double dou = 10.01;
            Espacio e = espacio;
            _logicaTipoDeCambio.PasarDeUYUAMonedaDestino(e, dou, f, str);
        }
        
        [TestMethod]
        public void PasarDeUYUADestino2()
        {
            DateTime f = DateTime.Today;
            string str = "USD";
            double dou = 10.01;
            Espacio e = espacio;
            TipoDeCambio tc = new TipoDeCambio()
            {
                Id = 2,
                Cotizacion = 1,
                Espacio = espacio,
                Fecha = f,
                Moneda = "USD",
            };
            _logicaTipoDeCambio.CrearCotizacion(tc);
            Assert.IsNotNull(_logicaTipoDeCambio.PasarDeUYUAMonedaDestino(e, dou, f, str));
        }
    }
}