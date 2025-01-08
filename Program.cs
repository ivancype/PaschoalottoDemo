using Microsoft.EntityFrameworkCore;
using PaschoalottoDemo;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Estou usando a querystring do appsettings para acessar o banco de dados PostgreSQL
builder.Services.AddDbContext<DemoDbContext>(
    opcoes => opcoes.UseNpgsql(builder.Configuration.GetConnectionString("qsPostgres")));

// Configurando o HTTP Client para acessar a API do Random User Generator
builder.Services.AddHttpClient("RandomUserGenerator", httpClient =>
{
    httpClient.BaseAddress = new Uri(@"https://randomuser.me/api/?results=50");
});

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuarios}/{action=Index}/{id?}");

app.Run();
