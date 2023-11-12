using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages
{
    public class RobotModel : PageModel
    {
        public string Message { get; private set; } = "";

        public void OnGet()
        {
            Message = "� ���������, �� �� ������ �������� �� ������! ���� �� �� ����� - �������� ��� �� ���� �� ����������� ����� (����� � ��������� �����).";
        }
    }
}
