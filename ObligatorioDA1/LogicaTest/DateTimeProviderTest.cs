using Logica;

namespace LogicaTest;
[TestClass]
public class DateTimeProviderTest
{
 
    [TestMethod]
    public void ObtenerFechaHoy_DebeDevolverFechaActual()
    {
        var dateTimeProvider = new DateTimeProvider();
        
        DateTime fechaHoy = dateTimeProvider.ObtenerFechaHoy();
        DateTime fechaActual = DateTime.Now;
        
        Assert.AreEqual(fechaActual.Date, fechaHoy.Date);
    }
    
}