using FluentValidation;
using GradTest.Application.BoundedContexts.Orders.Commands;
using GradTest.Application.BoundedContexts.Products.Commands;
using GradTest.Presentation.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ClaimsConstants = GradTest.Presentation.Auth.Claims.ClaimsConstants;
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
        
        builder.AddSwagger();
        
        builder.AddApiServices();
        
        builder.AddCors();
    }

    private static void AddAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        RoleClaimType = ClaimsConstants.Role 
                    };
                    
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = KeycloakRolesProcessor.Process
                    };
                    
                    options.Authority = builder.Configuration["OIDC:Authority"];
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["OIDC:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["OIDC:Audience"],
                    };
                }
            );

        builder.Services
            .AddAuthorizationBuilder()
            .AddPolicy("Admin", policy =>
            {
                policy.RequireRole("admin");
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
    
    
    // private static WebApplicationBuilder SetupCors(this WebApplicationBuilder builder)
    // {
    //     builder.Services.AddCors(options =>
    //     {
    //         options.AddPolicy("Application",
    //             corsPolicyBuilder => corsPolicyBuilder.WithOrigins("http://localhost:5073/")
    //                 .AllowAnyMethod()
    //                 .AllowAnyHeader()
    //                 .AllowCredentials());
    //     });
    //     
    //     return builder;
    // }

    private static void SetupMediatR(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly));
        
        builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));
        
        // builder.Services.AddMediatR(cfg =>
        //     cfg.RegisterServicesFromAssembly(typeof(UpdateProductCommand).Assembly));
        //
        // builder.Services.AddMediatR(cfg =>
        //     cfg.RegisterServicesFromAssembly(typeof(DeleteProductCommand).Assembly));

        builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderCommandValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();

        //builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
 
    }
    
    private static WebApplicationBuilder SetupSwaggerDoc(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        
        const string displayName = "Grad Test API";
        
        var authorizationUrl = builder.Configuration["OIDC:AuthorizeUrl"];
        
        var tokenUrl = builder.Configuration["OIDC:TokenUrl"];
        
        if (string.IsNullOrWhiteSpace(authorizationUrl) || string.IsNullOrWhiteSpace(tokenUrl))
        {
            throw new InvalidOperationException("Missing OIDC configuration for Swagger.");
        }
        
        builder.Services.AddSwaggerGen(options =>
        {
            //options.SchemaFilter<SmartEnumSchemaFilter<Category>>(); 
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
        
        return builder;
    }
    
    // private static WebApplicationBuilder SetupHangfireServices(this WebApplicationBuilder builder)
    // {
    //     var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    //     builder.Services.AddHangfire(config => config.UsePostgreSqlStorage(connectionString));
    //     builder.Services.AddHangfireServer();
    //     return builder;
    // }

    // TODO: setup exchange rate thing
    private static WebApplicationBuilder SetupRucurringJobs(this WebApplicationBuilder builder)
    {
        //builder.Services.AddHttpClient<IExchangeRateService, ExchangeRateService>();
        //builder.Services.AddScoped<IExchangeRateSyncJob, ExchangeRateSyncJob>();
        
        return builder;
    }
    
    public static void ConfigureSwaggerUi(this WebApplication app, IHostApplicationBuilder builder)
    {
        if (!app.Environment.IsDevelopment()) return;
        
        app.UseSwagger();
        
        app.UseSwaggerUI(options =>
        {
            options.OAuthClientId(builder.Configuration["OIDC:ClientId"]);
            options.OAuthUsePkce(); 
            options.OAuthAppName("Grad Test API");
        });
    }

}