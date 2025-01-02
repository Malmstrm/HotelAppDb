using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAppDb.Controllers
{
    public class NavigationMenu
    {
        public int DisplayMenu(string title, List<string> options)
        {
            int selectedIndex = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"--- {title} ---");
                Console.WriteLine(new string('-', title.Length + 6));

                for (int i = 0; i < options.Count; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.WriteLine($" > {options[i]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"   {options[i]}");
                    }
                }

                ConsoleKey key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (selectedIndex > 0) selectedIndex--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (selectedIndex < options.Count - 1) selectedIndex++;
                        break;
                    case ConsoleKey.Enter:
                        return selectedIndex; // Returnera det valda alternativets index
                }
            }
        }
    }
}
