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
    internal static new ManualLogSource Logger;
    internal static Harmony HarmonyInstance;
    internal static KeyCode toggleMenuKey = KeyCode.Insert;
    internal static Rect menuRect = new(10, 10, 200, 130);
    internal static Rect popupRect = new(100, 100, 300, 150);

    internal static bool bShowMenu = true;
    internal static bool bUnlockAllItems = false;
    internal static bool bShowAllPinups = false;
    internal static bool bShowConfirmPopup = false;

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

        menuRect = GUI.Window(0, menuRect, (id) =>
        {
            if (GUI.Button(new Rect(10, 100, 180, 30), "Unlock All Items")) {
                bShowConfirmPopup = true;
            }

            bShowAllPinups = GUI.Toggle(new Rect(10, 70, 180, 30), bShowAllPinups, "Show All Pinups");

            if (GUI.Button(new Rect(10, 30, 180, 30), "Unlock All Date Pics"))
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
                        foreach(int j in new int[] { 1, 2, 4, 8, 16 })
                        {
                            var result = Traverse.Create(typeof(Album)).Method("Add", (Requirement.DateType)j, girl).GetValue();
                            Logger.LogInfo($"Unlocked {Enum.GetName(typeof(Requirement.DateType), j)} pic for {Enum.GetName(typeof(Balance.GirlName), i)}");
                        }
                    }
                    catch
                    {
                        Logger.LogWarning($"Could not unlock pics for girl {(Balance.GirlName)i}");
                    }
                }
            }

            GUI.DragWindow(); // Make the entire window draggable (except where controls are)

        }, MyPluginInfo.PLUGIN_NAME);

        if (bShowConfirmPopup)
        {
            popupRect = GUI.Window(1, popupRect, (id) =>
            {
                GUI.Label(new Rect(10, 20, 280, 60), "This will unlock all items in the game including all girls and premium content.\nYou must save and reload for this to take effect.\n\nAre you sure?");

                if (GUI.Button(new Rect(10, 90, 135, 30), "Yes"))
                {
                    bUnlockAllItems = true;
                    bShowConfirmPopup = false;
                    Logger.LogInfo("All Items Unlocked feature enabled!");
                }

                if (GUI.Button(new Rect(155, 90, 135, 30), "No"))
                {
                    bShowConfirmPopup = false;
                }

                GUI.DragWindow();

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
            Plugin.Logger.LogInfo($"Pretending pinup {pinupRewardAmount} is unlocked");
            __result = true;
        }
    }
}