using Core.DomainServices;
using Core.DomainServices.Interfaces;
using Core.DomainServices.Services;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure culture to use dot as decimal separator
var cultureInfo = new CultureInfo("nl-NL");
cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IScoutRepo, ScoutRepo>();
builder.Services.AddScoped<IPlayerRepo, PlayerRepo>();
builder.Services.AddScoped<IScoutService, ScoutService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();

//DB connection
builder.Services.AddDbContext<ScoutSpaceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();