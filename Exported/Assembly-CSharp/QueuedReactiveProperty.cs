using System;
using System.Collections.Generic;

// Token: 0x02000166 RID: 358
public class QueuedReactiveProperty
{
	// Token: 0x06000A43 RID: 2627 RVA: 0x00053D50 File Offset: 0x00051F50
	public static void ProcessQueue()
	{
		for (int i = 0; i < QueuedReactiveProperty.queuedReactiveProperties.Count; i++)
		{
			QueuedReactiveProperty queuedReactiveProperty = QueuedReactiveProperty.queuedReactiveProperties[i];
			if (queuedReactiveProperty.dirty)
			{
				queuedReactiveProperty.Invoke();
			}
		}
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x00053D98 File Offset: 0x00051F98
	protected virtual void Invoke()
	{
	}

	// Token: 0x040009D5 RID: 2517
	protected static List<QueuedReactiveProperty> queuedReactiveProperties = new List<QueuedReactiveProperty>();

	// Token: 0x040009D6 RID: 2518
	protected bool dirty;
}
