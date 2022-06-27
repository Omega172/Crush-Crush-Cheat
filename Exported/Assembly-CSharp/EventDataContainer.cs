using System;
using UnityEngine;

// Token: 0x020000D7 RID: 215
[Serializable]
public abstract class EventDataContainer
{
	// Token: 0x060004C8 RID: 1224
	public abstract void Deserialize(Transform rootTransform, Transform targetTransform, EventDataContainer dataContainer);
}
