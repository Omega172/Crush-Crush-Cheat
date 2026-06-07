using System;
using System.Text.RegularExpressions;
using BepInEx.Configuration;
using UnityEngine;

namespace OmegaWare_CrushCrush.UI;

internal sealed class CheatMenuRenderer
{
    private readonly Regex digitsRegex = new("[^0-9-]");
    private Vector2 menuScrollPosition;

    internal void Render(ConfigFile configFile)
    {
        Color originalBg = GUI.backgroundColor;
        Color originalContent = GUI.contentColor;

        if (!ModContext.Config.ShowMenu.Value)
            return;

        GUI.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
        GUI.contentColor = Color.cyan;

        const float menuControlWidth = 270f;
        const float menuControlHeight = 30f;
        const float menuPadding = 10f;
        const float menuSpacing = 5f;
        const float menuTitleBarHeight = 24f;
        const float minMenuHeight = 240f;
        const float maxMenuScreenRatio = 0.75f;

        float menuWidth = menuControlWidth + (menuPadding * 2f);
        float maxMenuHeight = Mathf.Max(minMenuHeight, Screen.height * maxMenuScreenRatio);
        float measuredMenuHeight = Mathf.Clamp(ModContext.State.MenuRect.height, minMenuHeight, maxMenuHeight);
        ModContext.State.MenuRect = new Rect(ModContext.State.MenuRect.x, ModContext.State.MenuRect.y, menuWidth, measuredMenuHeight);
        float measuredContentHeight = 0f;
        float scrollViewHeight = Mathf.Max(40f, measuredMenuHeight - menuTitleBarHeight - menuPadding - (menuSpacing * 2f));

        ModContext.State.MenuRect = GUI.Window(0, ModContext.State.MenuRect, _ =>
        {
            GUILayout.BeginVertical(GUILayout.Width(menuControlWidth));
            GUILayout.Space(menuSpacing);

            menuScrollPosition = GUILayout.BeginScrollView(
                menuScrollPosition,
                false,
                true,
                GUIStyle.none,
                GUI.skin.verticalScrollbar,
                GUILayout.Height(scrollViewHeight));

            GUILayout.BeginVertical();

            if (GUILayout.Button("Unlock All Items", GUILayout.Height(menuControlHeight)))
                ModContext.State.ShowConfirmPopup = true;

            if (GUILayout.Button("Unlock All Girls", GUILayout.Height(menuControlHeight)))
                ModContext.Actions.UnlockAllGirls();

            GUILayout.Space(menuSpacing);
            ModContext.Config.ShowAllPinups.Value = GUILayout.Toggle(ModContext.Config.ShowAllPinups.Value, "Show All Pinups", GUILayout.Height(menuControlHeight));

            GUILayout.Space(menuSpacing);
            if (GUILayout.Button("Unlock All Date Pics", GUILayout.Height(menuControlHeight)))
                ModContext.Actions.UnlockAllDatePics();

            GUILayout.Space(menuSpacing);
            GUILayout.Label($"Timescale: {ModContext.Config.CustomTimescale.Value:F2}");
            ModContext.Config.CustomTimescale.Value = GUILayout.HorizontalSlider(ModContext.Config.CustomTimescale.Value, 0.1f, 5f, GUILayout.Height(menuControlHeight));

            GUILayout.Space(menuSpacing);
            if (GUILayout.Button("Set Timescale", GUILayout.Height(menuControlHeight)))
            {
                Time.timeScale = ModContext.Config.CustomTimescale.Value;
                ModContext.Logger.LogInfo($"Timescale set to {ModContext.Config.CustomTimescale.Value}");
            }

            GUILayout.Space(menuSpacing);
            if (GUILayout.Button("Reset Timescale", GUILayout.Height(menuControlHeight)))
            {
                Time.timeScale = ModContext.State.OriginalTimescale;
                ModContext.Config.CustomTimescale.Value = ModContext.State.OriginalTimescale;
                ModContext.Logger.LogInfo("Timescale reset to original value");
            }

            GUILayout.Space(menuSpacing);
            GUILayout.Label("Diamonds:");
            ModContext.State.DiamondInputText = GUILayout.TextField(ModContext.State.DiamondInputText, GUILayout.Height(menuControlHeight));
            if (!TryParseSignedInt(ref ModContext.State.DiamondInputText, out ModContext.State.DiamondValue))
                GUILayout.Label("Invalid integer");

            if (GUILayout.Button("Add Diamonds", GUILayout.Height(menuControlHeight)))
                ModContext.Actions.AddDiamonds(ModContext.State.DiamondValue);

            if (GUILayout.Button("Set Current Girl ToLover", GUILayout.Height(menuControlHeight)))
                ModContext.Actions.SetCurrentGirlToLover();

            if (GUILayout.Button("Set All Girls To Lover", GUILayout.Height(menuControlHeight)))
                ModContext.Actions.SetAllGirlsToLover();

            ModContext.Config.EnableNSFW.Value = GUILayout.Toggle(ModContext.Config.EnableNSFW.Value, "Enable NSFW Content", GUILayout.Height(menuControlHeight));
            GameState.NSFW = ModContext.Config.EnableNSFW.Value;
            GameState.NSFWAllowed = ModContext.Config.EnableNSFW.Value;

            ModContext.Config.OverrideGiftQuantity.Value = GUILayout.Toggle(ModContext.Config.OverrideGiftQuantity.Value, "Override Gift Quantity", GUILayout.Height(menuControlHeight));

            GUILayout.Space(menuSpacing);
            GUILayout.Label("Gift Quantity:");
            ModContext.State.OverrideGiftQuantityInputText = GUILayout.TextField(ModContext.State.OverrideGiftQuantityInputText, GUILayout.Height(menuControlHeight));
            if (!TryParseSignedInt(ref ModContext.State.OverrideGiftQuantityInputText, out ModContext.State.OverrideGiftQuantityValue))
                GUILayout.Label("Invalid integer");

            if (GUILayout.Button("Skip Phone Timer", GUILayout.Height(menuControlHeight)))
                ModContext.Actions.SkipPhoneTimer();

            ModContext.Config.ShowAllPhoneConversations.Value = GUILayout.Toggle(ModContext.Config.ShowAllPhoneConversations.Value, "All Phone Conversations Unlocked", GUILayout.Height(menuControlHeight));

            GUILayout.Space(menuSpacing);
            GUILayout.Label($"Skip Phone Timer: {ModContext.Config.SkipPhoneTimerHotkey.Value}");
            if (GUILayout.Button(ModContext.State.ListeningForSkipPhoneTimerHotkey ? "Press any key..." : "Bind Skip Hotkey", GUILayout.Height(menuControlHeight)))
                ModContext.State.ListeningForSkipPhoneTimerHotkey = true;

            ModContext.Config.DlcUnlocker.Value = GUILayout.Toggle(ModContext.Config.DlcUnlocker.Value, "Enable All DLC", GUILayout.Height(menuControlHeight));
            ModContext.Config.UnlockAllOutfits.Value = GUILayout.Toggle(ModContext.Config.UnlockAllOutfits.Value, "Unlock All Outfits", GUILayout.Height(menuControlHeight));

            if (GUILayout.Button("Meet Current Girl Heart Requirement", GUILayout.Height(menuControlHeight)))
                ModContext.Actions.MeetCurrentGirlHeartRequirement();

            if (GUILayout.Button("Meet All Current Girl Requirements", GUILayout.Height(menuControlHeight)))
                ModContext.Actions.MeetAllCurrentGirlRequirements();

            if (GUILayout.Button("Save Config", GUILayout.Height(menuControlHeight)))
            {
                configFile.Save();
                ModContext.Logger.LogInfo("Config saved!");
            }

            if (Event.current.type == EventType.Repaint)
            {
                Rect lastControlRect = GUILayoutUtility.GetLastRect();
                measuredContentHeight = lastControlRect.yMax + menuSpacing;
            }

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            GUI.DragWindow(new Rect(0, 0, 10000, 22));
        }, MyPluginInfo.PLUGIN_NAME);

        if (Event.current.type == EventType.Repaint)
        {
            float desiredMenuHeight = measuredContentHeight + menuTitleBarHeight + menuPadding + (menuSpacing * 2f);
            measuredMenuHeight = Mathf.Clamp(desiredMenuHeight, minMenuHeight, maxMenuHeight);

            if (!Mathf.Approximately(ModContext.State.MenuRect.height, measuredMenuHeight))
            {
                ModContext.State.MenuRect = new Rect(
                    ModContext.State.MenuRect.x,
                    ModContext.State.MenuRect.y,
                    ModContext.State.MenuRect.width,
                    measuredMenuHeight);
            }

            ClampMenuRectToScreen();
        }

        if (Event.current.type != EventType.Repaint)
        {
            ClampMenuRectToScreen();
        }

        if (ModContext.State.ShowConfirmPopup)
        {
            const float popupWidth = 320f;
            const float popupButtonHeight = 30f;
            const float popupSpacing = 10f;
            const float popupContentWidth = 290f;
            ModContext.State.PopupRect = new Rect(ModContext.State.PopupRect.x, ModContext.State.PopupRect.y, popupWidth, 175f);

            ModContext.State.PopupRect = GUI.Window(1, ModContext.State.PopupRect, _ =>
            {
                GUILayout.BeginVertical(GUILayout.Width(popupContentWidth));
                GUILayout.Space(popupSpacing);
                GUILayout.Label("This will unlock all items in the game including all girls and premium content.\nYou must save and reload for this to take effect.\n\nAre you sure?");
                GUILayout.Space(popupSpacing);
                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Yes", GUILayout.Height(popupButtonHeight)))
                {
                    ModContext.Config.UnlockAllItems.Value = true;
                    ModContext.State.ShowConfirmPopup = false;
                    ModContext.Logger.LogInfo("All Items Unlocked feature enabled!");
                }

                if (GUILayout.Button("No", GUILayout.Height(popupButtonHeight)))
                {
                    ModContext.Config.UnlockAllItems.Value = false;
                    ModContext.State.ShowConfirmPopup = false;
                }

                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUI.DragWindow(new Rect(0, 0, 10000, 22));
            }, "Confirm Action");
        }

        ConsumeMenuMouseInput();
        GUI.backgroundColor = originalBg;
        GUI.contentColor = originalContent;
    }

