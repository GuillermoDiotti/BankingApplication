using System.Transactions;

namespace Dominio;

public class Categoria
{
    
    public int Id { get; set; }
    public string Nombre { get; set; }
    public DateTime FechaCreacion { get; set; }
    public string Estatus { get; set; }
    public string Tipo { get; set; }
    public Espacio Espacio { get; set; }
    
    public List<ObjetivosGastos> ObjetivosGastosList = new List<ObjetivosGastos>();
    
    public override string ToString()
    {
        return this.Nombre;
        
    }

    public override bool Equals(object obj)
    {
        if (obj is Categoria otraCategoria)
        {
            return Nombre == otraCategoria.Nombre;
        }
        return false;
    }
    
    public override int GetHashCode()
    {
        return Nombre.GetHashCode();
    }
    
}