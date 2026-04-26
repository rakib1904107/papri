namespace Papri.Services;

public static class AllowedFiles
{
    public static readonly string[] Images = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
    public static readonly string[] Pdfs = { ".pdf" };
    public static readonly string[] PdfAndDocs = { ".pdf", ".doc", ".docx" };
    public static readonly string[] Documents =
    {
        ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".txt", ".csv"
    };

    public const long ImageMaxBytes = 5L * 1024 * 1024;       // 5 MB
    public const long DocumentMaxBytes = 25L * 1024 * 1024;   // 25 MB
}
