using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace first_project.Pages;


    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        public void OnGet() { }
    }
