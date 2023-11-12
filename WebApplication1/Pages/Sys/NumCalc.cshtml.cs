using Microsoft.AspNetCore.Mvc.RazorPages;
using Astro;
using Calculations;

namespace WebApplication1.Pages.Sys
{
    public class NumCalcModel : PageModel
    {
        public void OnGet()
        {
        }

        public string Title { get; private set; } = "";
        public string SlavNums { get; private set; } = "";
        public List<string> NumDefs { get; private set; } = new();
        public bool IsCalc { get; private set; } = false;

        public void OnPost(int number)
        {
            Title = $"Скрытые Образы Числа - {number} :";
            string temp = "";
            List<int> numbers = new();
            List<string> defs = new();
            do
            {
                numbers.Clear();
                var letters = Letter.GetSlavNumDef(number);
                temp += letters.Item1;
                defs.AddRange(letters.Item2);
                temp += " ≡ ";
                numbers = StatFunc.SplitNum(number);
                number = numbers.Sum();
            } while (numbers.Count > 1);

            NumDefs = defs.Distinct().ToList();
            SlavNums = temp[..^3];
            IsCalc = true;
        }
    }
}