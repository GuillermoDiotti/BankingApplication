using Dominio;

namespace DominioTest;

[TestClass]
public class TarjetaCreditoTest
{
    private TarjetaCredito tarjeta;
    
    [TestInitialize]
    public void SetUp()
    {
        tarjeta = new TarjetaCredito()
        {
            Nombre = "Sant",
            FechaCreacion = DateTime.Today,
            Moneda = "UYU",
            BancoEmisor = "Santander",
            CreditoDisponible = 3000,
            FechaCierre = DateTime.Today,
            UltimosDigitos = "9999",
        };
    }

    [TestMethod]
    public void GetUltimosDigitosTest()
    {
        Assert.AreEqual("9999", tarjeta.UltimosDigitos);
    }
    
    [TestMethod]
    public void GetBancoTest()
    {
        Assert.AreEqual("Santander", tarjeta.BancoEmisor);
    }
    
    [TestMethod]
    public void GetFechaCierreTest()
    {
        Assert.AreEqual(DateTime.Today, tarjeta.FechaCierre);
    }
}