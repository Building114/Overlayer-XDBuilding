// XDB Patch modification: 2026-06-03
// Purpose: compatibility with ADOFAI r141+ field and method changes.
// Based on modlist-org/Overlayer 3.42.0, licensed under GPL-3.0.

using Overlayer.Tags.Attributes;
using Overlayer.Utils;

namespace Overlayer.Tags;

public static class Bpm {
    [Tag(ProcessingFlags = ValueProcessing.RoundNumber)]
    public static double TileBpm;
    [Tag(ProcessingFlags = ValueProcessing.RoundNumber)]
    public static double CurBpm;
    [Tag(ProcessingFlags = ValueProcessing.RoundNumber)]
    public static double RecKPS;
    [Tag(ProcessingFlags = ValueProcessing.RoundNumber)]
    public static double TileBpmWithoutPitch;
    [Tag(ProcessingFlags = ValueProcessing.RoundNumber)]
    public static double CurBpmWithoutPitch;
    [Tag(ProcessingFlags = ValueProcessing.RoundNumber)]
    public static double RecKPSWithoutPitch;

    public static float bpm, pitch, bpmwithoutpitch, playbackSpeed = 1;

    public static void Init(scrController __instance) {
        if(scnGame.instance == null && scnEditor.instance == null && !VersionSafe.IsGameWorld()) {
            return;
        }

        if(scnGame.instance != null) {
            pitch = (float)scnGame.instance.levelData.pitch / 100;
            if(ADOBase.isOfficialLevel) {
                pitch *= scrConductor.instance.song.pitch;
            }
            if(ADOBase.isCLSLevel) {
                pitch *= GCS.currentSpeedTrial;
            }
            if(scnEditor.instance != null) {
                pitch *= scnEditor.instance.playbackSpeed;
            }
            bpm = scnGame.instance.levelData.bpm * pitch;
            bpmwithoutpitch = scnGame.instance.levelData.bpm;
        } else {
            pitch = scrConductor.instance.song.pitch;
            bpm = scrConductor.instance.bpm * pitch;
            bpmwithoutpitch = scrConductor.instance.bpm;
        }
        float cur = bpm;
        if(VersionSafe.GetCurrentSeqID(__instance) != 0) {
            double speed = VersionSafe.GetPlanetSpeed(scrController.instance);
            cur = (float)(bpm * speed);
        }
        TileBpm = cur;
        CurBpm = cur;
        RecKPS = cur / 60;
    }

    public static double GetRealBpm(scrFloor floor, float bpm) {
        return floor == null
            ? (double)bpm
            : floor.nextfloor == null ? VersionSafe.GetPlanetSpeed(scrController.instance) * bpm : 60.0 / (floor.nextfloor.entryTime - floor.entryTime);
    }

    public static void Update(scrFloor floor) {
        if(floor.nextfloor == null) {
            return;
        }

        double curBPM = GetRealBpm(floor, bpm) * pitch;

        TileBpm = bpm * VersionSafe.GetPlanetSpeed(scrController.instance);
        CurBpm = curBPM;
        RecKPS = curBPM / 60;

        double curBPMWithoutPitch = GetRealBpm(floor, bpmwithoutpitch);
        TileBpmWithoutPitch = bpmwithoutpitch * VersionSafe.GetPlanetSpeed(scrController.instance);
        CurBpmWithoutPitch = curBPMWithoutPitch;
        RecKPSWithoutPitch = curBPMWithoutPitch / 60;
    }

    public static void Reset() {
        TileBpm = CurBpm = RecKPS = 0;
        TileBpmWithoutPitch = CurBpmWithoutPitch = RecKPSWithoutPitch = 0;
    }
}
