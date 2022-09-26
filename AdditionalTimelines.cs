using Modding;
using FStats;

namespace AdditionalTimelines
{
    public class AdditionalTimelines : Mod, IGlobalSettings<GlobalSettings>
    {
        public static GlobalSettings GS = new();
        public GlobalSettings OnSaveGlobal() => GS;
        public void OnLoadGlobal(GlobalSettings gs) => GS = gs;

        public override string GetVersion() => "1.0.1";
        public override void Initialize()
        {
            Log("Initializing Mod...");
            API.OnGenerateFile += gen => gen(new KeyTimeline());
            API.OnGenerateFile += gen => gen(new StagTimeline());
        }
    }
}