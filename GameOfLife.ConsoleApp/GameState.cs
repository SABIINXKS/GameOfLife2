using System;
using System.IO;
using System.Text.Json;

// Class that represents the state of the game
public class GameState
{
    // Size of the game field (e.g., 10 means a 10x10 grid)
    public int Size { get; set; }

    // 2D array representing the game field: true = alive cell, false = dead cell
    public bool[,] Field { get; set; }

    // The current generation number of the game
    public int Generation { get; set; }

    // File path where the game state will be saved
    private static readonly string SaveFilePath = "gamestate.json";

    // Helper class to convert the 2D array into a serializable format
    private class SerializableState
    {
        public int Size { get; set; }
        public bool[][] Field { get; set; } // Jagged array used for JSON serialization
        public int Generation { get; set; }
    }

    // Method to save the current game state to a JSON file
    public void SaveToFile()
    {
        // Create an instance of SerializableState to hold the serializable data
        var serializable = new SerializableState
        {
            Size = Size,
            Field = new bool[Size][],
            Generation = Generation
        };

        // Convert the 2D array (bool[,]) to a jagged array (bool[][])
        for (int i = 0; i < Size; i++)
        {
            serializable.Field[i] = new bool[Size];
            for (int j = 0; j < Size; j++)
                serializable.Field[i][j] = Field[i, j];
        }

        // Optional: format the JSON output with indentation for readability
        var options = new JsonSerializerOptions { WriteIndented = true };

        // Serialize the SerializableState object to JSON
        string json = JsonSerializer.Serialize(serializable, options);

        // Write the JSON string to the specified file
        File.WriteAllText(SaveFilePath, json);
    }

    // Method to load the game state from a JSON file
    public static GameState LoadFromFile()
    {
        // Check if the save file exists; return null if not found
        if (!File.Exists(SaveFilePath))
            return null;

        // Read the JSON string from the file
        string json = File.ReadAllText(SaveFilePath);

        // Deserialize the JSON string back into a SerializableState object
        var serializable = JsonSerializer.Deserialize<SerializableState>(json);

        // If deserialization failed, return null
        if (serializable == null) return null;

        // Create a new GameState instance using the data from SerializableState
        var state = new GameState
        {
            Size = serializable.Size,
            Field = new bool[serializable.Size, serializable.Size],
            Generation = serializable.Generation
        };

        // Convert the jagged array (bool[][]) back to a 2D array (bool[,])
        for (int i = 0; i < serializable.Size; i++)
            for (int j = 0; j < serializable.Size; j++)
                state.Field[i, j] = serializable.Field[i][j];

        // Return the restored game state
        return state;
    }
}
