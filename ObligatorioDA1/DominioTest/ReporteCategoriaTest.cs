using Dominio;

namespace DominioTest;

[TestClass]
public class ReporteCategoriaTest
{
    private ReporteDeCategoria rep;
    
    [TestInitialize]
    public void SetUp()
    {
        rep = new ReporteDeCategoria()
        {
            GastoPorCategoria = 200,
            NombreCategoria = "Cate",
            PorcentajeDeLTotal = 10,
        };
    }
    
    [TestMethod]
    public void GetSetTest()
    {
        Assert.AreEqual(200, rep.GastoPorCategoria);
        Assert.AreEqual("Cate", rep.NombreCategoria);
        Assert.AreEqual(10, rep.PorcentajeDeLTotal);
    }
}