using Astro;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages.Sys
{
    public class TestModel : PageModel
    {
        public DateTime DatetimeNow { get; private set; }
        public DateTime Birthday { get; private set; }
        public int Utc { get; private set; } = 0;
        public string Firstname { get; private set; } = "";
        public string Lastname { get; private set; } = "";
        public Person person { get; private set; } = new();

        public void OnGet()
        {
            DatetimeNow = DateTime.UtcNow.AddHours(-Convert.ToInt32(HttpContext.Session.GetString("timeZoneOffset")));
            Utc = Convert.ToInt32(HttpContext.Session.GetString("utc")!);
            Birthday = new DateTime(Convert.ToInt64(HttpContext.Session.GetString("birthday")), DateTimeKind.Utc).AddHours(Utc);
            Firstname = HttpContext.Session.GetString("firstname")!;
            Lastname = HttpContext.Session.GetString("lastname")!;
            person = new Person(Birthday, Firstname, Lastname);
        }


    }
}
