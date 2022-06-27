using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200013F RID: 319
public class JobModel
{
	// Token: 0x06000839 RID: 2105 RVA: 0x0004BB04 File Offset: 0x00049D04
	public JobModel(List<string[]> states)
	{
		string[] array = states[0];
		if (array.Length != 11)
		{
			Debug.LogError(array[0] + " did not load correctly.");
		}
		else
		{
			this.Name = array[0];
			this.Id = short.Parse(array[1]);
			this.SpriteKey = array[6];
			if (!string.IsNullOrEmpty(array[7]))
			{
				if (Universe.StringToResource.ContainsKey(array[7]))
				{
					this.UnlockHobby = new short[2];
					this.UnlockLevel = new int[2];
					this.UnlockHobby[0] = Universe.StringToResource[array[7]].Id;
					this.UnlockLevel[0] = int.Parse(array[8]);
					this.UnlockHobby[1] = Universe.StringToResource[array[9]].Id;
					this.UnlockLevel[1] = int.Parse(array[10]);
				}
				else
				{
					this.UnlockDLC = array[7];
				}
			}
		}
		this.Available = true;
		this.States = new JobModel.JobState[states.Count - 1];
		for (int i = 0; i < this.States.Length; i++)
		{
			this.States[i] = new JobModel.JobState(states[i + 1]);
		}
	}

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x0600083A RID: 2106 RVA: 0x0004BC50 File Offset: 0x00049E50
	// (set) Token: 0x0600083B RID: 2107 RVA: 0x0004BC58 File Offset: 0x00049E58
	public short Id { get; private set; }

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x0600083C RID: 2108 RVA: 0x0004BC64 File Offset: 0x00049E64
	// (set) Token: 0x0600083D RID: 2109 RVA: 0x0004BC6C File Offset: 0x00049E6C
	public JobModel.JobState[] States { get; private set; }

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x0600083E RID: 2110 RVA: 0x0004BC78 File Offset: 0x00049E78
	// (set) Token: 0x0600083F RID: 2111 RVA: 0x0004BC80 File Offset: 0x00049E80
	public string SpriteKey { get; private set; }

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x06000840 RID: 2112 RVA: 0x0004BC8C File Offset: 0x00049E8C
	// (set) Token: 0x06000841 RID: 2113 RVA: 0x0004BC94 File Offset: 0x00049E94
	public short[] UnlockHobby { get; private set; }

	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x06000842 RID: 2114 RVA: 0x0004BCA0 File Offset: 0x00049EA0
	// (set) Token: 0x06000843 RID: 2115 RVA: 0x0004BCA8 File Offset: 0x00049EA8
	public int[] UnlockLevel { get; private set; }

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x06000844 RID: 2116 RVA: 0x0004BCB4 File Offset: 0x00049EB4
	// (set) Token: 0x06000845 RID: 2117 RVA: 0x0004BCBC File Offset: 0x00049EBC
	public string Name { get; private set; }

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x06000846 RID: 2118 RVA: 0x0004BCC8 File Offset: 0x00049EC8
	// (set) Token: 0x06000847 RID: 2119 RVA: 0x0004BCD0 File Offset: 0x00049ED0
	public bool Available { get; private set; }

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x06000848 RID: 2120 RVA: 0x0004BCDC File Offset: 0x00049EDC
	// (set) Token: 0x06000849 RID: 2121 RVA: 0x0004BCE4 File Offset: 0x00049EE4
	public string UnlockDLC { get; private set; }

	// Token: 0x02000140 RID: 320
	public struct JobState
	{
		// Token: 0x0600084A RID: 2122 RVA: 0x0004BCF0 File Offset: 0x00049EF0
		public JobState(string[] csv)
		{
			if (csv.Length != 11)
			{
				Debug.LogError(csv[0] + " did not load correctly.");
				this.Name = string.Empty;
				this.TimeToComplete = 0.0;
				this.TimeBlocks = 0;
				this.Money = 0.0;
				this.CyclesToLevel = 0L;
			}
			else
			{
				this.Name = csv[0];
				this.TimeToComplete = double.Parse(csv[2]);
				this.TimeBlocks = int.Parse(csv[3]);
				this.Money = double.Parse(csv[4]);
				this.CyclesToLevel = long.Parse(csv[5]);
			}
		}

		// Token: 0x040008A1 RID: 2209
		public string Name;

		// Token: 0x040008A2 RID: 2210
		public double TimeToComplete;

		// Token: 0x040008A3 RID: 2211
		public int TimeBlocks;

		// Token: 0x040008A4 RID: 2212
		public double Money;

		// Token: 0x040008A5 RID: 2213
		public long CyclesToLevel;
	}
}
