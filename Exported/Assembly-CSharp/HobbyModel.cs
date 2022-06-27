using System;
using UnityEngine;

// Token: 0x02000141 RID: 321
public class HobbyModel
{
	// Token: 0x0600084B RID: 2123 RVA: 0x0004BD98 File Offset: 0x00049F98
	public HobbyModel(string[] csv)
	{
		ResourceModel resource;
		if (csv.Length != 7)
		{
			Debug.LogError(csv[1] + " did not load correctly.");
		}
		else if (Universe.StringToResource.TryGetValue(csv[1], out resource))
		{
			this.Id = short.Parse(csv[0]);
			this.Resource = resource;
			this.InitialTime = double.Parse(csv[2]);
			this.IncreasePerLevel = double.Parse(csv[3]);
			this.SpriteKey = csv[4];
			this.Name = csv[5];
			this.TimeBlocks = int.Parse(csv[6]);
		}
		else
		{
			Debug.LogError("Resource " + csv[1] + " does not exist in the resource table.");
		}
	}

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x0600084C RID: 2124 RVA: 0x0004BE4C File Offset: 0x0004A04C
	// (set) Token: 0x0600084D RID: 2125 RVA: 0x0004BE54 File Offset: 0x0004A054
	public short Id { get; private set; }

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x0600084E RID: 2126 RVA: 0x0004BE60 File Offset: 0x0004A060
	// (set) Token: 0x0600084F RID: 2127 RVA: 0x0004BE68 File Offset: 0x0004A068
	public string Name { get; private set; }

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x06000850 RID: 2128 RVA: 0x0004BE74 File Offset: 0x0004A074
	// (set) Token: 0x06000851 RID: 2129 RVA: 0x0004BE7C File Offset: 0x0004A07C
	public ResourceModel Resource { get; private set; }

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x06000852 RID: 2130 RVA: 0x0004BE88 File Offset: 0x0004A088
	// (set) Token: 0x06000853 RID: 2131 RVA: 0x0004BE90 File Offset: 0x0004A090
	public double InitialTime { get; private set; }

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x06000854 RID: 2132 RVA: 0x0004BE9C File Offset: 0x0004A09C
	// (set) Token: 0x06000855 RID: 2133 RVA: 0x0004BEA4 File Offset: 0x0004A0A4
	public double IncreasePerLevel { get; private set; }

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x06000856 RID: 2134 RVA: 0x0004BEB0 File Offset: 0x0004A0B0
	// (set) Token: 0x06000857 RID: 2135 RVA: 0x0004BEB8 File Offset: 0x0004A0B8
	public string SpriteKey { get; private set; }

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x06000858 RID: 2136 RVA: 0x0004BEC4 File Offset: 0x0004A0C4
	// (set) Token: 0x06000859 RID: 2137 RVA: 0x0004BECC File Offset: 0x0004A0CC
	public int TimeBlocks { get; private set; }
}
