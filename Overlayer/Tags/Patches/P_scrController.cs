// XDB Patch modification: 2026-06-03
// Purpose: compatibility with ADOFAI r141+ field and method changes.
// Based on modlist-org/Overlayer 3.42.0, licensed under GPL-3.0.

using Overlayer.Core.Patches;
using System.Collections.Generic;
using Overlayer.Utils;

namespace Overlayer.Tags.Patches;

public class P_scrController : PatchBase<P_scrController> {
    [LazyPatch("Tags.P_scrController.HitTiming__Awake_Rewind", "scrController", "Awake_Rewind", Triggers = new string[] {
        nameof(HitTiming.Timing), nameof(HitTiming.TimingAvg),
    })]
    public static class HitTiming__Awake_Rewind {
        public static void Postfix() {
            HitTiming.Timing = 0;
            HitTiming.Timings = new List<double>();
        }
    }

    [LazyPatch("Tags.P_scrController.ProgressStats__FailAction", "scrController", "FailAction", Triggers = new string[] {
        nameof(ProgressStats.BestProgress)
    })]
    public static class ProgressStats__FailAction {
        public static void Postfix() => ProgressStats.BestProgress_Update();
    }

    [LazyPatch("Tags.P_scrController.Hit__OnDamage_R140", "scrController", "OnDamage", MaxVersion = 140, Triggers = new string[] {
        nameof(Hit.Multipress),
    })]
    [LazyPatch("Tags.P_scrController.Hit__OnDamage_R141", "scrPlayer", "OnDamage", MinVersion = 141, Triggers = new string[] {
        nameof(Hit.Multipress),
    })]
    public static class Hit__OnDamage {
        public static void Postfix(object __instance, bool multipress, bool applyMultipressDamage) {
            if(multipress) {
                if(applyMultipressDamage || VersionSafe.GetConsecMultipressCounter(__instance) > 5) {
                    Hit.Multipress++;
                }
            }
        }
    }

    [LazyPatch("Tags.P_scrController.ProgressStats__OnLandOnPortal", "scrController", "OnLandOnPortal", Triggers = new string[] {
        nameof(ProgressStats.BestProgress)
    })]
    public static class ProgressStats__OnLandOnPortal {
        public static void Postfix() => ProgressStats.BestProgress_Fix();
    }
}
