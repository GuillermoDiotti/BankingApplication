using System.Runtime.InteropServices;

namespace Dominio;

public class Usuario
{
    public List<Espacio> espacios = new List<Espacio>();
    
    public List<Espacio> espaciosAdministrados = new List<Espacio>();

    public int Id { get; set; }
    
    private string _name { get; set; }
    public string Name 
    {
        get => _name;
        set
        {
            if (value is null || value.Length == 0)
            {
                throw new DomainException("El nombre no puede estar vacio");
            }
            _name = value;
        }
    }
    
    private string _lastName { get; set; }
    
    private string _mail { get; set; }

    public string Mail
    {
        get => _mail;
        set
        {
            if (value.Contains("@"))
            {
                if (value.EndsWith(".com"))
                {
                    _mail = value;
                }
                else
                {
                    throw new DomainException("El mail debe terminar en '.com'");
                }
            }
            else
            {
                throw new DomainException("El mail debe contener un @");
            }
        }
    }
    public string LastName  {
        get => _lastName;
        set
        {
            if (value is null || value.Length == 0)
            {
                throw new DomainException("El apellido no puede estar vacio");
            }
            _lastName = value;
        }
    }
    public string? Address { get; set; }
    private string _password { get; set; }

    public string Password
    {
        get => _password;
        set
        {
            if (value.Length >= 10)
            {
                if (value.Length <= 30)
                {
                    if (value.Any(char.IsUpper))
                    {
                        _password = value;
                    }
                    else
                    {
                        throw new DomainException("La contraseña debe contener al menos una mayuscula");
                    }
                }
                else
                {
                    throw new DomainException("La contraseña no puede tener mas de 30 caracteres");
                }
            }
            else
            {
                throw new DomainException("La contraseña debe tener al menos 10 caracteres");
            }
        }
    }
    public bool IsNull(string str)
    {
        return str.Length == 0;
    }
    
}