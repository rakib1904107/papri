using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Papri.Pages;

[AllowAnonymous]
public class ContactModel : PageModel
{
    public void OnGet() { }
}
