using HarmonyLib;

namespace OmegaWare_CrushCrush.Patches;

[HarmonyPatch(typeof(Cellphone), "Update")]
internal class CellphoneUpdatePatch
{
    [HarmonyPrefix]
    private static void Prefix(Cellphone __instance)
    {
        ModContext.State.CellphoneInstance = __instance;
    }
}