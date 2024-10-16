using Logica;

namespace LogicaTest;

[TestClass]
public class LogicExceptionTest
{
    [TestMethod]
    public void MensajeTest()
    {
        string mensaje = "LogicException";
        Assert.AreEqual(mensaje, new LogicException(mensaje).mensaje);
    }
}