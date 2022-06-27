using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000090 RID: 144
public class DebugButton : MonoBehaviour, IDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
	// Token: 0x06000246 RID: 582 RVA: 0x0001051C File Offset: 0x0000E71C
	private void Start()
	{
		this._debugCanvasController = base.GetComponentInParent<DebugCanvasController>();
		this._mainCamera = Camera.main;
		this._rectTransform = base.GetComponent<RectTransform>();
		this._canvas = this._rectTransform.parent.GetComponent<RectTransform>();
		this._widget = base.transform.Find("Widget").gameObject;
		if (global::PlayerPrefs.HasKey("DO-NOT-BACKUPDebugPosX"))
		{
			float @float = global::PlayerPrefs.GetFloat("DO-NOT-BACKUPDebugPosX", this._rectTransform.anchoredPosition.x);
			float float2 = global::PlayerPrefs.GetFloat("DO-NOT-BACKUPDebugPosY", this._rectTransform.anchoredPosition.y);
			this._rectTransform.anchoredPosition = new Vector2(@float, float2);
		}
		this._debugCanvasController.Init();
	}

	// Token: 0x06000247 RID: 583 RVA: 0x000105E8 File Offset: 0x0000E7E8
	public void OnBeginDrag(PointerEventData eventData)
	{
		this._rectTransform.localScale = Vector3.one * 1.5f;
		if (this._widget != null && this._widget.activeInHierarchy)
		{
			this._widget.SetActive(false);
			this._cachedWidgetState = true;
		}
	}

	// Token: 0x06000248 RID: 584 RVA: 0x00010644 File Offset: 0x0000E844
	public void OnDrag(PointerEventData eventData)
	{
		Vector2 a;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this._canvas, eventData.position, this._mainCamera, out a);
		base.transform.position = this._canvas.transform.TransformPoint(a - this._offset);
	}

	// Token: 0x06000249 RID: 585 RVA: 0x00010698 File Offset: 0x0000E898
	public void OnEndDrag(PointerEventData eventData)
	{
		this._rectTransform.localScale = Vector3.one;
		global::PlayerPrefs.SetFloat("DO-NOT-BACKUPDebugPosX", this._rectTransform.anchoredPosition.x);
		global::PlayerPrefs.SetFloat("DO-NOT-BACKUPDebugPosY", this._rectTransform.anchoredPosition.y);
		if (this._cachedWidgetState)
		{
			this._widget.SetActive(this._cachedWidgetState);
			this._cachedWidgetState = false;
		}
		GameState.CurrentState.QueueSave();
	}

	// Token: 0x0600024A RID: 586 RVA: 0x0001071C File Offset: 0x0000E91C
	public void OnPointerDown(PointerEventData eventData)
	{
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this._rectTransform, eventData.position, this._mainCamera, out this._offset);
		this._time = Time.realtimeSinceStartup;
	}

	// Token: 0x0600024B RID: 587 RVA: 0x00010748 File Offset: 0x0000E948
	public void OnPointerUp(PointerEventData eventData)
	{
		if (Time.realtimeSinceStartup - this._time <= this._detectionTime)
		{
			this._debugCanvasController.EnablePanel();
		}
	}

	// Token: 0x040002BE RID: 702
	public const string PrefsSavedPosX = "DO-NOT-BACKUPDebugPosX";

	// Token: 0x040002BF RID: 703
	public const string PrefsSavedPosY = "DO-NOT-BACKUPDebugPosY";

	// Token: 0x040002C0 RID: 704
	private RectTransform _rectTransform;

	// Token: 0x040002C1 RID: 705
	private RectTransform _canvas;

	// Token: 0x040002C2 RID: 706
	private Camera _mainCamera;

	// Token: 0x040002C3 RID: 707
	private Vector2 _offset;

	// Token: 0x040002C4 RID: 708
	private float _detectionTime = 0.15f;

	// Token: 0x040002C5 RID: 709
	private float _time;

	// Token: 0x040002C6 RID: 710
	private DebugCanvasController _debugCanvasController;

	// Token: 0x040002C7 RID: 711
	private GameObject _widget;

	// Token: 0x040002C8 RID: 712
	private bool _cachedWidgetState;
}
