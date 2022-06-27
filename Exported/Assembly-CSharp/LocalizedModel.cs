using System;

// Token: 0x0200013D RID: 317
public class LocalizedModel
{
	// Token: 0x06000829 RID: 2089 RVA: 0x0004B9C0 File Offset: 0x00049BC0
	public LocalizedModel()
	{
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x0004B9C8 File Offset: 0x00049BC8
	public LocalizedModel(LocalizationContext singular, LocalizationContext plural)
	{
		this.Singular = singular;
		this.Plural = plural;
	}

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x0600082B RID: 2091 RVA: 0x0004B9E0 File Offset: 0x00049BE0
	// (set) Token: 0x0600082C RID: 2092 RVA: 0x0004B9E8 File Offset: 0x00049BE8
	public LocalizationContext Singular { get; protected set; }

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x0600082D RID: 2093 RVA: 0x0004B9F4 File Offset: 0x00049BF4
	// (set) Token: 0x0600082E RID: 2094 RVA: 0x0004B9FC File Offset: 0x00049BFC
	public LocalizationContext Plural { get; protected set; }

	// Token: 0x0600082F RID: 2095 RVA: 0x0004BA08 File Offset: 0x00049C08
	public string GetLocalizedToken(int quantity)
	{
		if (quantity == 1)
		{
			return this.Singular.English;
		}
		return this.Plural.English;
	}
}
