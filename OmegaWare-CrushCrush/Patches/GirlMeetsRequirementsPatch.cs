using HarmonyLib;

namespace OmegaWare_CrushCrush.Patches;

[HarmonyPatch(typeof(Girl), "MeetsRequirements")]
internal class GirlMeetsRequirementsPatch
{
    [HarmonyPostfix]
    private static void Postfix(Girl __instance, ref bool __result)
    {
        if (!ModContext.State.BypassCurrentGirlOtherRequirementsOnce || __instance == null)
            return;

        if (__instance.GirlName != ModContext.State.BypassOtherRequirementsGirl)
            return;

        if (__instance.Love > ModContext.State.BypassOtherRequirementsLove)
        {
            ModContext.State.BypassCurrentGirlOtherRequirementsOnce = false;
            ModContext.State.BypassOtherRequirementsGirl = Balance.GirlName.Unknown;
            ModContext.State.BypassOtherRequirementsLove = -1;
            return;
        }

        __result = true;
    }
}