// XDB Patch modification: 2026-06-03
// Purpose: compatibility with ADOFAI r141+ field and method changes.
// Based on modlist-org/Overlayer 3.42.0, licensed under GPL-3.0.

using Overlayer.Tags.Attributes;
using Overlayer.Utils;

namespace Overlayer.Tags;

public static class Tile {
    [Tag]
    public static int LeftTile;
    [Tag]
    public static int CurTile;
    [Tag]
    public static int TotalTile;
    [Tag]
    public static int StartTile;
    [Tag(ProcessingFlags = ValueProcessing.RoundNumber)]
    public static double StartProgress;
    [Tag]
    public static bool IsStarted;

    public static void Started_Reset(scrController controller) => IsStarted = false;

    public static void Start_Init(scrController controller) {
        if(!IsStarted) {
            IsStarted = true;
            if(VersionSafe.IsGameWorld(controller)) {
                StartProgress = VersionSafe.GetPercentComplete(controller) * 100;
                StartTile = VersionSafe.GetCurrentSeqID(controller) + 1;
            }
        }
    }

    [Tag(ProcessingFlags = ValueProcessing.RoundNumber)]
    public static double MarginScale => VersionSafe.GetMarginScale();

    [Tag(ProcessingFlags = ValueProcessing.RoundNumber)]
    public static double TileAngle;

    [Tag(ProcessingFlags = ValueProcessing.RoundNumber)]
    public static double TileEntryAngle;

    [Tag(ProcessingFlags = ValueProcessing.RoundNumber)]
    public static double TileExitAngle;

    const double RAD_TO_DEG = 57.29577951308232;
    public static void Angle_Update(scrFloor floor) {
        TileAngle = floor.angleLength * RAD_TO_DEG;
        TileEntryAngle = floor.entryangle * RAD_TO_DEG;
        TileExitAngle = floor.exitangle * RAD_TO_DEG;
    }

    public static void Reset() {
        LeftTile = CurTile = TotalTile = StartTile = 0;
        StartProgress = 0;
        IsStarted = false;
        TileAngle = TileEntryAngle = TileExitAngle = double.NaN;
    }
}
