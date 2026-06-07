using BepInEx.Logging;
using HarmonyLib;
using OmegaWare_CrushCrush.Config;
using OmegaWare_CrushCrush.Services;
using OmegaWare_CrushCrush.State;

namespace OmegaWare_CrushCrush;

internal static class ModContext
{
    internal static ManualLogSource Logger = null!;
    internal static Harmony HarmonyInstance = null!;
    internal static PluginConfig Config = null!;
    internal static PluginState State { get; } = new();
    internal static GameActionsService Actions = null!;
}