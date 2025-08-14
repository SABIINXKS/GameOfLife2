[Serializable]
public class GameState
{
    public int Size { get; set; }
    public bool[,] Field { get; set; }
    public int GenerationCount { get; set; }
}
// data contains the state of the game