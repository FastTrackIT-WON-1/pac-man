using System;
using System.IO;

namespace PacMan
{
    public static class MapUtils
    {
        public static MapCell[,] ReadMap(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (!File.Exists(path))
            {
                throw new ArgumentException($"File '{path}' doesn't exist");
            }

            string[] lines = File.ReadAllLines(path);

            if (lines is null || lines.Length == 0)
            {
                throw new FormatException($"File '{path}' must have at least 1 row.");
            }


            if (!int.TryParse(lines[0], out int size))
            {
                throw new FormatException($"File '{path}' must have on its 1 line the matrix size. Cannot convert {lines[0]} to an integer value.");
            }

            if (lines.Length != size + 1)
            {
                throw new FormatException($"File '{path}' must have on {size + 1} lines.");
            }

            MapCell[,] map = new MapCell[size, size];
            for (int i = 1; i <= size; i++)
            {
                string line = lines[i];
                string[] parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != size)
                {
                    throw new FormatException($"File '{path}', line {i + 1} must have {size} cells.");
                }

                for (int j = 0; j < size; j++)
                {
                    if (!int.TryParse(parts[j], out int cellValue))
                    {
                        throw new FormatException($"File '{path}', line {i + 1}, cell {j + 1} must have a valid numeric value. Couldn't convert '{parts[j]}' to integer.");
                    }

                    MapCell cell = new MapCell(cellValue, i - 1, j);
                    map[i - 1, j] = cell;
                }
            }

            return map;
        }

        public static void DisplayMap(MapCell[,] map)
        {
            int rowsCount = map.GetLength(0);
            int colsCount = map.GetLength(1);

            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine();

            for (int i = 0; i < rowsCount; i++)
            {
                for (int j = 0; j < colsCount; j++)
                {
                    if (map[i, j].IsWall)
                    {
                        PrintWall();
                    }
                    else if (map[i, j].IsPacMan)
                    {
                        PrintPacMan();
                    }
                    else if (map[i, j].IsExit)
                    {
                        PrintExit();
                    }
                    else if (map[i, j].IsEmpty)
                    {
                        PrintEmptyCell(map[i, j].IsOnShortestPath);
                    }
                }

                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();
            }
        }

        public static void DisplayWeightsMap(MapCell[,] map)
        {
            int rowsCount = map.GetLength(0);
            int colsCount = map.GetLength(1);

            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine();
            int maxWeightDigitsNo = FindMaxWeight(map).ToString().Length + 1;

            for (int i = 0; i < rowsCount; i++)
            {
                for (int j = 0; j < colsCount; j++)
                {
                    if (map[i, j].IsWall)
                    {
                        PrintWall(maxWeightDigitsNo);
                    }
                    else if (map[i, j].IsPacMan)
                    {
                        PrintPacMan(maxWeightDigitsNo);
                    }
                    else if (map[i, j].IsExit)
                    {
                        PrintExit(maxWeightDigitsNo);
                    }
                    else if (map[i, j].IsEmpty)
                    {
                        PrintEmptyCell(map[i, j].IsOnShortestPath, maxWeightDigitsNo, map[i, j].Weight);
                    }
                }

                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();
            }
        }

        private static int FindMaxWeight(MapCell[,] map)
        {
            int rowsCount = map.GetLength(0);
            int colsCount = map.GetLength(1);
            int maxWeight = 0;

            for (int i = 0; i < rowsCount; i++)
            {
                for (int j = 0; j < colsCount; j++)
                {
                    if (map[i, j].Weight > maxWeight)
                    {
                        maxWeight = map[i, j].Weight;
                    }
                }
            }

            return maxWeight;
        }

        private static void PrintWall(int noOfChars = 1)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write(new string('#', noOfChars));
            Console.BackgroundColor = ConsoleColor.Black;
        }

        private static void PrintPacMan(int noOfChars = 1)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(new string('*', noOfChars));
            Console.BackgroundColor = ConsoleColor.Black;
        }

        private static void PrintExit(int noOfChars = 1)
        {
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write(new string(' ', noOfChars));
            Console.BackgroundColor = ConsoleColor.Black;
        }

        private static void PrintEmptyCell(bool isOnShortestPath = false, int noOfChars = 1, int weight = 0)
        {
            if (isOnShortestPath)
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Black;
            }

            if (weight > 0)
            {
                Console.Write(weight.ToString().PadLeft(noOfChars, ' '));
            }
            else
            {
                Console.Write(new string(' ', noOfChars));
            }

            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
