using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

// Token: 0x02000147 RID: 327
public class StatsModel
{
	// Token: 0x060008F1 RID: 2289 RVA: 0x0004CEF4 File Offset: 0x0004B0F4
	public StatsModel(string[] csv)
	{
		if (csv.Length != 13)
		{
			Debug.LogError(csv[0] + " did not load correctly.");
		}
		else
		{
			if (csv[0].Contains("nsfw"))
			{
				string s = csv[0].Substring(0, csv[0].IndexOf('_'));
				this.Id = 1000 + short.Parse(s);
			}
			else
			{
				this.Id = short.Parse(csv[0]);
			}
			this.Age = csv[1];
			this.Birthday = csv[2];
			if (this.Birthday.Contains("00:00:00"))
			{
				this.Birthday = this.Birthday.Substring(0, this.Birthday.IndexOf("00:00:00"));
				DateTime dateTime = DateTime.Parse(this.Birthday);
				this.Birthday = string.Format("{0} {1}", new DateTimeFormatInfo().GetMonthName(dateTime.Month), dateTime.Day.ToString());
			}
			this.FavouriteHobby = csv[3];
			this.BloodType = csv[4];
			foreach (KeyValuePair<short, JobModel> keyValuePair in Universe.Jobs)
			{
				if (keyValuePair.Value.Name == csv[5])
				{
					this.FavouriteJob = keyValuePair.Value;
				}
			}
			this.FavouriteFood = csv[6];
			foreach (KeyValuePair<short, GiftModel> keyValuePair2 in Universe.Gifts)
			{
				if (keyValuePair2.Value.Name == csv[7])
				{
					this.FavouriteGift = keyValuePair2.Value;
				}
			}
			this.Occupation = csv[8];
			foreach (KeyValuePair<short, HobbyModel> keyValuePair3 in Universe.Hobbies)
			{
				if (keyValuePair3.Value.Resource.Name == csv[9])
				{
					this.FavouriteSkill = keyValuePair3.Value;
				}
			}
			this.Bust = csv[10];
			this.PrestigePerLevel = float.Parse(csv[11]);
		}
	}

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x060008F2 RID: 2290 RVA: 0x0004D1A0 File Offset: 0x0004B3A0
	// (set) Token: 0x060008F3 RID: 2291 RVA: 0x0004D1A8 File Offset: 0x0004B3A8
	public short Id { get; private set; }

	// Token: 0x17000104 RID: 260
	// (get) Token: 0x060008F4 RID: 2292 RVA: 0x0004D1B4 File Offset: 0x0004B3B4
	// (set) Token: 0x060008F5 RID: 2293 RVA: 0x0004D1BC File Offset: 0x0004B3BC
	public string Age { get; private set; }

	// Token: 0x17000105 RID: 261
	// (get) Token: 0x060008F6 RID: 2294 RVA: 0x0004D1C8 File Offset: 0x0004B3C8
	// (set) Token: 0x060008F7 RID: 2295 RVA: 0x0004D1D0 File Offset: 0x0004B3D0
	public string Birthday { get; private set; }

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x060008F8 RID: 2296 RVA: 0x0004D1DC File Offset: 0x0004B3DC
	// (set) Token: 0x060008F9 RID: 2297 RVA: 0x0004D1E4 File Offset: 0x0004B3E4
	public string FavouriteHobby { get; private set; }

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x060008FA RID: 2298 RVA: 0x0004D1F0 File Offset: 0x0004B3F0
	// (set) Token: 0x060008FB RID: 2299 RVA: 0x0004D1F8 File Offset: 0x0004B3F8
	public string BloodType { get; private set; }

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x060008FC RID: 2300 RVA: 0x0004D204 File Offset: 0x0004B404
	// (set) Token: 0x060008FD RID: 2301 RVA: 0x0004D20C File Offset: 0x0004B40C
	public JobModel FavouriteJob { get; private set; }

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x060008FE RID: 2302 RVA: 0x0004D218 File Offset: 0x0004B418
	// (set) Token: 0x060008FF RID: 2303 RVA: 0x0004D220 File Offset: 0x0004B420
	public string FavouriteFood { get; private set; }

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x06000900 RID: 2304 RVA: 0x0004D22C File Offset: 0x0004B42C
	// (set) Token: 0x06000901 RID: 2305 RVA: 0x0004D234 File Offset: 0x0004B434
	public GiftModel FavouriteGift { get; private set; }

	// Token: 0x1700010B RID: 267
	// (get) Token: 0x06000902 RID: 2306 RVA: 0x0004D240 File Offset: 0x0004B440
	// (set) Token: 0x06000903 RID: 2307 RVA: 0x0004D248 File Offset: 0x0004B448
	public string Occupation { get; private set; }

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x06000904 RID: 2308 RVA: 0x0004D254 File Offset: 0x0004B454
	// (set) Token: 0x06000905 RID: 2309 RVA: 0x0004D25C File Offset: 0x0004B45C
	public HobbyModel FavouriteSkill { get; private set; }

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x06000906 RID: 2310 RVA: 0x0004D268 File Offset: 0x0004B468
	// (set) Token: 0x06000907 RID: 2311 RVA: 0x0004D270 File Offset: 0x0004B470
	public string Bust { get; private set; }

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x06000908 RID: 2312 RVA: 0x0004D27C File Offset: 0x0004B47C
	// (set) Token: 0x06000909 RID: 2313 RVA: 0x0004D284 File Offset: 0x0004B484
	public float PrestigePerLevel { get; private set; }
}
