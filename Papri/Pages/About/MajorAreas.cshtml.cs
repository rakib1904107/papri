using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Papri.Pages.About;

[AllowAnonymous]
public class MajorAreasModel : PageModel
{
    public void OnGet() { }
}
