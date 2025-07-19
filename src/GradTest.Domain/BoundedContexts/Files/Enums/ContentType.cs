using Ardalis.SmartEnum;

namespace GradTest.Domain.BoundedContexts.Files.Enums;

public abstract class ContentType: SmartEnum<ContentType>
{
    public static readonly ContentType Pdf = new PdfContent();
    public static readonly ContentType Csv = new CsvContent();
    
    protected ContentType(string name, int value) : base(name, value) { }
    public abstract string Extension();
    
    private sealed class PdfContent : ContentType
    {
        public PdfContent() : base("application/pdf", 0) { }
        public override string Extension() => ".pdf";
    }
    
    private sealed class CsvContent : ContentType
    {
        public CsvContent() : base("text/csv", 1) { }
        public override string Extension() => ".csv";
    }
}