using System;

namespace GameOfLife.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var runner = new GameRunner();
            runner.Run();
        }
    }
}