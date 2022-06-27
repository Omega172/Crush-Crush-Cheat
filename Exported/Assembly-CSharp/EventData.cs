using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000143 RID: 323
public class EventData : LocalizedModel, IEquatable<EventData>
{
	// Token: 0x06000877 RID: 2167 RVA: 0x0004C0DC File Offset: 0x0004A2DC
	public EventData(string[] csv, List<TaskManager.EventRewardType> rewardType, List<long> rewardAmount)
	{
		if (csv.Length != 19)
		{
			Debug.LogError(csv[0] + " did not load correctly.");
		}
		else
		{
			this.Name = csv[0];
			this.EventID = short.Parse(csv[1]);
			this.PreviousEventID = this.EventID - 1;
			int year = int.Parse(csv[2]);
			int month = int.Parse(csv[3]);
			int day = (int)byte.Parse(csv[4]);
			this.StartTimeUTC = new DateTime(year, month, day, 0, 0, 0);
			year = int.Parse(csv[5]);
			month = int.Parse(csv[6]);
			day = (int)byte.Parse(csv[7]);
			this.EndTimeUTC = new DateTime(year, month, day, 0, 0, 0);
			this.TokenRequirement = int.Parse(csv[8]);
			this.FinishCost = int.Parse(csv[9]);
			this.RewardText = csv[13];
			string[] array = csv[14].Split(new char[]
			{
				' '
			});
			this.RewardGirlsID = new short[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				this.RewardGirlsID[i] = short.Parse(array[i]);
			}
			this.MissedEventBlurb = csv[15];
			this.CostPerToken = ((!string.IsNullOrEmpty(csv[16])) ? int.Parse(csv[16]) : ((this.TokenRequirement <= 7) ? 7 : 15));
			this.EpicTask = Universe.Tasks[0];
			this.RewardType = rewardType;
			this.RewardAmount = rewardAmount;
			this.RewardSpriteName = csv[12];
			if (this.RewardSpriteName.StartsWith("LTEreward_outfits"))
			{
				this.RewardSpriteName = "LTEreward_outfits";
			}
			this.AssetBundleName = csv[18];
			this.IconName = csv[17];
		}
	}

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x06000878 RID: 2168 RVA: 0x0004C2D0 File Offset: 0x0004A4D0
	// (set) Token: 0x06000879 RID: 2169 RVA: 0x0004C2D8 File Offset: 0x0004A4D8
	public string Name { get; private set; }

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x0600087A RID: 2170 RVA: 0x0004C2E4 File Offset: 0x0004A4E4
	// (set) Token: 0x0600087B RID: 2171 RVA: 0x0004C2EC File Offset: 0x0004A4EC
	public DateTime StartTimeUTC { get; private set; }

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x0600087C RID: 2172 RVA: 0x0004C2F8 File Offset: 0x0004A4F8
	// (set) Token: 0x0600087D RID: 2173 RVA: 0x0004C300 File Offset: 0x0004A500
	public DateTime EndTimeUTC { get; private set; }

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x0600087E RID: 2174 RVA: 0x0004C30C File Offset: 0x0004A50C
	// (set) Token: 0x0600087F RID: 2175 RVA: 0x0004C314 File Offset: 0x0004A514
	public int TokenRequirement { get; private set; }

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x06000880 RID: 2176 RVA: 0x0004C320 File Offset: 0x0004A520
	// (set) Token: 0x06000881 RID: 2177 RVA: 0x0004C328 File Offset: 0x0004A528
	public int FinishCost { get; private set; }

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x06000882 RID: 2178 RVA: 0x0004C334 File Offset: 0x0004A534
	// (set) Token: 0x06000883 RID: 2179 RVA: 0x0004C33C File Offset: 0x0004A53C
	public string RewardText { get; private set; }

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x06000884 RID: 2180 RVA: 0x0004C348 File Offset: 0x0004A548
	// (set) Token: 0x06000885 RID: 2181 RVA: 0x0004C350 File Offset: 0x0004A550
	public short[] RewardGirlsID { get; private set; }

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x06000886 RID: 2182 RVA: 0x0004C35C File Offset: 0x0004A55C
	// (set) Token: 0x06000887 RID: 2183 RVA: 0x0004C364 File Offset: 0x0004A564
	public List<TaskManager.EventRewardType> RewardType { get; private set; }

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x06000888 RID: 2184 RVA: 0x0004C370 File Offset: 0x0004A570
	// (set) Token: 0x06000889 RID: 2185 RVA: 0x0004C378 File Offset: 0x0004A578
	public List<long> RewardAmount { get; private set; }

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x0600088A RID: 2186 RVA: 0x0004C384 File Offset: 0x0004A584
	// (set) Token: 0x0600088B RID: 2187 RVA: 0x0004C38C File Offset: 0x0004A58C
	public string MissedEventBlurb { get; private set; }

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x0600088C RID: 2188 RVA: 0x0004C398 File Offset: 0x0004A598
	// (set) Token: 0x0600088D RID: 2189 RVA: 0x0004C3A0 File Offset: 0x0004A5A0
	public short[] TaskIDs { get; private set; }

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x0600088E RID: 2190 RVA: 0x0004C3AC File Offset: 0x0004A5AC
	// (set) Token: 0x0600088F RID: 2191 RVA: 0x0004C3B4 File Offset: 0x0004A5B4
	public short EventID { get; private set; }

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x06000890 RID: 2192 RVA: 0x0004C3C0 File Offset: 0x0004A5C0
	// (set) Token: 0x06000891 RID: 2193 RVA: 0x0004C3C8 File Offset: 0x0004A5C8
	public int CostPerToken { get; private set; }

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x06000892 RID: 2194 RVA: 0x0004C3D4 File Offset: 0x0004A5D4
	// (set) Token: 0x06000893 RID: 2195 RVA: 0x0004C3DC File Offset: 0x0004A5DC
	public string AssetBundleName { get; private set; }

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x06000894 RID: 2196 RVA: 0x0004C3E8 File Offset: 0x0004A5E8
	// (set) Token: 0x06000895 RID: 2197 RVA: 0x0004C3F0 File Offset: 0x0004A5F0
	public string IconName { get; private set; }

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x06000896 RID: 2198 RVA: 0x0004C3FC File Offset: 0x0004A5FC
	// (set) Token: 0x06000897 RID: 2199 RVA: 0x0004C404 File Offset: 0x0004A604
	public string RewardSpriteName { get; private set; }

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x06000898 RID: 2200 RVA: 0x0004C410 File Offset: 0x0004A610
	// (set) Token: 0x06000899 RID: 2201 RVA: 0x0004C418 File Offset: 0x0004A618
	public short PreviousEventID { get; private set; }

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x0600089A RID: 2202 RVA: 0x0004C424 File Offset: 0x0004A624
	// (set) Token: 0x0600089B RID: 2203 RVA: 0x0004C42C File Offset: 0x0004A62C
	public TaskData EpicTask { get; private set; }

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x0600089C RID: 2204 RVA: 0x0004C438 File Offset: 0x0004A638
	// (set) Token: 0x0600089D RID: 2205 RVA: 0x0004C440 File Offset: 0x0004A640
	public Sprite Icon { get; set; }

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x0600089E RID: 2206 RVA: 0x0004C44C File Offset: 0x0004A64C
	// (set) Token: 0x0600089F RID: 2207 RVA: 0x0004C454 File Offset: 0x0004A654
	public Sprite RewardSprite { get; set; }

