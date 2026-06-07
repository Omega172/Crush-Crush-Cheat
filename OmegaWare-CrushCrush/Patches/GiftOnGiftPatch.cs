using HarmonyLib;

namespace OmegaWare_CrushCrush.Patches;

[HarmonyPatch(typeof(Gift), "OnGift", typeof(int))]
internal class GiftOnGiftPatch
{
    [HarmonyPrefix]
    private static void Prefix(ref int quantity)
    {
        if (ModContext.Config.OverrideGiftQuantity.Value)
        {
            quantity = ModContext.State.OverrideGiftQuantityValue;
            if (ModContext.State.ExtraDebugLogs)
                ModContext.Logger.LogInfo($"Overriding gift quantity to {quantity}");
        }
    }
}