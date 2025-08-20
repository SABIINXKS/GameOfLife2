using System;
using System.Threading;
using LifeEngineLib;

namespace GameOfLife.ConsoleApp
{
    public class GameRunner
    {
        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Game of Life ===");

                GameState loadedState = GameStateManager.LoadFromFile();
                bool useLoaded = false;

                if (loadedState != null)
                {
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
                    size = loadedState.Size;
                    engine = new LifeEngine(size);
                    engine.Field = loadedState.Field;
                    generationCount = loadedState.Generation;
                }
                else
                {
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
                            return;
                        }

                        if (int.TryParse(input, out int choice))
                        {
                            size = choice switch
                            {
                                1 => 10,
                                2 => 20,
                                3 => 30,
                                _ => 10
                            };
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter 1, 2, 3, or 'b' to exit.");
                        }
                    }

                    engine = new LifeEngine(size);
                    engine.InitializeField();
                    generationCount = 0;
                }

                Console.Clear();
                Console.WriteLine($"Game of Life - Field size: {size}x{size}");
                Console.WriteLine("Press 'b' to go back to menu. Press 'Esc' to exit immediately.");

                while (true)
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
                            GameStateManager.SaveToFile(gameState);

                            Console.Clear();
                            break;
                        }
                        else if (key.Key == ConsoleKey.Escape)
                        {
                            Console.WriteLine("Application stopped by user.");
                            Environment.Exit(0);
                        }
                    }

                    engine.NextGeneration();
                    generationCount++;

                    int livingCells = engine.GetLivingCellsCount();

                    Console.Clear();
                    Console.WriteLine($"Game of Life - Generation {generationCount}");
                    ShowField(engine.Field, size);
                    Console.WriteLine($"\nLiving Cells: {livingCells}");
                    Console.WriteLine("Press 'b' to return to menu. Press 'Esc' to exit immediately.");

                    Thread.Sleep(1000);
                }
            }
        }

        private void ShowField(bool[,] field, int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(field[i, j] ? "*" : ".");
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }
}