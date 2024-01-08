using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace IdentityService.Pages.Diagnostics;

[SecurityHeaders]
[Authorize]
public class Index : PageModel
{
    public ViewModel View { get; set; }
        
    public async Task<IActionResult> OnGet()
    {
        string localIp = HttpContext.Connection.LocalIpAddress.ToString();
        string remoteIp = HttpContext.Connection.RemoteIpAddress.ToString();
        string dockerLocalIp = localIp.Remove(localIp.Length - 1, 1) + "1";

        string[] localAddresses = { dockerLocalIp, "127.0.0.1", "::1", localIp };
        
        if (!localAddresses.Contains(remoteIp)) return NotFound();
        View = new ViewModel(await HttpContext.AuthenticateAsync());
            
        return Page();
    }
}