using System;

// Token: 0x0200013C RID: 316
public class LocalizationContext
{
	// Token: 0x06000823 RID: 2083 RVA: 0x0004B964 File Offset: 0x00049B64
	public LocalizationContext(string english)
	{
		this.English = english;
		this.Id = string.Empty;
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x0004B980 File Offset: 0x00049B80
	public LocalizationContext(string english, string id)
	{
		this.English = english;
		this.Id = id;
	}

	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x06000825 RID: 2085 RVA: 0x0004B998 File Offset: 0x00049B98
	// (set) Token: 0x06000826 RID: 2086 RVA: 0x0004B9A0 File Offset: 0x00049BA0
	public string English { get; private set; }

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x06000827 RID: 2087 RVA: 0x0004B9AC File Offset: 0x00049BAC
	// (set) Token: 0x06000828 RID: 2088 RVA: 0x0004B9B4 File Offset: 0x00049BB4
	public string Id { get; private set; }
}
