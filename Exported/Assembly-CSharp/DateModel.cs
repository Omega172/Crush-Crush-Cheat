using System;
using UnityEngine;

// Token: 0x02000142 RID: 322
public class DateModel : LocalizedModel
{
	// Token: 0x0600085A RID: 2138 RVA: 0x0004BED8 File Offset: 0x0004A0D8
	public DateModel(string[] csv)
	{
		if (csv.Length != 11)
		{
			Debug.LogError(csv[0] + " did not load correctly.");
		}
		else
		{
			this.Name = csv[0];
			this.Id = short.Parse(csv[1]);
			base.Singular = new LocalizationContext(csv[2]);
			base.Plural = new LocalizationContext(csv[3]);
			this.SpriteKey = csv[4];
			this.Price = long.Parse(csv[5]);
			this.Hearts = long.Parse(csv[6]);
			this.TimeBlocks = int.Parse(csv[7]);
			this.Time = double.Parse(csv[8]);
			this.DiamondCost = int.Parse(csv[9]);
			this.Phrase = csv[3];
			this.IconSpriteKey = csv[10];
			this.DateType = (Requirement.DateType)((int)Enum.Parse(typeof(Requirement.DateType), this.Name));
		}
	}

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x0600085B RID: 2139 RVA: 0x0004BFC4 File Offset: 0x0004A1C4
	// (set) Token: 0x0600085C RID: 2140 RVA: 0x0004BFCC File Offset: 0x0004A1CC
	public int TimeBlocks { get; private set; }

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x0600085D RID: 2141 RVA: 0x0004BFD8 File Offset: 0x0004A1D8
	// (set) Token: 0x0600085E RID: 2142 RVA: 0x0004BFE0 File Offset: 0x0004A1E0
	public long Price { get; private set; }

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x0600085F RID: 2143 RVA: 0x0004BFEC File Offset: 0x0004A1EC
	// (set) Token: 0x06000860 RID: 2144 RVA: 0x0004BFF4 File Offset: 0x0004A1F4
	public int DiamondCost { get; private set; }

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x06000861 RID: 2145 RVA: 0x0004C000 File Offset: 0x0004A200
	// (set) Token: 0x06000862 RID: 2146 RVA: 0x0004C008 File Offset: 0x0004A208
	public long Hearts { get; private set; }

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x06000863 RID: 2147 RVA: 0x0004C014 File Offset: 0x0004A214
	// (set) Token: 0x06000864 RID: 2148 RVA: 0x0004C01C File Offset: 0x0004A21C
	public double Time { get; private set; }

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x06000865 RID: 2149 RVA: 0x0004C028 File Offset: 0x0004A228
	// (set) Token: 0x06000866 RID: 2150 RVA: 0x0004C030 File Offset: 0x0004A230
	public short Id { get; private set; }

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x06000867 RID: 2151 RVA: 0x0004C03C File Offset: 0x0004A23C
	// (set) Token: 0x06000868 RID: 2152 RVA: 0x0004C044 File Offset: 0x0004A244
	public string Name { get; private set; }

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x06000869 RID: 2153 RVA: 0x0004C050 File Offset: 0x0004A250
	// (set) Token: 0x0600086A RID: 2154 RVA: 0x0004C058 File Offset: 0x0004A258
	public string SpriteKey { get; private set; }

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x0600086B RID: 2155 RVA: 0x0004C064 File Offset: 0x0004A264
	// (set) Token: 0x0600086C RID: 2156 RVA: 0x0004C06C File Offset: 0x0004A26C
	public string IconSpriteKey { get; private set; }

	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x0600086D RID: 2157 RVA: 0x0004C078 File Offset: 0x0004A278
	// (set) Token: 0x0600086E RID: 2158 RVA: 0x0004C080 File Offset: 0x0004A280
	public string Phrase { get; private set; }

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x0600086F RID: 2159 RVA: 0x0004C08C File Offset: 0x0004A28C
	// (set) Token: 0x06000870 RID: 2160 RVA: 0x0004C094 File Offset: 0x0004A294
	public Requirement.DateType DateType { get; private set; }

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x06000871 RID: 2161 RVA: 0x0004C0A0 File Offset: 0x0004A2A0
	// (set) Token: 0x06000872 RID: 2162 RVA: 0x0004C0A8 File Offset: 0x0004A2A8
	public Sprite Sprite1 { get; set; }

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x06000873 RID: 2163 RVA: 0x0004C0B4 File Offset: 0x0004A2B4
	// (set) Token: 0x06000874 RID: 2164 RVA: 0x0004C0BC File Offset: 0x0004A2BC
	public Sprite Sprite2 { get; set; }

	// Token: 0x170000CC RID: 204
	// (get) Token: 0x06000875 RID: 2165 RVA: 0x0004C0C8 File Offset: 0x0004A2C8
	// (set) Token: 0x06000876 RID: 2166 RVA: 0x0004C0D0 File Offset: 0x0004A2D0
	public Sprite Icon { get; set; }
}
