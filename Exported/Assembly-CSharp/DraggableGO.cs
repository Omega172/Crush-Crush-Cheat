using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000134 RID: 308
public class DraggableGO : MonoBehaviour, IDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
	// Token: 0x06000801 RID: 2049 RVA: 0x0004B040 File Offset: 0x00049240
	private void Start()
	{
		this._mainCamera = Camera.main;
		this._rectTransform = base.GetComponent<RectTransform>();
		this._canvas = this._rectTransform.parent.GetComponent<RectTransform>();
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x0004B07C File Offset: 0x0004927C
	public void OnBeginDrag(PointerEventData eventData)
	{
	}

	// Token: 0x06000803 RID: 2051 RVA: 0x0004B080 File Offset: 0x00049280
	public void OnDrag(PointerEventData eventData)
	{
		Vector2 a;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this._canvas, eventData.position, this._mainCamera, out a);
		Vector3 position = this._canvas.transform.TransformPoint(a - this._offset);
		position = new Vector3(Mathf.Min((float)(Screen.width / 2 - 250), Mathf.Max((float)(-(float)Screen.width / 2 + 250), position.x)), 0f, position.z);
		base.transform.position = position;
		base.transform.parent.Find("Phone UI").position = new Vector3(base.transform.position.x + 2f, base.transform.position.y, base.transform.position.z);
		base.transform.parent.Find("Foreground").position = base.transform.position;
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x0004B19C File Offset: 0x0004939C
	public void OnEndDrag(PointerEventData eventData)
	{
		this._rectTransform.localScale = Vector3.one;
		GameState.CurrentState.QueueSave();
	}

	// Token: 0x06000805 RID: 2053 RVA: 0x0004B1B8 File Offset: 0x000493B8
	public void OnPointerDown(PointerEventData eventData)
	{
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this._rectTransform, eventData.position, this._mainCamera, out this._offset);
		this._time = Time.realtimeSinceStartup;
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x0004B1E4 File Offset: 0x000493E4
	public void OnPointerUp(PointerEventData eventData)
	{
	}

	// Token: 0x0400087B RID: 2171
	private RectTransform _rectTransform;

	// Token: 0x0400087C RID: 2172
	private RectTransform _canvas;

	// Token: 0x0400087D RID: 2173
	private Camera _mainCamera;

	// Token: 0x0400087E RID: 2174
	private Vector2 _offset;

	// Token: 0x0400087F RID: 2175
	private float _detectionTime = 0.15f;

	// Token: 0x04000880 RID: 2176
	private float _time;
}
