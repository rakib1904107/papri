using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.Reports;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly FileUploadService _files;
    public CreateModel(ApplicationDbContext db, FileUploadService files) { _db = db; _files = files; }

    [BindProperty] public Report Item { get; set; } = new();
    [BindProperty] public IFormFile? Upload { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("Item.FilePath");
        ModelState.Remove("Item.FileType");
        if (Upload is null || Upload.Length == 0)
            ModelState.AddModelError("Upload", "A file is required.");
        if (!ModelState.IsValid) return Page();

        try
        {
            Item.FilePath = await _files.SaveAsync(Upload!, "reports", AllowedFiles.Documents, AllowedFiles.DocumentMaxBytes);
            Item.FileType = Path.GetExtension(Upload!.FileName).TrimStart('.').ToLowerInvariant();
            Item.FileSizeBytes = Upload.Length;
        }
        catch (FileUploadException ex)
        {
            ModelState.AddModelError("Upload", ex.Message);
            return Page();
        }

        _db.Reports.Add(Item);
        await _db.SaveChangesAsync();
        TempData["StatusMessage"] = "Report uploaded.";
        return Redirect("/admin/reports");
    }
}
