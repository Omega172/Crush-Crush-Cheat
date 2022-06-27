using System;
using System.Collections.Generic;

// Token: 0x02000157 RID: 343
public class CreditsModel : LocalizedModel
{
	// Token: 0x0600098E RID: 2446 RVA: 0x00050298 File Offset: 0x0004E498
	public CreditsModel(string category, List<string> names, string sprite = null)
	{
		this.Category = category;
		this.Names = names;
		this.Sprite = sprite;
	}

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x0600098F RID: 2447 RVA: 0x000502C0 File Offset: 0x0004E4C0
	// (set) Token: 0x06000990 RID: 2448 RVA: 0x000502C8 File Offset: 0x0004E4C8
	public string Category { get; private set; }

	// Token: 0x17000142 RID: 322
	// (get) Token: 0x06000991 RID: 2449 RVA: 0x000502D4 File Offset: 0x0004E4D4
	// (set) Token: 0x06000992 RID: 2450 RVA: 0x000502DC File Offset: 0x0004E4DC
	public List<string> Names { get; private set; }

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x06000993 RID: 2451 RVA: 0x000502E8 File Offset: 0x0004E4E8
	// (set) Token: 0x06000994 RID: 2452 RVA: 0x000502F0 File Offset: 0x0004E4F0
	public string Sprite { get; private set; }

	// Token: 0x17000144 RID: 324
	// (get) Token: 0x06000995 RID: 2453 RVA: 0x000502FC File Offset: 0x0004E4FC
	public bool IsPortraitType
	{
		get
		{
			return !string.IsNullOrEmpty(this.Sprite);
		}
	}
}
