using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000146 RID: 326
public class OutfitModel : LocalizedModel
{
	// Token: 0x060008DD RID: 2269 RVA: 0x0004CD48 File Offset: 0x0004AF48
	public OutfitModel(string[] csv)
	{
		if (csv.Length != 9)
		{
			Debug.LogError(csv[0] + " did not load correctly.");
		}
		else
		{
			this.Name = csv[0];
			this.Id = short.Parse(csv[1]);
			base.Singular = new LocalizationContext(csv[2]);
			this.Girls = new List<short>();
			string[] array = csv[3].Split(new char[]
			{
				','
			});
			foreach (string s in array)
			{
				this.Girls.Add(short.Parse(s));
			}
			this.SpriteKey = csv[4];
			this.Price = double.Parse(csv[5]);
			this.Available = (csv[6] == "TRUE");
			this.AssetName = csv[7];
			this.LegacyId = int.Parse(csv[8]);
		}
	}

	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x060008DE RID: 2270 RVA: 0x0004CE38 File Offset: 0x0004B038
	// (set) Token: 0x060008DF RID: 2271 RVA: 0x0004CE40 File Offset: 0x0004B040
	public short Id { get; private set; }

	// Token: 0x170000FA RID: 250
	// (get) Token: 0x060008E0 RID: 2272 RVA: 0x0004CE4C File Offset: 0x0004B04C
	// (set) Token: 0x060008E1 RID: 2273 RVA: 0x0004CE54 File Offset: 0x0004B054
	public string Name { get; private set; }

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x060008E2 RID: 2274 RVA: 0x0004CE60 File Offset: 0x0004B060
	// (set) Token: 0x060008E3 RID: 2275 RVA: 0x0004CE68 File Offset: 0x0004B068
	public string SpriteKey { get; private set; }

	// Token: 0x170000FC RID: 252
	// (get) Token: 0x060008E4 RID: 2276 RVA: 0x0004CE74 File Offset: 0x0004B074
	// (set) Token: 0x060008E5 RID: 2277 RVA: 0x0004CE7C File Offset: 0x0004B07C
	public double Price { get; private set; }

	// Token: 0x170000FD RID: 253
	// (get) Token: 0x060008E6 RID: 2278 RVA: 0x0004CE88 File Offset: 0x0004B088
	// (set) Token: 0x060008E7 RID: 2279 RVA: 0x0004CE90 File Offset: 0x0004B090
	public bool Available { get; private set; }

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x060008E8 RID: 2280 RVA: 0x0004CE9C File Offset: 0x0004B09C
	// (set) Token: 0x060008E9 RID: 2281 RVA: 0x0004CEA4 File Offset: 0x0004B0A4
	public string AssetName { get; private set; }

	// Token: 0x170000FF RID: 255
	// (get) Token: 0x060008EA RID: 2282 RVA: 0x0004CEB0 File Offset: 0x0004B0B0
	// (set) Token: 0x060008EB RID: 2283 RVA: 0x0004CEB8 File Offset: 0x0004B0B8
	public List<short> Girls { get; private set; }

	// Token: 0x17000100 RID: 256
	// (get) Token: 0x060008EC RID: 2284 RVA: 0x0004CEC4 File Offset: 0x0004B0C4
	// (set) Token: 0x060008ED RID: 2285 RVA: 0x0004CECC File Offset: 0x0004B0CC
	public Sprite Sprite { get; set; }

	// Token: 0x17000101 RID: 257
	// (get) Token: 0x060008EE RID: 2286 RVA: 0x0004CED8 File Offset: 0x0004B0D8
	// (set) Token: 0x060008EF RID: 2287 RVA: 0x0004CEE0 File Offset: 0x0004B0E0
	public int LegacyId { get; set; }

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x060008F0 RID: 2288 RVA: 0x0004CEEC File Offset: 0x0004B0EC
	public Requirement.OutfitType LegacyOutfitType
	{
		get
		{
			return (Requirement.OutfitType)this.LegacyId;
		}
	}
}
