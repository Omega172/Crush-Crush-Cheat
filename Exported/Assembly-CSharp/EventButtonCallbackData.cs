using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000D5 RID: 213
[Serializable]
public class EventButtonCallbackData : EventDataContainer
{
	// Token: 0x060004C5 RID: 1221 RVA: 0x00026638 File Offset: 0x00024838
	public override void Deserialize(Transform rootTransform, Transform targetTransform, EventDataContainer dataContainer)
	{
		Button component = targetTransform.GetComponent<Button>();
		if (component == null)
		{
			Debug.LogError("No button present on: " + targetTransform.name);
			return;
		}
		EventPopupButtonProxy buttonProxy = rootTransform.GetComponent<EventPopupButtonProxy>();
		if (buttonProxy == null)
		{
			buttonProxy = rootTransform.gameObject.AddComponent<EventPopupButtonProxy>();
		}
		ButtonCallbackType[] onClickCallbacks = this.OnClickCallbacks;
		for (int i = 0; i < onClickCallbacks.Length; i++)
		{
			ButtonCallbackType temp2 = onClickCallbacks[i];
			ButtonCallbackType temp = temp2;
			Debug.Log("Tried to register onClick with callback " + temp);
			component.onClick.AddListener(delegate()
			{
				buttonProxy.OnButtonEvent(temp);
			});
		}
	}

	// Token: 0x040004DF RID: 1247
	public ButtonCallbackType[] OnClickCallbacks;
}
