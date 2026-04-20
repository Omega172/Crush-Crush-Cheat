using System;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using HarmonyLib;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

namespace OmegaWare_CrushCrush;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("CrushCrush.exe")]
public class Plugin : BaseUnityPlugin
{
    // Static Fields
    internal static bool bExtraDebugLogs = false;
    internal static new ManualLogSource Logger;
    internal static Harmony HarmonyInstance;
    internal static Rect menuRect = new(10, 10, 210, 130);
    internal static Rect popupRect = new(100, 100, 300, 150);
    
    // Class Instances
    internal static Girls girlsInstance = null;
    internal static Cellphone cellphoneInstance = null;

    // Config Entries

    internal static ConfigEntry<bool> bDisableAnalyticsManager;
    internal static ConfigEntry<KeyCode> toggleMenuKey;
    internal static ConfigEntry<bool> bShowMenu;
    internal static ConfigEntry<bool> bUnlockAllItems;
    internal static ConfigEntry<bool> bShowAllPinups;
    internal static ConfigEntry<float> customTimescale;
    internal static ConfigEntry<bool> bShowAllPhoneConversations;
    internal static ConfigEntry<bool> bEnableNSFW;
    internal static ConfigEntry<bool> bOverrideGiftQuantity;
    internal static ConfigEntry<KeyCode> skipPhoneTimerHotkey;
    internal static ConfigEntry<bool> bDlcUnlocker;
    internal static ConfigEntry<bool> bUnlockAllOutfits;

    // Helper Fields
    internal static float originalTimescale = 0f;
    internal static bool bShowConfirmPopup = false;
    internal static string diamondInputText = "1000";
    internal static int diamondValue = 1000;
    internal static string overrideGiftQuantityInputText = "1000";
    internal static int overrideGiftQuantityValue = 1000;
    internal static bool bListeningForSkipPhoneTimerHotkey = false;
    internal static bool bBypassCurrentGirlOtherRequirementsOnce = false;
    internal static Balance.GirlName bypassOtherRequirementsGirl = Balance.GirlName.Unknown;
    internal static int bypassOtherRequirementsLove = -1;

    private bool bPendingAnalyticsManagerDisable;

    private GameObject inputBlockerCanvasObject;
    private RectTransform menuInputBlocker;
    private RectTransform popupInputBlocker;

    private void Awake()
    {
        // Plugin startup logic
        Logger = BepInEx.Logging.Logger.CreateLogSource($" {MyPluginInfo.PLUGIN_NAME}");
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        Logger.LogWarning("This plugin is an alpha build, expect crashes and bugs!");

        Logger.LogInfo("Loading config...");
        bDisableAnalyticsManager = Config.Bind("General", "DisableAnalyticsManager", true, "Whether to destroy and clear Analytics.AnalyticsManager on startup.");
        toggleMenuKey = Config.Bind("General", "ToggleMenuKey", KeyCode.Insert, "Key to toggle the cheat menu");
        bShowMenu = Config.Bind("General", "ShowMenu", true, "Whether to show the cheat menu");
        bUnlockAllItems = Config.Bind("General", "UnlockAllItems", false, "Whether to unlock all items in the game");
        bShowAllPinups = Config.Bind("General", "ShowAllPinups", false, "Whether to show all pinups in the album");
        customTimescale = Config.Bind("General", "CustomTimescale", 1f, "Custom timescale value to set when clicking 'Set Timescale'");
        bShowAllPhoneConversations = Config.Bind("General", "ShowAllPhoneConversations", false, "Whether to unlock all phone conversations");
        bEnableNSFW = Config.Bind("General", "EnableNSFW", false, "Whether to enable NSFW content");
        bOverrideGiftQuantity = Config.Bind("General", "OverrideGiftQuantity", false, "Whether to override the quantity of gifts received");
        skipPhoneTimerHotkey = Config.Bind("General", "SkipPhoneTimerHotkey", KeyCode.Mouse3, "Hotkey to hold for skipping phone timer");
        bDlcUnlocker = Config.Bind("General", "DlcUnlocker", false, "Whether to unlock all DLC content");
        bUnlockAllOutfits = Config.Bind("General", "UnlockAllOutfits", false, "Whether to auto-unlock outfits from Gift.Init by OR-ing OutfitType into current girl's LifetimeOutfits.");

        originalTimescale = Time.timeScale;
        Logger.LogInfo($"Original timescale: {originalTimescale}");

        Logger.LogInfo("Config loaded!");

        bPendingAnalyticsManagerDisable = bDisableAnalyticsManager.Value;

        Logger.LogInfo("Patching game methods...");
        HarmonyInstance = new Harmony(MyPluginInfo.PLUGIN_GUID);
        HarmonyInstance.PatchAll();

        Logger.LogInfo("Patches applied!");

        CreateInputBlockers();
    }

