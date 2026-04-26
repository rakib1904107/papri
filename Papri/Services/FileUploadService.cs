namespace Papri.Services;

public class FileUploadException : Exception
{
    public FileUploadException(string message) : base(message) { }
}

public class FileUploadService
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<FileUploadService> _logger;

    public FileUploadService(IWebHostEnvironment env, ILogger<FileUploadService> logger)
    {
        _env = env;
        _logger = logger;
    }

    public async Task<string> SaveAsync(
        IFormFile file,
        string subfolder,
        string[] allowedExtensions,
        long maxBytes,
        CancellationToken ct = default)
    {
        if (file is null || file.Length == 0)
            throw new FileUploadException("No file uploaded.");

        if (file.Length > maxBytes)
            throw new FileUploadException($"File exceeds the maximum size of {maxBytes / 1024 / 1024} MB.");

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(ext) || !allowedExtensions.Contains(ext))
            throw new FileUploadException($"File type '{ext}' is not allowed.");

        var safeSubfolder = SanitizeSubfolder(subfolder);
        var folderAbs = Path.Combine(_env.WebRootPath, "uploads", safeSubfolder);
        Directory.CreateDirectory(folderAbs);

        var fileName = $"{Guid.NewGuid():N}{ext}";
        var fullPath = Path.Combine(folderAbs, fileName);

        await using (var stream = File.Create(fullPath))
        {
            await file.CopyToAsync(stream, ct);
        }

        var relativePath = $"/uploads/{safeSubfolder}/{fileName}";
        _logger.LogInformation("Uploaded file {Path} ({Bytes} bytes)", relativePath, file.Length);
        return relativePath;
    }

    public void Delete(string? relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath)) return;
        if (!relativePath.StartsWith("/uploads/", StringComparison.Ordinal)) return;

        var fullPath = Path.Combine(
            _env.WebRootPath,
            relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

        try
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                _logger.LogInformation("Deleted file {Path}", relativePath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to delete file {Path}", relativePath);
        }
    }

    private static string SanitizeSubfolder(string subfolder)
    {
        if (string.IsNullOrWhiteSpace(subfolder))
            throw new FileUploadException("Subfolder is required.");

        var clean = subfolder.Trim().Trim('/', '\\');
        if (clean.Contains("..", StringComparison.Ordinal) ||
            clean.Contains(':', StringComparison.Ordinal) ||
            clean.Any(Path.GetInvalidFileNameChars().Contains))
        {
            throw new FileUploadException("Invalid subfolder name.");
        }
        return clean;
    }
}
