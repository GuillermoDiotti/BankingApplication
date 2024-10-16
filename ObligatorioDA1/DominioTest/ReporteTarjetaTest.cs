using Dominio;

namespace DominioTest;

[TestClass]
public class ReporteTarjetaTest
{
    private ReporteDeTarjeta reporte;
    
    [TestInitialize]
    public void SetUp()
    {
        reporte = new ReporteDeTarjeta()
        {
            Titulo = "Titulo",
            Moneda = "UYU",
            Fecha = DateTime.Today,
            Gasto = 100,
        };
    }

    [TestMethod]
    public void GetSetTest()
    {
        Assert.AreEqual("Titulo", reporte.Titulo);
        Assert.AreEqual("UYU", reporte.Moneda);
        Assert.AreEqual(DateTime.Today, reporte.Fecha);
        Assert.AreEqual(100, reporte.Gasto);
    }
}