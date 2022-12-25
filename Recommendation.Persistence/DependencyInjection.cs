using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendation.Application.Common.Constants;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;
using Recommendation.Persistence.Contexts;

namespace Recommendation.Persistence;

public static class DependencyInjection
{
    private const string AllowedCharacters = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ" +
                                             "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ ";

    public static IServiceCollection AddPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        var serviceProvider = services.BuildServiceProvider();
        var connectionString = GetConnectionString(configuration);

        services.AddDbContext<RecommendationDbContext>(options =>
            options.UseNpgsql(connectionString, o =>
            {
                o.MigrationsAssembly(typeof(RecommendationDbContext).Assembly.FullName);
                o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            }));

        services.AddScoped<IRecommendationDbContext, RecommendationDbContext>();
        services.AddScoped<RecommendationDbContext>();
        services.AddIdentityConfiguration();
        services.AddConfigureApplicationCookie();
        return services;
    }

    private static void AddIdentityConfiguration(this IServiceCollection services)
    {
        services.AddIdentity<UserApp, IdentityRole<Guid>>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = AllowedCharacters;
                options.Password.RequiredLength = 5;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<RecommendationDbContext>();
    }

    private static void AddConfigureApplicationCookie(this IServiceCollection services)
    {
        Task<int> OverrideStatusCode(BaseContext<CookieAuthenticationOptions> context,
            int statusCode)
        {
            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            return Task.FromResult(0);
        }

        services.ConfigureApplicationCookie(options =>
        {
            options.Events = new CookieAuthenticationEvents()
            {
                OnRedirectToLogin = context => OverrideStatusCode(context, 401),
                OnRedirectToAccessDenied = context => OverrideStatusCode(context, 403)
            };
        });
    }

    private static string GetConnectionString(IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable(EnvironmentConfiguration.AspNetEnvironment)
                               == EnvironmentConfiguration.ProductionType
            ? configuration["ConnectionStrings:RecommendationDbConnectionStringProduction"]
            : configuration["ConnectionStrings:RecommendationDbConnectionStringDevelop"];

        return connectionString
               ?? throw new NullReferenceException("Missing database connection string");
    }
}