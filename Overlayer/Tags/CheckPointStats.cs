// XDB Patch modification: 2026-06-03
// Purpose: compatibility with ADOFAI r141+ field and method changes.
// Based on modlist-org/Overlayer 3.42.0, licensed under GPL-3.0.

using Overlayer.Tags.Attributes;
using System.Collections;
using System.Collections.Generic;
using Overlayer.Utils;

namespace Overlayer.Tags;

public static class CheckPointStats {
    [Tag]
    public static int CheckPointUsed => VersionSafe.GetCheckpointsUsed();
    [Tag]
    public static int CurCheckPoint;
    [Tag]
    public static int TotalCheckPoints;

    public static void TotalCheckPoients_Update() {
        // 使用 VersionSafe.GetLevelFloors() 兼容新旧版 scrLevelMaker 字段变化
        IList floors = VersionSafe.GetLevelFloors();
        if (floors == null) { TotalCheckPoints = 0; return; }
        int count = 0;
        foreach (object f in floors) {
            if (f is scrFloor sf && sf.GetComponent<ffxCheckpoint>() != null)
                count++;
        }
        TotalCheckPoints = count;
    }

    public static List<scrFloor> AllCheckPoints;

    public static void AllCheckPoints_Set() {
        IList floors = VersionSafe.GetLevelFloors();
        AllCheckPoints = new List<scrFloor>();
        if (floors == null) return;
        foreach (object f in floors) {
            if (f is scrFloor sf && sf.GetComponent<ffxCheckpoint>() != null)
                AllCheckPoints.Add(sf);
        }
    }

    public static void InterCheckPoints_Update() {
        AllCheckPoints_Set();
        TotalCheckPoints = AllCheckPoints.Count;
    }

    public static int GetCheckPointIndex(scrFloor floor) {
        if (floor == null || AllCheckPoints == null) return 0;
        int i = 0;
        foreach (scrFloor chkPt in AllCheckPoints) {
            if (floor.seqID + 1 <= chkPt.seqID) return i;
            i++;
        }
        return i;
    }

    public static void Reset() => CurCheckPoint = TotalCheckPoints = 0;
}
