namespace WADCommon
{
    public static class SequentialIDGenerator
    {
        private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static int[] indexes = new int[4];
        private static char[] id = new char[4];

        public static string GenerateNextID()
        {
            for (int i = 0; i < 4; i++)
            {
                id[i] = Characters[indexes[i]];
            }
            IncrementIndexes();
            return new string(id);
        }

        private static void IncrementIndexes()
        {
            for (int i = indexes.Length - 1; i >= 0; i--)
            {
                indexes[i]++;
                if (indexes[i] < Characters.Length)
                {
                    break;
                }
                indexes[i] = 0;
            }
        }
    }
}