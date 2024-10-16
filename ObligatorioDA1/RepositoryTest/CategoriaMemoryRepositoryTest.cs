using Dominio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository;

namespace RepositoryTest;

[TestClass]
public class CategoriaMemoryRepositoryTest
{
    public CategoriaMemoryRepository repository;
    public Categoria c;

    [TestInitialize]
    public void setup()
    {
        repository = new CategoriaMemoryRepository();
        c = new Categoria() { Nombre = "cat"};
    }

    [TestMethod]
    public void AddCategoriaTest()
    {
        repository.Add(c);
        Assert.AreEqual(1, repository.FindAll().Count);
    }

    [TestMethod]
    public void DeleteCategoriaTest()
    {
        repository.Add(c);
        repository.Delete(1);
        Assert.AreEqual(0, repository.FindAll().Count);
    }

    
    [TestMethod]
    public void FindCategoriaTest()
    {
        repository.Add(c);
        var found = repository.Find(f => f.Nombre == "cat");
        Assert.IsNotNull(found);
    }
    
    [TestMethod]
    public void UpdateCategoriaTest()
    {
        repository.Add(c);
        Categoria c2 = new Categoria() { Nombre = "cat2", Id = 1};
        repository.Update(c2);
        Assert.AreNotEqual(c,c2);
    }

    
}