using System.Diagnostics.Tracing;
using Dominio;
using Logica;
using Repository;

using LogicaTest.Context;

namespace LogicaTest
{
    [TestClass]
    public class LogicaTransaccionTests
    {
        private IRepository<Transaccion> _transaccionRepository;
        private LogicaTransaccion _logicaTransaccion;
        private LogicaTipoDeCambio _logicaTipoDeCambio;
        private LogicaCategoria _logicaCategoria;
        private LogicaEspacio _logicaEspacio;
        private LogicaUsuario _logicaUsuario;
        private LogicaCuenta _logicaCuenta;
        private Usuario user;
        private Espacio espacio;
        private SqlContext _context;
        private Categoria categoria;
        private CuentaMonetaria cuenta;

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
            _transaccionRepository = new TransaccionDatabaseRepository(_context);
            IRepository<TipoDeCambio> tc = new TiposDeCambioDatabaseRepository(_context);
            IRepository<Categoria> cat = new CategoriaDatabaseRepository(_context);
            IRepository<Espacio> es = new EspacioDatabaseRepository(_context);
            IRepository<Usuario> us = new UsuarioDatabaseRepository(_context);
            IRepository<Cuenta> cu = new CuentaDatabaseRepository(_context);
            _logicaEspacio = new LogicaEspacio(es);
            _logicaCategoria = new LogicaCategoria(cat);
            _logicaTransaccion = new LogicaTransaccion(_transaccionRepository);
            _logicaTipoDeCambio = new LogicaTipoDeCambio(tc);
            _logicaUsuario = new LogicaUsuario(us);
            _logicaCuenta = new LogicaCuenta(cu);
            _logicaUsuario.AgregarUsuario(user);
            _logicaEspacio.CrearEspacio(espacio);
            
            categoria = new Categoria 
            { 
                Nombre = "Alimentos",
                Estatus = "Activa",
                Tipo = "Costo", 
                Espacio = espacio
            };
            _logicaCategoria.AgregarCategoria(categoria);
            cuenta = new CuentaMonetaria()
            {
                Nombre = "Cuenta",
                MontoInicial = 100,
                Espacio = espacio,
                Moneda = "USD",
            };
            _logicaCuenta.AgregarCuentaMonetaria(cuenta, espacio);
        }
        
