using System;
using System.Threading;
using LifeEngineLib;

namespace GameOfLife.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("=== Game of Life === ");
                int size = 0;

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

                var engine = new LifeEngine(size);
                engine.InitializeField();
                int generationCount = 0;

                Console.Clear();
                Console.WriteLine($"You selected field size {size}x{size}");
                Console.WriteLine("Initial field:");
                ShowField(engine.Field, size);
                Console.WriteLine("Game loop starting. Press 'b' anytime to go back to menu.");
                Thread.Sleep(2000);

                while (true)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.B)
                        {
                            Console.Clear();
                            break;
                        }
                    }

                    engine.NextGeneration();
                    generationCount++;
                    int livingCells = engine.GetLivingCellsCount();

                    Console.Clear();
                    Console.WriteLine($"Game of Life - Generation {generationCount}");
                    ShowField(engine.Field, size);
                    Console.WriteLine($"\nLiving Cells: {livingCells}");
                    Console.WriteLine("Press 'b' to return to menu.");
                    Thread.Sleep(1000);
                }
            }
        }

        static void ShowField(bool[,] field, int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(field[i, j] ? "*" : ".");
                    Console.Write(" ");
                }
                Console.WriteLine();

                // comment 
            }
        }
    }
}
