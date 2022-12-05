﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recommendation.Application.ConfigurationModels;
using Recommendation.Application.Interfaces;
using Recommendation.Domain;
using Recommendation.Persistence.Contexts;

namespace Recommendation.Persistence;

public static class DependencyInjection
{
    private const string AspNetEnvironment = "ASPNETCORE_ENVIRONMENT";
    private const string TypeEnvironment = "Production";

    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var connectionString = GetConnectionString(serviceProvider);

        services.AddDbContext<RecommendationDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        services.AddScoped<IRecommendationDbContext, RecommendationDbContext>();
        services.AddIdentityConfiguration();
        return services;
    }

    private static void AddIdentityConfiguration(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ ";
                options.Password.RequiredLength = 5;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<RecommendationDbContext>();
    }

    private static string GetConnectionString(IServiceProvider serviceProvider)
    {
        var configuration = serviceProvider.GetRequiredService<ConnectionStringsConfiguration>();
        var connectionString = Environment.GetEnvironmentVariable(AspNetEnvironment)
                               == TypeEnvironment
            ? configuration.RecommendationDbConnectionStringProduction
            : configuration.RecommendationDbConnectionStringDevelop;

        return connectionString;
    }
}