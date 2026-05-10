using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.AppreciativeStories;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly FileUploadService _files;
    public CreateModel(ApplicationDbContext db, FileUploadService files) { _db = db; _files = files; }

    [BindProperty] public AppreciativeStory Item { get; set; } = new();
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
                Item.ImagePath = await _files.SaveAsync(ImageFile, "appreciative-stories", AllowedFiles.Images, AllowedFiles.ImageMaxBytes);
            }
            catch (FileUploadException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return Page();
            }
        }

        _db.AppreciativeStories.Add(Item);
        await _db.SaveChangesAsync();
        TempData["StatusMessage"] = "Appreciative story created.";
        return Redirect("/admin/appreciative-stories");
    }
}
