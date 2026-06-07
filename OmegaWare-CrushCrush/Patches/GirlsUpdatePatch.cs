using HarmonyLib;

namespace OmegaWare_CrushCrush.Patches;

[HarmonyPatch(typeof(Girls), "Update")]
internal class GirlsUpdatePatch
{
    [HarmonyPrefix]
    private static void Prefix(Girls __instance)
    {
        ModContext.State.GirlsInstance = __instance;
    }
}