using Astro;

namespace WebApplication1
{
    public class BukvitsaService
    {
        public string[] LetterFullDefs { get; } = Letter.GetInfoArr(4);
    }
}