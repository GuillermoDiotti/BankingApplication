using Dominio;

namespace DominioTest;

[TestClass]
public class ReporteObjetivosGastos
{
    private ReporteDeobjetivosDeGastos reporte;
    
    [TestInitialize]
    public void SetUp()
    {
        reporte = new ReporteDeobjetivosDeGastos()
        {
            CumpleObjetivo = false,
            MontoDefinido = 100,
            MontoGastado = 50,
            TituloObjetivo = "Objetivo",
        };
    }

    [TestMethod]
    public void GetSetTest()
    {
        Assert.AreEqual("Objetivo", reporte.TituloObjetivo);
        Assert.AreEqual(100, reporte.MontoDefinido);
        Assert.AreEqual(50, reporte.MontoGastado);
        Assert.IsFalse(reporte.CumpleObjetivo);
    }
}