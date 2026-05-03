using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Papri.Data;
using Papri.Models;
using Papri.Services;

namespace Papri.Pages;

[AllowAnonymous]
public class NoticeModel : PageModel
{
    private const int PageSize = 10;

    private readonly ApplicationDbContext _db;
    public NoticeModel(ApplicationDbContext db) => _db = db;

    public PaginatedList<Notice> Items { get; set; } = PaginatedList<Notice>.Empty();

    public string? SearchTerm { get; set; }

    public async Task OnGetAsync([FromQuery] string? q, [FromQuery(Name = "page")] int? pageNumber)
    {
        SearchTerm = q;

        var query = _db.Notices.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
        {
            query = query.Where(n =>
                n.Title.Contains(q) ||
                (n.Body != null && n.Body.Contains(q)));
        }

        query = query.OrderByDescending(n => n.Date).ThenByDescending(n => n.Id);
        Items = await PaginatedList<Notice>.CreateAsync(query, pageNumber ?? 1, PageSize);
    }
}
