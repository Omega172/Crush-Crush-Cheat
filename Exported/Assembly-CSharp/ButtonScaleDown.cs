using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000131 RID: 305
public class ButtonScaleDown : EventTrigger
{
	// Token: 0x060007F7 RID: 2039 RVA: 0x0004AEF0 File Offset: 0x000490F0
	private bool Interactable()
	{
		Button component = base.GetComponent<Button>();
		return component == null || component.interactable;
	}

	// Token: 0x060007F8 RID: 2040 RVA: 0x0004AF18 File Offset: 0x00049118
	public override void OnPointerDown(PointerEventData eventData)
	{
		if (!this.Interactable())
		{
			return;
		}
		base.transform.localScale = new Vector3(0.99f, 0.99f, 0.99f);
	}

	// Token: 0x060007F9 RID: 2041 RVA: 0x0004AF50 File Offset: 0x00049150
	public override void OnPointerUp(PointerEventData eventData)
	{
		base.transform.localScale = new Vector3(1f, 1f, 1f);
	}
}
