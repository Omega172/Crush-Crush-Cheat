using System;
using BepInEx;
using BepInEx.Unity.Mono;
using HarmonyLib;
using OmegaWare_CrushCrush.Config;
using OmegaWare_CrushCrush.Services;
using OmegaWare_CrushCrush.UI;
using UnityEngine;

namespace OmegaWare_CrushCrush;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("CrushCrush.exe")]
public class Plugin : BaseUnityPlugin
{
    private bool pendingAnalyticsManagerDisable;
    private CheatMenuRenderer cheatMenuRenderer;
    private InputBlockerController inputBlockerController;

    private void Awake()
    {
        ModContext.Logger = BepInEx.Logging.Logger.CreateLogSource($" {MyPluginInfo.PLUGIN_NAME}");
        ModContext.Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        ModContext.Logger.LogWarning("This plugin is an alpha build, expect crashes and bugs!");

        ModContext.Logger.LogInfo("Loading config...");
        PluginConfig pluginConfig = new();
        pluginConfig.Bind(Config);
        ModContext.Config = pluginConfig;

        ModContext.State.OriginalTimescale = Time.timeScale;
        ModContext.Logger.LogInfo($"Original timescale: {ModContext.State.OriginalTimescale}");
        ModContext.Logger.LogInfo("Config loaded!");

        ModContext.Actions = new GameActionsService();

        pendingAnalyticsManagerDisable = ModContext.Config.DisableAnalyticsManager.Value;

        ModContext.Logger.LogInfo("Patching game methods...");
        ModContext.HarmonyInstance = new Harmony(MyPluginInfo.PLUGIN_GUID);
        ModContext.HarmonyInstance.PatchAll();
        ModContext.Logger.LogInfo("Patches applied!");

        cheatMenuRenderer = new CheatMenuRenderer();
        inputBlockerController = new InputBlockerController();
        inputBlockerController.Create();
    }

    private void LateUpdate()
    {
        if (pendingAnalyticsManagerDisable && ModContext.Actions.TryDisableAnalyticsManager())
            pendingAnalyticsManagerDisable = false;

        if (Input.GetKeyDown(ModContext.Config.ToggleMenuKey.Value))
            ModContext.Config.ShowMenu.Value = !ModContext.Config.ShowMenu.Value;

        if (ModContext.State.ListeningForSkipPhoneTimerHotkey)
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key) && key != KeyCode.Escape)
                {
                    ModContext.Config.SkipPhoneTimerHotkey.Value = key;
                    ModContext.Logger.LogInfo($"Skip Phone Timer hotkey set to {key}");
                    ModContext.State.ListeningForSkipPhoneTimerHotkey = false;
                    break;
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ModContext.State.ListeningForSkipPhoneTimerHotkey = false;
                    break;
                }
            }
        }

        if (!ModContext.State.ListeningForSkipPhoneTimerHotkey && Input.GetKey(ModContext.Config.SkipPhoneTimerHotkey.Value))
            ModContext.Actions.SkipPhoneTimer();

        inputBlockerController.Update();
    }

    private void OnGUI()
    {
        cheatMenuRenderer.Render(Config);
    }
}