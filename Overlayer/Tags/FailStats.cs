// XDB Patch modification: 2026-06-03
// Purpose: compatibility with ADOFAI r141+ field and method changes.
// Based on modlist-org/Overlayer 3.42.0, licensed under GPL-3.0.

using Overlayer.Tags.Attributes;
using Overlayer.Utils;

namespace Overlayer.Tags;

public static class FailStats
{

    [Tag(ProcessingFlags = ValueProcessing.RoundNumber)]
    public static float OverloadCounter()
    {
        var controller = scrController.instance;
        if (controller == null)
        {
            return float.NaN;
        }

        float raw = VersionSafe.GetFailCounter(controller, "overloadCounter");

        return float.IsNaN(raw)
            ? float.NaN
            : IsImmortal(controller) ? 100f : CalculateFailValue(raw);
    }

    [Tag(ProcessingFlags = ValueProcessing.RoundNumber)]
    public static float MultipressCounter()
    {
        var controller = scrController.instance;
        if (controller == null)
        {
            return float.NaN;
        }

        float raw = VersionSafe.GetFailCounter(controller, "multipressCounter");

        return float.IsNaN(raw)
            ? float.NaN
            : IsImmortal(controller) ? 100f : CalculateFailValue(raw);
    }

    public static bool IsImmortal(scrController controller)
        => ADOBase.isOfficialLevel && VersionSafe.IsGameWorld(controller) && VersionSafe.GetPercentComplete(controller) >= 0.96f;

    public static float CalculateFailValue(float value)
        => value > 1f ? 0f : (1f - value) * 100f;

    [Tag(ProcessingFlags = ValueProcessing.RoundNumber)]
    public static float OverloadCounterRaw => VersionSafe.GetFailCounter(scrController.instance, "overloadCounter");

    [Tag(ProcessingFlags = ValueProcessing.RoundNumber)]
    public static float MultipressCounterRaw => VersionSafe.GetFailCounter(scrController.instance, "multipressCounter");
}
