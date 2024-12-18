using Microsoft.EntityFrameworkCore;
using Projeto_Transportadora_MVC.Context;
using Projeto_Transportadora_MVC.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TransportadoraContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddScoped<NotaFiscalService>();
builder.Services.AddScoped<ImportarExcelService>();
builder.Services.AddScoped<AcaoNotaFiscalService>();


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
    pattern: "{controller=Home}/{action=Menu}/{id?}");

app.Run();
