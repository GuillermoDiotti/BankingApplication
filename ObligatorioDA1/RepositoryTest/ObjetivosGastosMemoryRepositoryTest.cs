using System.Collections;
using Dominio;

namespace RepositoryTest;

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository;

[TestClass]
public class ObjetivosGastosMemoryRepositoryTest
{
    public ObjetivosGastosMemoryRepository repository;
    
    [TestInitialize]
    public void SetUp()
    {
        repository = new ObjetivosGastosMemoryRepository();
    }

    [TestMethod]
    public void AgregarObjTest()
    {
        ObjetivosGastos obj = new ObjetivosGastos();
        repository.Add(obj);
        Assert.AreEqual(1, repository.FindAll().Count);
    }
    
    [TestMethod]
    public void FindObjTest()
    {
        ObjetivosGastos obj = new ObjetivosGastos(){ Titulo = "tit"};
        repository.Add(obj);
        var found = repository.Find(x => x.Titulo == "tit");
        Assert.IsNotNull(found);
    }
    
    [TestMethod]
    public void UpdateObjTest()
    {
        ObjetivosGastos obj = new ObjetivosGastos(){ Titulo = "Manzana"};
        ObjetivosGastos obj2 = new ObjetivosGastos() {Id = 1,Titulo = "Frutilla" };
        repository.Add(obj);
        repository.Update(obj2);
        Assert.AreEqual("Frutilla", repository.Find(x => x.Id == 1).Titulo);
    }
    
    [TestMethod]
    public void DeleteObjTest()
    {
        ObjetivosGastos obj = new ObjetivosGastos(){ Titulo = "Manzana"};
        repository.Add(obj);
        repository.Delete(1);
        Assert.IsNull( repository.Find(x => x.Id == 1));
    }
}