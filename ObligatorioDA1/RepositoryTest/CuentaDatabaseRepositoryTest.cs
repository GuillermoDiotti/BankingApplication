using Dominio;
using LogicaTest.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository;

namespace RepositoryTest;

[TestClass]
public class CuentaDatabaseRepositoryTest
{
    public CuentaDatabaseRepository repository;
    public Cuenta c;
    private SqlContext _context;
    private Espacio espacio;
    private Usuario usuario;

    [TestInitialize]
    public void setup()
    {
        SqlContextFactory sqlContextFactory = new SqlContextFactory();
        _context = sqlContextFactory.CreateMemoryContext();
        repository = new CuentaDatabaseRepository(_context);
        
        usuario = new Usuario()
        {
            Name = "Lucas",
            LastName = "Perez",
            Password = "123456789A",
            Mail = "hola@www.com",
        };
        
        espacio = new Espacio()
        {
            AdminEspacio  = usuario,
            MiembrosEspacio = new List<Usuario>(),
            NombreEspacio = "ESPACIO",
            AdminEspacioId = usuario.Id,
        };
        
        c = new CuentaMonetaria()
        {
            Nombre = "Cuenta",
            Moneda = "UYU",
            Espacio = espacio,
            FechaCreacion = DateTime.Now,
            MontoInicial = 1000,
        };
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }


    [TestMethod]
    public void FindCuentaTest()
    {
        repository.Add(c);
        var found = repository.Find(x => x.Id == c.Id);
        Assert.IsNotNull(found);
    }
    
    [TestMethod]
    public void DeleteCuentaTest()
    {
        repository.Add(c);
        repository.Delete(c.Id);
        Assert.AreEqual(0, repository.FindAll().Count);
    }
    
    [TestMethod]
    public void UpdateCuentaTest()
    {
        repository.Add(c);
        CuentaMonetaria c2 = new CuentaMonetaria()
        {
            Nombre = "Cuenta2",
            Moneda = "UYU",
            Espacio = espacio,
            FechaCreacion = DateTime.Today,
            MontoInicial = 300,
            Id = c.Id,
        };
        repository.Update(c2);
        Assert.AreNotEqual(c,c2);
    }
    
    [TestMethod]
    public void UpdateCuentaTest2()
    {
        TarjetaCredito t = new TarjetaCredito()
        {
            Nombre = "Tarjeta",
            Moneda = "UYU",
            Espacio = espacio,
            FechaCreacion = DateTime.Today,
            BancoEmisor = "BROU",
            UltimosDigitos = "5345",
            CreditoDisponible = 300,
            FechaCierre = DateTime.MaxValue,
        };
        
        repository.Add(t);
        
        TarjetaCredito t2 = new TarjetaCredito()
        {
            Nombre = "Tarjeta2",
            Moneda = "UYU",
            Id = t.Id,
            Espacio = espacio,
            FechaCreacion = DateTime.Today,
            BancoEmisor = "SA",
            UltimosDigitos = "5783",
            CreditoDisponible = 300,
            FechaCierre = DateTime.MaxValue,
        };
        repository.Update(t2);
        Assert.AreNotEqual(t,t2);
    }

    [TestMethod]
    public void DeleteCuentaTest2()
    {
        Cuenta acc = new CuentaMonetaria()
        {
            Nombre = "Cuenta",
            Moneda = "UYU",
            Espacio = espacio,
            FechaCreacion = DateTime.Today,
            MontoInicial = 200,
        };

        repository.Add(acc);
        repository.Delete(acc.Id);
        Assert.AreEqual(0, repository.FindAll().Count);
    }
}