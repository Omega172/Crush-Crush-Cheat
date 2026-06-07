using HarmonyLib;

namespace OmegaWare_CrushCrush.Patches;

[HarmonyPatch(typeof(Album), "IsPinupUnlocked", typeof(int))]
internal class AlbumIsPinupUnlockedPatch
{
    [HarmonyPostfix]
    private static void Postfix(ref bool __result, int pinupRewardAmount)
    {
        if (!__result && ModContext.Config.ShowAllPinups.Value)
        {
            if (ModContext.State.ExtraDebugLogs)
                ModContext.Logger.LogInfo($"Pretending pinup {pinupRewardAmount} is unlocked");
            __result = true;
        }
    }
}