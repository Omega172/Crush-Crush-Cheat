using System;
using HarmonyLib;
using OmegaWare_CrushCrush.Services;

namespace OmegaWare_CrushCrush.Patches;

[HarmonyPatch(typeof(Gift), "Init", typeof(OutfitModel))]
internal class GiftInitPatch
{
    [HarmonyPostfix]
    private static void Postfix(Gift __instance)
    {
        if (!ModContext.Config.UnlockAllOutfits.Value)
            return;

        if (Girls.CurrentGirl == null)
            return;

        if (!GameActionsService.TryGetGiftOutfitType(__instance, out Requirement.OutfitType outfitType))
            return;

        if (outfitType == Requirement.OutfitType.None)
            return;

        try
        {
            Girls.CurrentGirl.LifetimeOutfits |= outfitType;
            Girls.CurrentGirl.StoreState();

            if (ModContext.State.ExtraDebugLogs)
            {
                ModContext.Logger.LogInfo($"Unlocked outfit {outfitType} for {Enum.GetName(typeof(Balance.GirlName), Girls.CurrentGirl.GirlName)}");
            }
        }
        catch (Exception ex)
        {
            if (ModContext.State.ExtraDebugLogs)
                ModContext.Logger.LogWarning($"Failed to unlock outfit from Gift.Init: {ex.Message}");
        }
    }
}