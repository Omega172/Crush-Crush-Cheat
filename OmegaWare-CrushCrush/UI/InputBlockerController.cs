using UnityEngine;
using UnityEngine.UI;

namespace OmegaWare_CrushCrush.UI;

internal sealed class InputBlockerController
{
    private GameObject inputBlockerCanvasObject;
    private RectTransform menuInputBlocker;
    private RectTransform popupInputBlocker;

    internal void Create()
    {
        inputBlockerCanvasObject = new GameObject("OmegaWareInputBlockers");
        Object.DontDestroyOnLoad(inputBlockerCanvasObject);

        Canvas canvas = inputBlockerCanvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = short.MaxValue;
        inputBlockerCanvasObject.AddComponent<GraphicRaycaster>();

        menuInputBlocker = CreateInputBlockerRect("MenuInputBlocker");
        popupInputBlocker = CreateInputBlockerRect("PopupInputBlocker");

        inputBlockerCanvasObject.SetActive(false);
    }

    internal void Update()
    {
        if (inputBlockerCanvasObject == null)
            return;

        bool showMenuBlocker = ModContext.Config.ShowMenu != null && ModContext.Config.ShowMenu.Value;
        bool showPopupBlocker = showMenuBlocker && ModContext.State.ShowConfirmPopup;
        inputBlockerCanvasObject.SetActive(showMenuBlocker || showPopupBlocker);
        if (!inputBlockerCanvasObject.activeSelf)
            return;

        SetBlockerRect(menuInputBlocker, ModContext.State.MenuRect, showMenuBlocker);
        SetBlockerRect(popupInputBlocker, ModContext.State.PopupRect, showPopupBlocker);
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

    private static void SetBlockerRect(RectTransform blocker, Rect guiRect, bool visible)
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