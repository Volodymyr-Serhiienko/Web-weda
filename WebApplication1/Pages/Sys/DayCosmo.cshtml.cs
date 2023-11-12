using Astro;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages.Sys
{
    public class DayCosmoModel : PageModel
    {
        public DateTime DatetimeNow { get; private set; }
        public AstroObject? today { get; private set; }

        public void OnGet()
        {
            DatetimeNow = DateTime.UtcNow.AddHours(-Convert.ToInt32(HttpContext.Session.GetString("timeZoneOffset")));
            today = new(DatetimeNow);
        }

        public void OnPost(int utc, DateTime datetime)
        {
            DatetimeNow = new DateTime(datetime.AddHours(-utc).Ticks, DateTimeKind.Utc).AddHours(utc);
            today = new(DatetimeNow);
        }
    }
}