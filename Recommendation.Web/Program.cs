using System.Reflection;
using Recommendation.Application;
using Recommendation.Application.Common.Mappings;
using Recommendation.Application.Hubs;
using Recommendation.Application.Interfaces;
using Recommendation.Persistence;
using Recommendation.Persistence.Initializers;
using Recommendation.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var policyOptions = new CookiePolicyOptions { Secure = CookieSecurePolicy.Always };

builder.Services.AddSignalR();

builder.Configuration.AddEnvironmentVariables()
    .AddUserSecrets(Assembly.GetExecutingAssembly(), true);
builder.Configuration
    .AddJsonFile("/etc/secrets/secrets.json", true);

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
using (var scope = app.Services.CreateScope())
{
    await RoleInitializer.InitializerAsync(scope.ServiceProvider);
    await UserDefaultInitializer.InitializerAsync(scope.ServiceProvider, configuration);
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseCustomExceptionHandler();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCookiePolicy(policyOptions);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.MapHub<CommentHub>("/comment-hub");

app.MapFallbackToFile("index.html");

app.Run();