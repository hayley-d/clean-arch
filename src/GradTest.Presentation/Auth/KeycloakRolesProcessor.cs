namespace GradTest.Presentation.Auth;

using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

public static class ClaimsConstants
{
    public const string Role = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
}

public static class KeycloakRolesProcessor
{
    private class KeycloakRolesDto
    {
        private const string KeycloakClientName = "gradtest-api";
        [JsonPropertyName(KeycloakClientName)]
        public TokenRoles ApiRoles { get; init; }
    }
    
    private class ResourceAccessToken
    {
        [JsonPropertyName("roles")]
        public string[] Roles { get; set; } = [];
    }

    private class TokenRoles
    {
        [JsonPropertyName("roles")]
        public string[] Roles { get; set; } = [];
    }
    
    public static readonly Func<TokenValidatedContext, Task> Process = context =>
    {
        var tokenClaims = context.Principal!.Claims;
        IEnumerable<Claim> claims = [];

        var resourceAccess = tokenClaims
            .FirstOrDefault(c => c.Type == "resource_access")?
            .Value;

        if (!string.IsNullOrEmpty(resourceAccess))
        {
            var dict = JsonSerializer.Deserialize<Dictionary<string, ResourceAccessToken>>(resourceAccess);
            if (dict != null && dict.TryGetValue("gradtest-api", out var clientRoles))
            {
                claims = clientRoles.Roles.Select(role => new Claim(ClaimsConstants.Role, role));
            }
        }

        var identity = context.Principal!.Identity as ClaimsIdentity;
        identity!.AddClaims(claims);

        return Task.CompletedTask;
    };
}