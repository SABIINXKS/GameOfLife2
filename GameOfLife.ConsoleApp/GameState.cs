using System;

// Class that represents the state of the game
public class GameState
{
    // Size of the game field (e.g., 10 means a 10x10 grid)
    public int Size { get; set; }

    // 2D array representing the game field: true = alive cell, false = dead cell
    public bool[,] Field { get; set; }

    // The current generation number of the game
    public int Generation { get; set; }
}