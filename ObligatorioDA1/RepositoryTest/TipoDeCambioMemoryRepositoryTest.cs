using Dominio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository;

namespace RepositoryTest;

[TestClass]
public class TipoDeCambioMemoryRepositoryTest
{
    public TipoDeCambioMemoryRepository repository;
    public TipoDeCambio tc;

    [TestInitialize]
    public void setup()
    {
        repository = new TipoDeCambioMemoryRepository();
        tc = new TipoDeCambio()
        {
            Cotizacion = 2,
            Moneda = "USD",
            Fecha = DateTime.Today
        };
    }

    [TestMethod]
    public void AddTCTest()
    {
        repository.Add(tc);
        Assert.AreEqual(1, repository.FindAll().Count);
    }
    
    [TestMethod]
    public void DeleteTCTest()
    {
        repository.Add(tc);
        repository.Delete(1);
        Assert.AreEqual(0, repository.FindAll().Count);
    }
    
    [TestMethod]
    public void FindTCTest()
    {
        repository.Add(tc);
        Assert.IsNotNull(repository.Find(x => x.Id == 1));
    }
    
    [TestMethod]
    public void UpdateTCTest()
    {
        repository.Add(tc);
        TipoDeCambio tc2 = new TipoDeCambio()
        {
            Id = 1,
            Cotizacion = 3,
            Moneda = "USD",
        };
        repository.Update(tc2);
        Assert.AreNotEqual(tc, tc2);
    }
    
}