    private void LateUpdate()
    {
        if (bPendingAnalyticsManagerDisable && TryDisableAnalyticsManager())
            bPendingAnalyticsManagerDisable = false;

        if (Input.GetKeyDown(toggleMenuKey.Value))
            bShowMenu.Value = !bShowMenu.Value;

        // Listen for skip phone timer hotkey binding
        if (bListeningForSkipPhoneTimerHotkey)
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key) && key != KeyCode.Escape)
                {
                    skipPhoneTimerHotkey.Value = key;
                    Logger.LogInfo($"Skip Phone Timer hotkey set to {key}");
                    bListeningForSkipPhoneTimerHotkey = false;
                    break;
                }

                // Escape cancels hotkey binding
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    bListeningForSkipPhoneTimerHotkey = false;
                    break;
                }
            }
        }

        // Check if skip phone timer hotkey is held
        if (!bListeningForSkipPhoneTimerHotkey && Input.GetKey(skipPhoneTimerHotkey.Value))
        {
            SkipPhoneTimer();
        }

        UpdateInputBlockers();
    }

    private void SkipPhoneTimer()
    {
        Traverse.Create(cellphoneInstance).Method("Debug_SkipMessage").GetValue();
        if (bExtraDebugLogs)
            Logger.LogInfo("Skipped phone timer");
    }

    private bool TryDisableAnalyticsManager()
    {
        try
        {
            Type analyticsManagerType = AccessTools.TypeByName("Analytics.AnalyticsManager");
            if (analyticsManagerType == null)
            {
                return false;
            }

            var instanceField = AccessTools.Field(analyticsManagerType, "s_instance");
            if (instanceField == null)
            {
                Logger.LogWarning("Analytics.AnalyticsManager.s_instance field was not found.");
                return true;
            }

            object instance = instanceField?.GetValue(null);
            if (instance == null)
                return false;

            var destroyInstanceMethod = AccessTools.Method(analyticsManagerType, "DestroyInstance");
            if (destroyInstanceMethod != null)
            {
                object target = destroyInstanceMethod.IsStatic ? null : instance;
                destroyInstanceMethod.Invoke(target, null);
            }

            instanceField?.SetValue(null, null);
            Logger.LogInfo("Analytics manager disabled.");
            return true;
        }
        catch (Exception ex)
        {
            Logger.LogWarning($"Failed to disable analytics manager: {ex.Message}");
            return true;
        }
    }

    private void OnGUI()
    {
        // Save original colors
        Color originalBg = GUI.backgroundColor;
        Color originalContent = GUI.contentColor;

        if (!bShowMenu.Value)
            return;

        // Set custom colors
        GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f); // Dark gray background
        GUI.contentColor = Color.cyan; // Cyan text

        const float menuControlWidth = 270f;
        const float menuControlHeight = 30f;
        const float menuPadding = 10f;
        const float menuSpacing = 5f;
        const int menuControlCount = 26; // includes Unlock All Outfits toggle

        float menuWidth = menuControlWidth + (menuPadding * 2f);
        float menuHeight = 40f + (menuControlCount * menuControlHeight) + ((menuControlCount + 1) * menuSpacing) + menuPadding;
        menuRect = new Rect(menuRect.x, menuRect.y, menuWidth, menuHeight);

        menuRect = GUI.Window(0, menuRect, (id) =>
        {
            GUILayout.BeginVertical(GUILayout.Width(menuControlWidth));
            GUILayout.Space(menuSpacing);

            if (GUILayout.Button("Unlock All Items", GUILayout.Height(menuControlHeight)))
            {
                bShowConfirmPopup = true;
            }

            if (GUILayout.Button("Unlock All Girls", GUILayout.Height(menuControlHeight)))
            {
                Balance.GirlName newestGirl = Enum.GetValues(typeof(Balance.GirlName))
                    .Cast<Balance.GirlName>()
                    .Where(g => (int)g < 1000)
                    .OrderByDescending(g => (int)g)
                    .FirstOrDefault();

                for (int i = 1; i <= (int)newestGirl; i++)
                {
                    try
                    {
                        Traverse.Create(girlsInstance).Method("UnlockGirl", i).GetValue();
                        if (bExtraDebugLogs)
                            Logger.LogInfo($"Unlocked girl {Enum.GetName(typeof(Balance.GirlName), i)}");
                    }
                    catch
                    {
                        Logger.LogWarning($"Could not unlock girl {Enum.GetName(typeof(Balance.GirlName), i)}");
                    }
                }
            }

            GUILayout.Space(menuSpacing);
            bShowAllPinups.Value = GUILayout.Toggle(bShowAllPinups.Value, "Show All Pinups", GUILayout.Height(menuControlHeight));

            GUILayout.Space(menuSpacing);
            if (GUILayout.Button("Unlock All Date Pics", GUILayout.Height(menuControlHeight)))
            {
                Balance.GirlName newestGirl = Enum.GetValues(typeof(Balance.GirlName))
                    .Cast<Balance.GirlName>()
                    .Where(g => (int)g < 1000)
                    .OrderByDescending(g => (int)g)
                    .FirstOrDefault();

                for (int i = 1; i <= (int)newestGirl; i++)
                {
                    try
                    {
                        Girl girl = Traverse.Create(typeof(Girl)).Method("FindGirl", (Balance.GirlName)i).GetValue<Girl>();
                        foreach (int j in new int[] { 1, 2, 4, 8, 16 })
                        {
                            Traverse.Create(typeof(Album)).Method("Add", (Requirement.DateType)j, girl).GetValue();
                            if (bExtraDebugLogs)
                                Logger.LogInfo($"Unlocked {Enum.GetName(typeof(Requirement.DateType), j)} pic for {Enum.GetName(typeof(Balance.GirlName), i)}");
                        }
                    }
                    catch
                    {
                        Logger.LogWarning($"Could not unlock pics for girl {Enum.GetName(typeof(Balance.GirlName), i)}");
                    }
                }
            }

            GUILayout.Space(menuSpacing);
            GUILayout.Label($"Timescale: {customTimescale.Value:F2}");
            customTimescale.Value = GUILayout.HorizontalSlider(customTimescale.Value, 0.1f, 5f, GUILayout.Height(menuControlHeight));

            GUILayout.Space(menuSpacing);
            if (GUILayout.Button("Set Timescale", GUILayout.Height(menuControlHeight)))
            {
                Time.timeScale = customTimescale.Value;
                Logger.LogInfo($"Timescale set to {customTimescale.Value}");
            }

            GUILayout.Space(menuSpacing);
            if (GUILayout.Button("Reset Timescale", GUILayout.Height(menuControlHeight)))
            {
                Time.timeScale = originalTimescale;
                customTimescale.Value = originalTimescale;
                Logger.LogInfo("Timescale reset to original value");
            }

            GUILayout.Space(menuSpacing);
            GUILayout.Label("Diamonds:");
            diamondInputText = GUILayout.TextField(diamondInputText, GUILayout.Height(menuControlHeight));

            diamondInputText = System.Text.RegularExpressions.Regex.Replace(diamondInputText, "[^0-9-]", "");
            if (diamondInputText.Contains("-"))
            {
                int minusIndex = diamondInputText.IndexOf("-");
                if (minusIndex > 0)
                {
                    diamondInputText = diamondInputText.Replace("-", "");
                }
                else if (diamondInputText.IndexOf("-", 1) >= 0)
                {
                    diamondInputText = diamondInputText.Substring(0, 1) + diamondInputText.Substring(1).Replace("-", "");
                }
            }

            if (!string.IsNullOrEmpty(diamondInputText) && int.TryParse(diamondInputText, out int result))
            {
                diamondValue = result;
            }
            else if (!string.IsNullOrEmpty(diamondInputText))
            {
                GUILayout.Label("Invalid integer");
            }

            if (GUILayout.Button("Add Diamonds", GUILayout.Height(menuControlHeight)))
            {
                Traverse.Create(typeof(Utilities)).Method("AwardDiamonds", diamondValue, false).GetValue();
                Logger.LogInfo($"Added {diamondValue} diamonds!");
            }

            if (GUILayout.Button("Set Current Girl ToLover", GUILayout.Height(menuControlHeight)))
            {
                Traverse.Create(Girls.CurrentGirl).Method("SetLove", Girl.LoveLevel.Lover).GetValue();

                if (bExtraDebugLogs)
                    Logger.LogInfo($"Set current girl {Enum.GetName(typeof(Balance.GirlName), Girls.CurrentGirl.GirlName)} love to lover");
            }

            if (GUILayout.Button("Set All Girls To Lover", GUILayout.Height(menuControlHeight)))
            {
                Balance.GirlName newestGirl = Enum.GetValues(typeof(Balance.GirlName))
                    .Cast<Balance.GirlName>()
                    .Where(g => (int)g < 1000)
                    .OrderByDescending(g => (int)g)
                    .FirstOrDefault();

                for (int i = 1; i <= (int)newestGirl; i++)
                {
                    try
                    {
                        Girl girl = Traverse.Create(typeof(Girl)).Method("FindGirl", (Balance.GirlName)i).GetValue<Girl>();
                        Traverse.Create(girl).Method("SetLove", Girl.LoveLevel.Lover).GetValue();
                        if (bExtraDebugLogs)
                            Logger.LogInfo($"Set girl {Enum.GetName(typeof(Balance.GirlName), i)} love to lover");
                    }
                    catch (Exception ex)
                    {
                        if (bExtraDebugLogs)
                            Logger.LogError($"Error setting girl {i} to lover: {ex.Message}");
                    }
                }
            }
            
            GameState.NSFW = GUILayout.Toggle(GameState.NSFW, "Enable NSFW Content", GUILayout.Height(menuControlHeight));
            GameState.NSFWAllowed = GameState.NSFW;
            
            bOverrideGiftQuantity.Value = GUILayout.Toggle(bOverrideGiftQuantity.Value, "Override Gift Quantity", GUILayout.Height(menuControlHeight));
            
            GUILayout.Space(menuSpacing);
            GUILayout.Label("Gift Quantity:");
            overrideGiftQuantityInputText = GUILayout.TextField(overrideGiftQuantityInputText, GUILayout.Height(menuControlHeight));

            overrideGiftQuantityInputText = System.Text.RegularExpressions.Regex.Replace(overrideGiftQuantityInputText, "[^0-9-]", "");
            if (overrideGiftQuantityInputText.Contains("-"))
            {
                int minusIndex = overrideGiftQuantityInputText.IndexOf("-");
                if (minusIndex > 0)
                {
                    overrideGiftQuantityInputText = overrideGiftQuantityInputText.Replace("-", "");
                }
                else if (overrideGiftQuantityInputText.IndexOf("-", 1) >= 0)
                {
                    overrideGiftQuantityInputText = overrideGiftQuantityInputText.Substring(0, 1) + overrideGiftQuantityInputText.Substring(1).Replace("-", "");
                }
            }

            if (!string.IsNullOrEmpty(overrideGiftQuantityInputText) && int.TryParse(overrideGiftQuantityInputText, out int giftResult))
            {
                overrideGiftQuantityValue = giftResult;
            }
            else if (!string.IsNullOrEmpty(overrideGiftQuantityInputText))
            {
                GUILayout.Label("Invalid integer");
            }

            if (GUILayout.Button("Skip Phone Timer", GUILayout.Height(menuControlHeight)))
            {
                SkipPhoneTimer();
            }

            bShowAllPhoneConversations.Value = GUILayout.Toggle(bShowAllPhoneConversations.Value, "All Phone Conversations Unlocked", GUILayout.Height(menuControlHeight));
            
            GUILayout.Space(menuSpacing);
            GUILayout.Label($"Skip Phone Timer: {skipPhoneTimerHotkey}");
            if (GUILayout.Button(bListeningForSkipPhoneTimerHotkey ? "Press any key..." : "Bind Skip Hotkey", GUILayout.Height(menuControlHeight)))
            {
                bListeningForSkipPhoneTimerHotkey = true;
            }

            bDlcUnlocker.Value = GUILayout.Toggle(bDlcUnlocker.Value, "Enable All DLC", GUILayout.Height(menuControlHeight));
            bUnlockAllOutfits.Value = GUILayout.Toggle(bUnlockAllOutfits.Value, "Unlock All Outfits", GUILayout.Height(menuControlHeight));

            if (GUILayout.Button("Meet Current Girl Heart Requirement", GUILayout.Height(menuControlHeight)))
            {
                if (Girls.CurrentGirl == null)
                {
                    Logger.LogWarning("No current girl is selected.");
                }
                else
                {
                    long heartRequirement = Girls.CurrentGirl.HeartRequirement;
                    if (heartRequirement >= 0)
                    {
                        Girls.CurrentGirl.Hearts = heartRequirement;
                        Logger.LogInfo($"Set {Enum.GetName(typeof(Balance.GirlName), Girls.CurrentGirl.GirlName)} hearts to requirement {heartRequirement}");
                    }
                }
            }

            if (GUILayout.Button("Meet All Current Girl Requirements", GUILayout.Height(menuControlHeight)))
            {
                if (Girls.CurrentGirl == null)
                {
                    Logger.LogWarning("No current girl is selected.");
                }
                else
                {
                    long heartRequirement = Girls.CurrentGirl.HeartRequirement;
                    if (heartRequirement >= 0)
                    {
                        Girls.CurrentGirl.Hearts = heartRequirement;
                        Logger.LogInfo($"Set {Enum.GetName(typeof(Balance.GirlName), Girls.CurrentGirl.GirlName)} hearts to requirement {heartRequirement}");
                    }

                    bBypassCurrentGirlOtherRequirementsOnce = true;
                    bypassOtherRequirementsGirl = Girls.CurrentGirl.GirlName;
                    bypassOtherRequirementsLove = Girls.CurrentGirl.Love;
                    Logger.LogInfo($"Armed one-time requirement bypass for {Enum.GetName(typeof(Balance.GirlName), bypassOtherRequirementsGirl)} at love level {bypassOtherRequirementsLove}.");
                }
            }

            if (GUILayout.Button("Save Config", GUILayout.Height(menuControlHeight)))
            {
                Config.Save();
                Logger.LogInfo("Config saved!");
            }

            GUILayout.EndVertical();

            GUI.DragWindow(new Rect(0, 0, 10000, 22));

        }, MyPluginInfo.PLUGIN_NAME);

        if (bShowConfirmPopup)
        {
            const float popupWidth = 320f;
            const float popupButtonHeight = 30f;
            const float popupSpacing = 10f;
            const float popupContentWidth = 290f;
            popupRect = new Rect(popupRect.x, popupRect.y, popupWidth, 175f);

            popupRect = GUI.Window(1, popupRect, (id) =>
            {
                GUILayout.BeginVertical(GUILayout.Width(popupContentWidth));
                GUILayout.Space(popupSpacing);

                GUILayout.Label("This will unlock all items in the game including all girls and premium content.\nYou must save and reload for this to take effect.\n\nAre you sure?");

                GUILayout.Space(popupSpacing);
                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Yes", GUILayout.Height(popupButtonHeight)))
                {
                    bUnlockAllItems.Value = true;
                    bShowConfirmPopup = false;
                    Logger.LogInfo("All Items Unlocked feature enabled!");
                }

                if (GUILayout.Button("No", GUILayout.Height(popupButtonHeight)))
                {
                    bUnlockAllItems.Value = false;
                    bShowConfirmPopup = false;
                }

                GUILayout.EndHorizontal();
                GUILayout.EndVertical();

                GUI.DragWindow(new Rect(0, 0, 10000, 22));

            }, "Confirm Action");
        }        

        ConsumeMenuMouseInput();

        // Restore original colors
        GUI.backgroundColor = originalBg;
        GUI.contentColor = originalContent;
    }

    private void ConsumeMenuMouseInput()
    {
        Event currentEvent = Event.current;
        if (currentEvent == null)
            return;

        bool isMouseEvent = currentEvent.type == EventType.MouseDown
            || currentEvent.type == EventType.MouseUp
            || currentEvent.type == EventType.MouseDrag
            || currentEvent.type == EventType.ScrollWheel
            || currentEvent.type == EventType.ContextClick;

        if (!isMouseEvent)
            return;

        bool pointerOverMenu = menuRect.Contains(currentEvent.mousePosition);
        bool pointerOverPopup = bShowConfirmPopup && popupRect.Contains(currentEvent.mousePosition);
        if (!pointerOverMenu && !pointerOverPopup)
            return;

        Input.ResetInputAxes();
        currentEvent.Use();
    }

    private void CreateInputBlockers()
    {
        inputBlockerCanvasObject = new GameObject("OmegaWareInputBlockers");
        DontDestroyOnLoad(inputBlockerCanvasObject);

        Canvas canvas = inputBlockerCanvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = short.MaxValue;
        inputBlockerCanvasObject.AddComponent<GraphicRaycaster>();

        menuInputBlocker = CreateInputBlockerRect("MenuInputBlocker");
        popupInputBlocker = CreateInputBlockerRect("PopupInputBlocker");

        inputBlockerCanvasObject.SetActive(false);
    }

    private RectTransform CreateInputBlockerRect(string objectName)
    {
        GameObject blockerObject = new GameObject(objectName);
        blockerObject.transform.SetParent(inputBlockerCanvasObject.transform, false);

        RectTransform rectTransform = blockerObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0f, 1f);
        rectTransform.anchorMax = new Vector2(0f, 1f);
        rectTransform.pivot = new Vector2(0f, 1f);

        Image image = blockerObject.AddComponent<Image>();
        image.color = new Color(0f, 0f, 0f, 0f);
        image.raycastTarget = true;

        return rectTransform;
    }

    private void UpdateInputBlockers()
    {
        if (inputBlockerCanvasObject == null)
            return;

        bool showMenuBlocker = bShowMenu != null && bShowMenu.Value;
        bool showPopupBlocker = showMenuBlocker && bShowConfirmPopup;
        inputBlockerCanvasObject.SetActive(showMenuBlocker || showPopupBlocker);
        if (!inputBlockerCanvasObject.activeSelf)
            return;

        SetBlockerRect(menuInputBlocker, menuRect, showMenuBlocker);
        SetBlockerRect(popupInputBlocker, popupRect, showPopupBlocker);
    }

    private void SetBlockerRect(RectTransform blocker, Rect guiRect, bool visible)
    {
        if (blocker == null)
            return;

        blocker.gameObject.SetActive(visible);
        if (!visible)
            return;

        blocker.anchoredPosition = new Vector2(guiRect.x, -guiRect.y);
        blocker.sizeDelta = new Vector2(guiRect.width, guiRect.height);
    }
}

