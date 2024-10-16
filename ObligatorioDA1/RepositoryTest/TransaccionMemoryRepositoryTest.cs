using Dominio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository;

namespace RepositoryTest;

[TestClass]
public class TransaccionMemoryRepositoryTest
{
    public TransaccionMemoryRepository repository;
    public Transaccion tra;

    [TestInitialize]
    public void setup()
    {
        repository = new TransaccionMemoryRepository();
        tra = new Transaccion() { Titulo = "T1" };
    }

    [TestMethod]
    public void DeleteTransaccionTest()
    {
        repository.Add(tra);
        repository.Delete(tra.Id);
        Assert.AreEqual(0, repository.FindAll().Count);
    }
    
    [TestMethod]
    public void UpdateTransaccionTest()
    {
        repository.Add(tra);
        Transaccion tra2 = new Transaccion()
        {
            Titulo = "T2",
            Id = 1,
        };
        repository.Update(tra2);
        Assert.AreNotEqual(tra, tra2);
    }
}