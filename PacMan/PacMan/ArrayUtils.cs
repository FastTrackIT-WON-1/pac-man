namespace PacMan
{
    public static class ArrayUtils
    {
        public static bool Contains(int[] array, int value)
        {
            if (array is null || array.Length == 0)
            {
                return false;
            }

            foreach (int element in array)
            {
                if (element == value)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
