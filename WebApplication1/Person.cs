using System;

namespace Astro
{
    public class Person : AstroObject
    {
        public string Firstname { get; } = "";
        public string Lastname { get; } = "";
        public DateTime BirthDate { get; }
        public int LifeNum
        {
            get
            {
                int num = DateInfo.Chris.Year + DateInfo.Chris.Month + DateInfo.Chris.Day;
            start:
                int sum = 0;
                while (num > 0)
                {
                    sum += num % 10;
                    num /= 10;
                }
                if (sum > 9) { num = sum; goto start; }
                return sum;
            }
        }
        public int GuardNum
        {
            get
            {
                int num = DateInfo.Slav.Year + DateInfo.Slav.Month + DateInfo.Slav.Day;
            start:
                int sum = 0;
                while (num > 0)
                {
                    sum += num % 10;
                    num /= 10;
                }
                if (sum > 9) { num = sum; goto start; }
                return sum;
            }
        }

        public Person(DateTime datetime, string firstname, string lastname)
            : base(datetime)
        {
            Firstname = firstname;
            Lastname = lastname;
            BirthDate = datetime;
        }
        public Person() : base(DateTime.Now) { }
    }
}
