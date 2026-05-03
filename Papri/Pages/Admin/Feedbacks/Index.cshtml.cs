using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages.Admin.Feedbacks;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public IndexModel(ApplicationDbContext db) => _db = db;

    public PaginatedList<Feedback> Items { get; set; } = PaginatedList<Feedback>.Empty();
    public int TotalCount { get; set; }
    public int ReadCount { get; set; }
    public int UnreadCount { get; set; }
    public string Filter { get; set; } = "all";
    public string? SearchTerm { get; set; }

    [TempData] public string? StatusMessage { get; set; }

    public async Task OnGetAsync(
        [FromQuery] string? q,
        [FromQuery(Name = "filter")] string? filter,
        [FromQuery(Name = "page")] int? pageNumber)
    {
        SearchTerm = q;
        Filter = (filter ?? "all").ToLowerInvariant() switch
        {
            "read" => "read",
            "unread" => "unread",
            _ => "all"
        };

        TotalCount = await _db.Feedbacks.CountAsync();
        ReadCount = await _db.Feedbacks.CountAsync(f => f.IsRead);
        UnreadCount = TotalCount - ReadCount;

        var query = _db.Feedbacks.AsQueryable();

        if (Filter == "read") query = query.Where(f => f.IsRead);
        else if (Filter == "unread") query = query.Where(f => !f.IsRead);

        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(f =>
                f.Name.Contains(q) ||
                f.Email.Contains(q) ||
                f.Message.Contains(q));

        query = query.OrderByDescending(f => f.SubmittedAt).ThenByDescending(f => f.Id);
        Items = await PaginatedList<Feedback>.CreateAsync(query, pageNumber ?? 1, 10);
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var item = await _db.Feedbacks.FindAsync(id);
        if (item is not null)
        {
            _db.Feedbacks.Remove(item);
            await _db.SaveChangesAsync();
            StatusMessage = "Feedback deleted.";
        }
        return Redirect("/admin/feedbacks");
    }

    public async Task<IActionResult> OnPostMarkReadAsync(int id)
    {
        var item = await _db.Feedbacks.FindAsync(id);
        if (item is not null && !item.IsRead)
        {
            item.IsRead = true;
            await _db.SaveChangesAsync();
        }
        return new JsonResult(new { ok = true });
    }

    public async Task<IActionResult> OnPostToggleReadAsync(int id)
    {
        var item = await _db.Feedbacks.FindAsync(id);
        if (item is not null)
        {
            item.IsRead = !item.IsRead;
            await _db.SaveChangesAsync();
            StatusMessage = item.IsRead ? "Marked as read." : "Marked as unread.";
        }
        return Redirect("/admin/feedbacks");
    }
}
