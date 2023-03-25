using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlantingCalendar.Controllers;
using PlantingCalendar.DataAccess;
using PlantingCalendar.Interfaces;
using PlantingCalendar.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddPageRoute("/Home", ""); 
});

//Dependency Injection
builder.Services.AddScoped<ICalendarDataAccess, CalendarDataAccess>();
builder.Services.AddScoped<ISeedDataAccess, SeedDataAccess>();
builder.Services.Configure<DataAccessSettings>(builder.Configuration.GetSection(DataAccessSettings.SectionName));

builder.Services.AddControllers();

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

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
