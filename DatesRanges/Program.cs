using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DatesRanges
{
    static class Program
    {
        static void Main(string[] args)
        {
            Calendar calendar = new CultureInfo("en-US").Calendar;
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            //calendar.GetWeekOfYear(currentDay, CalendarWeekRule.FirstDay, DayOfWeek.Monday) // Show the week number of the year
            int count = 0;
            List<DatesRanges> list = new List<DatesRanges>();
            foreach (Tuple<DateTime, DateTime> tuple in GetWeeksInMonth(startDate))
            {
                count = count + 1;
                foreach (DateTime currentDate in EachDay(tuple.Item1, tuple.Item2))
                {
                    if (!currentDate.IsWeekend()) //ignore weekends
                    {
                        DatesRanges detail = new DatesRanges();
                        detail.Date = currentDate;
                        detail.Week = count;
                        list.Add(detail);
                    }
                }
            }

            if(list.Count > 0)
            {
                foreach (DatesRanges item in list)
                {
                    Console.WriteLine($"Date: {item.Date.ToString("dd-MMM-yyyy")} | Week: {item.Week}");
                }
            }
        }

        public static IEnumerable<DateTime> EachDay(DateTime startDate, DateTime endDate)
        {
            for (var day = startDate.Date; day.Date <= endDate.Date; day = day.AddDays(1))
                yield return day;
        }

        public static Tuple<DateTime, DateTime> GetWeekRange(DateTime basedatetime)
        {
            try
            {
                DateTime datechange = basedatetime;

                var startweek = datechange.StartOfWeek(DayOfWeek.Monday);
                return new Tuple<DateTime, DateTime>(startweek, startweek.AddDays(6));
            }
            catch (Exception xx)
            {
                throw xx;
            }
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static List<Tuple<DateTime, DateTime>> GetWeeksInMonth(DateTime reference)
        {
            Calendar calendar = CultureInfo.CurrentCulture.Calendar;

            IEnumerable<int> daysInMonth = Enumerable.Range(1, calendar.GetDaysInMonth(reference.Year, reference.Month));

            List<Tuple<DateTime, DateTime>> weeks = daysInMonth.Select(day => new DateTime(reference.Year, reference.Month, day))
                .GroupBy(d => calendar.GetWeekOfYear(d, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday))
                .Select(g => new Tuple<DateTime, DateTime>(g.First(), g.Last()))
                .ToList();

            return weeks;
        }

        public static bool IsWeekend(this DateTime source)
        {
            return source.DayOfWeek == DayOfWeek.Saturday || source.DayOfWeek == DayOfWeek.Sunday;
        }
    }

    public class DatesRanges
    {
        public int Week { get; set; }
        public DateTime Date { get; set; }
    }
}
