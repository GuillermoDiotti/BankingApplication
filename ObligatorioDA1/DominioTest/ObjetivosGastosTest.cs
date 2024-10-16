using Dominio;

namespace DominioTest;
[TestClass]
public class ObjetivosGastosTest
{
    private ObjetivosGastos objGastos;

    [TestInitialize]
    public void setUp()
    {
        Usuario usuario = new Usuario()
        {
            Name = "Arturo",
            Mail = "arturo@fff.com",
            LastName = "Rodriguez",
            Password = "123456789A",
        };
        
        objGastos = new ObjetivosGastos()
        {
            Titulo = "Reducir gastos uber",
            MontoMaximo = 5000,
            Categorias = null,
            GastoActual = 0,
            URLHabilitada = true,
            Espacio = new Espacio()
            {
                AdminEspacio = usuario,
                NombreEspacio = "General",
            },
            UsuarioCreador = usuario,
        };
    }
    
    [TestMethod]
    public void GetSetTest()
    {
        Assert.AreEqual("Reducir gastos uber", objGastos.Titulo);
        Assert.AreEqual(5000, objGastos.MontoMaximo);
        Assert.AreEqual(null, objGastos.Categorias);
        Assert.AreEqual(0, objGastos.GastoActual);
        Assert.AreEqual(true, objGastos.URLHabilitada);
        Assert.IsNotNull(objGastos.Espacio);
        Assert.IsNotNull(objGastos.UsuarioCreador);
    }
    
}