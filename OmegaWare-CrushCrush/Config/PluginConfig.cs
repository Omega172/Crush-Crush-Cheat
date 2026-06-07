using BepInEx.Configuration;
using UnityEngine;

namespace OmegaWare_CrushCrush.Config;

internal sealed class PluginConfig
{
    internal ConfigEntry<bool> DisableAnalyticsManager = null!;
    internal ConfigEntry<KeyCode> ToggleMenuKey = null!;
    internal ConfigEntry<bool> ShowMenu = null!;
    internal ConfigEntry<bool> UnlockAllItems = null!;
    internal ConfigEntry<bool> ShowAllPinups = null!;
    internal ConfigEntry<float> CustomTimescale = null!;
    internal ConfigEntry<bool> ShowAllPhoneConversations = null!;
    internal ConfigEntry<bool> EnableNSFW = null!;
    internal ConfigEntry<bool> OverrideGiftQuantity = null!;
    internal ConfigEntry<KeyCode> SkipPhoneTimerHotkey = null!;
    internal ConfigEntry<bool> DlcUnlocker = null!;
    internal ConfigEntry<bool> UnlockAllOutfits = null!;

    internal void Bind(ConfigFile configFile)
    {
        DisableAnalyticsManager = configFile.Bind("General", "DisableAnalyticsManager", true, "Whether to destroy and clear Analytics.AnalyticsManager on startup.");
        ToggleMenuKey = configFile.Bind("General", "ToggleMenuKey", KeyCode.Insert, "Key to toggle the cheat menu");
        ShowMenu = configFile.Bind("General", "ShowMenu", true, "Whether to show the cheat menu");
        UnlockAllItems = configFile.Bind("General", "UnlockAllItems", false, "Whether to unlock all items in the game");
        ShowAllPinups = configFile.Bind("General", "ShowAllPinups", false, "Whether to show all pinups in the album");
        CustomTimescale = configFile.Bind("General", "CustomTimescale", 1f, "Custom timescale value to set when clicking 'Set Timescale'");
        ShowAllPhoneConversations = configFile.Bind("General", "ShowAllPhoneConversations", false, "Whether to unlock all phone conversations");
        EnableNSFW = configFile.Bind("General", "EnableNSFW", false, "Whether to enable NSFW content");
        OverrideGiftQuantity = configFile.Bind("General", "OverrideGiftQuantity", false, "Whether to override the quantity of gifts received");
        SkipPhoneTimerHotkey = configFile.Bind("General", "SkipPhoneTimerHotkey", KeyCode.Mouse3, "Hotkey to hold for skipping phone timer");
        DlcUnlocker = configFile.Bind("General", "DlcUnlocker", false, "Whether to unlock all DLC content");
        UnlockAllOutfits = configFile.Bind("General", "UnlockAllOutfits", false, "Whether to auto-unlock outfits from Gift.Init by OR-ing OutfitType into current girl's LifetimeOutfits.");
    }
}