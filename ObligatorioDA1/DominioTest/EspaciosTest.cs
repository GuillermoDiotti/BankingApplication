using System.Xml;
using Dominio;

namespace DominioTest;

[TestClass]
public class EspaciosTest
{
    private Espacio _espacio;
    
    [TestInitialize]
    public void Setup()
    {
        _espacio = new Espacio()
        {
            NombreEspacio = "Espacio1",
            AdminEspacio = new Usuario()
            {
                Name = "Admin",
                LastName = "Admin",
                Mail = "admin@gmail.com",
                Password = "123456789A"
            },
            AdminEspacioId = 33,
                
        };
    }
    
    
    
    [TestMethod]
    public void GetSetTest()
    {
        Assert.AreEqual("Espacio1", _espacio.NombreEspacio);
        Assert.AreEqual("Admin", _espacio.AdminEspacio.Name);
        Assert.AreEqual(33, _espacio.AdminEspacioId);
    }
    
    
}