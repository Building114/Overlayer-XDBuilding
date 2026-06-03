// XDB Patch modification: 2026-06-03
// Purpose: compatibility with ADOFAI r141+ field and method changes.
// Based on modlist-org/Overlayer 3.42.0, licensed under GPL-3.0.

using Overlayer.Tags.Attributes;
using Overlayer.Utils;

namespace Overlayer.Tags;

public static class Status {
    [Tag]
    public static bool IsAutoEnabled => ADOFAI.RDC?.auto ?? false;
    [Tag]
    public static bool IsOldAutoEnabled => ADOFAI.RDC?.useOldAuto ?? false;
    [Tag]
    public static bool IsPracticeModeEnabled => ADOFAI.RDC?.practice ?? false;
    [Tag]
    public static bool IsNoFailEnabled => VersionSafe.IsNoFail(ADOFAI.Controller);
    [Tag]
    public static bool IsSpeedTrialEnabled => GCS.speedTrialMode;
    [Tag(NotPlaying = true)]
    public static int Deaths => VersionSafe.GetDeaths();
    [Tag]
    public static int PlayerCount => VersionSafe.GetPlayerCount();
    [Tag]
    public static int MaxPlayerCount => VersionSafe.GetMaxPlayerCount();
    [Tag]
    public static bool IsCoopModeEnabled => VersionSafe.IsCoopMode();
    [Tag]
    public static int Attempts;

    public static void Attempts_Update() {
        if(scnGame.instance == null) {
            Attempts = scrController.instance != null && scrConductor.instance != null
                ? ADOBase.sceneName.Contains("-") && !VersionSafe.IsNoFail(scrController.instance) && VersionSafe.IsGameWorld(scrController.instance)
                    ? Persistence.GetWorldAttempts(VersionSafe.GetCurrentWorld())
                    : 0
                : 0;
        } else {
            if(scnEditor.instance == null) {
                var level = ADOFAI.LevelData;
                Attempts = level != null
                    ? Persistence.GetCustomWorldAttempts(
                        MD5Hash.GetHash(level.author + level.artist + level.song)
                    )
                    : 0;
            } else {
                Attempts = 0;
            }
        }
    }

    public static void Attempts_UpdateGame() {
        var level = ADOFAI.LevelData;
        Attempts = Persistence.GetCustomWorldAttempts(
            MD5Hash.GetHash(level.author + level.artist + level.song)
        );
    }

    public static void Attempts_UpdateOfficial() {
        Attempts = ADOBase.sceneName.Contains("-") && !VersionSafe.IsNoFail(scrController.instance) && VersionSafe.IsGameWorld(scrController.instance)
            ? Persistence.GetWorldAttempts(VersionSafe.GetCurrentWorld())
            : 0;
    }

    [Tag(ProcessingFlags = ValueProcessing.RoundNumber)]
    public static double Pitch => GCS.currentSpeedTrial;
    [Tag(ProcessingFlags = ValueProcessing.RoundNumber)]
    public static double EditorPitch => (ADOFAI.LevelData?.pitch ?? 0) / 100.0;
}
