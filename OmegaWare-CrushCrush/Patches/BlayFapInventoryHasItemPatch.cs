using HarmonyLib;

namespace OmegaWare_CrushCrush.Patches;

[HarmonyPatch(typeof(BlayFapInventory), "HasItem", typeof(string))]
internal class BlayFapInventoryHasItemPatch
{
    [HarmonyPostfix]
    private static void Postfix(ref bool __result, string id)
    {
        if (!__result && ModContext.Config.UnlockAllItems.Value)
        {
            if (ModContext.State.ExtraDebugLogs)
                ModContext.Logger.LogInfo($"Pretending player has item {id}");
            __result = true;
        }
    }
}