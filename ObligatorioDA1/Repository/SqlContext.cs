using Dominio;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class SqlContext : DbContext
{
    
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Espacio> Espacios { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Cuenta> Cuentas{ get; set; }
    public DbSet<TipoDeCambio> TiposDeCambio { get; set; }
    public DbSet<Transaccion> Transacciones { get; set; }
    public DbSet<ObjetivosGastos> Objetivos { get; set; }
    
    public SqlContext(DbContextOptions<SqlContext> options) : base(options)
    {
        if (!Database.IsInMemory())
        {
            this.Database.Migrate();
        }
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Cuenta>().HasOne(e => e.Espacio).WithMany().IsRequired();
        modelBuilder.Entity<CuentaMonetaria>().HasBaseType<Cuenta>();
        modelBuilder.Entity<TarjetaCredito>().HasBaseType<Cuenta>();
        
        modelBuilder.Entity<Espacio>()
            .HasMany(us => us.MiembrosEspacio)
            .WithMany(s => s.espacios);
        modelBuilder.Entity<Espacio>()
            .HasOne(us => us.AdminEspacio)
            .WithMany(s => s.espaciosAdministrados);
        
        modelBuilder.Entity<ObjetivosGastos>()
            .HasMany(us => us.Categorias)
            .WithMany(s => s.ObjetivosGastosList);
       
    }
    
    
    
    
    
    
    
    
}
    