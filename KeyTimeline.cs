using Modding;
using System.Collections.Generic;
using System.Linq;
using FStats;

namespace AdditionalTimelines
{
    public class KeyTimeline : StatController
    {
        internal static readonly Dictionary<string, string> BoolNames = new()
        {
            [nameof(PlayerData.hasDreamGate)] = "Dreamgate",
            [nameof(PlayerData.dreamNailUpgraded)] = "Awoken Dream Nail",
            [nameof(PlayerData.hasCyclone)] = "Cyclone Slash",
            [nameof(PlayerData.hasDashSlash)] = "Great Slash",
            [nameof(PlayerData.hasUpwardSlash)] = "Dash Slash",
            [nameof(PlayerData.lurienDefeated)] = "Lurien",
            [nameof(PlayerData.monomonDefeated)] = "Monomon",
            [nameof(PlayerData.hegemolDefeated)] = "Herrah",
            [nameof(PlayerData.royalCharmState) + "/1"] = "White Fragment",
            [nameof(PlayerData.royalCharmState) + "/2"] = "White Fragment",
            [nameof(PlayerData.royalCharmState) + "/3"] = "Kingsoul",
            [nameof(PlayerData.gotShadeCharm)] = "Void Heart",
            [nameof(PlayerData.hasSlykey)] = "Shopkeeper's Key",
            [nameof(PlayerData.hasWhiteKey)] = "Elegant Key",
            [nameof(PlayerData.hasLoveKey)] = "Love Key",
            [nameof(PlayerData.hasTramPass)] = "Tram Pass",
            [nameof(ItemChanger.Modules.ElevatorPass.hasElevatorPass)] = "Elevator Pass",
            [nameof(PlayerData.hasLantern)] = "Lumafly Lantern",
            [nameof(PlayerData.hasKingsBrand)] = "King's Brand",
            [nameof(PlayerData.hasCityKey)] = "City Crest",
            [nameof(PlayerData.gotCharm_40)] = "Grimmchild",
            [nameof(PlayerData.gotCharm_17)] = "Spore Shroom",
            [nameof(PlayerData.gotCharm_10)] = "Defender's Crest",
            [nameof(PlayerData.gotCharm_23)] = "Fragile Heart",
            [nameof(PlayerData.gotCharm_24)] = "Fragile Greed",
            [nameof(PlayerData.gotCharm_25)] = "Fragile Strength",
            ["Dreamer"] = "Dreamer",
        };

        public Dictionary<string, float> KeyObtainTimeline = new();

        public override void Initialize()
        {
            ModHooks.SetPlayerIntHook += RecordPlayerDataInt;
            ModHooks.SetPlayerBoolHook += RecordPlayerDataBool;
        }
        public override void Unload()
        {
            ModHooks.SetPlayerIntHook -= RecordPlayerDataInt;
            ModHooks.SetPlayerBoolHook -= RecordPlayerDataBool;
        }
        private int RecordPlayerDataInt(string name, int orig)
        {
            if (name == "guardiansDefeated" && !KeyObtainTimeline.ContainsKey("Dreamer") && orig - BoolToInt(KeyObtainTimeline.ContainsKey("lurienDefeated")) - BoolToInt(KeyObtainTimeline.ContainsKey("monomonDefeated")) - BoolToInt(KeyObtainTimeline.ContainsKey("hegemolDefeated")) > 0)
            {
                KeyObtainTimeline["Dreamer"] = FStats.StatControllers.Common.Instance.CountedTime;
                return orig;
            }
            Record($"{name}/{orig}");
            return orig;
        }
        private bool RecordPlayerDataBool(string name, bool orig)
        {
            if (orig) Record(name);
            return orig;
        }

        private int BoolToInt(bool b)
        {
            return b ? 1 : 0;
        }

        private void Record(string s)
        {
            if (BoolNames.ContainsKey(s) && !KeyObtainTimeline.ContainsKey(s))
            {
                KeyObtainTimeline[s] = FStats.StatControllers.Common.Instance.CountedTime;
            }
        }
        public override IEnumerable<DisplayInfo> GetDisplayInfos() {
            List<string> Lines = BoolNames
                .Where(kvp => KeyObtainTimeline.ContainsKey(kvp.Key))
                .Where(kvp => IsIncluded(kvp.Key))
                .OrderBy(kvp => KeyObtainTimeline[kvp.Key])
                .Select(kvp => $"{kvp.Value}: {KeyObtainTimeline[kvp.Key].PlaytimeHHMMSS()}")
                .ToList();

            List<string> Columns;

            if (Lines.Count <= 10)
            {
                Columns = new() { string.Join("\n", Lines) };
            }
            else if (Lines.Count <= 20)
            {
                Columns = new()
                {
                    string.Join("\n", Lines.Slice(0, 2)),
                    string.Join("\n", Lines.Slice(1, 2)),
                };
            }
            else
            {
                Columns = new()
                {
                    string.Join("\n", Lines.Slice(0, 3)),
                    string.Join("\n", Lines.Slice(1, 3)),
                    string.Join("\n", Lines.Slice(2, 3)),
                };
            }

            yield return new()
            {
                Title = "Key Timeline",
                MainStat = FStats.StatControllers.Common.Instance.TotalTimeString,
                StatColumns = Columns,
                Priority = -49_000,
            };
        }
        private bool IsIncluded(string keyName)
        {
            switch (keyName)
            {
                case nameof(PlayerData.gotCharm_17): // spore shroom
                    if (AdditionalTimelines.GS.ShowSporeShroom)
                    {
                        bool temp = false;
                        if (ModHooks.GetMod("RandoPlus", true) is Mod)
                        {
                            temp |= MrMushroom.IsEnabled();
                        }
                        if (ModHooks.GetMod("Randomizer 4", true) is Mod)
                        {
                            temp |= RandoRando.LoreTabletsOn();
                        }
                        return temp;
                    }
                    return false;
                case nameof(PlayerData.gotCharm_10): // defender's crest
                    if (AdditionalTimelines.GS.ShowDefendersCrest)
                    {
                        if (ModHooks.GetMod("Randomizer 4", true) is Mod)
                        {
                            return RandoRando.RancidEggsOn();
                        }
                    }
                    return false;
                case nameof(PlayerData.gotCharm_23): // fragile charms
                case nameof(PlayerData.gotCharm_24):
                case nameof(PlayerData.gotCharm_25):
                    if (AdditionalTimelines.GS.ShowFragileCharms)
                    {
                        if (ModHooks.GetMod("Randomizer 4", true) is Mod)
                        {
                            return RandoRando.CharmsOn();
                        }
                    }
                    return false;
            }

            return true;
        }
    }
}