[HarmonyPatch(typeof(BlayFapInventory), "HasItem", typeof(string))]
public class BlayFapInventory_HasItem_Patch
{
    [HarmonyPostfix]
    static void Postfix(ref bool __result, string id)
    {
        if (!__result && Plugin.bUnlockAllItems.Value)
        {
            if (Plugin.bExtraDebugLogs)
                Plugin.Logger.LogInfo($"Pretending player has item {id}");
            __result = true;
        }
    }   
}

[HarmonyPatch(typeof(Album), "IsPinupUnlocked", typeof(int))]
public class Album_IsPinupUnlocked_Patch
{
    [HarmonyPostfix]
    static void Postfix(ref bool __result, int pinupRewardAmount)
    {
        if (!__result && Plugin.bShowAllPinups.Value)
        {
            if (Plugin.bExtraDebugLogs)
                Plugin.Logger.LogInfo($"Pretending pinup {pinupRewardAmount} is unlocked");
            __result = true;
        }
    }
}

[HarmonyPatch(typeof(Girls), "Update")]
public class Girls_Update_Patch
{
    [HarmonyPrefix]
    static void Prefix(Girls __instance)
    {
        Plugin.girlsInstance = __instance;
    }
}

[HarmonyPatch(typeof(Cellphone), "Update")]
public class Cellphone_Update_Patch
{
    [HarmonyPrefix]
    static void Prefix(Cellphone __instance)
    {
        Plugin.cellphoneInstance = __instance;
    }
}