        [TestCleanup]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }

        [TestMethod]
        public void NuevaTransaccion_ShouldAddTransactionToRepository()
        {
            var transaccion = new Transaccion
            {
                Categoria = categoria,
                TipoTransaccion = "Costo",
                Titulo = "TTT",
                Espacio = espacio,
                Cuenta = cuenta
            };
            _logicaTransaccion.NuevaTransaccion(transaccion);
            
            var listaTransacciones = _transaccionRepository.FindAll().ToList();
            Assert.AreEqual(1, listaTransacciones.Count);
        }

        [TestMethod]
        public void ModificarTransaccion_ShouldModifyTransactionProperties()
        {
            var transaccion = new Transaccion
            {
                Categoria = categoria,
                TipoTransaccion = "Costo",
                Titulo = "TTT",
                Espacio = espacio,
                Cuenta = cuenta,
                Moneda = "USD",
                Monto = 100.0
            };
            _logicaTransaccion.NuevaTransaccion(transaccion);
            var transaccionNueva = new Transaccion
            {
                Id = transaccion.Id,
                Moneda = "EUR",
                Monto = 120.0
            };
  
            _logicaTransaccion.ModificarTransaccion(transaccionNueva);

            
            Assert.AreEqual("EUR", transaccion.Moneda);
            Assert.AreEqual(120.0, transaccion.Monto);
        }
        
        [TestMethod]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void ModificarTransaccion_NoModifica()
        {
            var transaccion = new Transaccion
            {
                Categoria = categoria,
                TipoTransaccion = "Costo",
                Titulo = "TTT",
                Espacio = espacio,
                Cuenta = cuenta,
                Moneda = "USD",
                Monto = 100.0
            };
            var transaccionNueva = new Transaccion
            {
                Id = transaccion.Id,
                Moneda = "EUR",
                Monto = 120.0
            };
            _logicaTransaccion.ModificarTransaccion(transaccionNueva);
        }

        [TestMethod]
        public void ListarTransacciones_ShouldReturnTransactionsForGivenSpace()
        {
            var transaccion = new Transaccion
            {
                Categoria = categoria,
                TipoTransaccion = "Costo",
                Titulo = "TTT",
                Espacio = espacio,
                Cuenta = cuenta,
                Moneda = "USD",
                Monto = 100.0
            };
            _logicaTransaccion.NuevaTransaccion(transaccion);
            
            var transacciones = _logicaTransaccion.ListarTransacciones(espacio);

            
            Assert.AreEqual(1, transacciones.Count);
            Assert.AreEqual(espacio, transacciones[0].Espacio);
        }

        [TestMethod]
        public void TotalGastadoPorMes_ShouldReturnTotalCostForGivenMonth()
        {
            var transaccion = new Transaccion
            {
                Categoria = categoria,
                TipoTransaccion = "Costo",
                Titulo = "TTT",
                Espacio = espacio,
                Cuenta = cuenta,
                Moneda = "USD",
                Monto = 100.0,
                Fecha = new DateTime(2023, 10, 1)
            };
            _logicaTransaccion.NuevaTransaccion(transaccion);
            var transaccion2 = new Transaccion
            {
                Categoria = categoria,
                TipoTransaccion = "Costo",
                Titulo = "TTT",
                Espacio = espacio,
                Cuenta = cuenta,
                Moneda = "USD",
                Monto = 100.0,
                Fecha = new DateTime(2023, 10, 1)
            };
            _logicaTransaccion.NuevaTransaccion(transaccion2);

            var mes = 10;

            
            var totalGastado = _logicaTransaccion.TotalGastadoPorMes(espacio, mes);

            
            Assert.AreEqual(200.0, totalGastado);
        }
        
        [TestMethod]
        public void TotalGastadoPorMes_ShouldReturnTotalCostForGivenMonth2()
        {
            var transaccion = new Transaccion
            {
                Categoria = categoria,
                TipoTransaccion = "Costo",
                Titulo = "TTT",
                Espacio = espacio,
                Cuenta = cuenta,
                Moneda = "USD",
                Monto = 100.0,
                Fecha = new DateTime(2023, 10, 1)
            };
            _logicaTransaccion.NuevaTransaccion(transaccion);

            var mes = 15;

            
            var totalGastado = _logicaTransaccion.TotalGastadoPorMes(espacio, mes);

            
            Assert.AreEqual(0, totalGastado);
        }

        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void ValidarInputsTest()
        {
            _logicaTransaccion.ValidarInputs(new Categoria(), null, null);
        }
        
        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void ValidarInputsTest2()
        {
            _logicaTransaccion.ValidarInputs(null, new CuentaMonetaria(), null);
        }
        
        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void ValidarInputsTest3()
        {
            _logicaTransaccion.ValidarInputs(null, null, 100);
        }
        
        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void ValidarInputsTest4()
        {
            _logicaTransaccion.ValidarInputs(null, null, -100);
        }
        
        [TestMethod]
        public void ValidarInputsTest5()
        {
            _logicaTransaccion.ValidarInputs(new Categoria(), new CuentaMonetaria(), 100);
        }
        
        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void ValidarInputsTest6()
        {
            _logicaTransaccion.ValidarInputs(new Categoria(), null, 100);
        }
        
        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void ValidarInputsTest7()
        {
            _logicaTransaccion.ValidarInputs(null, new CuentaMonetaria(), 100);
        }
        
        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void ValidarInputsTest8()
        {
            _logicaTransaccion.ValidarInputs(new Categoria(), new CuentaMonetaria(), -90);
        }

        [TestMethod]
        public void TotalIngresosPorDiaAnio()
        {
            double res = _logicaTransaccion.TotalIngresosPorDiaEnElAño(espacio, 1, 10, _logicaTipoDeCambio);
            Assert.AreEqual(0, res);
        }
        
        
        
        [TestMethod]
        public void TotalEgresosPorDiaAnio()
        {
            double res = _logicaTransaccion.TotalEgresosPorDiaEnElAño(espacio, 1, 10, _logicaTipoDeCambio);
            Assert.AreEqual(0, res);
        }
    }
}