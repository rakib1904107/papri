using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.Projects;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly FileUploadService _files;
    public CreateModel(ApplicationDbContext db, FileUploadService files) { _db = db; _files = files; }

    [BindProperty] public Project Item { get; set; } = new() { Date = DateTime.UtcNow };
    [BindProperty] public IFormFile? ImageFile { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("Item.ImagePath");
        if (!ModelState.IsValid) return Page();

        if (ImageFile is { Length: > 0 })
        {
            try
            {
                Item.ImagePath = await _files.SaveAsync(ImageFile, "projects", AllowedFiles.Images, AllowedFiles.ImageMaxBytes);
            }
            catch (FileUploadException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return Page();
            }
        }

        _db.Projects.Add(Item);
        await _db.SaveChangesAsync();
        TempData["StatusMessage"] = "Project created.";
        return Redirect("/admin/projects");
    }
}
