using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.Sliders;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly FileUploadService _files;
    public EditModel(ApplicationDbContext db, FileUploadService files) { _db = db; _files = files; }

    [BindProperty] public Slider Item { get; set; } = default!;
    [BindProperty] public IFormFile? ImageFile { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var entity = await _db.Sliders.FindAsync(id);
        if (entity is null) return NotFound();
        Item = entity;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        ModelState.Remove("Item.ImagePath");
        if (!ModelState.IsValid)
        {
            var current = await _db.Sliders.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            if (current is not null) Item.ImagePath = current.ImagePath;
            return Page();
        }

        var entity = await _db.Sliders.FindAsync(id);
        if (entity is null) return NotFound();

        entity.Caption = Item.Caption;
        entity.DisplayOrder = Item.DisplayOrder;
        entity.IsActive = Item.IsActive;

        if (ImageFile is { Length: > 0 })
        {
            try
            {
                var newPath = await _files.SaveAsync(ImageFile, "sliders", AllowedFiles.Images, AllowedFiles.ImageMaxBytes);
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
        TempData["StatusMessage"] = "Slider updated.";
        return Redirect("/admin/sliders");
    }
}
