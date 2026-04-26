using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.Sliders;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly FileUploadService _files;
    public CreateModel(ApplicationDbContext db, FileUploadService files) { _db = db; _files = files; }

    [BindProperty] public Slider Item { get; set; } = new();
    [BindProperty] public IFormFile? ImageFile { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("Item.ImagePath");
        if (ImageFile is null || ImageFile.Length == 0)
            ModelState.AddModelError("ImageFile", "An image is required.");
        if (!ModelState.IsValid) return Page();

        try
        {
            Item.ImagePath = await _files.SaveAsync(ImageFile!, "sliders", AllowedFiles.Images, AllowedFiles.ImageMaxBytes);
        }
        catch (FileUploadException ex)
        {
            ModelState.AddModelError("ImageFile", ex.Message);
            return Page();
        }

        _db.Sliders.Add(Item);
        await _db.SaveChangesAsync();
        TempData["StatusMessage"] = "Slider created.";
        return Redirect("/admin/sliders");
    }
}
