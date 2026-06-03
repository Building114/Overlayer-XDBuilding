// XDB Patch modification: 2026-06-03
// Purpose: compatibility with ADOFAI r141+ field and method changes.
// Based on modlist-org/Overlayer 3.42.0, licensed under GPL-3.0.

using Overlayer.Tags.Attributes;
using System;
using UnityEngine;
using Overlayer.Utils;

namespace Overlayer.Tags;

public static class ProgressStats {
    [Tag(ProcessingFlags = ValueProcessing.RoundNumber)]
    public static double Progress => VersionSafe.GetPercentComplete() * 100;
    [Tag(ProcessingFlags = ValueProcessing.RoundNumber)]
    public static double ActualProgress() {
        var listFloors = VersionSafe.GetLevelFloors();
        var currentFloor = VersionSafe.GetCurrentFloor();
        if(listFloors == null || listFloors.Count < 2 || currentFloor == null) {
            return 0;
        }

        if(listFloors[1] is not scrFloor firstFloor || listFloors[listFloors.Count - 1] is not scrFloor lastFloor) {
            return 0;
        }

        var totalTime = lastFloor.entryTime - firstFloor.entryTime;
        if(totalTime <= 0) {
            return 0;
        }

        var actualProgress = (currentFloor.entryTime - firstFloor.entryTime) / totalTime * 100;
        return (double)Mathf.Clamp((float)actualProgress, 0, 100);
    }
    [Tag]
    public static double BestProgress;

    public static void BestProgress_Reset() => BestProgress = 0;

    public static void BestProgress_Update() {
        if(scrLevelMaker.instance == null) {
            return;
        }
        BestProgress = Math.Max(BestProgress, VersionSafe.GetPercentComplete() * 100);
    }

    public static void BestProgress_Fix() {
        if(VersionSafe.IsGameWorld()) {
            BestProgress = 100;
        }
    }
}
