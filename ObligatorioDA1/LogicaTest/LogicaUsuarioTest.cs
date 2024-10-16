using System.Net.Mail;
using System.Reflection.Metadata;
using Dominio;
using Logica;
using LogicaTest.Context;
using Repository;

namespace LogicaTest;

[TestClass]
public class LogicaUsuarioTest
{

    private LogicaUsuario _logicaUsuario;
    private IRepository<Usuario> _UserRepository;
    private SqlContext _context;
    
    public Usuario usuarioGeneral;
    public Usuario usuario1;
    
    [TestInitialize]
    public void setup()
    {
        usuarioGeneral = new Usuario
        {
            Name = "Juan",
            Mail = "juan@gmail.com",
            LastName = "Gonzalez",
            Password = "123456789A",
            Address = "456 Av Italia"
        };
        usuario1 = new Usuario
        {
            Name = "Pedro",
            Mail = "pedro@gmail.com",
            LastName = "Rodriguez",
            Password = "123456789A",
            Address = "456 Av Italia"
        };

        SqlContextFactory sqlContextFactory = new SqlContextFactory();
        _context = sqlContextFactory.CreateMemoryContext();
        
        _UserRepository = new UsuarioDatabaseRepository(_context);
        _logicaUsuario = new LogicaUsuario(_UserRepository);
        _logicaUsuario.AgregarUsuario(usuarioGeneral);
    }
    
    [TestCleanup]
    public void TestCleanup()
    {
        _context.Database.EnsureDeleted();
    }
    
    
    [TestMethod]
    public void AgregarUsuarioTest()
    {
        _logicaUsuario.AgregarUsuario(usuario1);
        Usuario? usuarioExiste = _logicaUsuario.FindByMail(usuario1.Mail);
        Assert.IsNotNull(usuarioExiste);
        Assert.AreEqual(usuario1.Mail, usuarioExiste.Mail);
        Assert.AreEqual("Pedro", usuario1.Name);
        Assert.AreEqual("pedro@gmail.com",usuario1.Mail);
        Assert.AreEqual("Rodriguez", usuario1.LastName);
        Assert.AreEqual( "123456789A",usuario1.Password );
        Assert.AreEqual("456 Av Italia", usuario1.Address);
    }
    
    [TestMethod]
    public void UsuarioUnicoTest()
    {
        Usuario usuario1 = new Usuario
        {
            Name = "Pedro",
            Mail = "pedro@gmail.com",
            LastName = "Rodriguez",
            Password = "123456789A",
            Address = "456 Av Italia"
        };
        _logicaUsuario.AgregarUsuario(usuario1);
        Usuario usuario2 = new Usuario
        {
            Name = "Juan",
            Mail = "pedro@gmail.com",
            LastName = "Hernandez",
            Password = "123456789A",
            Address = "456 Av Italia"
        };
        Assert.IsFalse(_logicaUsuario.MailUnico(usuario2.Mail));
    }
    
    [TestMethod]
    public void UsuarioUnicoTest2()
    {
        Usuario usuario1 = new Usuario
        {
            Name = "Pedro",
            Mail = "pedro@gmail.com",
            LastName = "Rodriguez",
            Password = "123456789A",
            Address = "456 Av Italia"
        };
        _logicaUsuario.AgregarUsuario(usuario1);
        Usuario usuario2 = new Usuario
        {
            Name = "Juan",
            Mail = "pedro2@gmail.com",
            LastName = "Hernandez",
            Password = "123456789A",
            Address = "456 Av Italia"
        };
        Assert.IsTrue(_logicaUsuario.MailUnico(usuario2.Mail));
    }

    [TestMethod]
    [ExpectedException(typeof(LogicException))]
    public void IniciarSesionTest()
    {
        Usuario usuario1 = new Usuario
        {
            Name = "Pedro",
            Mail = "pedro@gmail.com",
            LastName = "Rodriguez",
            Password = "123456789A",
            Address = "456 Av Italia"
        };
        _logicaUsuario.AgregarUsuario(usuario1);
        string mailPrueba = "pedro@gmail.com";
        string passPrueba = "hola";
        _logicaUsuario.IniciarSesion(mailPrueba, passPrueba);
    }
    
    [TestMethod]
    [ExpectedException(typeof(LogicException))]
    public void iniciarSesionTest2()
    {
        new Usuario
        {
            Name = "Pedro",
            Mail = "pedro@gmail.com",
            LastName = "Rodriguez",
            Password = "123456789A",
            Address = "456 Av Italia"
        };
        string mailPrueba = "gabriel@gmail.com";
        string passPrueba = "123456789A";
        _logicaUsuario.IniciarSesion(mailPrueba, passPrueba);
    }

    [TestMethod]
    public void iniciarSesionTest3()
    {
        _logicaUsuario.AgregarUsuario(usuario1);
        _logicaUsuario.IniciarSesion(usuario1.Mail, usuario1.Password);
    }

    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void RegistroDeUsuarioTest()
    {
        new Usuario
        {
            Name = "Pedro",
            Mail = "pedro@gmail.com",
            LastName = "Rodriguez",
            Password = "123",
            Address = "456 Av Italia"
        };
    }
    
