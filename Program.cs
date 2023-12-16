using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace Threading_CarRace
{
    internal class Program
    {
        // Global variables 

        // Create default race variables 
        static int raceDistance = 5; // Change to change the default race distance (km)
        static int eventSeconds = 30;// Change how often random events take place (seconds)
        static int carSpeed = 120; // Change to change the default speed of the cars (km/h)

        static bool raceIsWon = false;
        static object lockObject = new object();
        static int cursorPos = 17;
        static List<Car> winOrder = new List<Car>();
        public static Stopwatch stopwatch = new Stopwatch();
        static Stopwatch eventTimer = new Stopwatch();
        static TimeSpan eventInterval = new TimeSpan(0, 0, eventSeconds);

        static void Main(string[] args)
        {
            // Take user input to change settings
            Console.WriteLine("Welcome to Formula Chas simulator - the pinnacle of motorsports!");
            Console.WriteLine("----------------------------------------------------------------");

            raceDistance = UserInput.setRaceDistance(raceDistance);
            carSpeed = UserInput.setCarSpeed(carSpeed);
            eventInterval = TimeSpan.FromSeconds(UserInput.setEventInterval(eventSeconds));

            // No more text inputs in program - hide cursor
            Console.CursorVisible = false;
            Console.Clear();

            // Create an instance of each of the 10 cars that are racing
            Car redbullCar = new Car { Name = "RedBull", Speed = carSpeed };
            Car mercedesCar = new Car { Name = "Mercedes", Speed = carSpeed };
            Car ferrariCar = new Car { Name = "Ferrari", Speed = carSpeed };
            Car mclarenCar = new Car { Name = "McLaren", Speed = carSpeed };
            Car astonmartinCar = new Car { Name = "Aston Martin", Speed = carSpeed };
            Car alpineCar = new Car { Name = "Alpine", Speed = carSpeed };
            Car williamsCar = new Car { Name = "Williams", Speed = carSpeed };
            Car alphatauriCar = new Car { Name = "AlphaTauri", Speed = carSpeed };
            Car alfaromeoCar = new Car { Name = "Alfa Romeo", Speed = carSpeed };
            Car haasCar = new Car { Name = "Haas", Speed = carSpeed };

            // Create a list of all the cars
            List<Car> list = new List<Car> { redbullCar, mercedesCar, ferrariCar, mclarenCar, astonmartinCar, alpineCar, williamsCar, alphatauriCar, alfaromeoCar, haasCar };

            // Create threads for each car and the race method
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

            Thread raceThread = new Thread(() => { raceStatus(list); });

            // Start all threads
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

            raceThread.Start();
            
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
            raceThread.Join();

            // After race is complete

            // Clears everything on the screen
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < cursorPos + 1; i++)
            {
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop + 1);
            }

            Console.SetCursorPosition(0, 0);

            Utility.PrintRaceResults(winOrder);
        }

        // Handles everything for the race threads
        static void race(Car car)
        {
            ManualResetEvent pauseEvent = new ManualResetEvent(true);
            car.ReachedGoal = false;
            eventTimer.Start();
            stopwatch.Start();

            // Races until car has reached the goal
            while (!car.ReachedGoal)
            {
                Thread.Sleep(1000); // Simulate 1 second

                pauseEvent.WaitOne(); // Wait for the event to be signaled;


                // When seconds equal the selected interval, trigger a random event
                if (eventTimer.Elapsed >= eventInterval)
                {
                    randomEvent(car, pauseEvent);

                    eventTimer.Restart(); // Reset seconds
                }

                // Adds the distance travelled per second ex: 0,0277 = 100 * (1 hour / 60 to get minutes / 60 to get seconds)
                car.DistanceTravelled += car.Speed * (1.0 / 60 / 60);

                // If car has travelled further or equal to the total distance of the race - end the race
                if (car.DistanceTravelled >= raceDistance)
                {
                    lock (lockObject)
                    {
                        // If no one has won the race, this car is the winner
                        if (!raceIsWon)
                        {
                            raceIsWon = true;
                            car.ReachedGoal = true;
                            car.RaceTime = stopwatch.Elapsed; 

                            Console.SetCursorPosition(0, cursorPos);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write($"{car.Name} ");
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("HAS WON THE RACE!");
                            Console.ResetColor();
                            cursorPos++;

                            winOrder.Add(car);
                        }
                    }

                    lock (lockObject)
                    {
                        // Makes sure that someone has won the race and prevents that the winner gets access to this piece of code
                        if (raceIsWon && !car.ReachedGoal)
                        {
                            car.ReachedGoal = true;
                            car.RaceTime = stopwatch.Elapsed;

                            Console.SetCursorPosition(0, cursorPos);
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write($"{car.Name} ");
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("has passed the checkered flag.");
                            Console.ResetColor();
                            cursorPos++;

                            winOrder.Add(car);

                            // If last car has passed finish line then print message to let user know they need to press enter to continue
                            if (winOrder.Count == 10)
                                Console.WriteLine("\nPress [ENTER] to continue");
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

            // Selects a random event based on chance
            // Locks each part where printing text is done to make sure several threads don't collide and mess up the text
            // Adds one to cursorPos on each print so the next text wont print over the last one - that's why cursorPos is only used in this method
            // Code looks a bit messy but that is only because I wanted to highlight each car name in a light color with the background in a darker color.
            // I also wanted to make the messages when a car is back in the race to be a different color to stand out. So due to formatting.
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

                // Sort the list of cars by the distance they've travelled
                List<Car> sortedCarList = carList.OrderByDescending(car => car.DistanceTravelled).ToList();

                // Prints all the cars and their standings in the race
                Utility.PrintRaceStatus(sortedCarList);
            }
        }
    }
}