using Dominio;
using LogicaTest.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository;

namespace RepositoryTest;

[TestClass]
public class ObjetivosGastosDatabaseTest
{
    public ObjetivosGastosDatabaseRepository repository;
    private SqlContext _context;
    private Usuario usuario;
    private ObjetivosGastos og;

    [TestInitialize]
    public void setup()
    {
        SqlContextFactory sqlContextFactory = new SqlContextFactory();
        _context = sqlContextFactory.CreateMemoryContext();
        repository = new ObjetivosGastosDatabaseRepository(_context);
        
        usuario = new Usuario()
        {
            Name = "Lucas",
            LastName = "Perez",
            Password = "123456789A",
            Mail = "hola@www.com",
        };
        
        og = new ObjetivosGastos(){
            Categorias = new List<Categoria>(),
            Titulo = "Objetivo",
            UsuarioCreador = usuario,
        };
    
    }

    [TestCleanup]
    public void CleanUp()
    {
     _context.Database.EnsureDeleted();
     }

    [TestMethod]
    public void AddObjetivoTest()
    {
        repository.Add(og);
        Assert.IsNotNull(repository.Find(x => x.Id == og.Id));
    }
    
    [TestMethod]
    public void DeleteObjetivoTest()
    {
        repository.Add(og);
        int id = og.Id;
        repository.Delete(id);
        Assert.IsNull(repository.Find(x => x.Id == id));
    }

    [TestMethod]
    public void UpdateObjetivoTest()
    {
        ObjetivosGastos og2 = new ObjetivosGastos(){
            Categorias = new List<Categoria>(),
            Titulo = "Objetivo",
            UsuarioCreador = usuario,
            Id = 1,
        };

        repository.Add(og);
        repository.Update(og2);
        
        Assert.AreNotEqual(og, og2);
    }
    
}