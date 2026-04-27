using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;

namespace Papri.Pages.Media;

[AllowAnonymous]
public class PhotoGalleryModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public PhotoGalleryModel(ApplicationDbContext db) => _db = db;

    public List<Slider> Photos { get; set; } = new();

    public async Task OnGetAsync()
    {
        Photos = await _db.Sliders
            .OrderBy(s => s.DisplayOrder)
            .ThenByDescending(s => s.CreatedAt)
            .ThenBy(s => s.Id)
            .ToListAsync();
    }
}
