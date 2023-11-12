namespace Calculations;

public static class StatFunc
{
    // Разбить число на цифры
    public static List<int> SplitNum(int number)
    {
        List<int> result = new List<int>();
        while (number > 0)
        {
            result.Add(number % 10);
            number /= 10;
        }
        return result;
    }

    // Дата и время записи в лог-файл
    public static DateTime LogDateTimeNow() => DateTime.UtcNow.AddHours(3);

}