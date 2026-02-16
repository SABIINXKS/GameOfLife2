using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

/// <summary>
/// Manages saving and loading of GameState objects to and from files.
/// </summary>
public static class GameStateManager
{
    private static readonly string SaveFilePath = "gamestate.json";
    private static readonly string SaveAllFilePath = "gamestates.json";

    // Internal class for serializing GameState data
    private class SerializableState
    {
        public int Size { get; set; }
        public bool[][] Field { get; set; }
        public int Generation { get; set; }
    }

    /// <summary>
    /// Saves a single GameState to a JSON file.
    /// </summary>
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
    /// Loads a single GameState from a JSON file.
    /// </summary>
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

    /// <summary>
    /// Saves a list of GameState objects to a JSON file.
    /// </summary>
    /// <param name="states">List of GameState objects to save.</param>
    public static void SaveAllToFile(List<GameState> states)
    {
        var serializableList = new List<SerializableState>();
        foreach (var state in states)
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
            serializableList.Add(serializable);
        }
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(serializableList, options);
        File.WriteAllText(SaveAllFilePath, json);
    }

    /// <summary>
    /// Loads all GameState objects from a JSON file.
    /// </summary>
    /// <returns>List of loaded GameState objects.</returns>
    public static List<GameState> LoadAllFromFile()
    {
        if (!File.Exists(SaveAllFilePath))
            return new List<GameState>();

        string json = File.ReadAllText(SaveAllFilePath);
        var serializableList = JsonSerializer.Deserialize<List<SerializableState>>(json);
        var states = new List<GameState>();
        if (serializableList != null)
        {
            foreach (var serializable in serializableList)
            {
                var state = new GameState
                {
                    Size = serializable.Size,
                    Field = new bool[serializable.Size, serializable.Size],
                    Generation = serializable.Generation
                };
                for (int i = 0; i < serializable.Size; i++)
                    for (int j = 0; j < serializable.Size; j++)
                        state.Field[i, j] = serializable.Field[i][j];
                states.Add(state);
            }
        }
        return states;
    }
}