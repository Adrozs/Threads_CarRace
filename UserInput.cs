using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Threading_CarRace
{
    internal class UserInput
    {
        public static int setRaceDistance(int defaultRaceDistance)
        {
            int inputDistance;
            do
            {
                // Clears only below the welcome text
                Console.SetCursorPosition(0, 3);
                for (int i = 0; i < 3; i++)
                {
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, Console.CursorTop + 1);
                }
                Console.SetCursorPosition(0, 2);

                Console.WriteLine($"How far do you want the cars to drive? Default is {defaultRaceDistance}km");
                Console.WriteLine("Enter a value between 1-100 to change distance or press enter to continue with default");
                Console.Write("Value: ");
                string input = Console.ReadLine();

                if (input == "")
                {
                    inputDistance = defaultRaceDistance;
                    break;
                }
                else if (int.TryParse(input, out inputDistance))
                {
                    if (inputDistance > 1 || inputDistance < 100)
                        break;
                }
            }
            while (true);
            defaultRaceDistance = inputDistance;

            return defaultRaceDistance;

        }

        public static int setEventInterval(int defaultEventInterval)
        {
            int inputEventInterval;
            do
            {
                // Clears only below the welcome text
                Console.SetCursorPosition(0, 3);
                for (int i = 0; i < 3; i++)
                {
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, Console.CursorTop + 1);
                }
                Console.SetCursorPosition(0, 2);

                Console.WriteLine($"How often do you wish for an possible event to take place? Default is {defaultEventInterval} seconds");
                Console.WriteLine("Enter a value between 1-100 to change interval or press enter to continue with default");
                Console.Write("Value: ");
                string input = Console.ReadLine();

                if (input == "")
                {
                    inputEventInterval = defaultEventInterval;
                    break;
                }
                else if (int.TryParse(input, out inputEventInterval))
                {
                    if (inputEventInterval > 1 || inputEventInterval < 100)
                        break;
                }
            }
            while (true);
            defaultEventInterval = inputEventInterval;

            return defaultEventInterval;
        }

        public static int setCarSpeed(int defaultCarSpeed)
        {
            int inputCarSpeed;
            do
            {
                // Clears only below the welcome text
                Console.SetCursorPosition(0, 3);
                for (int i = 0; i < 3; i++)
                {
                    Console.Write(new string(' ', Console.WindowWidth+30));
                    Console.SetCursorPosition(0, Console.CursorTop + 1);
                }
                Console.SetCursorPosition(0, 2);

                Console.WriteLine($"How fast do you want the cars to drive? Default is {defaultCarSpeed}km/h                       ");
                Console.WriteLine("Enter a value between 1-1000 to change interval or press enter to continue with default");
                Console.Write("Value: ");
                string input = Console.ReadLine();

                if (input == "")
                {
                    inputCarSpeed = defaultCarSpeed;
                    break;
                }
                else if (int.TryParse(input, out inputCarSpeed))
                {
                    if (inputCarSpeed > 1 || inputCarSpeed < 1000)
                        break;
                }
            }
            while (true);
            defaultCarSpeed = inputCarSpeed;

            return defaultCarSpeed;
        }
    }
}
