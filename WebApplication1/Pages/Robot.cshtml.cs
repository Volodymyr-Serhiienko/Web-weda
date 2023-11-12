using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages
{
    public class RobotModel : PageModel
    {
        public string Message { get; private set; } = "";

        public void OnGet()
        {
            Message = "К сожалению, Вы не прошли проверку на робота! Если Вы не робот - напишите нам об этом по электронной почте (адрес в Контактах сайта).";
        }
    }
}
