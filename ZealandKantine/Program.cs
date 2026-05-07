using ZealandKantine.Helpers;
using ZealandKantine.Models;
using ZealandKantine.Services;
using ZealandKantine.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<MenuItemRepository>();
builder.Services.AddDbContext<CafeZea>(o => o.UseSqlServer(ConnectionString.GetConnectionString()));


var app = builder.Build();

Console.WriteLine(ConnectionString.GetConnectionString());

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
