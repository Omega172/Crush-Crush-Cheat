using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000130 RID: 304
public class ButtonScale : EventTrigger
{
	// Token: 0x060007F1 RID: 2033 RVA: 0x0004AE08 File Offset: 0x00049008
	private bool Interactable()
	{
		Button component = base.GetComponent<Button>();
		return component == null || component.interactable;
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x0004AE30 File Offset: 0x00049030
	public override void OnPointerEnter(PointerEventData eventData)
	{
		if (!this.Interactable())
		{
			return;
		}
		base.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x0004AE68 File Offset: 0x00049068
	public override void OnPointerExit(PointerEventData eventData)
	{
		base.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	// Token: 0x060007F4 RID: 2036 RVA: 0x0004AE8C File Offset: 0x0004908C
	public override void OnPointerDown(PointerEventData eventData)
	{
		if (!this.Interactable())
		{
			return;
		}
		base.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x0004AEC4 File Offset: 0x000490C4
	public override void OnPointerUp(PointerEventData eventData)
	{
		base.transform.localScale = new Vector3(1f, 1f, 1f);
	}
}
