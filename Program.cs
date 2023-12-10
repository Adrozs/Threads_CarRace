using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace Threading_CarRace
{
    internal class Program
    {
        static int raceDistance = 3;
        static int eventInterval = 30;
        static bool raceIsWon = false;
        static bool allCarsInGoal = false;
        static object lockObject = new object();
        static int cursorPos = 16;
        static List<Car> winOrder = new List<Car>();

        static void Main(string[] args)
        {
            int defaultCarSpeed = 120;
            Console.CursorVisible = false;

            // Create an instance of each car that's racing
            Car redbullCar = new Car { Name = "RedBull", Speed = defaultCarSpeed };
            Car mercedesCar = new Car { Name = "Mercedes", Speed = defaultCarSpeed };
            Car ferrariCar = new Car { Name = "Ferrari", Speed = defaultCarSpeed };
            Car mclarenCar = new Car { Name = "McLaren", Speed = defaultCarSpeed };
            Car astonmartinCar = new Car { Name = "Aston Martin", Speed = defaultCarSpeed };
            Car alpineCar = new Car { Name = "Alpine", Speed = defaultCarSpeed };
            Car williamsCar = new Car { Name = "Williams", Speed = defaultCarSpeed };
            Car alphatauriCar = new Car { Name = "AlphaTauri", Speed = defaultCarSpeed };
            Car alfaromeoCar = new Car { Name = "Alfa Romeo", Speed = defaultCarSpeed };
            Car haasCar = new Car { Name = "Haas", Speed = defaultCarSpeed };

            // Create threads for each car and their race
            Thread redbull = new Thread(() => { race(redbullCar); });
            Thread mercedes = new Thread(() => { race(mercedesCar); });
            Thread ferrari = new Thread(() => { race(ferrariCar); });
            Thread mclaren = new Thread(() => { race(mclarenCar); });
            Thread astonmartin = new Thread(() => { race(astonmartinCar); });
            Thread alpine = new Thread(() => { race(alpineCar); });
            Thread williams = new Thread(() => { race(williamsCar); });
            Thread alphatauri = new Thread(() => { race(alphatauriCar); });
            Thread alfaromeo = new Thread(() => { race(alfaromeoCar); });
            Thread haas = new Thread(() => { race(haasCar); });


            List<Car> list = new List<Car> {redbullCar, mercedesCar, ferrariCar, mclarenCar, astonmartinCar, alpineCar, williamsCar, alphatauriCar, alfaromeoCar, haasCar };

            // Create status thread that prints out the race status on user input
            Thread status = new Thread(() => { raceStatus(list); });


            // Start all race- and the status thread(s)
            Console.WriteLine("Race has started! \n\n\n");
            redbull.Start();
            mercedes.Start();
            ferrari.Start();
            mclaren.Start();
            astonmartin.Start();
            alpine.Start();
            williams.Start();
            alphatauri.Start();
            alfaromeo.Start();
            haas.Start();

            status.Start();
            
            // Wait for all threads to complete
            redbull.Join();
            mercedes.Join();
            ferrari.Join();
            mclaren.Join();
            astonmartin.Join();
            alpine.Join();
            williams.Join();
            alphatauri.Join();
            alfaromeo.Join();
            haas.Join();


            // After race is complete 
            Console.WriteLine("\nRace is over!");


            // Clears everything on the screen
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < cursorPos+1; i++)
            {
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop + 1);
            }

            // Reset cursor position
            Console.SetCursorPosition(0, 0);


            Console.WriteLine("|  POSITION  |  CONSTRUCTOR  |");
            Console.WriteLine("|----------------------------|");

            Console.CursorTop = 2;

            int position = 1;
            foreach (var car in winOrder)
            {
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

                Console.SetCursorPosition(3, Console.CursorTop);
                Console.Write($"[{position}]:");

                Console.SetCursorPosition(14, Console.CursorTop);
                Console.Write("|");

                Console.SetCursorPosition(16, Console.CursorTop);
                Console.Write($"{car.Name}");

                Console.SetCursorPosition(28, Console.CursorTop);
                Console.Write("|");

                Console.ResetColor();
                Console.CursorTop++;
                position++;
            }

        }

        // Handles everything for the race threads
        static void race(Car car)
        {
            ManualResetEvent pauseEvent = new ManualResetEvent(true);
            car.seconds = 0;
            car.reachedGoal = false;
            //Timer timer = new Timer(randomEvent, name, 0, 30000);

            while (!car.reachedGoal)
            {
                pauseEvent.WaitOne(); // Wait for the event to be signaled

                Thread.Sleep(1000);
                car.seconds++;

                if (car.seconds == eventInterval)
                {
                    randomEvent(car, pauseEvent);

                    car.seconds = 0;
                }

                // Gets the distance travelled per second ex: x = 100 * (1 hour / 60 to get minutes / 60 to get seconds)
                car.DistanceTravelled += car.Speed * (1.0 / 60 / 60);

                if (car.DistanceTravelled >= raceDistance)
                {
                    //timer.Dispose();

                    lock (lockObject)
                    {
                        if (!raceIsWon)
                        {
                            raceIsWon = true;
                            car.reachedGoal = true;

                            Console.SetCursorPosition(0, cursorPos);
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write($"{car.Name}");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("HAS WON THE RACE!");
                            Console.ResetColor();
                            cursorPos++;

                            winOrder.Add(car);
                        }
                    }

                    lock (lockObject)
                    {
                        if (raceIsWon)
                        {
                            car.reachedGoal = true;

                            Console.SetCursorPosition(0, cursorPos);
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write($"{car.Name} ");
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("has passed the checkered flag.");
                            Console.ResetColor();
                            cursorPos++;

                            winOrder.Add(car);
                        }
                    }
                }
            }
        }

        // Handles all incidents that could occur during the race
        static void randomEvent(Car car, ManualResetEvent pauseEvent)
        {
            Random rnd = new Random();
            int chance = rnd.Next(0, 50);            

            if (chance == 1) // 1/50 chance
            {
                lock (lockObject)
                {
                    Console.SetCursorPosition(0, cursorPos);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("Huge mistake by the ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{car.Name} ");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("team, they ran out of gas!");
                    Console.ResetColor();
                    cursorPos++;
                }

                pauseEvent.Reset();
                Task.Delay(30000).Wait();
                pauseEvent.Set();

                lock (lockObject)
                {
                    Console.SetCursorPosition(0, cursorPos);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"{car.Name}'s ");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("tank is filled up and they're back on track!");
                    Console.ResetColor();
                    cursorPos++;
                }
            }
            else if (chance <= 3) // 2/50 chance
            {
                lock (lockObject)
                {
                    Console.SetCursorPosition(0, cursorPos);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("Oh no! The ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{car.Name}'s ");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("ran over a pothole and got a flat tire!");
                    Console.ResetColor();
                    cursorPos++;
                }
                
                pauseEvent.Reset();
                Task.Delay(20000).Wait();
                pauseEvent.Set();

                lock (lockObject)
                {
                    Console.SetCursorPosition(0, cursorPos);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write("The ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"{car.Name}'s ");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("mechanics finally patched up the car in and they're off again.");
                    Console.ResetColor();
                    cursorPos++;
                }
            }
            else if (chance <= 8) // 5/50 chance
            {
                lock (lockObject)
                {
                    Console.SetCursorPosition(0, cursorPos);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("Look out! A bird smashed into the ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{car.Name}'s ");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("window! ");
                    Console.ResetColor();
                    cursorPos++;
                }

                pauseEvent.Reset();
                Task.Delay(10000).Wait();
                pauseEvent.Set();

                lock (lockObject)
                {
                    Console.SetCursorPosition(0, cursorPos);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"{car.Name} ");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("finally managed to get rid of the bird and are getting back into the race!");
                    Console.ResetColor();
                    cursorPos++;
                }
                        
            }
            else if (chance <= 18) // 10/50 chance
            {
                lock (lockObject)
                {
                    Console.SetCursorPosition(0, cursorPos);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("Catastrophic! The ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{car.Name} ");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("has engine troubles and are loosing speed! ");
                    Console.ResetColor();
                    cursorPos++;
                    car.Speed--;
                }
            }
        }

        // Waits for user input to display the races' current status
        static void raceStatus(List<Car> carList)
        {
            while (true)
            { 

                // If all 10 cars are in goal break out of the loop
                if (winOrder.Count == 10)
                {
                    break;
                }

                Console.SetCursorPosition(0, 0);
                Console.WriteLine("| POS  | CONSTRUCTOR | DISTANCE |  SPEED  |");
                Console.WriteLine("|-----------------------------------------|");
                Console.CursorTop = 2;

                List<Car> sortedCarList = carList.OrderByDescending(car => car.DistanceTravelled).ToList();

                // Print out all car stats in the order of who has travelled the furthest i.e leading the race
                int position = 1;
                foreach (var car in sortedCarList)
                {
                    string distanceFormatted = car.DistanceTravelled.ToString("0.000"); // Format so there's always 3 decimals showing - for concistency 

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

                    Console.SetCursorPosition(9, Console.CursorTop);
                    Console.Write($"{car.Name}");

                    Console.SetCursorPosition(21, Console.CursorTop);
                    Console.Write("|");

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
                Console.WriteLine("Live grid updates:");            

                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    // Clears only the status window when prompted so as to keep the events printed out from clearing
                    Console.SetCursorPosition(0, 0);
                    for (int i = 0; i < carList.Count; i++)
                    {
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, Console.CursorTop + 1);
                    }
                }
            }
        }
    }
}