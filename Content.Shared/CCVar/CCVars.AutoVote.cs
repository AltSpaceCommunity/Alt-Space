using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

[CVarDefs]
public sealed partial class CCVars
{
    /*
     * Existing Votes
     */

    /// Automatically starts a map vote when returning to the lobby.
    /// Requires auto voting to be enabled.
    public static readonly CVarDef<bool> MapAutoVoteEnabled =
        CVarDef.Create("vote.map_autovote_enabled", false, CVar.SERVERONLY);

    /// Automatically starts a gamemode vote when returning to the lobby.
    /// Requires auto voting to be enabled.
    public static readonly CVarDef<bool> PresetAutoVoteEnabled =
        CVarDef.Create("vote.preset_autovote_enabled", false, CVar.SERVERONLY);

    /*
     * Auto Round End
     */

    /// Интервал между автоматическими голосованиями за конец раунда (в часах).
    public static readonly CVarDef<float> AutoVoteInterval =
        CVarDef.Create("auto_round_end.vote_interval", 3.0f, CVar.SERVERONLY);

    /// Время, после которого шаттл вызывается принудительно (в часах).
    public static readonly CVarDef<float> AutoHardEndThreshold =
        CVarDef.Create("auto_round_end.hard_end_threshold", 9.0f, CVar.SERVERONLY);
}