using Dominio;
using InterfazUsuario.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor;
using Logica;
using Microsoft.EntityFrameworkCore;
using Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddDbContext<SqlContext>( options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

builder.Services.AddScoped<IRepository<Usuario>, UsuarioDatabaseRepository>();
builder.Services.AddScoped<IRepository<Espacio>, EspacioDatabaseRepository>();
builder.Services.AddScoped<IRepository<Categoria>, CategoriaDatabaseRepository>(); 
builder.Services.AddScoped<IRepository<Cuenta>, CuentaDatabaseRepository>();

builder.Services.AddScoped<IRepository<Transaccion>, TransaccionDatabaseRepository>();
builder.Services.AddScoped<IRepository<ObjetivosGastos>, ObjetivosGastosDatabaseRepository>();
builder.Services.AddScoped<IRepository<TipoDeCambio>, TiposDeCambioDatabaseRepository>();


builder.Services.AddScoped<LogicaUsuario>();
builder.Services.AddScoped<LogicaCategoria>();
builder.Services.AddScoped<LogicaCuenta>();
builder.Services.AddScoped<LogicaTransaccion>();
builder.Services.AddScoped<LogicaObjetivos>();
builder.Services.AddScoped<LogicaTipoDeCambio>();
builder.Services.AddScoped<LogicaReporte>();
builder.Services.AddScoped<LogicaEspacio>();
    
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddSyncfusionBlazor();

builder.Services.AddSingleton<SessionLogic>();
builder.Services.AddSingleton<DateTimeProvider>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();