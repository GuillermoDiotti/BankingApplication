using Dominio;
using Logica;

namespace DominioTest;

[TestClass]

public class TestTransaccion
{
    private Transaccion transaccion;
    
    [TestInitialize]
    public void SetUp()
    {
        Cuenta cuenta = new CuentaMonetaria()
        {
            Nombre = "Caja de ahorros Santander",
            MontoInicial = 160,
            Moneda = "USD",
            FechaCreacion = DateTime.Now.Date
        };

        Categoria categoria = new Categoria()
        {
            Nombre = "Comida",
            FechaCreacion = DateTime.Now.Date,
            Estatus = "Activo",
            Tipo = "Gasto"
        };
        
        transaccion = new Transaccion()
        {
            Titulo = "Compra de comida",
            Monto = 100.00,
            Moneda = "USD",
            Cuenta = cuenta,
            Categoria = categoria,
            TipoTransaccion = "Costo",
            Fecha = new DateTime(12/12/2020)
        };
    }

    [TestMethod]
    public void GetSetTest()
    {
        Assert.AreEqual("Compra de comida", transaccion.Titulo);
        Assert.AreEqual(new DateTime(12/12/2020), transaccion.Fecha);
        Assert.AreEqual("USD", transaccion.Moneda);
        Assert.AreEqual("Costo", transaccion.TipoTransaccion);
        
        Assert.AreEqual("Caja de ahorros Santander", transaccion.Cuenta.Nombre);
        Assert.AreEqual(DateTime.Now.Date, transaccion.Cuenta.FechaCreacion);
        Assert.AreEqual("USD", transaccion.Cuenta.Moneda);
        
        Assert.AreEqual("Comida", transaccion.Categoria.Nombre);
        Assert.AreEqual(DateTime.Now.Date, transaccion.Categoria.FechaCreacion);
        Assert.AreEqual("Activo", transaccion.Categoria.Estatus);
        Assert.AreEqual("Gasto", transaccion.Categoria.Tipo);
        
    }
}