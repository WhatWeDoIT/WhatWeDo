using WhatWeDo.Servicios.Configuracion;
using WhatWeDo.Servicios.Contratos;
using WhatWeDo.Servicios.Implementacion;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Auth/Login";
        option.ExpireTimeSpan = TimeSpan.FromDays(30);
        //option.AccessDeniedPath = "/Home/Index";
    });

//Agregamos la conexion a bbdd
builder.Services.Configure<ConfiguracionConexion>(builder.Configuration.GetSection("ConfiguracionConexion"));
//Agregamos servicios por tabla
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IEmpresaService, EmpresaService>();
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<IUbicacionService, UbicacionService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IDescuentoService, DescuentoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Eventos}");

app.Run();
