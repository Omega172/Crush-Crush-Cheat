using HarmonyLib;
using Steamworks;

namespace OmegaWare_CrushCrush.Patches;

[HarmonyPatch(typeof(SteamApps), "BIsDlcInstalled", typeof(AppId_t))]
internal class SteamworksBIsDlcInstalledPatch
{
    [HarmonyPostfix]
    private static void Postfix(ref bool __result, AppId_t appID)
    {
        if (!__result && ModContext.Config.DlcUnlocker.Value)
        {
            if (ModContext.State.ExtraDebugLogs)
                ModContext.Logger.LogInfo($"Pretending DLC {appID} is installed");
            __result = true;
        }
    }
}