using AspNetCore.Swagger.Themes;
using Microsoft.EntityFrameworkCore;
using GradTest.Presentation.Endpoints;
using GradTest.Infrastructure.Persistence;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace GradTest.Presentation.Common.Configuration.Extensions;

public static class WebApplicationExtensions
{
    public static async Task ConfigureAsync(this WebApplication app, string[] args, ILogger logger)
    {
        app.AddSwaggerDoc();

        app.UseHttpsRedirection();
        app.UseHsts();
        app.UseCors();
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"error\": \"An unexpected error occurred.\"}");
            });
        });

        app.MapApiEndpoints();

        if (app.Environment.IsDevelopment())
        {
        }

        await app.MigrateAsync(args);
    }

    private static void AddSwaggerDoc(this WebApplication app)
    {
        var swaggerIsEnabled = app.Configuration.GetRequiredSection("SwaggerEnabled").Get<bool>();
        
        if (app.Environment.IsDevelopment() || swaggerIsEnabled)
        {
            var authClientId = app.Configuration["OIDC:ClientId"]!;
    
            app.UseSwagger();
            app.UseSwaggerUI(style: Style.Dark, c =>
            {
                c.OAuthClientId(authClientId);
                c.OAuthAppName("Grad Test .NET");
                c.OAuthUsePkce();
            });
        }
    }
    
    private static async Task MigrateAsync(this WebApplication app, string[] args)
    {
        if (!app.Environment.IsDevelopment() && !args.Contains("Migrate"))
        {
            return;
        }

        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var applicationDbContext = services.GetRequiredService<ApplicationDbContext>();
        if (applicationDbContext.Database.IsNpgsql())
        {
            await applicationDbContext.Database.MigrateAsync();
        }
    }
}