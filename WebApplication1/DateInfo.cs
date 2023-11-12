using System;

namespace Astro
{
    public class Christian
    {
        private DateTime Datetime { get; }
        
        public Christian(DateTime dateTime) => Datetime = dateTime;
        
        public int Year { get => Datetime.Year; }
        public int Month { get => Datetime.Month; }
        public int Day { get => Datetime.Day; }
        public int Hour { get => Datetime.Hour; }
        public int Min { get => Datetime.Minute; }
    }
    public class Slavian
    {
        public int Year { get; }
        public int Year144 { get; }
        public int Year16 { get; }
        public int Month { get; }
        public string MonthName { get; }
        public int Day { get; }
        public string Season { get; }
        public string DayName { get; }
        public string Нoliday { get; }

        public Slavian(DateTime dateTime)
        {
            DateTime slavDate;
            if (dateTime.Hour >= 18)
                slavDate = new DateTime(dateTime.AddDays(1).Year, dateTime.AddDays(1).Month, dateTime.AddDays(1).Day, dateTime.AddDays(1).Hour, dateTime.AddDays(1).Minute, dateTime.AddDays(1).Second);
            else
                slavDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);

            Year = slavDate.Year + 5508;
            Year144 = (Year % 144) + 112; if (Year144 > 144) Year144 -= 144;
            Year16 = Year % 16; if (Year16 == 0) Year16 = 16;

            if ((Year16 == 1 || Year16 == 2 || Year16 == 3 || Year16 == 4) && ((slavDate.Month >= 9 && slavDate.Day >= 23) || slavDate.Month >= 10))
                { Year += 1; Year144 += 1; Year16 += 1; }
            else if ((Year16 == 5 || Year16 == 6 || Year16 == 7 || Year16 == 8) && ((slavDate.Month >= 9 && slavDate.Day >= 22) || slavDate.Month >= 10))
                { Year += 1; Year144 += 1; Year16 += 1; }
            else if ((Year16 == 9 || Year16 == 10 || Year16 == 11 || Year16 == 12) && ((slavDate.Month >= 9 && slavDate.Day >= 21) || slavDate.Month >= 10))
                { Year += 1; Year144 += 1; Year16 += 1; }
            else if ((Year16 == 13 || Year16 == 14 || Year16 == 15 || Year16 == 16) && ((slavDate.Month >= 9 && slavDate.Day >= 20) || slavDate.Month >= 10))
                { Year += 1; Year144 += 1; Year16 += 1; }
            
            if ((Year16 == 4 && slavDate.Month == 9 && slavDate.Day == 22) || (Year16 == 8 && slavDate.Month == 9 && slavDate.Day == 21) || (Year16 == 12 && slavDate.Month == 9 && slavDate.Day == 20) || (Year16 == 16 && slavDate.Month == 9 && slavDate.Day == 19))
                { Year += 1; Year144 += 1; Year16 += 1; }
            if (Year144 > 144) Year144 -= 144;
            if (Year16 > 16) Year16 -= 16;
            
            string[] slavCal = SlavAstronomy.DateConvert(Year16, slavDate.Month, slavDate.Day, slavDate.Year);
            Month = int.Parse(slavCal[1]);
            MonthName = slavCal[0];
            Day = int.Parse(slavCal[2]);
            Season = slavCal[3];
            DayName = SlavAstronomy.DayCalc(Year144, Month, Day);
            Нoliday = slavCal[4];
        }

        public override string ToString() => $"ЛѢто {Year}, {Day} {MonthName}({Month}) {DayName}, {Season}";
    }

    public class DateInfo
    {
        private DateTime Datetime { get; }
        
        public DateInfo(DateTime dateTime) => Datetime = dateTime;

        public Christian Chris { get => new(Datetime); }
        public Slavian Slav { get => new(Datetime); }
    }
}