using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000D4 RID: 212
[RequireComponent(typeof(Button))]
public class EventButtonCallbackController : EventDataContainerController
{
	// Token: 0x060004C1 RID: 1217 RVA: 0x00026604 File Offset: 0x00024804
	public override bool CanSerialize()
	{
		return base.GetComponent<Button>() != null;
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x00026614 File Offset: 0x00024814
	public override EventDataContainer GetDataContainer()
	{
		return this.dataContainer;
	}

	// Token: 0x060004C3 RID: 1219 RVA: 0x0002661C File Offset: 0x0002481C
	public override string GetDataContainerType()
	{
		return typeof(EventButtonCallbackData).ToString();
	}

	// Token: 0x040004DE RID: 1246
	[SerializeField]
	private EventButtonCallbackData dataContainer;
}
