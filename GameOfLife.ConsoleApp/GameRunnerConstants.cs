namespace GameOfLife.ConsoleApp
{
    /// <summary>
    /// Contains constant string values used in the GameRunner class.
    /// </summary>
    public static class GameRunnerConstants
    {
        public const string Title = "=== Game of Life ===";
        public const string SavedGameFound = "Saved game found.";
        public const string ContinuePrompt = "Do you want to continue from the last saved game? (y/n): ";
        public const string ChooseFieldSize = "Choose field size: ";
        public const string SizeOption1 = "1 - 10x10";
        public const string SizeOption2 = "2 - 20x20";
        public const string SizeOption3 = "3 - 30x30";
        public const string GoBackOption = "b - Go back (exit)";
        public const string YourChoicePrompt = "Your choice: ";
        public const string ExitingProgram = "Exiting program...";
        public const string InvalidInput = "Invalid input. Please enter 1, 2, 3, or 'b' to exit.";
        public const string GameFieldSize = "Game of Life - Field size: {0}x{1}";
        public const string MenuInstructions = "Press 'b' to go back to menu. Press 'Esc' to exit immediately.";
        public const string ApplicationStopped = "Application stopped by user.";
        public const string GenerationInfo = "Game of Life - Generation {0}";
        public const string LivingCellsInfo = "Living Cells: {0}";
        public const string AliveCell = "*";
        public const string DeadCell = ".";
        public const string YesOption = "y";
        public const string BackOption = "b";
    }
}