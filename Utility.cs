using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threading_CarRace
{
    internal class Utility
    {
        public static void PrintRaceStatus(List<Car> sortedCarList)
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("| POS  | CONSTRUCTOR | DISTANCE |  SPEED  |");
            Console.WriteLine("|-----------------------------------------|");
            Console.CursorTop = 2;

            // Print out all car stats in the order of who has travelled the furthest i.e leading the race
            int position = 1;
            foreach (var car in sortedCarList)
            {
                // Print out text in different colors for the top 3 in the race
                switch (position)
                {
                    case 1:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        break;
                    case 2:
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        break;
                    case 3:
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        break;
                }

                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write("|");

                Console.SetCursorPosition(2, Console.CursorTop);
                Console.Write($"[{position}]:");

                Console.SetCursorPosition(7, Console.CursorTop);
                Console.Write("|");

                // Print over the name with 11 "blank" chars to avoid chars staying from previous name. (11 because longest car name is 11 chars long). 
                Console.SetCursorPosition(9, Console.CursorTop);
                Console.Write($"             ");

                Console.SetCursorPosition(9, Console.CursorTop);
                Console.Write($"{car.Name}");

                Console.SetCursorPosition(21, Console.CursorTop);
                Console.Write("|");

                string distanceFormatted = car.DistanceTravelled.ToString("0.000"); // Format so there's always 3 decimals showing - for concistency 

                Console.SetCursorPosition(23, Console.CursorTop);
                Console.Write($"{distanceFormatted}km");

                Console.SetCursorPosition(32, Console.CursorTop);
                Console.Write("|");

                Console.SetCursorPosition(34, Console.CursorTop);
                Console.Write($"{car.Speed}km/h");

                Console.SetCursorPosition(42, Console.CursorTop);
                Console.Write("|");

                Console.ResetColor();
                Console.CursorTop++;
                position++;
            }

            Console.SetCursorPosition(0, 12);

            Console.WriteLine("|-----------------------------------------|");
            Console.WriteLine("|   Press [ENTER] to update race status   |");
            Console.WriteLine("|-----------------------------------------|");
            Console.WriteLine("|            Live grid updates:           |");

            // Waits for user to press enter to clear screen and re-print race status
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter)
            {
                // Clears only the status window when prompted so as to keep the events printed out from clearing
                Console.SetCursorPosition(0, 0);
                for (int i = 0; i < sortedCarList.Count; i++)
                {
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, Console.CursorTop + 1);
                }
            }
        }

        public static void PrintRaceResults(List<Car> winOrder)
        {
            // Print out all cars in the order they finished the race
            Console.WriteLine("|  POSITION   |  CONSTRUCTOR  |     TIME     |");
            Console.WriteLine("|--------------------------------------------|");

            Console.CursorTop = 2;

            int position = 1; // 1 because first time is always the winner - increments each loop
            foreach (var car in winOrder)
            {
                switch (position)
                {
                    case 1:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case 2:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                    case 3:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        break;
                }

                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write("|");

                switch (position)
                {
                    case 1:
                        Console.SetCursorPosition(3, Console.CursorTop);
                        Console.Write($"GOLD!");
                        break;
                    case 2:
                        Console.SetCursorPosition(3, Console.CursorTop);
                        Console.Write($"SILVER");
                        break;
                    case 3:
                        Console.SetCursorPosition(3, Console.CursorTop);
                        Console.Write($"BRONZE");
                        break;
                    // Prints out the divider before the 4th place. Looks wierd but necessary to make it match the rest of the textformatting
                    case 4:
                        Console.WriteLine("--------------------------------------------|");
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write($"|  {position}th place");
                        break;
                    default:
                        Console.SetCursorPosition(3, Console.CursorTop);
                        Console.Write($"{position}th place");
                        break;
                }

                Console.SetCursorPosition(14, Console.CursorTop);
                Console.Write("|");

                Console.SetCursorPosition(17, Console.CursorTop);
                Console.Write($"{car.Name}");

                Console.SetCursorPosition(30, Console.CursorTop);
                Console.Write("|");

                string raceTimeFormatted = car.raceTime.TotalSeconds.ToString("0.0000"); // Format so there's always 5 decimals showing - for consistency

                Console.SetCursorPosition(32, Console.CursorTop);
                Console.Write($"{raceTimeFormatted} sec");

                Console.SetCursorPosition(45, Console.CursorTop);
                Console.Write("|");

                Console.ResetColor();
                Console.CursorTop++;
                position++;
            }

            Console.SetCursorPosition(0, Console.CursorTop);
            Console.WriteLine("|--------------------------------------------|");

            System.Environment.Exit(0);
        }
    }
}
