using System;
using UnityEngine;

// Token: 0x02000144 RID: 324
public class TaskData : LocalizedModel
{
	// Token: 0x060008AA RID: 2218 RVA: 0x0004C808 File Offset: 0x0004AA08
	public TaskData(string[] csv)
	{
		if (csv.Length != 8)
		{
			Debug.LogError(csv[0] + " did not load correctly.");
		}
		else
		{
			this.Name = csv[0];
			this.TaskID = int.Parse(csv[1]);
			this.TimeBlockRequirement = int.Parse(csv[2]);
			this.RewardType = (TaskManager.TaskRewardType)((byte)Enum.Parse(typeof(TaskManager.TaskRewardType), csv[3]));
			this.RewardAmount = int.Parse(csv[4]);
			int hours = (int)byte.Parse(csv[5]);
			int minutes = (int)byte.Parse(csv[6]);
			int seconds = (int)byte.Parse(csv[7]);
			this.Duration = new TimeSpan(hours, minutes, seconds);
		}
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x060008AB RID: 2219 RVA: 0x0004C8B4 File Offset: 0x0004AAB4
	// (set) Token: 0x060008AC RID: 2220 RVA: 0x0004C8BC File Offset: 0x0004AABC
	public string Name { get; private set; }

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x060008AD RID: 2221 RVA: 0x0004C8C8 File Offset: 0x0004AAC8
	// (set) Token: 0x060008AE RID: 2222 RVA: 0x0004C8D0 File Offset: 0x0004AAD0
	public TimeSpan Duration { get; private set; }

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x060008AF RID: 2223 RVA: 0x0004C8DC File Offset: 0x0004AADC
	// (set) Token: 0x060008B0 RID: 2224 RVA: 0x0004C8E4 File Offset: 0x0004AAE4
	public int TimeBlockRequirement { get; private set; }

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x060008B1 RID: 2225 RVA: 0x0004C8F0 File Offset: 0x0004AAF0
	// (set) Token: 0x060008B2 RID: 2226 RVA: 0x0004C8F8 File Offset: 0x0004AAF8
	public TaskManager.TaskRewardType RewardType { get; private set; }

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x060008B3 RID: 2227 RVA: 0x0004C904 File Offset: 0x0004AB04
	// (set) Token: 0x060008B4 RID: 2228 RVA: 0x0004C90C File Offset: 0x0004AB0C
	public int RewardAmount { get; private set; }

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x060008B5 RID: 2229 RVA: 0x0004C918 File Offset: 0x0004AB18
	// (set) Token: 0x060008B6 RID: 2230 RVA: 0x0004C920 File Offset: 0x0004AB20
	public int TaskID { get; private set; }

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x060008B7 RID: 2231 RVA: 0x0004C92C File Offset: 0x0004AB2C
	// (set) Token: 0x060008B8 RID: 2232 RVA: 0x0004C934 File Offset: 0x0004AB34
	private long startTimeUtc { get; set; }
}
