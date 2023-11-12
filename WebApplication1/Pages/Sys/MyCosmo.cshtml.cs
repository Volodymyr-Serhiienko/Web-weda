using Microsoft.AspNetCore.Mvc.RazorPages;
using Astro;

namespace WebApplication1.Pages.Sys
{
    public class MyCosmoModel : PageModel
    {
        public DateTime Birthday { get; private set; }
        public int Utc { get; private set; } = 0;
        public string Firstname { get; private set; } = "";
        public string Lastname { get; private set; } = "";
        public Person person { get; private set; } = new();

        public void OnGet()
        {
            Utc = Convert.ToInt32(HttpContext.Session.GetString("utc")!);
            Birthday = new DateTime(Convert.ToInt64(HttpContext.Session.GetString("birthday")), DateTimeKind.Utc).AddHours(Utc);
            Firstname = HttpContext.Session.GetString("firstname")!;
            Lastname = HttpContext.Session.GetString("lastname")!;
            person = new Person(Birthday, Firstname, Lastname);
        }
    }
}
