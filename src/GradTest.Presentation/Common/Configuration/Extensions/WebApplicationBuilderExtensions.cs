using System.Globalization;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using GradTest.Application.Common.Services;
using GradTest.Domain.BoundedContexts.Users.Policies;
using GradTest.Domain.BoundedContexts.Users.Roles;
using GradTest.Presentation.Auth.Claims;
using GradTest.Presentation.Auth.Services;
using GradTest.Presentation.Middleware;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Serilog.Sinks.OpenTelemetry;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace GradTest.Presentation.Common.Configuration.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static ILogger CreateStartupLogger(this WebApplicationBuilder builder)
    {
        using var sp = builder.Services.BuildServiceProvider();
        var logger = sp.GetRequiredService<ILogger<Program>>();
        return logger;
    }

    public static void AddPresentationDependencies(this WebApplicationBuilder builder, ILogger logger)
    {
        builder.AddAuthentication();
        
        builder.AddAuthorization();
        
        builder.AddSwagger();
        
        builder.AddApiServices();
        
        builder.AddCors();
    }

    private static void AddAuthentication(this WebApplicationBuilder builder)
    {
        var audience = builder.Configuration["OIDC:Audience"]!;
        var authority = builder.Configuration["OIDC:Authority"]!;
        var metadataAddress = builder.Configuration["OIDC:MetadataAddress"]!;
        
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Authority = authority;
            options.Audience = audience;
            options.MetadataAddress = metadataAddress;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                RoleClaimType = ClaimsConstants.Role,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidAudience = audience,
                ValidIssuer = authority,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                TryAllIssuerSigningKeys = true
            };
        });
    }
    
    private static void AddAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthorizationBuilder()
            .AddPolicy(Policies.Admin, policy =>
            {
                policy.RequireRole(nameof(Role.Admin));
                policy.RequireAuthenticatedUser();
            });
    }

    private static void AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        const string displayName = $".NET GradTest API";
        
        var authorizationUrl = builder.Configuration["OIDC:AuthorizeUrl"]!;
        var tokenUrl = builder.Configuration["OIDC:TokenUrl"]!;
        
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = displayName,
                Version = "v1"
            });
    
            options.AddSecurityDefinition("OIDC", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(authorizationUrl),
                        TokenUrl = new Uri(tokenUrl),
                        Scopes = new Dictionary<string, string>
                        {
                            { "openid", "OpenID Connect scope" },
                            { "profile", "Profile scope" },
                            { "email", "Email scope" }
                        }
                    }
                }
            });
    
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "OIDC"
                        }
                    },
                    new[] { "openid", "profile", "email" }
                }
            });
        });
    }

    private static void AddApiServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
    }
    
    private static void AddCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(x => x.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        }));
    }
}