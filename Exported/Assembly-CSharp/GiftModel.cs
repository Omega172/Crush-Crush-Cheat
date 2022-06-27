using System;
using UnityEngine;

// Token: 0x02000145 RID: 325
public class GiftModel
{
	// Token: 0x060008B9 RID: 2233 RVA: 0x0004C940 File Offset: 0x0004AB40
	public GiftModel(string[] csv)
	{
		if (csv.Length != 16)
		{
			Debug.LogError(csv[0] + " did not load correctly.");
		}
		else
		{
			this.Name = csv[0];
			this.Id = short.Parse(csv[1]);
			this.Names = new LocalizedModel(new LocalizationContext(csv[2]), new LocalizationContext(csv[3]));
			this.SpriteKey = csv[4];
			this.Price = long.Parse(csv[5]);
			this.Hearts = long.Parse(csv[6]);
			this.DiamondCategory = int.Parse(csv[7]);
			this.Legacy = (csv[8].ToLowerInvariant().Trim() == "true");
			this.OnlyWhenRequired = (csv[9].ToLowerInvariant().Trim() == "true");
			this.ExploraSpriteKey = csv[10];
			if (!string.IsNullOrEmpty(csv[11]))
			{
				this.ExploraNames = new LocalizedModel(new LocalizationContext(csv[11]), new LocalizationContext(csv[12]));
			}
			this.MallorySpriteKey = csv[13];
			if (!string.IsNullOrEmpty(csv[14]))
			{
				this.MalloryNames = new LocalizedModel(new LocalizationContext(csv[14]), new LocalizationContext(csv[15]));
			}
		}
	}

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x060008BA RID: 2234 RVA: 0x0004CA98 File Offset: 0x0004AC98
	// (set) Token: 0x060008BB RID: 2235 RVA: 0x0004CAA0 File Offset: 0x0004ACA0
	public short Id { get; private set; }

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x060008BC RID: 2236 RVA: 0x0004CAAC File Offset: 0x0004ACAC
	// (set) Token: 0x060008BD RID: 2237 RVA: 0x0004CAB4 File Offset: 0x0004ACB4
	public string Name { get; private set; }

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x060008BE RID: 2238 RVA: 0x0004CAC0 File Offset: 0x0004ACC0
	// (set) Token: 0x060008BF RID: 2239 RVA: 0x0004CAC8 File Offset: 0x0004ACC8
	public string SpriteKey { get; private set; }

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x060008C0 RID: 2240 RVA: 0x0004CAD4 File Offset: 0x0004ACD4
	// (set) Token: 0x060008C1 RID: 2241 RVA: 0x0004CADC File Offset: 0x0004ACDC
	public long Price { get; private set; }

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x060008C2 RID: 2242 RVA: 0x0004CAE8 File Offset: 0x0004ACE8
	// (set) Token: 0x060008C3 RID: 2243 RVA: 0x0004CAF0 File Offset: 0x0004ACF0
	private long Hearts { get; set; }

