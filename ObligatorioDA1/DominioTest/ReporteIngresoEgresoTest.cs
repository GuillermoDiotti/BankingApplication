using System.Xml;
using Dominio;

namespace DominioTest;

[TestClass]
public class ReporteIngresoEgresoTest
{
    private ReporteIngresosEgresos rep;

    [TestInitialize]
    public void setup()
    {
        rep = new ReporteIngresosEgresos();
    }
    
    [TestMethod]
    public void GetSetTest()
    {
        rep.Egreso = 200;
        rep.Ingreso = 500;
        rep.DiaDelMes = 4;
        Assert.AreEqual(200, rep.Egreso);
        Assert.AreEqual(500, rep.Ingreso);
        Assert.AreEqual(4, rep.DiaDelMes);
    }
}