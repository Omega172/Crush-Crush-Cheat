using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace OmegaWare_CrushCrush.Services;

internal sealed class GameActionsService
{
    internal void SkipPhoneTimer()
    {
        if (ModContext.State.CellphoneInstance == null)
            return;

        Traverse.Create(ModContext.State.CellphoneInstance).Method("Debug_SkipMessage").GetValue();
        if (ModContext.State.ExtraDebugLogs)
            ModContext.Logger.LogInfo("Skipped phone timer");
    }

    internal bool TryDisableAnalyticsManager()
    {
        try
        {
            Type analyticsManagerType = AccessTools.TypeByName("Analytics.AnalyticsManager");
            if (analyticsManagerType == null)
                return false;

            var instanceField = AccessTools.Field(analyticsManagerType, "s_instance");
            if (instanceField == null)
            {
                ModContext.Logger.LogWarning("Analytics.AnalyticsManager.s_instance field was not found.");
                return true;
            }

            object instance = instanceField.GetValue(null);
            if (instance == null)
                return false;

            var destroyInstanceMethod = AccessTools.Method(analyticsManagerType, "DestroyInstance");
            if (destroyInstanceMethod != null)
            {
                object target = destroyInstanceMethod.IsStatic ? null : instance;
                destroyInstanceMethod.Invoke(target, null);
            }

            instanceField.SetValue(null, null);
            ModContext.Logger.LogInfo("Analytics manager disabled.");
            return true;
        }
        catch (Exception ex)
        {
            ModContext.Logger.LogWarning($"Failed to disable analytics manager: {ex.Message}");
            return true;
        }
    }

    internal void UnlockAllGirls()
    {
        if (ModContext.State.GirlsInstance == null)
            return;

        Balance.GirlName newestGirl = GetNewestGirl();
        for (int i = 1; i <= (int)newestGirl; i++)
        {
            try
            {
                Traverse.Create(ModContext.State.GirlsInstance).Method("UnlockGirl", i).GetValue();
                if (ModContext.State.ExtraDebugLogs)
                    ModContext.Logger.LogInfo($"Unlocked girl {Enum.GetName(typeof(Balance.GirlName), i)}");
            }
            catch
            {
                ModContext.Logger.LogWarning($"Could not unlock girl {Enum.GetName(typeof(Balance.GirlName), i)}");
            }
        }
    }

    internal void UnlockAllDatePics()
    {
        Balance.GirlName newestGirl = GetNewestGirl();
        for (int i = 1; i <= (int)newestGirl; i++)
        {
            try
            {
                Girl girl = Traverse.Create(typeof(Girl)).Method("FindGirl", (Balance.GirlName)i).GetValue<Girl>();
                foreach (int j in new[] { 1, 2, 4, 8, 16 })
                {
                    Traverse.Create(typeof(Album)).Method("Add", (Requirement.DateType)j, girl).GetValue();
                    if (ModContext.State.ExtraDebugLogs)
                        ModContext.Logger.LogInfo($"Unlocked {Enum.GetName(typeof(Requirement.DateType), j)} pic for {Enum.GetName(typeof(Balance.GirlName), i)}");
                }
            }
            catch
            {
                ModContext.Logger.LogWarning($"Could not unlock pics for girl {Enum.GetName(typeof(Balance.GirlName), i)}");
            }
        }
    }

    internal void AddDiamonds(int amount)
    {
        Traverse.Create(typeof(Utilities)).Method("AwardDiamonds", amount, false).GetValue();
        ModContext.Logger.LogInfo($"Added {amount} diamonds!");
    }

    internal void SetCurrentGirlToLover()
    {
        if (Girls.CurrentGirl == null)
            return;

        Traverse.Create(Girls.CurrentGirl).Method("SetLove", Girl.LoveLevel.Lover).GetValue();
        if (ModContext.State.ExtraDebugLogs)
            ModContext.Logger.LogInfo($"Set current girl {Enum.GetName(typeof(Balance.GirlName), Girls.CurrentGirl.GirlName)} love to lover");
    }