	// Token: 0x060008C4 RID: 2244 RVA: 0x0004CAFC File Offset: 0x0004ACFC
	public long GetHearts(Balance.GirlName girl)
	{
		if (girl == Balance.GirlName.Mallory && this.Id == 24)
		{
			return 2835000000000000000L;
		}
		if (girl == Balance.GirlName.Explora && (this.Id == 5 || this.Id == 8))
		{
			return -this.Hearts;
		}
		return this.Hearts;
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x060008C5 RID: 2245 RVA: 0x0004CB58 File Offset: 0x0004AD58
	// (set) Token: 0x060008C6 RID: 2246 RVA: 0x0004CB60 File Offset: 0x0004AD60
	public Sprite Sprite { get; set; }

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x060008C7 RID: 2247 RVA: 0x0004CB6C File Offset: 0x0004AD6C
	// (set) Token: 0x060008C8 RID: 2248 RVA: 0x0004CB74 File Offset: 0x0004AD74
	public int DiamondCategory { get; set; }

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x060008C9 RID: 2249 RVA: 0x0004CB80 File Offset: 0x0004AD80
	// (set) Token: 0x060008CA RID: 2250 RVA: 0x0004CB88 File Offset: 0x0004AD88
	public bool Legacy { get; set; }

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x060008CB RID: 2251 RVA: 0x0004CB94 File Offset: 0x0004AD94
	// (set) Token: 0x060008CC RID: 2252 RVA: 0x0004CB9C File Offset: 0x0004AD9C
	public bool OnlyWhenRequired { get; set; }

	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x060008CD RID: 2253 RVA: 0x0004CBA8 File Offset: 0x0004ADA8
	// (set) Token: 0x060008CE RID: 2254 RVA: 0x0004CBB0 File Offset: 0x0004ADB0
	public string ExploraSpriteKey { get; set; }

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x060008CF RID: 2255 RVA: 0x0004CBBC File Offset: 0x0004ADBC
	// (set) Token: 0x060008D0 RID: 2256 RVA: 0x0004CBEC File Offset: 0x0004ADEC
	public Sprite ExploraSprite
	{
		get
		{
			return (!(this._exploraSprite == null)) ? this._exploraSprite : this.Sprite;
		}
		set
		{
			this._exploraSprite = value;
		}
	}

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x060008D1 RID: 2257 RVA: 0x0004CBF8 File Offset: 0x0004ADF8
	// (set) Token: 0x060008D2 RID: 2258 RVA: 0x0004CC00 File Offset: 0x0004AE00
	public string MallorySpriteKey { get; set; }

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x060008D3 RID: 2259 RVA: 0x0004CC0C File Offset: 0x0004AE0C
	// (set) Token: 0x060008D4 RID: 2260 RVA: 0x0004CC3C File Offset: 0x0004AE3C
	public Sprite MallorySprite
	{
		get
		{
			return (!(this._mallorySprite == null)) ? this._mallorySprite : this.Sprite;
		}
		set
		{
			this._mallorySprite = value;
		}
	}

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x060008D5 RID: 2261 RVA: 0x0004CC48 File Offset: 0x0004AE48
	// (set) Token: 0x060008D6 RID: 2262 RVA: 0x0004CC50 File Offset: 0x0004AE50
	public LocalizedModel ExploraNames { get; set; }

	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x060008D7 RID: 2263 RVA: 0x0004CC5C File Offset: 0x0004AE5C
	// (set) Token: 0x060008D8 RID: 2264 RVA: 0x0004CC64 File Offset: 0x0004AE64
	public LocalizedModel MalloryNames { get; set; }

	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x060008D9 RID: 2265 RVA: 0x0004CC70 File Offset: 0x0004AE70
	// (set) Token: 0x060008DA RID: 2266 RVA: 0x0004CC78 File Offset: 0x0004AE78
	public LocalizedModel Names { get; set; }

	// Token: 0x060008DB RID: 2267 RVA: 0x0004CC84 File Offset: 0x0004AE84
	public int GetGiftDiamondCost(int quantity)
	{
		int num = 1;
		if (quantity != 5)
		{
			if (quantity != 10)
			{
				if (quantity != 25)
				{
					if (quantity != 50)
					{
						if (quantity != 100)
						{
							if (quantity != 1000)
							{
								if (quantity != 10000)
								{
									if (quantity != 100000)
									{
										if (quantity == 1000000)
										{
											num = 10;
										}
									}
									else
									{
										num = 9;
									}
								}
								else
								{
									num = 8;
								}
							}
							else
							{
								num = 7;
							}
						}
						else
						{
							num = 6;
						}
					}
					else
					{
						num = 5;
					}
				}
				else
				{
					num = 4;
				}
			}
			else
			{
				num = 3;
			}
		}
		else
		{
			num = 2;
		}
		return num * this.DiamondCategory;
	}

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x060008DC RID: 2268 RVA: 0x0004CD38 File Offset: 0x0004AF38
	public Requirement.GiftType Requirement
	{
		get
		{
			return (Requirement.GiftType)(1 << (int)(this.Id - 1));
		}
	}

	// Token: 0x040008D9 RID: 2265
	private Sprite _exploraSprite;

	// Token: 0x040008DA RID: 2266
	private Sprite _mallorySprite;
}
