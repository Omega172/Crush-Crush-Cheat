using System;
using UnityEngine;

// Token: 0x02000163 RID: 355
public class EnumFlagAttribute : PropertyAttribute
{
	// Token: 0x06000A25 RID: 2597 RVA: 0x00053B1C File Offset: 0x00051D1C
	public EnumFlagAttribute()
	{
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x00053B24 File Offset: 0x00051D24
	public EnumFlagAttribute(string name)
	{
		this.enumName = name;
	}

	// Token: 0x040009D0 RID: 2512
	public string enumName;
}
