using Dominio;
using LogicaTest.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository;

namespace LogicaTest;

[TestClass]
public class TiposDeCambioDatabaseRepositoryTest
{
    public TiposDeCambioDatabaseRepository repository;
    public TipoDeCambio tc;
    private SqlContext _context;
    private Espacio espacio;
    private Usuario usuario;
    
    [TestInitialize]
    public void setup()
    {
        SqlContextFactory sqlContextFactory = new SqlContextFactory();
        _context = sqlContextFactory.CreateMemoryContext();
        repository = new TiposDeCambioDatabaseRepository(_context);
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
        tc = new TipoDeCambio()
        {
            Espacio = espacio,
            Fecha = DateTime.Today,
            Cotizacion = 3,
            Moneda = "USD",
        };
        
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void UpdateTCTest()
    {
        repository.Add(tc);
        TipoDeCambio tc2 = new TipoDeCambio()
        {
            Espacio = espacio,
            Fecha = DateTime.Today,
            Cotizacion = 4,
            Moneda = "USD",
            Id = 1,
        };
        repository.Update(tc2);
        Assert.AreNotEqual(tc2,tc);
    }
    
}