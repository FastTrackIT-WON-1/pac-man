using System;

namespace PacMan
{
    public class MapCell
    {
        private static readonly int[] AllowedValues = { 0, -1, int.MaxValue, int.MinValue };

        public MapCell(int value, int row, int col)
        {
            if (!ArrayUtils.Contains(AllowedValues, value))
            {
                throw new ArgumentException($"Value {value} is not allowed!");
            }

            if (row < 0)
            {
                throw new ArgumentException($"Row index {row} must be positive!");
            }

            if (col < 0)
            {
                throw new ArgumentException($"Col index {col} must be positive!");
            }

            this.Value = value;
            this.Row = row;
            this.Col = col;
        }

        public int Value { get; }

        public int Row { get; }

        public int Col { get; }

        public int Weight { get; set; }

        public bool IsOnShortestPath { get; set; }

        public bool IsEmpty
        {
            get
            {
                return this.Value == 0;
            }
        }

        public bool IsWall
        {
            get
            {
                return this.Value == -1;
            }
        }

        public bool IsPacMan
        {
            get
            {
                return this.Value == int.MaxValue;
            }
        }

        public bool IsExit
        {
            get
            {
                return this.Value == int.MinValue;
            }
        }
    }
}