    private static void ClampMenuRectToScreen()
    {
        float maxX = Mathf.Max(0f, Screen.width - ModContext.State.MenuRect.width);
        float maxY = Mathf.Max(0f, Screen.height - ModContext.State.MenuRect.height);

        float clampedX = Mathf.Clamp(ModContext.State.MenuRect.x, 0f, maxX);
        float clampedY = Mathf.Clamp(ModContext.State.MenuRect.y, 0f, maxY);

        if (!Mathf.Approximately(ModContext.State.MenuRect.x, clampedX)
            || !Mathf.Approximately(ModContext.State.MenuRect.y, clampedY))
        {
            ModContext.State.MenuRect = new Rect(
                clampedX,
                clampedY,
                ModContext.State.MenuRect.width,
                ModContext.State.MenuRect.height);
        }
    }

    private bool TryParseSignedInt(ref string input, out int value)
    {
        input = digitsRegex.Replace(input, "");
        if (input.Contains("-"))
        {
            int minusIndex = input.IndexOf("-", StringComparison.Ordinal);
            if (minusIndex > 0)
            {
                input = input.Replace("-", "");
            }
            else if (input.IndexOf("-", 1, StringComparison.Ordinal) >= 0)
            {
                input = input.Substring(0, 1) + input.Substring(1).Replace("-", "");
            }
        }

        if (string.IsNullOrEmpty(input))
        {
            value = 0;
            return true;
        }

        return int.TryParse(input, out value);
    }

    private static void ConsumeMenuMouseInput()
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

        bool pointerOverMenu = ModContext.State.MenuRect.Contains(currentEvent.mousePosition);
        bool pointerOverPopup = ModContext.State.ShowConfirmPopup && ModContext.State.PopupRect.Contains(currentEvent.mousePosition);
        if (!pointerOverMenu && !pointerOverPopup)
            return;

        Input.ResetInputAxes();
        currentEvent.Use();
    }
}