using Dominio;

namespace DominioTest;

[TestClass]
public class TestCuenta
{
    private CuentaMonetaria cuenta;
    
    [TestInitialize]
    public void setUp()
    {
        cuenta = new CuentaMonetaria()
        {
            Nombre = "Juan",
            MontoInicial = 1000.09,
            Moneda = "UYU",
            FechaCreacion = default,
        };
    }

    [TestMethod]
    public void GetSetTest()
    {
        Assert.AreEqual("Juan", cuenta.Nombre);
        Assert.AreEqual(1000.09, cuenta.MontoInicial);
        Assert.AreEqual("UYU", cuenta.Moneda);
        Assert.AreEqual(default, cuenta.FechaCreacion);
    }
    
    [TestMethod]
    public void NotEmptyNameTest1()
    {
        Assert.IsFalse(cuenta.Nombre.Length == 0);
    }
    
    [TestMethod]
    public void NotEmptyNameTest2()
    {
        cuenta.Nombre = "";
        Assert.IsTrue(cuenta.Nombre.Length == 0);
    }
    
    /*TODO: test que un Usuario no pueda tener 2 cuentas mismo nombre en clase Usuario!!!*/
    
    
}