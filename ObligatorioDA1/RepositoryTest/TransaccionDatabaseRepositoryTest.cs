using Dominio;
using LogicaTest.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository;

namespace RepositoryTest;

[TestClass]
public class TransaccionDatabaseRepositoryTest
{
    public TransaccionDatabaseRepository repository;
    private SqlContext _context;
    private Transaccion tra;
    private Usuario usuario;
    private Espacio e;

    [TestInitialize]
    public void setup()
    {
        SqlContextFactory sqlContextFactory = new SqlContextFactory();
        _context = sqlContextFactory.CreateMemoryContext();

        repository = new TransaccionDatabaseRepository(_context);
        
        usuario = new Usuario()
        {
            Name = "Lucas",
            LastName = "Perez",
            Password = "123456789A",
            Mail = "hola@www.com",
        };
        e = new Espacio()
        {
            AdminEspacio  = usuario,
            MiembrosEspacio = new List<Usuario>(),
            NombreEspacio = "ESPACIO",
            AdminEspacioId = usuario.Id,
        };
        
        tra = new Transaccion()
        {
            Categoria = new Categoria()
            {
               Espacio = e,
                FechaCreacion = DateTime.Today,
                Estatus = "Activa",
                Nombre = "Cat",
                Tipo = "Costo"
            },
            Espacio = e,
            Fecha = DateTime.Today,
            Moneda = "USD",
            Monto = 2000,
            TipoTransaccion = "Costo",
            Titulo = "TRA",
            Cuenta = new CuentaMonetaria()
            {
                Espacio = e,
                FechaCreacion = DateTime.Today,
                Moneda = "USD",
                Nombre = "Cuenta",
                MontoInicial = 3000,
            }
            
        };
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void DeleteTranTest()
    {
        repository.Add(tra);
        repository.Delete(tra.Id);
    }
    
    [TestMethod]
    public void UpdateTranTest()
    {
        repository.Add(tra);

        Transaccion tra2 = new Transaccion()
        {
            Categoria = new Categoria()
            {
                Espacio = e,
                FechaCreacion = DateTime.Today,
                Estatus = "Activa",
                Nombre = "Cat2",
                Tipo = "Costo"
            },
            Espacio = e,
            Fecha = DateTime.Today,
            Moneda = "USD",
            Monto = 1000,
            TipoTransaccion = "Costo",
            Titulo = "TRA2",
            Id = 1,
            Cuenta = new CuentaMonetaria()
            {
                Espacio = e,
                FechaCreacion = DateTime.Today,
                Moneda = "USD",
                Nombre = "Cuenta",
                MontoInicial = 5000,
            }

        };

        repository.Update(tra2);

    }
}