using System;
using Steamworks;

// Token: 0x0200001C RID: 28
public class SteamStatsAndAchievements : PlatformAchievements
{
	// Token: 0x060000C6 RID: 198 RVA: 0x00007B2C File Offset: 0x00005D2C
	protected override string GetID(PlatformAchievements.PlatformAchievement achievement)
	{
		return achievement.ToString();
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00007B3C File Offset: 0x00005D3C
	protected override bool Initialized()
	{
		return SteamManager.Initialized;
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00007B44 File Offset: 0x00005D44
	public override void Init()
	{
		base.Init();
		if (!this.Initialized())
		{
			return;
		}
		this.gameID = (ulong)new CGameID(SteamUtils.GetAppID());
		this.userAchievementStoredCallback = Callback<UserAchievementStored_t>.Create(new Callback<UserAchievementStored_t>.DispatchDelegate(this.OnAchievementStored));
		for (int i = 0; i < this.achievements.Length; i++)
		{
			bool flag = false;
			SteamUserStats.GetAchievement(this.achievements[i].ID, out flag);
			if (flag)
			{
				this.achievements[i].Achieved = true;
			}
			else
			{
				this.achievements[i].Achieved = false;
			}
		}
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00007BE8 File Offset: 0x00005DE8
	protected override void SetAchievement(string id)
	{
		SteamUserStats.SetAchievement(id);
		SteamUserStats.StoreStats();
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00007BF8 File Offset: 0x00005DF8
	public void ClearAllAchievements()
	{
		for (int i = 0; i < this.achievements.Length; i++)
		{
			SteamUserStats.ClearAchievement(this.achievements[i].ID);
		}
		SteamUserStats.StoreStats();
	}

	// Token: 0x060000CB RID: 203 RVA: 0x00007C38 File Offset: 0x00005E38
	private void OnAchievementStored(UserAchievementStored_t pCallback)
	{
		if (this.gameID != pCallback.m_nGameID || pCallback.m_nMaxProgress == 0U)
		{
		}
	}

	// Token: 0x040000AB RID: 171
	private ulong gameID;

	// Token: 0x040000AC RID: 172
	protected Callback<UserAchievementStored_t> userAchievementStoredCallback;
}
