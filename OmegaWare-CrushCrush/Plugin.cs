using System;
using System.Linq;
using System.Runtime.InteropServices;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using HarmonyLib;
using UnityEngine;

namespace OmegaWare_CrushCrush;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("CrushCrush.exe")]
public class Plugin : BaseUnityPlugin
{
    internal static bool bExtraDebugLogs = false;

    internal static new ManualLogSource Logger;
    internal static Harmony HarmonyInstance;
    internal static KeyCode toggleMenuKey = KeyCode.Insert;
    internal static Rect menuRect = new(10, 10, 200, 130);
    internal static Rect popupRect = new(100, 100, 300, 150);

    internal static Girls girlsInstance = null;
    internal static Cellphone cellphoneInstance = null;

    internal static bool bShowMenu = true;
    internal static bool bUnlockAllItems = false;
    internal static bool bShowAllPinups = false;
    internal static bool bShowConfirmPopup = false;
    internal static float originalTimescale = 1f;
    internal static float timescale = 1f;
    internal static string intInputText = "1000";
    internal static int intDiamondValue = 1000;
    internal static bool bShowAllPhoneConversations = false;
    internal static bool bEnableNSFW = false;

    private void Awake()
    {
        // Plugin startup logic
        Logger = BepInEx.Logging.Logger.CreateLogSource($" {MyPluginInfo.PLUGIN_NAME}");
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        Logger.LogWarning("This plugin is an alpha build, expect crashes and bugs!");

        Logger.LogInfo("Patching game methods...");
        HarmonyInstance = new Harmony(MyPluginInfo.PLUGIN_GUID);
        HarmonyInstance.PatchAll();

        Logger.LogInfo("Patches applied!");

        originalTimescale = Time.timeScale;
        Logger.LogInfo($"Original timescale: {originalTimescale}");
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(toggleMenuKey))
            bShowMenu = !bShowMenu;
    }

    private void OnGUI()
    {
        // Save original colors
        Color originalBg = GUI.backgroundColor;
        Color originalContent = GUI.contentColor;

        if (!bShowMenu)
            return;

        // Set custom colors
        GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f); // Dark gray background
        GUI.contentColor = Color.cyan; // Cyan text

        const float menuControlWidth = 210f;
        const float menuControlHeight = 30f;
        const float menuPadding = 14f;
        const float menuSpacing = 5f;
        const int menuControlCount = 15; // 3 cheat buttons + 1 label + slider + 2 timescale buttons + 1 diamonds label + 1 text field

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
            bShowAllPinups = GUILayout.Toggle(bShowAllPinups, "Show All Pinups", GUILayout.Height(menuControlHeight));

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
            GUILayout.Label($"Timescale: {timescale:F2}");
            timescale = GUILayout.HorizontalSlider(timescale, 0.1f, 5f, GUILayout.Height(menuControlHeight));

            GUILayout.Space(menuSpacing);
            if (GUILayout.Button("Set Timescale", GUILayout.Height(menuControlHeight)))
            {
                Time.timeScale = timescale;
                Logger.LogInfo($"Timescale set to {timescale}");
            }

            GUILayout.Space(menuSpacing);
            if (GUILayout.Button("Reset Timescale", GUILayout.Height(menuControlHeight)))
            {
                Time.timeScale = originalTimescale;
                timescale = originalTimescale;
                Logger.LogInfo("Timescale reset to original value");
            }

            GUILayout.Space(menuSpacing);
            GUILayout.Label("Diamonds:");
            intInputText = GUILayout.TextField(intInputText, GUILayout.Height(menuControlHeight));

            intInputText = System.Text.RegularExpressions.Regex.Replace(intInputText, "[^0-9-]", "");
            if (intInputText.Contains("-"))
            {
                int minusIndex = intInputText.IndexOf("-");
                if (minusIndex > 0)
                {
                    intInputText = intInputText.Replace("-", "");
                }
                else if (intInputText.IndexOf("-", 1) >= 0)
                {
                    intInputText = intInputText.Substring(0, 1) + intInputText.Substring(1).Replace("-", "");
                }
            }

            if (!string.IsNullOrEmpty(intInputText) && int.TryParse(intInputText, out int result))
            {
                intDiamondValue = result;
            }
            else if (!string.IsNullOrEmpty(intInputText))
            {
                GUILayout.Label("Invalid integer");
            }

            if (GUILayout.Button("Add Diamonds", GUILayout.Height(menuControlHeight)))
            {
                Traverse.Create(typeof(Utilities)).Method("AwardDiamonds", intDiamondValue, false).GetValue();
                Logger.LogInfo($"Added {intDiamondValue} diamonds!");
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

            bShowAllPhoneConversations = GUILayout.Toggle(bShowAllPhoneConversations, "All Phone Conversations Unlocked", GUILayout.Height(menuControlHeight));
            GameState.NSFW = GUILayout.Toggle(GameState.NSFW, "Enable NSFW Content", GUILayout.Height(menuControlHeight));
            GameState.NSFWAllowed = GameState.NSFW;

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
                    bUnlockAllItems = true;
                    bShowConfirmPopup = false;
                    Logger.LogInfo("All Items Unlocked feature enabled!");
                }

                if (GUILayout.Button("No", GUILayout.Height(popupButtonHeight)))
                {
                    bUnlockAllItems = false;
                    bShowConfirmPopup = false;
                }

                GUILayout.EndHorizontal();
                GUILayout.EndVertical();

                GUI.DragWindow(new Rect(0, 0, 10000, 22));

            }, "Confirm Action");
        }        

        // Restore original colors
        GUI.backgroundColor = originalBg;
        GUI.contentColor = originalContent;
    }
}

[HarmonyPatch(typeof(BlayFapInventory), "HasItem", typeof(string))]
public class BlayFapInventory_HasItem_Patch
{
    [HarmonyPostfix]
    static void Postfix(ref bool __result, string id)
    {
        if (!__result && Plugin.bUnlockAllItems)
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
        if (!__result && Plugin.bShowAllPinups)
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
        if (!__result && Plugin.bShowAllPhoneConversations)
        {
            if (Plugin.bExtraDebugLogs)
                Plugin.Logger.LogInfo($"Pretending phone conversation {id} is unlocked");
            __result = true;
        }
    }
}