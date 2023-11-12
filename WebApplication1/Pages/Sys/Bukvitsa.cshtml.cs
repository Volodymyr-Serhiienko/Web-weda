using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages.Sys
{
    public class BukvitsaModel : PageModel
    {
        public string[] Letters { get; }

        public BukvitsaModel(BukvitsaService service) => Letters = service.LetterFullDefs;

        public void OnGet()
        {
        }
    }
}
