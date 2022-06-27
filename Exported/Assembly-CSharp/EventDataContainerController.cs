using System;
using UnityEngine;

// Token: 0x020000D8 RID: 216
[Serializable]
public abstract class EventDataContainerController : MonoBehaviour
{
	// Token: 0x060004CA RID: 1226
	public abstract EventDataContainer GetDataContainer();

	// Token: 0x060004CB RID: 1227
	public abstract string GetDataContainerType();

	// Token: 0x060004CC RID: 1228 RVA: 0x00026728 File Offset: 0x00024928
	public virtual bool CanSerialize()
	{
		return true;
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x0002672C File Offset: 0x0002492C
	public static string GetPathToParent(Transform targetTransform, Transform parent)
	{
		Transform transform = targetTransform;
		string text = string.Empty;
		while (transform != parent && transform != null)
		{
			if (transform != null)
			{
				text = "/" + transform.name + text;
			}
			transform = transform.parent;
		}
		if (!string.IsNullOrEmpty(text))
		{
			text = text.Remove(0, 1);
		}
		return text;
	}
}
