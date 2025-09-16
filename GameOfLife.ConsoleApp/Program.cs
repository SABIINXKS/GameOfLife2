using LifeEngineLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var runner = new GameRunner();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to Conway's Game of Life!");
                Console.WriteLine("1: Start a new game (choose field size)");
                Console.WriteLine("2: Select a pattern (Glider, Blinker, Block, etc)");
                Console.WriteLine("q: Quit");
                Console.Write("Your choice: ");
                var choice = Console.ReadLine()?.Trim().ToLower();

                if (choice == "1")
                {
                    runner.Run();
                }
                else if (choice == "2")
                {
                    RunPatternSelection(runner);
                }
                else if (choice == "q")
                {
                    Console.WriteLine("Exiting...");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option. Press Enter to try again.");
                    Console.ReadLine();
                }
            }
        }

        static void RunPatternSelection(GameRunner runner)
        {
            var glider = new bool[10, 10];
            glider[1, 2] = true;
            glider[2, 3] = true;
            glider[3, 1] = true;
            glider[3, 2] = true;
            glider[3, 3] = true;

            var blinker = new bool[10, 10];
            blinker[5, 5] = true;
            blinker[5, 6] = true;
            blinker[5, 7] = true;

            var block = new bool[10, 10];
            block[4, 4] = true;
            block[4, 5] = true;
            block[5, 4] = true;
            block[5, 5] = true;

            var patterns = new List<(string Name, bool[,] Field)>
            {
                ("Glider", glider),
                ("Blinker", blinker),
                ("Block", block)
            };

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Available patterns:");
                for (int i = 0; i < patterns.Count; i++)
                {
                    Console.WriteLine($"{i + 1}: {patterns[i].Name}");
                }

                Console.WriteLine("Enter pattern numbers to display (comma separated, max 8), or 'b' to go back:");
                var input = Console.ReadLine()?.Trim().ToLower();

                if (input == "b") return;

                var selectedIndices = new List<int>();
                foreach (var part in input.Split(','))
                {
                    if (int.TryParse(part.Trim(), out int idx) && idx >= 1 && idx <= patterns.Count)
                        selectedIndices.Add(idx - 1);
                }

                if (selectedIndices.Count == 0 || selectedIndices.Count > 8)
                {
                    Console.WriteLine("Invalid selection. Press Enter to try again.");
                    Console.ReadLine();
                    continue;
                }

                var initialFields = new List<bool[,]>();
                foreach (var idx in selectedIndices)
                {
                    initialFields.Add(patterns[idx].Field);
                }

                // Show selected patterns
                runner.ShowSelectedGames(initialFields, 10);

                // Generate stats
                var games = new List<LifeEngine>();
                foreach (var field in initialFields)
                {
                    games.Add(new LifeEngine(10) { Field = field });
                }

                int liveGames = games.Count;
                int totalLivingCells = games.Sum(g => g.GetLivingCellsCount());

                Console.WriteLine($"Live games: {liveGames}");
                Console.WriteLine($"Total living cells: {totalLivingCells}");

                var generationCounts = Enumerable.Repeat(0, games.Count).ToList();
                runner.SaveAllGames(games, generationCounts);

                Console.WriteLine("Press Enter to return to main menu.");
                Console.ReadLine();
                return;
            }
        }
    }
}
