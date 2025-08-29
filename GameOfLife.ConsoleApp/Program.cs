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

            // Create example fields (patterns)
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

            // Add patterns to the list of initial fields
            var initialFields = new List<bool[,]> { glider, blinker };

            // Show selected games on screen with specified patterns
            runner.ShowSelectedGames(initialFields, 10);

            // Create LifeEngine instances for each game
            var games = new List<LifeEngine>
            {
                new LifeEngine(10) { Field = glider },
                new LifeEngine(10) { Field = blinker }
            };

            // Set generation counts for each game (0 for new games)
            var generationCounts = new List<int> { 0, 0 };

            // Save all games at once
            runner.SaveAllGames(games, generationCounts);
        }
    }
}