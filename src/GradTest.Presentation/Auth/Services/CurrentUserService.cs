using GradTest.Application.Common.Services;
using GradTest.Domain.BoundedContexts.Users.Roles;
using GradTest.Domain.BoundedContexts.Users.ValueObjects;
using GradTest.Presentation.Auth.Claims;

namespace GradTest.Presentation.Auth.Services;

public class CurrentUserService: ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public HashSet<Role> GetRolesFromToken()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            return [Role.Unknown];
        }

        var roles = _httpContextAccessor.HttpContext.User
            .FindAll(ClaimsConstants.Role)
            .Select(x =>
            {
                var couldParse = Role.TryFromName(x.Value, out var role);

                return couldParse
                    ? role
                    : Role.Unknown;
            })
            .ToHashSet();
        
        return roles;
    }
    
    public UserId GetUserIdFromToken()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            return UserId.Empty;
        }
        
        var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimsConstants.UserId)?.Value;

        return UserId.TryParse(userId, null, out var parsedId) 
            ? parsedId 
            : UserId.Empty;
    }
}