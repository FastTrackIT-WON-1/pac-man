using System;
using System.IO;

namespace PacMan
{
    class Program
    {
        static void Main(string[] args)
        {
            string mapPath = Path.Combine(Environment.CurrentDirectory, @"../../../Map.txt");
            MapCell[,] map = MapUtils.ReadMap(mapPath);

            MapUtils.DisplayMap(map);
            PathFinder.FindShortestPath(map);
            MapUtils.DisplayMap(map);
        }
    }
}
