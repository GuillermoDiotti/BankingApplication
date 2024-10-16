using Dominio;

namespace DominioTest;

[TestClass]
public class CategoriaTest
{
    private Categoria categoria;
    private Categoria categoria2;
    private Categoria categoria3;
    
    [TestInitialize]
    public void SetUp()
    {
        categoria = new Categoria()
        {
            Nombre = "Cat",
            FechaCreacion = DateTime.Today,
            Estatus = "Activa",
            Tipo = "Ingreso",
        };
        categoria2 = new Categoria()
        {
            Nombre = "Cat",
            FechaCreacion = DateTime.Today,
            Estatus = "Activa",
            Tipo = "Costo",
        };
        categoria3 = new Categoria()
        {
            Nombre = "Categoria3",
            FechaCreacion = DateTime.Today,
            Estatus = "Activa",
            Tipo = "Costo",
        };
    }

    [TestMethod]
    public void ToStringTest()
    {
        Assert.AreEqual("Cat", categoria.ToString());
    }

    [TestMethod]
    public void EqualsTest()
    {
        Assert.IsTrue(categoria.Equals(categoria2));
    }
    
    [TestMethod]
    public void EqualsTest2()
    {
        Assert.IsFalse(categoria.Equals(categoria3));
    }
    
    [TestMethod]
    public void EqualsTest3()
    {
        Usuario u = new Usuario()
        {
            Name = "Pedro",
            Mail = "pedro@ort.com",
            LastName = "Cueva",
            Password = "1234567890Q",
            Address = null,
        };
        Assert.IsFalse(categoria.Equals(u));
    }

    [TestMethod]
    public void GetHashCode()
    {
        Assert.AreEqual(categoria.Nombre.GetHashCode(), categoria.GetHashCode());
    }
}