[HarmonyPatch(typeof(Cellphone), "IsUnlocked", typeof(short))]
public class Cellphone_IsUnlocked_Patch
{
    [HarmonyPostfix]
    static void Postfix(ref bool __result, short id)
    {
        if (!__result && Plugin.bShowAllPhoneConversations.Value)
        {
            if (Plugin.bExtraDebugLogs)
                Plugin.Logger.LogInfo($"Pretending phone conversation {id} is unlocked");
            __result = true;
        }
    }
}

[HarmonyPatch(typeof(Gift), "OnGift", typeof(int))]
public class Gift_OnGift_Patch
{
    [HarmonyPrefix]
    static void Prefix(ref int quantity)
    {
        if (Plugin.bOverrideGiftQuantity.Value)
        {
            quantity = Plugin.overrideGiftQuantityValue;
            if (Plugin.bExtraDebugLogs)
                Plugin.Logger.LogInfo($"Overriding gift quantity to {quantity}");
        }
    }
}

[HarmonyPatch(typeof(Gift), "Init", typeof(OutfitModel))]
public class Gift_Init_Patch
{
    [HarmonyPostfix]
    static void Postfix(Gift __instance)
    {
        if (Plugin.bUnlockAllOutfits == null || !Plugin.bUnlockAllOutfits.Value)
            return;

        if (Girls.CurrentGirl == null)
            return;

        if (!TryGetGiftOutfitType(__instance, out Requirement.OutfitType outfitType))
            return;

        if (outfitType == Requirement.OutfitType.None)
            return;

        try
        {
            Girls.CurrentGirl.LifetimeOutfits |= outfitType;
            Girls.CurrentGirl.StoreState();

            if (Plugin.bExtraDebugLogs)
            {
                Plugin.Logger.LogInfo($"Unlocked outfit {outfitType} for {Enum.GetName(typeof(Balance.GirlName), Girls.CurrentGirl.GirlName)}");
            }
        }
        catch (Exception ex)
        {
            if (Plugin.bExtraDebugLogs)
                Plugin.Logger.LogWarning($"Failed to unlock outfit from Gift.Init: {ex.Message}");
        }
    }

