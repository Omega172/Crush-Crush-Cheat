using System;
using UnityEngine;

// Token: 0x0200013E RID: 318
public class ResourceModel : LocalizedModel
{
	// Token: 0x06000830 RID: 2096 RVA: 0x0004BA34 File Offset: 0x00049C34
	public ResourceModel(string[] csv)
	{
		if (csv.Length != 5)
		{
			Debug.LogError(csv[0] + " did not load correctly.");
		}
		else
		{
			this.Name = csv[0];
			this.NameToLower = this.Name.ToLowerInvariant();
			this.Id = short.Parse(csv[1]);
			base.Singular = new LocalizationContext(csv[2]);
			base.Plural = new LocalizationContext(csv[3]);
			this.SpriteKey = csv[4];
		}
	}

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x06000831 RID: 2097 RVA: 0x0004BAB4 File Offset: 0x00049CB4
	// (set) Token: 0x06000832 RID: 2098 RVA: 0x0004BABC File Offset: 0x00049CBC
	public short Id { get; private set; }

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x06000833 RID: 2099 RVA: 0x0004BAC8 File Offset: 0x00049CC8
	// (set) Token: 0x06000834 RID: 2100 RVA: 0x0004BAD0 File Offset: 0x00049CD0
	public string Name { get; private set; }

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x06000835 RID: 2101 RVA: 0x0004BADC File Offset: 0x00049CDC
	// (set) Token: 0x06000836 RID: 2102 RVA: 0x0004BAE4 File Offset: 0x00049CE4
	public string NameToLower { get; private set; }

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x06000837 RID: 2103 RVA: 0x0004BAF0 File Offset: 0x00049CF0
	// (set) Token: 0x06000838 RID: 2104 RVA: 0x0004BAF8 File Offset: 0x00049CF8
	public string SpriteKey { get; private set; }
}
