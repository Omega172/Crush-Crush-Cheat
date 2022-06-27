using System;

// Token: 0x02000019 RID: 25
public abstract class PlatformAchievements
{
	// Token: 0x06000099 RID: 153 RVA: 0x00006F1C File Offset: 0x0000511C
	public virtual void Init()
	{
		PlatformAchievements.PlatformAchievementData[] array = new PlatformAchievements.PlatformAchievementData[25];
		array[0] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_LOVER_CASSIE), "Reach Lover level with Cassie.", string.Empty, () => Girl.ActiveGirls.Count > 0 && Girl.ActiveGirls[0].Love == 9);
		array[1] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_LOVER_MIO), "Reach Lover level with Mio.", string.Empty, () => Girl.ActiveGirls.Count > 1 && Girl.ActiveGirls[1].Love == 9);
		array[2] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_LOVER_QUILL), "Reach Lover level with Quill.", string.Empty, () => Girl.ActiveGirls.Count > 2 && Girl.ActiveGirls[2].Love == 9);
		array[3] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_LOVER_ELLE), "Reach Lover level with Elle.", string.Empty, () => Girl.ActiveGirls.Count > 3 && Girl.ActiveGirls[3].Love == 9);
		array[4] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_LOVER_IRO), "Reach Lover level with Iro.", string.Empty, () => Girl.ActiveGirls.Count > 5 && Girl.ActiveGirls[5].Love == 9);
		array[5] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_LOVER_BONNIBEL), "Reach Lover level with Bonnibel.", string.Empty, () => Girl.ActiveGirls.Count > 6 && Girl.ActiveGirls[6].Love == 9);
		array[6] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_LOVER_FUMI), "Reach Lover level with Fumi.", string.Empty, () => Girl.ActiveGirls.Count > 8 && Girl.ActiveGirls[8].Love == 9);
		array[7] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_LOVER_BEARVERLY), "Reach Lover level with Bearverly.", string.Empty, () => Girl.ActiveGirls.Count > 9 && Girl.ActiveGirls[9].Love == 9);
		array[8] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_LOVER_NINA), "Reach Lover level with Nina.", string.Empty, () => Girl.ActiveGirls.Count > 10 && Girl.ActiveGirls[10].Love == 9);
		array[9] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_LOVER_ALPHA), "Reach Lover level with Alpha.", string.Empty, () => Girl.ActiveGirls.Count > 11 && Girl.ActiveGirls[11].Love == 9);
		array[10] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_LOVER_PAMU), "Reach Lover level with Pamu.", string.Empty, () => Girl.ActiveGirls.Count > 12 && Girl.ActiveGirls[12].Love == 9);
		array[11] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_BANK), "In The Bank!", string.Empty, () => GameState.Money.Value > 1000000.0);
		array[12] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_DATES), "Date-able", string.Empty, () => GameState.DateCount > 100);
		array[13] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_MODEST), "Modest", string.Empty, () => GameState.GiftCount > 10000);
		array[14] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_HEART), "<3<3<3", string.Empty, () => GameState.HeartCount.Value > 1000000L);
		array[15] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_JOBS), "Job-a-holic!", string.Empty, delegate()
		{
			if (Job2.ActiveJobs.Count < 16)
			{
				return false;
			}
			for (int i = 0; i < Job2.ActiveJobs.Count; i++)
			{
				if (Job2.ActiveJobs[i].JobType != Requirement.JobType.Digger && Job2.ActiveJobs[i].JobType != Requirement.JobType.Planter)
				{
					if (Job2.ActiveJobs[i].ExperiencePayout != -1L)
					{
						return false;
					}
				}
			}
			return true;
		});
		array[16] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_LOVER_LUNA), "Reach Lover level with Luna.", string.Empty, () => Girl.ActiveGirls.Count > 13 && Girl.ActiveGirls[13].Love == 9);
		array[17] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_LOVER_EVA), "Reach Lover level with Eva.", string.Empty, () => Girl.ActiveGirls.Count > 14 && Girl.ActiveGirls[14].Love == 9);
		array[18] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_LOVER_AYANO), "Reach Lover level with Ayano.", string.Empty, () => Girl.ActiveGirls.Count > 7 && Girl.ActiveGirls[7].Love == 9);
		array[19] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_LOVER_KARMA), "Reach Lover level with Karma.", string.Empty, () => Girl.ActiveGirls.Count > 15 && Girl.ActiveGirls[15].Love == 9);
		array[20] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_LOVER_SUTRA), "Reach Lover level with Sutra.", string.Empty, () => Girl.ActiveGirls.Count > 16 && Girl.ActiveGirls[16].Love == 9);
		array[21] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_LOVER_DARKONE), "Reach Lover level with Dark One.", string.Empty, () => Girl.ActiveGirls.Count > 17 && Girl.ActiveGirls[17].Love == 9);
		array[22] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_LOVER_QPERNIKISS), "Reach Lover level with QPernikiss.", string.Empty, () => Girl.ActiveGirls.Count > 18 && Girl.ActiveGirls[18].Love == 9);
		array[23] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_LOVER_NUTAKU), "Reach Lover level with Nutaku.", string.Empty, () => Girl.ActiveGirls.Count > 4 && Girl.ActiveGirls[4].Love == 9);
		array[24] = new PlatformAchievements.PlatformAchievementData(this.GetID(PlatformAchievements.PlatformAchievement.ACH_LOVER_PEANUT), "Reach Lover level with Peanut.", string.Empty, () => Girl.ActiveGirls.Count > 30 && Girl.ActiveGirls[30].Love == 9);
		this.achievements = array;
	}

	// Token: 0x0600009A RID: 154
	protected abstract bool Initialized();

	// Token: 0x0600009B RID: 155 RVA: 0x0000749C File Offset: 0x0000569C
	public void CheckAchievements()
	{
		if (!this.Initialized())
		{
			return;
		}
		foreach (PlatformAchievements.PlatformAchievementData platformAchievementData in this.achievements)
		{
			if (!platformAchievementData.Achieved)
			{
				if (platformAchievementData.Check())
				{
					this.UnlockAchievement(platformAchievementData);
				}
			}
		}
	}

	// Token: 0x0600009C RID: 156 RVA: 0x000074FC File Offset: 0x000056FC
	protected void UnlockAchievement(PlatformAchievements.PlatformAchievementData achievement)
	{
		if (achievement.Achieved)
		{
			return;
		}
		achievement.Achieved = true;
		this.SetAchievement(achievement.ID);
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00007528 File Offset: 0x00005728
	public virtual void ShowAchievements()
	{
	}

	// Token: 0x0600009E RID: 158
	protected abstract void SetAchievement(string id);

	// Token: 0x0600009F RID: 159
	protected abstract string GetID(PlatformAchievements.PlatformAchievement achievement);

	// Token: 0x060000A0 RID: 160 RVA: 0x0000752C File Offset: 0x0000572C
	protected PlatformAchievements.PlatformAchievementData GetAchievementByID(string id)
	{
		for (int i = 0; i < this.achievements.Length; i++)
		{
			if (this.achievements[i].ID == id)
			{
				return this.achievements[i];
			}
		}
		return null;
	}

	// Token: 0x04000072 RID: 114
	protected PlatformAchievements.PlatformAchievementData[] achievements;

	// Token: 0x0200001A RID: 26
	public enum PlatformAchievement
	{
		// Token: 0x0400008D RID: 141
		ACH_LOVER_CASSIE,
		// Token: 0x0400008E RID: 142
		ACH_LOVER_MIO,
		// Token: 0x0400008F RID: 143
		ACH_LOVER_QUILL,
		// Token: 0x04000090 RID: 144
		ACH_LOVER_ELLE,
		// Token: 0x04000091 RID: 145
		ACH_LOVER_IRO,
		// Token: 0x04000092 RID: 146
		ACH_LOVER_BONNIBEL,
		// Token: 0x04000093 RID: 147
		ACH_LOVER_FUMI,
		// Token: 0x04000094 RID: 148
		ACH_LOVER_BEARVERLY,
		// Token: 0x04000095 RID: 149
		ACH_LOVER_NINA,
		// Token: 0x04000096 RID: 150
		ACH_LOVER_ALPHA,
		// Token: 0x04000097 RID: 151
		ACH_LOVER_PAMU,
		// Token: 0x04000098 RID: 152
		ACH_BANK,
		// Token: 0x04000099 RID: 153
		ACH_DATES,
		// Token: 0x0400009A RID: 154
		ACH_MODEST,
		// Token: 0x0400009B RID: 155
		ACH_HEART,
		// Token: 0x0400009C RID: 156
		ACH_JOBS,
		// Token: 0x0400009D RID: 157
		ACH_LOVER_LUNA,
		// Token: 0x0400009E RID: 158
		ACH_LOVER_EVA,
		// Token: 0x0400009F RID: 159
		ACH_LOVER_AYANO,
		// Token: 0x040000A0 RID: 160
		ACH_LOVER_KARMA,
		// Token: 0x040000A1 RID: 161
		ACH_LOVER_SUTRA,
		// Token: 0x040000A2 RID: 162
		ACH_LOVER_DARKONE,
		// Token: 0x040000A3 RID: 163
		ACH_LOVER_QPERNIKISS,
		// Token: 0x040000A4 RID: 164
		ACH_LOVER_NUTAKU,
		// Token: 0x040000A5 RID: 165
		ACH_LOVER_PEANUT
	}

	// Token: 0x0200001B RID: 27
	protected class PlatformAchievementData
	{
		// Token: 0x060000BA RID: 186 RVA: 0x00007A88 File Offset: 0x00005C88
		public PlatformAchievementData(string achievementID, string name, string desc, Achievements.CheckAchievement condition)
		{
			this.ID = achievementID;
			this.Name = name;
			this.Description = desc;
			this.Achieved = false;
			this.Check = condition;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00007AC0 File Offset: 0x00005CC0
		// (set) Token: 0x060000BC RID: 188 RVA: 0x00007AC8 File Offset: 0x00005CC8
		public string ID { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00007AD4 File Offset: 0x00005CD4
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00007ADC File Offset: 0x00005CDC
		public string Name { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00007AE8 File Offset: 0x00005CE8
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00007AF0 File Offset: 0x00005CF0
		public string Description { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00007AFC File Offset: 0x00005CFC
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00007B04 File Offset: 0x00005D04
		public bool Achieved { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00007B10 File Offset: 0x00005D10
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00007B18 File Offset: 0x00005D18
		public Achievements.CheckAchievement Check { get; private set; }
	}
}
