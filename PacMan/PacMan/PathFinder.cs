using System;
using System.Collections.Generic;

namespace PacMan
{
    public static class PathFinder
    {
        public static MapCell[] FindShortestPath(MapCell[,] map)
        {
            MapCell exitCell = FindExit(map) ?? throw new ArgumentException("Map doesn't contain an exit!");
            MapCell pacManCell = FindPacMan(map) ?? throw new ArgumentException("Map doesn't contain PacMan!");

            CalculateWeights(map, exitCell);

            // if debugging this may help seeing the calculated weights:
            // MapUtils.DisplayWeightsMap(map);

            MapCell[] shortestPath = VisitCellsOnShortestPath(map, pacManCell);

            return shortestPath;
        }

        private static MapCell FindPacMan(MapCell[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i,j].IsPacMan)
                    {
                        return map[i, j];
                    }
                }
            }

            return null;
        }

        private static MapCell FindExit(MapCell[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j].IsExit)
                    {
                        return map[i, j];
                    }
                }
            }

            return null;
        }

        private static void CalculateWeights(MapCell[,] map, MapCell currentCell)
        {
            Queue<MapCell> queue = new Queue<MapCell>();
            queue.Enqueue(currentCell);

            while (queue.Count > 0)
            {
                currentCell = queue.Dequeue();
                if (currentCell.IsPacMan)
                {
                    return;
                }

                MapCell[] neighbours = GetNonVisitedNeighbours(currentCell, map);
                foreach (MapCell n in neighbours)
                {
                    n.Weight = currentCell.Weight + 1;
                    queue.Enqueue(n);
                }
            }
        }

        private static MapCell[] VisitCellsOnShortestPath(MapCell[,] map, MapCell pacManCell)
        {
            int count = -1;
            MapCell currentCell = pacManCell;
            List<MapCell> exitPath = new List<MapCell>();
            int maxStepsCount = (map.GetLength(0) + 1) * (map.GetLength(1) + 1);
            while (!currentCell.IsExit)
            {
                count++;
                currentCell.IsOnShortestPath = true;
                exitPath.Add(currentCell);

                MapCell nextCell = GetMinWeightVisitedNeighbour(currentCell, map);

                if (nextCell == null)
                {
                    throw new Exception("Deadlocked: cannot find next move");
                }

                if (count >= maxStepsCount)
                {
                    throw new Exception("Deadlocked: exceeded max steps count");
                }

                currentCell = nextCell;
            }

            return exitPath.ToArray();
        }

        private static MapCell[] GetNonVisitedNeighbours(MapCell currentCell, MapCell[,] map)
        {
            List<MapCell> neighbours = new List<MapCell>();

            MapCell topNeighbour = GetNeighbour(currentCell, map, Direction.Up);
            if ((topNeighbour != null) &&
                (topNeighbour.IsEmpty || topNeighbour.IsPacMan) &&
                (topNeighbour.Weight == 0))
            {
                neighbours.Add(topNeighbour);
            }

            MapCell bottomNeighbour = GetNeighbour(currentCell, map, Direction.Down);
            if ((bottomNeighbour != null) &&
                (bottomNeighbour.IsEmpty || bottomNeighbour.IsPacMan) &&
                (bottomNeighbour.Weight == 0))
            {
                neighbours.Add(bottomNeighbour);
            }

            MapCell leftNeighbour = GetNeighbour(currentCell, map, Direction.Left);
            if ((leftNeighbour != null) &&
                (leftNeighbour.IsEmpty || leftNeighbour.IsPacMan) &&
                (leftNeighbour.Weight == 0))
            {
                neighbours.Add(leftNeighbour);
            }

            MapCell rightNeighbour = GetNeighbour(currentCell, map, Direction.Right);
            if ((rightNeighbour != null) &&
                (rightNeighbour.IsEmpty || rightNeighbour.IsPacMan) &&
                (rightNeighbour.Weight == 0))
            {
                neighbours.Add(rightNeighbour);
            }

            return neighbours.ToArray();
        }

        private static MapCell GetMinWeightVisitedNeighbour(MapCell currentCell, MapCell[,] map)
        {
            MapCell minWeightNeighbour = null;

            MapCell topNeighbour = GetNeighbour(currentCell, map, Direction.Up);
            if ((topNeighbour != null) &&
                (topNeighbour.IsEmpty || topNeighbour.IsPacMan || topNeighbour.IsExit) &&
                (topNeighbour.Weight > 0 || topNeighbour.IsExit))
            {
                if ((minWeightNeighbour == null) || (minWeightNeighbour.Weight > topNeighbour.Weight))
                {
                    minWeightNeighbour = topNeighbour;
                }
            }

            MapCell bottomNeighbour = GetNeighbour(currentCell, map, Direction.Down);
            if ((bottomNeighbour != null) &&
                (bottomNeighbour.IsEmpty || bottomNeighbour.IsPacMan || bottomNeighbour.IsExit) &&
                (bottomNeighbour.Weight > 0 || bottomNeighbour.IsExit))
            {
                if ((minWeightNeighbour == null) || (minWeightNeighbour.Weight > bottomNeighbour.Weight))
                {
                    minWeightNeighbour = bottomNeighbour;
                }
            }

            MapCell leftNeighbour = GetNeighbour(currentCell, map, Direction.Left);
            if ((leftNeighbour != null) &&
                (leftNeighbour.IsEmpty || leftNeighbour.IsPacMan || leftNeighbour.IsExit) &&
                (leftNeighbour.Weight > 0 || leftNeighbour.IsExit))
            {
                if ((minWeightNeighbour == null) || (minWeightNeighbour.Weight > leftNeighbour.Weight))
                {
                    minWeightNeighbour = leftNeighbour;
                }
            }

            MapCell rightNeighbour = GetNeighbour(currentCell, map, Direction.Right);
            if ((rightNeighbour != null) &&
                (rightNeighbour.IsEmpty || rightNeighbour.IsPacMan || rightNeighbour.IsExit) &&
                (rightNeighbour.Weight > 0 || rightNeighbour.IsExit))
            {
                if ((minWeightNeighbour == null) || (minWeightNeighbour.Weight > rightNeighbour.Weight))
                {
                    minWeightNeighbour = rightNeighbour;
                }
            }

            return minWeightNeighbour;
        }

        private static MapCell GetNeighbour(MapCell currentCell, MapCell[,] map, Direction direction)
        {
            if (currentCell is null)
            {
                return null;
            }

            switch (direction)
            {
                case Direction.Up:
                    bool canGoUp = currentCell.Row - 1 >= 0;
                    return canGoUp 
                            ? map[currentCell.Row - 1, currentCell.Col]
                            : null;

                case Direction.Down:
                    bool canGoDown = currentCell.Row + 1 < map.GetLength(0);
                    return canGoDown
                            ? map[currentCell.Row + 1, currentCell.Col]
                            : null;

                case Direction.Left:
                    bool canGoLeft = currentCell.Col - 1 >= 0;
                    return canGoLeft
                            ? map[currentCell.Row, currentCell.Col - 1]
                            : null;

                case Direction.Right:
                    bool canGoRight = currentCell.Col + 1 < map.GetLength(1);
                    return canGoRight
                            ? map[currentCell.Row, currentCell.Col + 1]
                            : null;

                default:
                    return null;
            }
        }
    }
}
