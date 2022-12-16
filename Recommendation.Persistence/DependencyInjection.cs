using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendation.Application.Common.Constants;
using Recommendation.Application.ConfigurationModels;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;
using Recommendation.Persistence.Contexts;

namespace Recommendation.Persistence;

public static class DependencyInjection
{
    private const string AllowedCharacters = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ" +
                                             "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ ";

    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var connectionString = GetConnectionString(serviceProvider);

        services.AddDbContext<RecommendationDbContext>(options =>
            options.UseNpgsql(connectionString, o =>
            {
                o.MigrationsAssembly(typeof(RecommendationDbContext).Assembly.FullName);
                o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            }));

        services.AddScoped<IRecommendationDbContext, RecommendationDbContext>();
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
            .AddEntityFrameworkStores<RecommendationDbContext>();
    }

    private static void AddConfigureApplicationCookie(this IServiceCollection services)
    {
        services.ConfigureApplicationCookie(options =>
        {
            options.Events = new CookieAuthenticationEvents()
            {
                OnRedirectToLogin = context =>
                {
                    context.Response.Clear();
                    context.Response.StatusCode = 401;
                    return Task.FromResult(0);
                }
            };
        });
    }

    private static string GetConnectionString(IServiceProvider serviceProvider)
    {
        var configuration = serviceProvider.GetRequiredService<ConnectionStringsConfiguration>();
        var connectionString = Environment.GetEnvironmentVariable(EnvironmentConfiguration.AspNetEnvironment)
                               == EnvironmentConfiguration.ProductionType
            ? configuration.RecommendationDbConnectionStringProduction
            : configuration.RecommendationDbConnectionStringDevelop;

        return connectionString;
    }
}