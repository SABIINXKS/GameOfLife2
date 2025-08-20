using System;

namespace LifeEngineLib
{
    public class LifeEngine
    {
        public int Size { get; }
        public bool[,] Field { get; set; }

        /// <summary>
        /// Initializes a new instance of the LifeEngine class with the specified field size.
        /// </summary>
        /// <param name="size">The size of the square game field.</param>
        public LifeEngine(int size)
        {
            Size = size;
            Field = new bool[size, size];
        }

        /// <summary>
        /// Randomly initializes the game field with living and dead cells.
        /// </summary>
        public void InitializeField()
        {
            Random random = new Random();
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    Field[i, j] = random.Next(100) < 30;
        }

        public void NextGeneration()
        {
            bool[,] next = new bool[Size, Size];
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                {
                    int neighbors = CountLiveNeighbors(i, j);
                    if (Field[i, j])
                        next[i, j] = neighbors == 2 || neighbors == 3;
                    else
                        next[i, j] = neighbors == 3;
                }
            Field = next;
        }

        public int GetLivingCellsCount()
        {
            int count = 0;
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    if (Field[i, j]) count++;
            return count;
        }

        // Count the number of live neighbors for a cell at (x, y)
        private int CountLiveNeighbors(int x, int y)
        {
            int count = 0;
            for (int dx = -1; dx <= 1; dx++)
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    int nx = x + dx, ny = y + dy;
                    if (nx >= 0 && nx < Size && ny >= 0 && ny < Size)
                        if (Field[nx, ny]) count++;
                }
            return count;

        }
    }
}