    internal void SetAllGirlsToLover()
    {
        Balance.GirlName newestGirl = GetNewestGirl();
        for (int i = 1; i <= (int)newestGirl; i++)
        {
            try
            {
                Girl girl = Traverse.Create(typeof(Girl)).Method("FindGirl", (Balance.GirlName)i).GetValue<Girl>();
                Traverse.Create(girl).Method("SetLove", Girl.LoveLevel.Lover).GetValue();
                if (ModContext.State.ExtraDebugLogs)
                    ModContext.Logger.LogInfo($"Set girl {Enum.GetName(typeof(Balance.GirlName), i)} love to lover");
            }
            catch (Exception ex)
            {
                if (ModContext.State.ExtraDebugLogs)
                    ModContext.Logger.LogError($"Error setting girl {i} to lover: {ex.Message}");
            }
        }
    }

    internal void MeetCurrentGirlHeartRequirement()
    {
        if (Girls.CurrentGirl == null)
        {
            ModContext.Logger.LogWarning("No current girl is selected.");
            return;
        }

        long heartRequirement = Girls.CurrentGirl.HeartRequirement;
        if (heartRequirement >= 0)
        {
            Girls.CurrentGirl.Hearts = heartRequirement;
            ModContext.Logger.LogInfo($"Set {Enum.GetName(typeof(Balance.GirlName), Girls.CurrentGirl.GirlName)} hearts to requirement {heartRequirement}");
        }
    }

    internal void MeetAllCurrentGirlRequirements()
    {
        if (Girls.CurrentGirl == null)
        {
            ModContext.Logger.LogWarning("No current girl is selected.");
            return;
        }

        long heartRequirement = Girls.CurrentGirl.HeartRequirement;
        if (heartRequirement >= 0)
        {
            Girls.CurrentGirl.Hearts = heartRequirement;
            ModContext.Logger.LogInfo($"Set {Enum.GetName(typeof(Balance.GirlName), Girls.CurrentGirl.GirlName)} hearts to requirement {heartRequirement}");
        }

        ModContext.State.BypassCurrentGirlOtherRequirementsOnce = true;
        ModContext.State.BypassOtherRequirementsGirl = Girls.CurrentGirl.GirlName;
        ModContext.State.BypassOtherRequirementsLove = Girls.CurrentGirl.Love;
        ModContext.Logger.LogInfo($"Armed one-time requirement bypass for {Enum.GetName(typeof(Balance.GirlName), ModContext.State.BypassOtherRequirementsGirl)} at love level {ModContext.State.BypassOtherRequirementsLove}.");
    }

    private static Balance.GirlName GetNewestGirl()
    {
        return Enum.GetValues(typeof(Balance.GirlName))
            .Cast<Balance.GirlName>()
            .Where(g => (int)g < 1000)
            .OrderByDescending(g => (int)g)
            .FirstOrDefault();
    }

    internal static bool TryGetGiftOutfitType(Gift gift, out Requirement.OutfitType outfitType)
    {
        outfitType = Requirement.OutfitType.None;
        if (gift == null)
            return false;

        Type giftType = gift.GetType();
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        foreach (PropertyInfo property in giftType.GetProperties(flags))
        {
            if (property.PropertyType != typeof(Requirement.OutfitType) || property.GetIndexParameters().Length != 0)
                continue;

            try
            {
                var value = (Requirement.OutfitType)property.GetValue(gift, null);
                if (value != Requirement.OutfitType.None)
                {
                    outfitType = value;
                    return true;
                }
            }
            catch
            {
            }
        }

        foreach (FieldInfo field in giftType.GetFields(flags))
        {
            if (field.FieldType != typeof(Requirement.OutfitType))
                continue;

            try
            {
                var value = (Requirement.OutfitType)field.GetValue(gift);
                if (value != Requirement.OutfitType.None)
                {
                    outfitType = value;
                    return true;
                }
            }
            catch
            {
            }
        }

        return false;
    }
}