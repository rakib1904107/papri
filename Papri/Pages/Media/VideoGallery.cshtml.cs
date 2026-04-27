using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;

namespace Papri.Pages.Media;

[AllowAnonymous]
public class VideoGalleryModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public VideoGalleryModel(ApplicationDbContext db) => _db = db;

    public List<Video> Videos { get; set; } = new();

    public async Task OnGetAsync()
    {
        Videos = await _db.Videos
            .OrderByDescending(v => v.AddedAt)
            .ThenByDescending(v => v.Id)
            .ToListAsync();
    }
}
