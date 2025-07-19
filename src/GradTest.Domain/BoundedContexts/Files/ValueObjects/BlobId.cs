using System.Diagnostics.CodeAnalysis;

namespace GradTest.Domain.BoundedContexts.Files.ValueObjects;

public readonly record struct BlobId: IParsable<BlobId>
{
    public required Guid Value { get; init; }
    
    public static BlobId New()
    {
        return new BlobId
        {
            Value = Guid.NewGuid()
        };
    }

    public static BlobId FromGuid(Guid userId)
    {
        return new BlobId
        {
            Value = userId
        };
    }
    
    public static readonly BlobId Empty = new BlobId { Value = Guid.Empty };
    
    public static BlobId Parse(string s, IFormatProvider? provider)
    {
        return FromGuid(Guid.Parse(s));
    }
    
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out BlobId result)
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