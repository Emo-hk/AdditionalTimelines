using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using FStats;

namespace AdditionalTimelines
{
    public class StagTimeline : StatController
    {
        internal static readonly Dictionary<string, string> BoolNames = new()
        {
            [nameof(PlayerData.openedTown)] = "Dirtmouth",
            [nameof(PlayerData.openedCrossroads)] = "Forgotten Crossroads",
            [nameof(PlayerData.openedGreenpath)] = "Greenpath",
            [nameof(PlayerData.openedFungalWastes)] = "Queen's Station",
            [nameof(PlayerData.openedRoyalGardens)] = "Queen's Gardens",
            [nameof(PlayerData.openedRuins1)] = "City Storerooms",
            [nameof(PlayerData.openedRuins2)] = "King's Station",
            [nameof(PlayerData.openedRestingGrounds)] = "Resting Grounds",
            [nameof(PlayerData.openedDeepnest)] = "Distant Village",
            [nameof(PlayerData.openedHiddenStation)] = "Hidden Station",
            [nameof(PlayerData.openedStagNest)] = "Stag Nest",
        };

        public Dictionary<string, float> StagObtainTimeline = new();

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
            Record($"{name}/{orig}");
            return orig;
        }
        private bool RecordPlayerDataBool(string name, bool orig)
        {
            if (orig) Record(name);
            return orig;
        }

        private void Record(string s)
        {
            if (BoolNames.ContainsKey(s) && !StagObtainTimeline.ContainsKey(s))
            {
                StagObtainTimeline[s] = FStats.StatControllers.Common.Instance.CountedTime;
            }
        }

        public override IEnumerable<DisplayInfo> GetDisplayInfos() {
            List<string> Lines = BoolNames
                .Where(kvp => StagObtainTimeline.ContainsKey(kvp.Key))
                .OrderBy(kvp => StagObtainTimeline[kvp.Key])
                .Select(kvp => $"{kvp.Value}: {StagObtainTimeline[kvp.Key].PlaytimeHHMMSS()}")
                .ToList();

            yield return new()
            {
                Title = "Stag Timeline",
                MainStat = FStats.StatControllers.Common.Instance.TotalTimeString,
                StatColumns = new() { string.Join("\n", Lines) },
                Priority = -48_000,
            };
        }
    }
}
