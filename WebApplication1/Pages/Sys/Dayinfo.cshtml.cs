using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Astro;

namespace WebApplication1.Pages.Sys
{
    public class DayinfoModel : PageModel
    {
        public string Email { get; private set; } = "";
        public string Firstname { get; private set; } = "";
        public string Lastname { get; private set; } = "";
        public DateTime Birthday { get; private set; }
        public int Utc { get; private set; } = 0;
        public string Latitude { get; private set; } = "Unknown...";
        public string Longitude { get; private set; } = "Unknown...";
        public Person Person { get; private set; } = new();

        public IActionResult OnGet()
        {
            // Получение данных пользователя
            Email = HttpContext.Session.GetString("email")!;
            Firstname = HttpContext.Session.GetString("firstname")!;
            Lastname = HttpContext.Session.GetString("lastname")!;
            Utc = Convert.ToInt32(HttpContext.Session.GetString("utc")!);
			Birthday = new DateTime(Convert.ToInt64(HttpContext.Session.GetString("birthday")), DateTimeKind.Utc).AddHours(Utc);
			Person = new Person(Birthday, Firstname, Lastname);
 
            return Page();
        }
    }
}
