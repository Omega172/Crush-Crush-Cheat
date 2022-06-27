using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000148 RID: 328
public class AchievementModel
{
	// Token: 0x0600090A RID: 2314 RVA: 0x0004D290 File Offset: 0x0004B490
	public AchievementModel(string[] csv)
	{
		if (csv.Length < 12)
		{
			Debug.LogError(csv[1] + " did not load correctly.");
		}
		else
		{
			this.Name = csv[1];
			this.Id = short.Parse(csv[0]);
			this.Description = csv[2];
			this.Notification = csv[3];
			this.Requirement = (AchievementModel.AchievementType)((int)Enum.Parse(typeof(AchievementModel.AchievementType), csv[4]));
			if (this.Requirement != AchievementModel.AchievementType.Special)
			{
				this.Value = double.Parse(csv[6]);
			}
			this.ValueString = csv[6];
			this.Reward = int.Parse(csv[7]);
			this.SpriteKey = csv[8];
			short num = 0;
			short.TryParse(csv[5], out num);
			if (num == 0 && this.Requirement == AchievementModel.AchievementType.SoftCurrency)
			{
				csv[5] = csv[5].ToLowerInvariant();
				if (csv[5] == "totalincome")
				{
					csv[5] = "totalmoney";
				}
				foreach (KeyValuePair<short, ResourceModel> keyValuePair in Universe.Resources)
				{
					if (keyValuePair.Value.NameToLower == csv[5])
					{
						num = keyValuePair.Key;
					}
				}
			}
			this.Target = num;
			this.Category = csv[9];
			if (!string.IsNullOrEmpty(csv[10]))
			{
				this.Localization = int.Parse(csv[10]);
			}
			this.CategoryId = int.Parse(csv[11]);
			if (this.Target == -1)
			{
				Debug.LogWarning("Could not find target " + this.Category + " for achievement type " + csv[3]);
			}
			if (!Achievements.Categories.Contains(this.Category))
			{
				Achievements.Categories.Add(this.Category, new Achievements.CategorySort(this.Category, this.CategoryId));
			}
		}
	}

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x0600090B RID: 2315 RVA: 0x0004D498 File Offset: 0x0004B698
	// (set) Token: 0x0600090C RID: 2316 RVA: 0x0004D4A0 File Offset: 0x0004B6A0
	public short Id { get; private set; }

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x0600090D RID: 2317 RVA: 0x0004D4AC File Offset: 0x0004B6AC
	// (set) Token: 0x0600090E RID: 2318 RVA: 0x0004D4B4 File Offset: 0x0004B6B4
	public string Name { get; private set; }

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x0600090F RID: 2319 RVA: 0x0004D4C0 File Offset: 0x0004B6C0
	// (set) Token: 0x06000910 RID: 2320 RVA: 0x0004D4C8 File Offset: 0x0004B6C8
	public string Description { get; private set; }

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x06000911 RID: 2321 RVA: 0x0004D4D4 File Offset: 0x0004B6D4
	// (set) Token: 0x06000912 RID: 2322 RVA: 0x0004D4DC File Offset: 0x0004B6DC
	public string Notification { get; private set; }

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x06000913 RID: 2323 RVA: 0x0004D4E8 File Offset: 0x0004B6E8
	// (set) Token: 0x06000914 RID: 2324 RVA: 0x0004D4F0 File Offset: 0x0004B6F0
	public AchievementModel.AchievementType Requirement { get; private set; }

	// Token: 0x17000114 RID: 276
	// (get) Token: 0x06000915 RID: 2325 RVA: 0x0004D4FC File Offset: 0x0004B6FC
	// (set) Token: 0x06000916 RID: 2326 RVA: 0x0004D504 File Offset: 0x0004B704
	public short Target { get; private set; }

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x06000917 RID: 2327 RVA: 0x0004D510 File Offset: 0x0004B710
	// (set) Token: 0x06000918 RID: 2328 RVA: 0x0004D518 File Offset: 0x0004B718
	public string Category { get; private set; }

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x06000919 RID: 2329 RVA: 0x0004D524 File Offset: 0x0004B724
	// (set) Token: 0x0600091A RID: 2330 RVA: 0x0004D52C File Offset: 0x0004B72C
	public double Value { get; private set; }

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x0600091B RID: 2331 RVA: 0x0004D538 File Offset: 0x0004B738
	// (set) Token: 0x0600091C RID: 2332 RVA: 0x0004D540 File Offset: 0x0004B740
	public string ValueString { get; private set; }

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x0600091D RID: 2333 RVA: 0x0004D54C File Offset: 0x0004B74C
	// (set) Token: 0x0600091E RID: 2334 RVA: 0x0004D554 File Offset: 0x0004B754
	public int Reward { get; private set; }

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x0600091F RID: 2335 RVA: 0x0004D560 File Offset: 0x0004B760
	// (set) Token: 0x06000920 RID: 2336 RVA: 0x0004D568 File Offset: 0x0004B768
	public string SpriteKey { get; private set; }

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x06000921 RID: 2337 RVA: 0x0004D574 File Offset: 0x0004B774
	// (set) Token: 0x06000922 RID: 2338 RVA: 0x0004D57C File Offset: 0x0004B77C
	public Sprite Sprite { get; private set; }

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x06000923 RID: 2339 RVA: 0x0004D588 File Offset: 0x0004B788
	// (set) Token: 0x06000924 RID: 2340 RVA: 0x0004D590 File Offset: 0x0004B790
	public int Localization { get; private set; }

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x06000925 RID: 2341 RVA: 0x0004D59C File Offset: 0x0004B79C
	// (set) Token: 0x06000926 RID: 2342 RVA: 0x0004D5A4 File Offset: 0x0004B7A4
	public int CategoryId { get; private set; }

	// Token: 0x02000149 RID: 329
	public enum AchievementType
	{
		// Token: 0x0400090D RID: 2317
		SoftCurrency,
		// Token: 0x0400090E RID: 2318
		Special,
		// Token: 0x0400090F RID: 2319
		Hobby,
		// Token: 0x04000910 RID: 2320
		Hobby2,
		// Token: 0x04000911 RID: 2321
		JobLevel,
		// Token: 0x04000912 RID: 2322
		GirlLevel
	}
}
