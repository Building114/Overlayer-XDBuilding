using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using static UnityModManagerNet.UnityModManager;

namespace Overlayer.Utils;

public static class AutoUpdater {
    public enum VersionType {
        Unknown,
        Old,
        Stable,
        Beta,
        OldBeta,
        UnknownBeta,
    }

    public const bool Disabled = true;
    private const string DisabledReason = "AutoUpdater is disabled in this fork.";

    public static bool IsLatest { get; private set; } = true;
    public static bool IsBeta { get; private set; } = false;
    public static string LatestUrl { get; private set; }
    public static string BetaUrl { get; private set; }
    public static Version LatestVersion;
    public static Version BetaVersion;
    public static VersionType CurrentVersionType = VersionType.Unknown;
    public static bool IsUpdating { get; private set; } = false;
    public static bool RequireRestart { get; private set; } = false;
    public static readonly string OverlayerGithubApiLink = string.Empty;
    public static bool IsRateLimited { get; private set; } = false;

    public static void Reload(ModEntry modEntry) {
        Type entryType = typeof(ModEntry);
        PropertyInfo canReloadProp = entryType.GetProperty("CanReload", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        MethodInfo setMethod = canReloadProp?.GetSetMethod(true);
        MethodInfo reloadMethod = entryType.GetMethod("Reload", BindingFlags.Instance | BindingFlags.NonPublic);
        setMethod?.Invoke(modEntry, new object[] { true });
        reloadMethod?.Invoke(modEntry, null);
        setMethod?.Invoke(modEntry, new object[] { false });
    }

    public static Task InitAndUpdate(ModEntry modEntry, bool update = false, bool allowBeta = false, Action ok = null, Action<string> err = null, bool latestIsError = false) {
        ResetState();
        DeletePendingUpdate(modEntry);
        return Task.CompletedTask;
    }

    public static Task InitUpdate(Version currentVersion, Action ok = null, Action<string> err = null) {
        ResetState();
        ok?.Invoke();
        return Task.CompletedTask;
    }

    public static Task CheckAndPrepareUpdate(ModEntry modEntry, bool allowBeta = false, Action ok = null, Action<string> err = null, bool latestPassIsError = false) {
        ResetState();
        DeletePendingUpdate(modEntry);
        err?.Invoke(Main.Lang?.Get("AUTOUPDATER_DISABLED", DisabledReason) ?? DisabledReason);
        return Task.CompletedTask;
    }

    public static Version UpdateBeforeLoad(ModEntry modEntry) {
        ResetState();
        DeletePendingUpdate(modEntry);
        return null;
    }

    private static void ResetState() {
        IsLatest = true;
        IsBeta = false;
        LatestUrl = null;
        BetaUrl = null;
        LatestVersion = null;
        BetaVersion = null;
        CurrentVersionType = VersionType.Unknown;
        IsUpdating = false;
        RequireRestart = false;
        IsRateLimited = false;
    }

    private static void DeletePendingUpdate(ModEntry modEntry) {
        if(modEntry == null || string.IsNullOrWhiteSpace(modEntry.Path)) {
            return;
        }

        string tempDir = Path.Combine(modEntry.Path, "updatetemp");
        if(!Directory.Exists(tempDir)) {
            return;
        }

        try {
            Directory.Delete(tempDir, true);
            Main.Logger?.Log("AutoUpdater is disabled; removed pending update temp directory.");
        } catch(Exception ex) {
            Main.Logger?.Error("Failed to remove pending update temp directory: " + ex.Message);
        }
    }
}
