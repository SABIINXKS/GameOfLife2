using System;
using System.Threading;

namespace GameOfLife
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true) // Main menu/game loop
            {
                Console.WriteLine("== Game of Life == ");
                int size = 0;

                // Field size selection menu
                while (true)
                {
                    Console.WriteLine("Choose field size: ");
                    Console.WriteLine("1 - 10x10");
                    Console.WriteLine("2 - 20x20");
                    Console.WriteLine("3 - 30x30");
                    Console.WriteLine("b - Go back (exit)");
                    Console.Write("Your choice: ");

                    string input = Console.ReadLine();

                    if (input?.Trim().ToLower() == "b")
                    {
                        Console.WriteLine("Exiting program...");
                        return; // Exit the application
                    }

                    if (int.TryParse(input, out int choice))
                    {
                        size = GetFieldSize(choice);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter 1, 2, 3, or 'b' to exit.");
                    }
                }

                // create game field
                bool[,] field = new bool[size, size];

                // initialize with some alive cells 
                InitializeField(field, size);

                Console.WriteLine($"You selected field size {size}x{size}");
                Console.WriteLine("Initial field:");
                ShowField(field, size);

                Console.WriteLine("Game loop started. Press 'b' to go back to menu.");

                // Game loop: auto-update every second
                while (true)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.B)
                        {
                            Console.Clear();
                            break; // Go back to menu
                        }
                    }
                    NextGeneration(field, size);
                    Console.Clear();
                    Console.WriteLine("Game of Life - Next Generation:");
                    ShowField(field, size);
                    Console.WriteLine("Press 'b' to go back to menu.");
                    Thread.Sleep(1000); // Wait 1 second
                }
            }
        }

        static int GetFieldSize(int choice)
        {
            switch (choice)
            {
                case 1: return 10;
                case 2: return 20;
                case 3: return 30;
                default: return 10; // Default to 10x10 if invalid choice
            }
        }

        static void InitializeField(bool[,] field, int size)
        {
            Random random = new Random();

            // Fill randomly (about 30% alive cells)
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    field[i, j] = random.Next(100) < 30; // 30% chance of being alive
                }
            }
        }

        static void ShowField(bool[,] field, int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(field[i, j] ? "*" : "."); // Use '*' for alive cells and '.' for dead cells
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        static int CountLiveNeighbors(bool[,] field, int size, int x, int y)
        {
            int count = 0;
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue; // Skip the cell itself
                    int nx = x + dx;
                    int ny = y + dy;
                    if (nx >= 0 && nx < size && ny >= 0 && ny < size)
                    {
                        if (field[nx, ny]) count++;
                    }
                }
            }
            return count;
        }

        static void NextGeneration(bool[,] field, int size)
        {
            bool[,] next = new bool[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int neighbors = CountLiveNeighbors(field, size, i, j);
                    if (field[i, j])
                    {
                        // Live cell survives with 2 or 3 neighbors
                        next[i, j] = neighbors == 2 || neighbors == 3;
                    }
                    else
                    {
                        // Dead cell becomes alive with exactly 3 neighbors
                        next[i, j] = neighbors == 3;
                    }
                }
            }
            // Copy next generation to field 
            Array.Copy(next, field, size * size);
        }
    }
}