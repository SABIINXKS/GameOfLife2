using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using LifeEngineLib;

namespace GameOfLife.ConsoleApp
{
    /// <summary>
    /// Handles the main game loop and user interaction for the Game of Life console application.
    /// </summary>
    public class GameRunner
    {
        /// <summary>
        /// Initializes and displays up to 8 selected games on the screen.
        /// </summary>
        /// <param name="initialFields">List of initial fields (patterns) for each game.</param>
        /// <param name="fieldSize">Size of each game field.</param>
        public void ShowSelectedGames(List<bool[,]> initialFields, int fieldSize = 10)
        {
            int gameCount = initialFields?.Count ?? 0;
            if (gameCount < 1 || gameCount > 8)
                gameCount = 8;

            var games = new List<LifeEngine>();
            for (int i = 0; i < gameCount; i++)
            {
                var engine = new LifeEngine(fieldSize);
                if (initialFields != null && initialFields.Count > i && initialFields[i] != null)
                    engine.Field = initialFields[i];
                else
                    engine.InitializeField();
                games.Add(engine);
            }

            Console.Clear();
            for (int i = 0; i < games.Count; i++)
            {
                Console.WriteLine($"Game {i + 1}:");
                ShowField(games[i].Field, games[i].Size);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Runs the Game of Life application, managing game state, user input, and game progression.
        /// </summary>
        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(GameRunnerConstants.Title);

                GameState loadedState = GameStateManager.LoadFromFile();
                bool useLoaded = false;

                if (loadedState != null)
                {
                    Console.WriteLine(GameRunnerConstants.SavedGameFound);
                    Console.Write(GameRunnerConstants.ContinuePrompt);
                    string choice = Console.ReadLine()?.Trim().ToLower();
                    useLoaded = choice == GameRunnerConstants.YesOption;
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
                        Console.WriteLine(GameRunnerConstants.ChooseFieldSize);
                        Console.WriteLine(GameRunnerConstants.SizeOption1);
                        Console.WriteLine(GameRunnerConstants.SizeOption2);
                        Console.WriteLine(GameRunnerConstants.SizeOption3);
                        Console.WriteLine(GameRunnerConstants.GoBackOption);
                        Console.Write(GameRunnerConstants.YourChoicePrompt);

                        string input = Console.ReadLine()!;

                        if (input?.Trim().ToLower() == GameRunnerConstants.BackOption)
                        {
                            Console.WriteLine(GameRunnerConstants.ExitingProgram);
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
                            Console.WriteLine(GameRunnerConstants.InvalidInput);
                        }
                    }

                    engine = new LifeEngine(size);
                    engine.InitializeField();
                    generationCount = 0;
                }

                Console.Clear();
                Console.WriteLine(string.Format(GameRunnerConstants.GameFieldSize, size, size));
                Console.WriteLine(GameRunnerConstants.MenuInstructions);

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
                            Console.WriteLine(GameRunnerConstants.ApplicationStopped);
                            Environment.Exit(0);
                        }
                    }

                    engine.NextGeneration();
                    generationCount++;

                    int livingCells = engine.GetLivingCellsCount();

                    Console.Clear();
                    Console.WriteLine(string.Format(GameRunnerConstants.GenerationInfo, generationCount));
                    ShowField(engine.Field, size);
                    Console.WriteLine($"\n{string.Format(GameRunnerConstants.LivingCellsInfo, livingCells)}");
                    Console.WriteLine(GameRunnerConstants.MenuInstructions);

                    Thread.Sleep(1000);
                }
            }
        }

        /// <summary>
        /// Executes 1000 Game of Life games in parallel using Parallel.ForEach.
        /// </summary>
        /// <param name="fieldSize">Size of the field for each game.</param>
        /// <param name="generations">Number of generations to simulate per game.</param>
        public void RunParallelGames(int fieldSize = 10, int generations = 100)
        {
            var gameIndices = Enumerable.Range(0, 1000);
            var results = new ConcurrentBag<int>();

            Parallel.ForEach(gameIndices, index =>
            {
                var engine = new LifeEngine(fieldSize);
                engine.InitializeField();

                for (int gen = 0; gen < generations; gen++)
                {
                    engine.NextGeneration();
                }

                int livingCells = engine.GetLivingCellsCount();
                results.Add(livingCells);
            });

            Console.WriteLine($"Executed {results.Count} games in parallel.");
        }

        /// <summary>
        /// Displays the current game field in the console.
        /// </summary>
        /// <param name="field">The game field as a 2D boolean array.</param>
        /// <param name="size">The size of the field.</param>
        private void ShowField(bool[,] field, int size)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write(field[i, j] ? GameRunnerConstants.AliveCell : GameRunnerConstants.DeadCell);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }
}