    private static bool TryGetGiftOutfitType(Gift gift, out Requirement.OutfitType outfitType)
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
                // Ignore reflection read failures and continue probing.
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
                // Ignore reflection read failures and continue probing.
            }
        }

        return false;
    }
}

[HarmonyPatch(typeof(Steamworks.SteamApps), "BIsDlcInstalled", typeof(AppId_t))]
public class Steamworks_BIsDlcInstalled_Patch
{
    [HarmonyPostfix]
    static void Postfix(ref bool __result, AppId_t appID)
    {
        if (!__result && Plugin.bDlcUnlocker.Value)
        {
            if (Plugin.bExtraDebugLogs)
                Plugin.Logger.LogInfo($"Pretending DLC {appID} is installed");
            __result = true;
        }
    }
}

[HarmonyPatch(typeof(Girl), "MeetsRequirements")]
public class Girl_MeetsRequirements_Patch
{
    [HarmonyPostfix]
    static void Postfix(Girl __instance, ref bool __result)
    {
        if (!Plugin.bBypassCurrentGirlOtherRequirementsOnce || __instance == null)
            return;

        if (__instance.GirlName != Plugin.bypassOtherRequirementsGirl)
            return;

        // Expire once this girl advances a level.
        if (__instance.Love > Plugin.bypassOtherRequirementsLove)
        {
            Plugin.bBypassCurrentGirlOtherRequirementsOnce = false;
            Plugin.bypassOtherRequirementsGirl = Balance.GirlName.Unknown;
            Plugin.bypassOtherRequirementsLove = -1;
            return;
        }

        __result = true;
    }
}