	// Token: 0x060008A0 RID: 2208 RVA: 0x0004C460 File Offset: 0x0004A660
	public EventData Clone()
	{
		return (EventData)base.MemberwiseClone();
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x0004C470 File Offset: 0x0004A670
	public void ModifyEvent(DateTime startTimeUTC, DateTime endTimeUTC, int tokenRequirement, int finishCost, short previousEventID = -1)
	{
		this.StartTimeUTC = startTimeUTC;
		this.EndTimeUTC = endTimeUTC;
		this.TokenRequirement = tokenRequirement;
		this.FinishCost = finishCost;
		this.PreviousEventID = previousEventID;
		this.TaskIDs = null;
		this.Validate();
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x0004C4B0 File Offset: 0x0004A6B0
	public bool Validate()
	{
		if (this.StartTimeUTC > this.EndTimeUTC)
		{
			Debug.Log(string.Format("Task {0} start/end time were invalid.", this.EventID.ToString()));
			return false;
		}
		this.timeSpan = this.EndTimeUTC - this.StartTimeUTC;
		if (this.TaskIDs == null || this.TaskIDs.Length == 0)
		{
			this.TaskIDs = new short[2 * Mathf.RoundToInt((float)this.timeSpan.TotalDays)];
			for (int i = 0; i < Mathf.RoundToInt((float)this.timeSpan.TotalDays); i++)
			{
				int num = (i + this.StartTimeUTC.Day) % this.builtInSmallIdsVer2.Length;
				int num2 = (i + this.StartTimeUTC.Day) % this.builtInMediumIdsVer3.Length;
				this.TaskIDs[i * 2] = this.builtInSmallIdsVer2[num];
				this.TaskIDs[i * 2 + 1] = this.builtInMediumIdsVer3[num2];
			}
		}
		else if (this.TaskIDs.Length != 2 * Mathf.CeilToInt((float)this.timeSpan.TotalDays))
		{
			Debug.Log(string.Format("Task {0} had the incorrect number of tasks assigned.  Expected {1} but got {2}.", this.EventID.ToString(), (2 * Mathf.CeilToInt((float)this.timeSpan.TotalDays)).ToString(), this.TaskIDs.Length.ToString()));
			return false;
		}
		return true;
	}

	// Token: 0x060008A3 RID: 2211 RVA: 0x0004C630 File Offset: 0x0004A830
	private bool RewardGirlIdsMatch(EventData currentEvent, EventData universeEvent)
	{
		for (int i = 0; i < currentEvent.RewardGirlsID.Length; i++)
		{
			if (currentEvent.RewardGirlsID[i] != universeEvent.RewardGirlsID[i])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x0004C670 File Offset: 0x0004A870
	private bool RewardAmountsMatch(EventData currentEvent, EventData universeEvent)
	{
		for (int i = 0; i < currentEvent.RewardGirlsID.Length; i++)
		{
			if (currentEvent.RewardAmount[i] != universeEvent.RewardAmount[i])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x0004C6B8 File Offset: 0x0004A8B8
	private bool RewardGirlTypesMatch(EventData currentEvent, EventData universeEvent)
	{
		for (int i = 0; i < currentEvent.RewardType.Count; i++)
		{
			if (currentEvent.RewardType[i] != universeEvent.RewardType[i])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x0004C704 File Offset: 0x0004A904
	public bool Equals(EventData other)
	{
		return !(other == null) && (this.RewardType.Count == other.RewardType.Count && this.RewardGirlTypesMatch(other, this) && this.RewardGirlsID.Length == other.RewardGirlsID.Length && this.RewardGirlIdsMatch(other, this) && this.RewardAmount.Count == other.RewardAmount.Count) && this.RewardAmountsMatch(other, this);
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x0004C790 File Offset: 0x0004A990
	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		EventData eventData = obj as EventData;
		return !(eventData == null) && this.Equals(eventData);
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x0004C7C4 File Offset: 0x0004A9C4
	public static bool operator ==(EventData evt1, EventData evt2)
	{
		if (evt1 == null || evt2 == null)
		{
			return object.Equals(evt1, evt2);
		}
		return evt1.Equals(evt2);
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x0004C7E4 File Offset: 0x0004A9E4
	public static bool operator !=(EventData evt1, EventData evt2)
	{
		if (evt1 == null || evt2 == null)
		{
			return !object.Equals(evt1, evt2);
		}
		return !evt1.Equals(evt2);
	}

	// Token: 0x040008BB RID: 2235
	internal TimeSpan timeSpan;

	// Token: 0x040008BC RID: 2236
	private readonly short[] builtInSmallIdsVer2 = new short[]
	{
		14,
		8,
		16,
		8,
		14,
		8,
		18,
		14,
		8,
		16,
		8,
		14,
		8,
		18,
		14,
		8,
		16,
		8,
		14,
		8,
		18,
		14,
		8,
		16,
		8,
		14,
		8,
		18,
		14,
		8,
		16
	};

	// Token: 0x040008BD RID: 2237
	private readonly short[] builtInMediumIdsVer3 = new short[]
	{
		20,
		20,
		20,
		20,
		20,
		20,
		21,
		20,
		20,
		20,
		20,
		20,
		20,
		11,
		20,
		20,
		20,
		20,
		20,
		20,
		21,
		20,
		20,
		20,
		20,
		20,
		20,
		11,
		20,
		20,
		20
	};
}
