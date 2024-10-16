using Dominio;
using LogicaTest.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository;

namespace RepositoryTest;

[TestClass]
public class EspacioDatabaseRepositoryTest
{
    public EspacioDatabaseRepository repository;
    private SqlContext _context;
    public Espacio e;

    [TestInitialize]
    public void setup()
    {
        SqlContextFactory sqlContextFactory = new SqlContextFactory();
        _context = sqlContextFactory.CreateMemoryContext();

        repository = new EspacioDatabaseRepository(_context);
        Usuario usuario = new Usuario()
        {
            Name = "Lucas",
            LastName = "Perez",
            Password = "123456789A",
            Mail = "hola@www.com",
        };
        e = new Espacio()
        {
            AdminEspacio  = usuario,
            MiembrosEspacio = new List<Usuario>(),
            NombreEspacio = "ESPACIO",
            AdminEspacioId = usuario.Id,
        };
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void DeleteEspacio()
    {
        repository.Add(e);
        repository.Delete(e.Id);
    }
}