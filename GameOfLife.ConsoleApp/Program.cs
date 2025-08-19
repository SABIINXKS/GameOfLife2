using System;
using System.Threading;
using LifeEngineLib;

namespace GameOfLife.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Main loop to allow restarting or exiting the game
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Game of Life ===");

                // Attempt to load a previously saved game state from file
                GameState loadedState = GameState.LoadFromFile();
                bool useLoaded = false;

                if (loadedState != null)
                {
                    // Ask the user if they want to continue the saved game
                    Console.WriteLine("Saved game found.");
                    Console.Write("Do you want to continue from the last saved game? (y/n): ");
                    string choice = Console.ReadLine()?.Trim().ToLower();
                    useLoaded = choice == "y";
                }

                int size;
                LifeEngine engine;
                int generationCount;

                if (useLoaded)
                {
                    // Initialize game engine with loaded game state
                    size = loadedState.Size;
                    engine = new LifeEngine(size);
                    engine.Field = loadedState.Field;          // Restore the saved field
                    generationCount = loadedState.Generation; // Restore the saved generation count
                }
                else
                {
                    // No saved game loaded, prompt user to select field size
                    size = 0;

                    while (true)
                    {
                        Console.WriteLine("Choose field size: ");
                        Console.WriteLine("1 - 10x10");
                        Console.WriteLine("2 - 20x20");
                        Console.WriteLine("3 - 30x30");
                        Console.WriteLine("b - Go back (exit)");
                        Console.Write("Your choice: ");

                        string input = Console.ReadLine()!;

                        if (input?.Trim().ToLower() == "b")
                        {
                            Console.WriteLine("Exiting program...");
                            return; // Exit application
                        }

                        // Parse user input and set field size accordingly
                        if (int.TryParse(input, out int choice))
                        {
                            size = choice switch
                            {
                                1 => 10,
                                2 => 20,
                                3 => 30,
                                _ => 10
                            };
                            break; // Exit size selection loop
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter 1, 2, 3, or 'b' to exit.");
                        }
                    }

                    // Create a new LifeEngine and initialize the game field
                    engine = new LifeEngine(size);
                    engine.InitializeField();
                    generationCount = 0;
                }

                Console.Clear();
                Console.WriteLine($"Game of Life - Field size: {size}x{size}");
                Console.WriteLine("Press 'b' to go back to menu or 'Esc' to exit game.");

                // Game loop: advances the simulation and listens for user input
                while (true)
                {
                    // Advance to the next generation
                    engine.NextGeneration();
                    generationCount++;

                    // Count living cells for display
                    int livingCells = engine.GetLivingCellsCount();

                    // Clear console and display current generation and field
                    Console.Clear();
                    Console.WriteLine($"Game of Life - Generation {generationCount}");
                    ShowField(engine.Field, size);
                    Console.WriteLine($"\nLiving Cells: {livingCells}");
                    Console.WriteLine("Press 'b' to return to menu or 'Esc' to exit game.");

                    // Wait for key press or timeout (1 sekunde, pārbaude ik pēc 50 ms)
                    int waitTime = 1000;
                    int elapsed = 0;
                    int interval = 50;

                    bool breakLoop = false;
                    while (elapsed < waitTime)
                    {
                        if (Console.KeyAvailable)
                        {
                            var key = Console.ReadKey(true);

                            if (key.Key == ConsoleKey.B)
                            {
                                var gameState = new GameState
                                {
                                    Size = size,
                                    Field = engine.Field,
                                    Generation = generationCount
                                };
                                gameState.SaveToFile();

                                Console.Clear();
                                breakLoop = true;
                                break;
                            }
                            else if (key.Key == ConsoleKey.Escape)
                            {
                                var gameState = new GameState
                                {
                                    Size = size,
                                    Field = engine.Field,
                                    Generation = generationCount
                                };
                                gameState.SaveToFile();

                                Console.WriteLine("Exiting game...");
                                Thread.Sleep(1000);
                                Environment.Exit(0);
                            }
                        }
                        Thread.Sleep(interval);
                        elapsed += interval;
                    }
                    if (breakLoop)
                        break;
                }
            }
        }

        // Helper method to print the game field to the console
        static void ShowField(bool[,] field, int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    // Print '*' for living cells, '.' for dead cells
                    Console.Write(field[i, j] ? "*" : ".");
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }
}