using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.Jobs;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly FileUploadService _files;
    public CreateModel(ApplicationDbContext db, FileUploadService files) { _db = db; _files = files; }

    [BindProperty] public JobOpportunity Item { get; set; } = new()
    {
        PostedDate = DateTime.UtcNow,
        Deadline = DateTime.UtcNow.AddDays(14)
    };

    [BindProperty] public IFormFile? CircularFile { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("Item.CircularPath");
        if (CircularFile is null || CircularFile.Length == 0)
            ModelState.AddModelError("CircularFile", "Circular PDF is required.");
        if (Item.Deadline < Item.PostedDate)
            ModelState.AddModelError("Item.Deadline", "Deadline cannot be before posted date.");
        if (!ModelState.IsValid) return Page();

        try
        {
            Item.CircularPath = await _files.SaveAsync(CircularFile!, "jobs", AllowedFiles.PdfAndDocs, AllowedFiles.DocumentMaxBytes);
        }
        catch (FileUploadException ex)
        {
            ModelState.AddModelError("CircularFile", ex.Message);
            return Page();
        }

        Item.CreatedAt = DateTime.UtcNow;
        _db.JobOpportunities.Add(Item);
        await _db.SaveChangesAsync();
        TempData["StatusMessage"] = "Job opportunity created.";
        return Redirect("/admin/jobs");
    }
}
