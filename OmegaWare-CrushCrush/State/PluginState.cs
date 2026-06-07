using UnityEngine;

namespace OmegaWare_CrushCrush.State;

internal sealed class PluginState
{
    internal bool ExtraDebugLogs;

    internal Rect MenuRect = new(10, 10, 210, 130);
    internal Rect PopupRect = new(100, 100, 300, 150);

    internal Girls GirlsInstance;
    internal Cellphone CellphoneInstance;

    internal float OriginalTimescale;
    internal bool ShowConfirmPopup;

    internal string DiamondInputText = "1000";
    internal int DiamondValue = 1000;

    internal string OverrideGiftQuantityInputText = "1000";
    internal int OverrideGiftQuantityValue = 1000;

    internal bool ListeningForSkipPhoneTimerHotkey;

    internal bool BypassCurrentGirlOtherRequirementsOnce;
    internal Balance.GirlName BypassOtherRequirementsGirl = Balance.GirlName.Unknown;
    internal int BypassOtherRequirementsLove = -1;
}