    [TestMethod]
    public void RegistroDeUsuarioTest2()
    {
        _logicaUsuario.AgregarUsuario(usuario1);
        Usuario? usuarioExiste = _logicaUsuario.FindByMail(usuario1.Mail);
        Assert.IsNotNull(usuarioExiste);
        Assert.AreEqual(usuario1.Mail, usuarioExiste.Mail);
    }

    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void FormatoMailTest1()
    {
        string mail="dasdsasdasd.com";
        usuarioGeneral.Mail = mail;
    }
    
    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void FormatoMailTest2()
    {
        string mail="dasdsasdasd@gmail";
        usuarioGeneral.Mail = mail;
    }
    
    [TestMethod]
    public void FormatoMailTest3()
    {
        string mail="dasdsasdasd@gmail.com";
        usuarioGeneral.Mail = mail;
        Assert.AreEqual(mail, usuarioGeneral.Mail);
    }

    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void FormatoPassTest1()
    {
        string pass = "123A";
        usuarioGeneral.Password = pass;
    }
    
    [TestMethod]
    [ExpectedException(typeof(DomainException))]
    public void FormatoPassTest2()
    {
        string pass = "1234567890";
        usuarioGeneral.Password = pass;
    }
    
    [TestMethod]
    public void FormatoPassTest3()
    {
        string pass = "1234567890A";
        usuarioGeneral.Password = pass;
        Assert.AreEqual(pass, usuarioGeneral.Password);
    }
    
    [TestMethod]
    public void ChangeNameTest()
    {
        Usuario oldUser = new Usuario()
        {
            Name = "DASD",
            Mail = "akdamv@gmail.com",
            LastName = "Rodriguez",
            Password = "123456789A",
        };
        _logicaUsuario.AgregarUsuario(oldUser);
        Usuario updatedUser = new Usuario()
        {
            Id = oldUser.Id,
            Name = "Arturo",
            LastName = "Gonzalez",
            Password = "333444AAA12121",
        };
        _logicaUsuario.Update(updatedUser);
        Assert.AreNotEqual(updatedUser.Name, usuario1.Name);
        Assert.AreNotEqual(updatedUser.LastName, usuario1.LastName);
        Assert.AreNotEqual(updatedUser.Password, usuario1.Password);
    }

    [TestMethod]
    public void MailUnico()
    {
        _logicaUsuario.AgregarUsuario(usuario1);
        string mailRepetido = "pedro@gmail.com";
        Assert.IsFalse(_logicaUsuario.MailUnico(mailRepetido));
    }

    [TestMethod]
    public void FindAllUsersTest()
    {
        Usuario nuevoUsuario = new Usuario()
        {
            Name = "Nahuel",
            Mail = "hola@gma.com",
            LastName = "Rodriguez",
            Password = "123456789A",
        };
        _logicaUsuario.AgregarUsuario(nuevoUsuario);
        Assert.AreEqual(2,_logicaUsuario.FindAllUsers().Count);
    }
    
    [TestMethod]
    public void FindAllUsersTest2()
    {
        Assert.AreEqual(1,_logicaUsuario.FindAllUsers().Count);
    }

    [TestMethod]
    public void ConseguirUsuarioPorMail()
    {
        Usuario us = new Usuario()
        {
            Id = 900,
            LastName = "PP",
            Mail = "marceliddddddddto@ort.com",
            Name = "NDNAND",
            Password = "123456789A"
        };
        _logicaUsuario.AgregarUsuario(us);
        string result = _logicaUsuario.ConseguirUsuarioPorMail(us.Mail).Mail;
        Assert.AreEqual(us.Mail, result);
    }
    
    [TestMethod]
    public void ConseguirUsuarioPorMail2()
    {
        Usuario us = new Usuario()
        {
            Id = 877,
            LastName = "PP",
            Mail = "marcelito@ort.com",
            Name = "NDNAND",
            Password = "123456789A"
        };
        _logicaUsuario.AgregarUsuario(us);
        Assert.IsNull(_logicaUsuario.ConseguirUsuarioPorMail("zucaritas@ddd.com"));
    }
    
    [TestMethod]
    public void ConseguirUsuarioPorMail3()
    {
        Assert.IsNull(_logicaUsuario.ConseguirUsuarioPorMail("gasgfds@gmail.com"));
    }
    
    [TestMethod]
    public void ConseguirUsuarioPorId()
    {
        Usuario us = new Usuario()
        {
            Id = 900,
            LastName = "PP",
            Mail = "marceliddddddddto@ort.com",
            Name = "NDNAND",
            Password = "123456789A"
        };
        _logicaUsuario.AgregarUsuario(us);
        Assert.AreEqual(us.Id,_logicaUsuario.ConseguirUsuarioPorId(900).Id);
    }
    
    [TestMethod]
    public void ConseguirUsuarioPorId2()
    {
        Usuario us = new Usuario()
        {
            Id = 877,
            LastName = "PP",
            Mail = "marcelito@ort.com",
            Name = "NDNAND",
            Password = "123456789A"
        };
        _logicaUsuario.AgregarUsuario(us);
        Assert.IsNull(_logicaUsuario.ConseguirUsuarioPorId(322));
    }
    
    [TestMethod]
    public void ConseguirUsuarioPorId3()
    {
        Assert.IsNull(_logicaUsuario.ConseguirUsuarioPorId(9987));
    }
}