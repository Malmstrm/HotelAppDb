using Spectre.Console;

namespace HotelAppDb.Utilities
{
    public class InputCalendar
    {
        public static DateTime SelectDate(
            DateTime initialDate,
            string prompt,
            bool allowPastDates = false,
            DateTime? minDate = null,
            DateTime? maxDate = null)
        {
            DateTime currentDate = initialDate;

            while (true)
            {
                Console.Clear();
                RenderCalendar(currentDate, prompt);

                if (minDate.HasValue && currentDate < minDate.Value)
                {
                    currentDate = minDate.Value;
                }
                if (maxDate.HasValue && currentDate > maxDate.Value)
                {
                    currentDate = maxDate.Value;
                }

                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.RightArrow:
                        currentDate = currentDate.AddDays(1);
                        break;
                    case ConsoleKey.LeftArrow:
                        currentDate = currentDate.AddDays(-1);
                        break;
                    case ConsoleKey.UpArrow:
                        currentDate = currentDate.AddDays(-7);
                        break;
                    case ConsoleKey.DownArrow:
                        currentDate = currentDate.AddDays(7);
                        break;
                    case ConsoleKey.Enter:
                        if (!allowPastDates && currentDate < DateTime.Today)
                        {
                            Console.WriteLine("Cannot select a past date. Please choose a future date.");
                            Thread.Sleep(2000);
                        }
                        else
                        {
                            return currentDate.Date;
                        }
                        break;
                    case ConsoleKey.Escape:
                        throw new OperationCanceledException("Date selection was canceled.");
                }
            }
        }

        private static void RenderCalendar(DateTime selectedDate, string prompt)
        {
            var calendarContent = new StringWriter();

            // Kalenderhuvud
            calendarContent.WriteLine($"[red]{selectedDate:MMMM}[/]".ToUpper());
            calendarContent.WriteLine("Mån  Tis  Ons  Tor  Fre  Lör  Sön");
            calendarContent.WriteLine("─────────────────────────────────");

            DateTime firstDayOfMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month);
            int startDay = (int)firstDayOfMonth.DayOfWeek;
            startDay = (startDay == 0) ? 6 : startDay - 1; // Justera för måndag som veckostart

            // Fyll med tomma platser innan första dagen i månaden
            for (int i = 0; i < startDay; i++)
            {
                calendarContent.Write("     ");
            }

            // Skriv ut dagarna
            for (int day = 1; day <= daysInMonth; day++)
            {
                var currentDay = new DateTime(selectedDate.Year, selectedDate.Month, day);
                bool isWeekend = currentDay.DayOfWeek == DayOfWeek.Saturday || currentDay.DayOfWeek == DayOfWeek.Sunday;

                if (day == selectedDate.Day)
                {
                    calendarContent.Write($"[green]{day,2}[/]   ");
                }
                else if (isWeekend)
                {
                    calendarContent.Write($"[red]{day,2}[/]   ");
                }
                else
                {
                    calendarContent.Write($"{day,2}   ");
                }

                if ((startDay + day) % 7 == 0)
                {
                    calendarContent.WriteLine();
                }
            }

            // Skapa en panel med dubbla kanter
            var panel = new Panel(calendarContent.ToString())
            {
                Border = BoxBorder.Double,
                Header = new PanelHeader(($"[red]{selectedDate:yyyy} - {prompt}[/]"), Justify.Center)
            };

            AnsiConsole.Write(panel);
            Console.WriteLine();
            AnsiConsole.MarkupLine($"\n[bold]{prompt}[/]");
            Console.WriteLine("Use arrow keys (Left/Up/Right/Down) to navigate and Enter to select.");

        }
    }
}
