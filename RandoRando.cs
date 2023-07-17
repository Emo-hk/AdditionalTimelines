namespace AdditionalTimelines
{
    internal static class RandoRando
    {
        public static bool LoreTabletsOn()
        {
            if (!RandomizerMod.RandomizerMod.IsRandoSave)
            {
                return true;
            }
            return RandomizerMod.RandomizerMod.RS.GenerationSettings.PoolSettings.LoreTablets;
        }
        public static bool RancidEggsOn()
        {
            if (!RandomizerMod.RandomizerMod.IsRandoSave)
            {
                return true;
            }
            return RandomizerMod.RandomizerMod.RS.GenerationSettings.PoolSettings.RancidEggs;
        }
        public static bool CharmsOn()
        {
            if (!RandomizerMod.RandomizerMod.IsRandoSave)
            {
                return true;
            }
            return RandomizerMod.RandomizerMod.RS.GenerationSettings.PoolSettings.Charms;
        }
    }
}
