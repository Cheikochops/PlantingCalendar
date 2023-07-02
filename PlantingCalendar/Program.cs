using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Razor;
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

builder.Services.AddControllers();

//Dependency Injection
builder.Services.AddSingleton<ICalendarDataAccess, CalendarDataAccess>();
builder.Services.AddSingleton<ISeedDataAccess, SeedDataAccess>();
builder.Services.AddSingleton<ITaskDataAccess, TaskDataAccess>();
builder.Services.AddSingleton<ICalendarHelper, CalendarHelper>();
builder.Services.AddSingleton<ISeedHelper, SeedHelper>();
builder.Services.Configure<DataAccessSettings>(builder.Configuration.GetSection(DataAccessSettings.SectionName));

builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationExpanders.Add(new ViewLocationExpander());
});

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

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllers();
});

app.Run();

