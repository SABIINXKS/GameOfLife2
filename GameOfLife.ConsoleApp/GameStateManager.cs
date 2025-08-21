using System;
using System.IO;
using System.Text.Json;

public static class GameStateManager
{
    private static readonly string SaveFilePath = "gamestate.json";

    private class SerializableState
    {
        public int Size { get; set; }
        public bool[][] Field { get; set; }
        public int Generation { get; set; }
    }

    /// <summary>
    /// Saves the specified <see cref="GameState"/> to a JSON file.
    /// </summary>
    /// <param name="state">The game state to save.</param>
    public static void SaveToFile(GameState state)
    {
        var serializable = new SerializableState
        {
            Size = state.Size,
            Field = new bool[state.Size][],
            Generation = state.Generation
        };

        for (int i = 0; i < state.Size; i++)
        {
            serializable.Field[i] = new bool[state.Size];
            for (int j = 0; j < state.Size; j++)
                serializable.Field[i][j] = state.Field[i, j];
        }

        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(serializable, options);
        File.WriteAllText(SaveFilePath, json);
    }

    /// <summary>
    /// Loads the game state from the JSON file.
    /// </summary>
    /// <returns>
    /// A <see cref="GameState"/> object if the file exists and is valid; otherwise, <c>null</c>.
    /// </returns>
    public static GameState LoadFromFile()
    {
        if (!File.Exists(SaveFilePath))
            return null;

        string json = File.ReadAllText(SaveFilePath);
        var serializable = JsonSerializer.Deserialize<SerializableState>(json);

        if (serializable == null) return null;

        var state = new GameState
        {
            Size = serializable.Size,
            Field = new bool[serializable.Size, serializable.Size],
            Generation = serializable.Generation
        };

        for (int i = 0; i < serializable.Size; i++)
            for (int j = 0; j < serializable.Size; j++)
                state.Field[i, j] = serializable.Field[i][j];

        return state;
    }
}