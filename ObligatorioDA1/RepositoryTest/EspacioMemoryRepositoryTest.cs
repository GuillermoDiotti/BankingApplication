using System.Collections;
using Dominio;

namespace RepositoryTest;

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository;

[TestClass]
public class EspacioMemoryRepositoryTest
{
    public EspacioMemoryRepository repository;
    public Espacio e;

    [TestInitialize]
    public void setup()
    {
        repository = new EspacioMemoryRepository();
        e = new Espacio()
        {
           MiembrosEspacio = new List<Usuario>(),
           NombreEspacio = "ESP",
        };
    }

    [TestMethod]
    public void AddEspacioTest()
    {
        repository.Add(e);
        Assert.AreEqual(1, repository.FindAll().Count);
    }
    
    [TestMethod]
    public void FindEspacioTest()
    {
        repository.Add(e);
        var found = repository.Find(x => x.NombreEspacio == "ESP");
        Assert.AreEqual(e.NombreEspacio, found.NombreEspacio);
    }
    
    [TestMethod]
    public void FindAllEspacioTest()
    {
        Espacio e2 = new Espacio()
        {
            MiembrosEspacio = new List<Usuario>(),
            NombreEspacio = "ENG",
        };
        repository.Add(e);
        repository.Add(e2);
        Assert.AreEqual(2, repository.FindAll().Count);
    }
    
    [TestMethod]
    public void UpdateEspacioTest()
    {
        repository.Add(e);
        Espacio e2 = new Espacio()
        {
            MiembrosEspacio = new List<Usuario>(),
            NombreEspacio = "ENG",
        };
        repository.Update(e2);
        Assert.AreNotEqual(e.NombreEspacio, e2.NombreEspacio);
    }
    
    [TestMethod]
    public void DeleteEspacioTest()
    {
        repository.Add(e);
        repository.Delete(1);
        Assert.AreEqual(0, repository.FindAll().Count());
    }
    
}