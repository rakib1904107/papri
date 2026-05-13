using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.Programs;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly FileUploadService _files;
    public EditModel(ApplicationDbContext db, FileUploadService files) { _db = db; _files = files; }

    [BindProperty] public ProgramItem Item { get; set; } = default!;
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

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var entity = await _db.Programs.FindAsync(id);
        if (entity is null) return NotFound();
        Item = entity;
        StartMonthInput = entity.StartDate == default ? null : entity.StartDate.ToString("yyyy-MM");
        EndMonthInput = entity.EndDate?.ToString("yyyy-MM");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
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

        if (!ModelState.IsValid)
        {
            var current = await _db.Programs.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (current is not null) Item.ImagePath = current.ImagePath;
            return Page();
        }

        var entity = await _db.Programs.FindAsync(id);
        if (entity is null) return NotFound();

        entity.Title = Item.Title;
        entity.DonorsPartner = Item.DonorsPartner;
        entity.TargetPeople = Item.TargetPeople;
        entity.WorkingArea = Item.WorkingArea;
        entity.StartDate = start;
        entity.EndDate = end;
        entity.Goal = Item.Goal;
        entity.Objective = Item.Objective;
        entity.MajorActivities = Item.MajorActivities;
        entity.Type = Item.Type;
        entity.DisplayOrder = Item.DisplayOrder;
        entity.IsActive = Item.IsActive;

        if (ImageFile is { Length: > 0 })
        {
            try
            {
                var newPath = await _files.SaveAsync(ImageFile, "programs", AllowedFiles.Images, AllowedFiles.ImageMaxBytes);
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
        TempData["StatusMessage"] = "Program updated.";
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
