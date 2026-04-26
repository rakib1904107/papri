using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Papri.Data;

namespace Papri.Pages.Admin;

public class DashboardModel : PageModel
{
    private readonly ApplicationDbContext _db;
    public DashboardModel(ApplicationDbContext db) => _db = db;

    public int HeadlineCount { get; set; }
    public int NoticeCount { get; set; }
    public int ProjectCount { get; set; }
    public int NewsCount { get; set; }
    public int SliderCount { get; set; }
    public int VideoCount { get; set; }
    public int ReportCount { get; set; }
    public int JobCount { get; set; }

    public async Task OnGetAsync()
    {
        HeadlineCount = await _db.Headlines.CountAsync();
        NoticeCount = await _db.Notices.CountAsync();
        ProjectCount = await _db.Projects.CountAsync();
        NewsCount = await _db.News.CountAsync();
        SliderCount = await _db.Sliders.CountAsync();
        VideoCount = await _db.Videos.CountAsync();
        ReportCount = await _db.Reports.CountAsync();
        JobCount = await _db.JobOpportunities.CountAsync();
    }
}
