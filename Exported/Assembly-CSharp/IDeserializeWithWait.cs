using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000DD RID: 221
public interface IDeserializeWithWait
{
	// Token: 0x060004DB RID: 1243
	IEnumerator Deserialize(Transform rootTransform, Transform targetTransform, EventDataContainer dataContainer);
}
