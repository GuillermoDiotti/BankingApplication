using Dominio;
using LogicaTest.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository;

namespace RepositoryTest;

[TestClass]
public class CategoriaDatabaseRepositoryTest
{
    public CategoriaDatabaseRepository repository;
    private SqlContext _context;
    private Categoria cat;
    private Usuario usuario;

    [TestInitialize]
    public void setup()
    {
        SqlContextFactory sqlContextFactory = new SqlContextFactory();
        _context = sqlContextFactory.CreateMemoryContext();
        repository = new CategoriaDatabaseRepository(_context);
        usuario = new Usuario()
        {
            Name = "Lucas",
            LastName = "Perez",
            Password = "123456789A",
            Mail = "hola@www.com",
        };
        cat = new Categoria()
        {
            Estatus = "Activa",
            Nombre = "Categoria",
            Espacio = new Espacio()
            {
                AdminEspacio  = usuario,
                MiembrosEspacio = new List<Usuario>(),
                NombreEspacio = "ESPACIO",
                AdminEspacioId = usuario.Id,
            },
            Tipo = "Costo",
            FechaCreacion = DateTime.Today,
            ObjetivosGastosList = new List<ObjetivosGastos>(),
        };
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void DeleteCategoriaTest()
    {
        repository.Add(cat);
        repository.Delete(cat.Id);
    }
    
    [TestMethod]
    public void UpdateCategoriaTest()
    {
        repository.Add(cat);
        Categoria cat2 = new Categoria()
        {
            Estatus = "Activa",
            Nombre = "Categoria2",
            Espacio = new Espacio()
            {
                AdminEspacio  = usuario,
                MiembrosEspacio = new List<Usuario>(),
                NombreEspacio = "ESPACIO",
                AdminEspacioId = usuario.Id,
            },
            Tipo = "Costo",
            FechaCreacion = DateTime.Today,
            ObjetivosGastosList = new List<ObjetivosGastos>(),
            Id = 1,
        };
        repository.Update(cat2);
    }
}