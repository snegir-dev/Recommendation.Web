using System.Reflection;
using Recommendation.Application;
using Recommendation.Application.Common.Mappings;
using Recommendation.Application.Interfaces;
using Recommendation.Persistence;
using Recommendation.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson();
builder.Services.AddApplication(configuration);
builder.Services.AddPersistence();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    config.AddProfile(new AssemblyMappingProfile(typeof(IRecommendationDbContext).Assembly));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseCustomExceptionHandler();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();