using Dominio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository;

namespace RepositoryTest;

[TestClass]
public class CuentaMemoryRepositoryTest
{
    public CuentaMemoryRepository repository;
    public CuentaMonetaria cm;
    public TarjetaCredito tc;

    [TestInitialize]
    public void setup()
    {
        repository = new CuentaMemoryRepository();
        cm = new CuentaMonetaria()
        {
            Nombre = "CM"
        };
        tc = new TarjetaCredito()
        {
            Nombre = "TC"
        };
    }

    [TestMethod]
    public void DeleteCuentaTest()
    {
        Cuenta c = new CuentaMonetaria();
        repository.Add(c);
        repository.Delete(c.Id);
        Assert.AreEqual(0, repository.FindAll().Count);
    }
    
    [TestMethod]
    public void UpdateCuentaTest()
    {
        Cuenta c = new CuentaMonetaria(){ Nombre = "CCC"};
        repository.Add(c);
        Cuenta c2 = new CuentaMonetaria(){ Id = 1};
        repository.Update(c2);
        Assert.AreNotEqual(c, c2);
    }
    
    [TestMethod]
    public void UpdateCuentaTest2()
    {
        repository.Add(cm);
        Cuenta cm2 = new CuentaMonetaria(){ Id = 1 };
        repository.Update(cm);
        Assert.AreNotEqual(cm, cm2);
    }
    
    [TestMethod]
    public void UpdateCuentaTest3()
    {
        repository.Add(tc);
        TarjetaCredito tc2 = new TarjetaCredito(){ Id = 1};
        repository.Update(tc2);
        Assert.AreNotEqual(tc, tc2);
    }
}