using System.Diagnostics.CodeAnalysis;

namespace GradTest.Domain.BoundedContexts.Users.ValueObjects;

public readonly record struct UserId: IParsable<UserId>
{
    public required Guid Value { get; init; }

    public static UserId New()
    {
        return new UserId
        {
            Value = Guid.NewGuid()
        };
    }

    public static UserId FromGuid(Guid userId)
    {
        return new UserId
        {
            Value = userId
        };
    }

    public static readonly UserId Empty = new UserId { Value = Guid.Empty };

    public static UserId Parse(string s, IFormatProvider? provider)
    {
        return FromGuid(Guid.Parse(s));
    }
    
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out UserId result)
    {
        if (Guid.TryParse(s, out var parsed))
        {
            result = FromGuid(parsed);
            return true;
        }

        result = Empty;
        return false;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}