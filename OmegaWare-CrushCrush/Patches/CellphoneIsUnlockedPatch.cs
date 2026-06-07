using HarmonyLib;

namespace OmegaWare_CrushCrush.Patches;

[HarmonyPatch(typeof(Cellphone), "IsUnlocked", typeof(short))]
internal class CellphoneIsUnlockedPatch
{
    [HarmonyPostfix]
    private static void Postfix(ref bool __result, short id)
    {
        if (!__result && ModContext.Config.ShowAllPhoneConversations.Value)
        {
            if (ModContext.State.ExtraDebugLogs)
                ModContext.Logger.LogInfo($"Pretending phone conversation {id} is unlocked");
            __result = true;
        }
    }
}