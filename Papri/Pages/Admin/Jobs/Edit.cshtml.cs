using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.Jobs;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly FileUploadService _files;
    public EditModel(ApplicationDbContext db, FileUploadService files) { _db = db; _files = files; }

    [BindProperty] public JobOpportunity Item { get; set; } = default!;
    [BindProperty] public IFormFile? CircularFile { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var entity = await _db.JobOpportunities.FindAsync(id);
        if (entity is null) return NotFound();
        Item = entity;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        ModelState.Remove("Item.CircularPath");
        if (Item.Deadline < Item.PostedDate)
            ModelState.AddModelError("Item.Deadline", "Deadline cannot be before posted date.");

        if (!ModelState.IsValid)
        {
            var current = await _db.JobOpportunities.AsNoTracking().FirstOrDefaultAsync(j => j.Id == id);
            if (current is not null) Item.CircularPath = current.CircularPath;
            return Page();
        }

        var entity = await _db.JobOpportunities.FindAsync(id);
        if (entity is null) return NotFound();

        entity.JobTitle = Item.JobTitle;
        entity.PostedDate = Item.PostedDate;
        entity.Deadline = Item.Deadline;

        if (CircularFile is { Length: > 0 })
        {
            try
            {
                var newPath = await _files.SaveAsync(CircularFile, "jobs", AllowedFiles.PdfAndDocs, AllowedFiles.DocumentMaxBytes);
                _files.Delete(entity.CircularPath);
                entity.CircularPath = newPath;
            }
            catch (FileUploadException ex)
            {
                ModelState.AddModelError("CircularFile", ex.Message);
                return Page();
            }
        }

        await _db.SaveChangesAsync();
        TempData["StatusMessage"] = "Job opportunity updated.";
        return Redirect("/admin/jobs");
    }
}
