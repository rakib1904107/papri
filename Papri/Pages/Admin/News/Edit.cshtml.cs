using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Services;

namespace Papri.Pages.Admin.News;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly FileUploadService _files;
    public EditModel(ApplicationDbContext db, FileUploadService files) { _db = db; _files = files; }

    [BindProperty] public Papri.Models.News Item { get; set; } = default!;
    [BindProperty] public IFormFile? ImageFile { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var entity = await _db.News.FindAsync(id);
        if (entity is null) return NotFound();
        Item = entity;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        ModelState.Remove("Item.ImagePath");
        if (!ModelState.IsValid)
        {
            var current = await _db.News.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
            if (current is not null) Item.ImagePath = current.ImagePath;
            return Page();
        }

        var entity = await _db.News.FindAsync(id);
        if (entity is null) return NotFound();

        entity.Title = Item.Title;
        entity.Details = Item.Details;
        entity.Date = Item.Date;

        if (ImageFile is { Length: > 0 })
        {
            try
            {
                var newPath = await _files.SaveAsync(ImageFile, "news", AllowedFiles.Images, AllowedFiles.ImageMaxBytes);
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
        TempData["StatusMessage"] = "News item updated.";
        return Redirect("/admin/news");
    }
}
