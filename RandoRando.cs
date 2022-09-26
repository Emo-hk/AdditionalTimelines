namespace AdditionalTimelines
{
    internal static class RandoRando
    {
        public static bool LoreTabletsOn()
        {
            return RandomizerMod.RandomizerMod.RS.GenerationSettings.PoolSettings.LoreTablets;
        }
        public static bool RancidEggsOn()
        {
            return RandomizerMod.RandomizerMod.RS.GenerationSettings.PoolSettings.RancidEggs;
        }
        public static bool CharmsOn()
        {
            return RandomizerMod.RandomizerMod.RS.GenerationSettings.PoolSettings.Charms;
        }
    }
}
