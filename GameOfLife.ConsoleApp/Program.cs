using LifeEngineLib;
using System;
using System.Collections.Generic;

namespace GameOfLife.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var runner = new GameRunner();

            // Define available patterns
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
                Console.WriteLine("Enter pattern numbers to display (comma separated, max 8), or 'q' to quit:");
                var input = Console.ReadLine();
                if (input?.Trim().ToLower() == "q") break;

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

                runner.ShowSelectedGames(initialFields, 10);

                // Create LifeEngine instances for each game
                var games = new List<LifeEngine>();
                foreach (var field in initialFields)
                {
                    games.Add(new LifeEngine(10) { Field = field });
                }

                // Real-time statistics
                int liveGames = games.Count;
                int totalLivingCells = 0;
                foreach (var game in games)
                {
                    totalLivingCells += game.GetLivingCellsCount();
                }
                Console.WriteLine($"Live games: {liveGames}");
                Console.WriteLine($"Total living cells: {totalLivingCells}");

                // Set generation counts for each game (0 for new games)
                var generationCounts = new List<int>();
                for (int i = 0; i < games.Count; i++) generationCounts.Add(0);

                // Save all games at once
                runner.SaveAllGames(games, generationCounts);

                Console.WriteLine("Press Enter to change selection or 'q' to quit.");
                var next = Console.ReadLine();
                if (next?.Trim().ToLower() == "q") break;
            }
        }
    }
}