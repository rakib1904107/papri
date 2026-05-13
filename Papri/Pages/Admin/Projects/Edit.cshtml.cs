using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.Projects;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly FileUploadService _files;
    public EditModel(ApplicationDbContext db, FileUploadService files) { _db = db; _files = files; }

    [BindProperty] public Project Item { get; set; } = default!;
    [BindProperty] public IFormFile? ImageFile { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var entity = await _db.Projects.FindAsync(id);
        if (entity is null) return NotFound();
        Item = entity;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        ModelState.Remove("Item.ImagePath");
        if (!ModelState.IsValid)
        {
            var current = await _db.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (current is not null) Item.ImagePath = current.ImagePath;
            return Page();
        }

        var entity = await _db.Projects.FindAsync(id);
        if (entity is null) return NotFound();

        entity.Title = Item.Title;
        entity.Details = Item.Details;
        entity.Date = Item.Date;
        entity.IsActive = Item.IsActive;

        if (ImageFile is { Length: > 0 })
        {
            try
            {
                var newPath = await _files.SaveAsync(ImageFile, "projects", AllowedFiles.Images, AllowedFiles.ImageMaxBytes);
                _files.Delete(entity.ImagePath);
                entity.ImagePath = newPath;
            }
            catch (FileUploadException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return Page();
            }
        }

        await _db.SaveChangesAsync();
        TempData["StatusMessage"] = "Project updated.";
        return Redirect("/admin/projects");
    }
}
