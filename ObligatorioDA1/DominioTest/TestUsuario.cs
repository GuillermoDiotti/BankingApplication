using Dominio;
using Logica;
using LogicaTest.Context;
using Repository;

namespace DominioTest;

[TestClass]
public class TestUsuario
{
    private Usuario user;
    private Usuario u;
    private LogicaCuenta _logicaCuenta;
    private LogicaTransaccion _logicaTransaccion;
    private SqlContext _context;
    
    [TestInitialize]
    public void setUp()
    {
        
        SqlContextFactory sqlContextFactory = new SqlContextFactory();
        _context = sqlContextFactory.CreateMemoryContext();
        user = new Usuario
        {
            Name = "Pedro",
            Mail = "pedro@gmail.com",
            LastName = "Rodriguez",
            Password = "123456789A",
            Address = "456 Av Italia"
        };
        
        Categoria categoriaGenerica = new Categoria
        {
            Nombre = "Comida",
            FechaCreacion = default,
            Estatus = "Activa",
            Tipo = "Costo",
        };
        
        u = new Usuario()
        {
            Address = null,
            Mail = "Pedro@outlook.com",
            Password = "123456789A",
            LastName = "Cueva",
            Name = "Pedro",
        };

        IRepository<Cuenta> IRepository = new CuentaDatabaseRepository(_context);
        IRepository<Transaccion> traRepository = new TransaccionDatabaseRepository(_context);
        _logicaCuenta = new LogicaCuenta(IRepository);
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }
    
    [TestMethod]
    public void ValuesNotNullTest()
    {
        Assert.AreEqual(false, user.IsNull(user.Name));
        Assert.AreEqual(false, user.IsNull(user.LastName));
    }

    [TestMethod]
    public void GetSetTest()
    {
        Assert.AreEqual("Pedro", user.Name);
        Assert.AreEqual("pedro@gmail.com", user.Mail);
        Assert.AreEqual("Rodriguez", user.LastName);
        Assert.AreEqual("123456789A", user.Password);
        Assert.AreEqual("456 Av Italia", user.Address);
    }
    
    [TestMethod]
    public void CrearCuentaMonetariaTest()
    {
        bool seAgrego = false;
        
        List<Usuario> list = new List<Usuario>();
        list.Add(user);
        Espacio e = new Espacio()
        {
            Id = 1,
            AdminEspacio = user,
            MiembrosEspacio = list,
            NombreEspacio = "General",
            AdminEspacioId = user.Id,
        };

        CuentaMonetaria cuenta1 = new CuentaMonetaria()
        {
            MontoInicial = 0,
            Moneda = "UYU",
            FechaCreacion = default,
            Espacio = e,
            Nombre = "CCCC",
            Id = 3,
        };
        _logicaCuenta.AgregarCuentaMonetaria(cuenta1, e);
        var c = _logicaCuenta.ListarCuentasMonetarias(e);
        foreach (CuentaMonetaria cuenta in c)
        {
            if (cuenta.Nombre.Equals(cuenta1.Nombre))
            {
                seAgrego = true;
            }
        }
        Assert.IsTrue(seAgrego);
    }

    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void SetPasswordTest()
    {
        u.Password = "a";
    }

    [TestMethod]
    public void SetPasswordTest2()
    {
        string pass = "1234567890ABC";
        u.Password = pass;
        
        Assert.AreEqual(pass,u.Password);
    }
    
    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void SetPasswordTest3()
    {
        u.Password = "Ahdbghdfbhvsbfvd8y7vydf7" +
                     "v8g7r6g76gv7d6g4567g7vgdyfvgdsv766";
    }
    
    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void SetNameTest()
    {
        u.Name = "";
    }
    
    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void SetLastNameTest()
    {
        u.LastName = "";
    }
}
