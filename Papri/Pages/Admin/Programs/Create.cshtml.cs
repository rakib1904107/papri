using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.Programs;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly FileUploadService _files;
    public CreateModel(ApplicationDbContext db, FileUploadService files) { _db = db; _files = files; }

    [BindProperty] public ProgramItem Item { get; set; } = new();
    [BindProperty] public IFormFile? ImageFile { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Start date is required.")]
    [RegularExpression(@"^\d{4}-\d{2}$", ErrorMessage = "Pick a month and year.")]
    [Display(Name = "Start (Month / Year)")]
    public string? StartMonthInput { get; set; }

    [BindProperty]
    [RegularExpression(@"^\d{4}-\d{2}$", ErrorMessage = "Pick a month and year.")]
    [Display(Name = "End (Month / Year)")]
    public string? EndMonthInput { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        ModelState.Remove("Item.ImagePath");
        ModelState.Remove("Item.StartDate");
        ModelState.Remove("Item.EndDate");

        if (!TryParseMonth(StartMonthInput, out var start))
        {
            ModelState.AddModelError(nameof(StartMonthInput), "Start date is required.");
        }

        DateOnly? end = null;
        if (!string.IsNullOrWhiteSpace(EndMonthInput))
        {
            if (!TryParseMonth(EndMonthInput, out var ed))
            {
                ModelState.AddModelError(nameof(EndMonthInput), "Invalid end date.");
            }
            else { end = ed; }
        }

        if (Item.Type == ProgramType.Previous && !end.HasValue)
        {
            ModelState.AddModelError(nameof(EndMonthInput), "End date is required when type is Previous.");
        }

        if (end.HasValue && end.Value < start)
        {
            ModelState.AddModelError(nameof(EndMonthInput), "End date cannot be before start date.");
        }

        if (!ModelState.IsValid) return Page();

        Item.StartDate = start;
        Item.EndDate = end;

        if (ImageFile is { Length: > 0 })
        {
            try
            {
                Item.ImagePath = await _files.SaveAsync(ImageFile, "programs", AllowedFiles.Images, AllowedFiles.ImageMaxBytes);
            }
            catch (FileUploadException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return Page();
            }
        }

        _db.Programs.Add(Item);
        await _db.SaveChangesAsync();
        TempData["StatusMessage"] = "Program created.";
        return Redirect("/admin/programs");
    }

    private static bool TryParseMonth(string? input, out DateOnly date)
    {
        date = default;
        if (string.IsNullOrWhiteSpace(input)) return false;
        if (!DateTime.TryParseExact(input + "-01", "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
            return false;
        date = DateOnly.FromDateTime(dt);
        return true;
    }
}
