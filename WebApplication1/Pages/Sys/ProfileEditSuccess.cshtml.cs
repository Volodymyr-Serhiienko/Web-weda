using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages.Sys
{
    public class ProfileEditSuccessModel : PageModel
    {
        public string Message { get; private set; } = "����� ������ ������ ������� ���� ������� ���������!";

        public void OnGet()
        {
        }
    }
}