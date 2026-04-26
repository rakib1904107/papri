using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.Reports;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly FileUploadService _files;
    public EditModel(ApplicationDbContext db, FileUploadService files) { _db = db; _files = files; }

    [BindProperty] public Report Item { get; set; } = default!;
    [BindProperty] public IFormFile? Upload { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var entity = await _db.Reports.FindAsync(id);
        if (entity is null) return NotFound();
        Item = entity;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        ModelState.Remove("Item.FilePath");
        ModelState.Remove("Item.FileType");
        if (!ModelState.IsValid)
        {
            var current = await _db.Reports.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            if (current is not null)
            {
                Item.FilePath = current.FilePath;
                Item.FileType = current.FileType;
                Item.FileSizeBytes = current.FileSizeBytes;
            }
            return Page();
        }

        var entity = await _db.Reports.FindAsync(id);
        if (entity is null) return NotFound();

        entity.Title = Item.Title;

        if (Upload is { Length: > 0 })
        {
            try
            {
                var newPath = await _files.SaveAsync(Upload, "reports", AllowedFiles.Documents, AllowedFiles.DocumentMaxBytes);
                _files.Delete(entity.FilePath);
                entity.FilePath = newPath;
                entity.FileType = Path.GetExtension(Upload.FileName).TrimStart('.').ToLowerInvariant();
                entity.FileSizeBytes = Upload.Length;
            }
            catch (FileUploadException ex)
            {
                ModelState.AddModelError("Upload", ex.Message);
                return Page();
            }
        }

        await _db.SaveChangesAsync();
        TempData["StatusMessage"] = "Report updated.";
        return Redirect("/admin/reports");
    }
}
