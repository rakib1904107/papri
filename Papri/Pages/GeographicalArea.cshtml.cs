using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Papri.Pages;

[AllowAnonymous]
public class GeographicalAreaModel : PageModel
{
    public void OnGet() { }
}
