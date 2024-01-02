using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Pages.Account.Register;

public class RegisterViewModel
{
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    public string UserName { get; set; }

    [Required]
    public string FullName { get; set; }
    public string ReturnUrl { get; set; }
    public string Button { get; set; }
}
