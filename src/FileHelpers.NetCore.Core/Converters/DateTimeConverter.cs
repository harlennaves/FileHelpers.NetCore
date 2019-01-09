using System;
using System.Globalization;

namespace FileHelpers.Core.Converters
{
    public class DateTimeConverter : ConverterBase
    {
        public string Format { get; }

        public DateTimeConverter(string format)
        {
            Format = format;
        }

        public override object StringToField(string from)
        {
            if (string.IsNullOrWhiteSpace(from))
            {
                return DateTime.MinValue;
            }

            if ("YYWWD".Equals(Format))
            {
                return YYWWDToDateTime(from);
            }

            if (!DateTime.TryParseExact(from, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return DateTime.MinValue;
            }


            return result;
        }

        public override string FieldToString(object from)
        {
            try
            {
                return "YYWWD".Equals(Format) ? DateTimeToYYWWD((DateTime)from) : string.Format("{0:" + Format + "}", from);
            }
            catch (Exception)
            {
                return new string('0', Format.Length);
            }
        }

        private DateTime YYWWDToDateTime(string from)
        {
            if (string.IsNullOrWhiteSpace(from))
            {
                return DateTime.MinValue;
            }

            if (from.Length != 5)
            {
                return DateTime.MinValue;
            }

            DateTime converted;
            int year;
            int weekOfYear;
            int day;

            int.TryParse(from.Substring(0, 2), out year);
            int.TryParse(from.Substring(2, 2), out weekOfYear);
            int.TryParse(from.Substring(4, 1), out day);
            day--;

            try
            {
                year += (year >= 90) ? 1900 : 2000;

                DateTime jan1 = new DateTime(year, 1, 1);
                int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

                DateTime firstThursday = jan1.AddDays(daysOffset);
                var cal = CultureInfo.CurrentCulture.Calendar;
                int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                var weekNum = weekOfYear;
                if (firstWeek <= 1)
                    weekNum -= 1;

                DateTime result = firstThursday.AddDays(weekNum * 7);
                converted = result.AddDays(day - 3);
            }
            catch (Exception)
            {
                converted = DateTime.MinValue;
            }

            return converted;
        }

        private string DateTimeToYYWWD(DateTime from)
        {
            int year = from.Year;
            int weekOfYear = GetIso8601WeekOfYear(from);
            int day = dayOfWeekTurbo(from);

            return $"{year:00}{weekOfYear:00}{day:0}";
        }

        // This presumes that weeks start with Monday.
        // Week 1 is the 1st week of the year with a Thursday in it.
        private int GetIso8601WeekOfYear(DateTime date)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }

            // Return the week of our adjusted day
            int a = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                date,
                CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday);

            return a;
        }

        private int dayOfWeekTurbo(DateTime from)
        {
            return (int)((((from.Ticks >> 14) / 52734375L) + 1) % 7);
        }
    }
}
