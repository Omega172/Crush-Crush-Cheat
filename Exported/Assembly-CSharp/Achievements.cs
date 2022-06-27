using System;
using System.Collections;
using System.Collections.Generic;
using AssetBundles;
using SadPanda.Platforms;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000014 RID: 20
public class Achievements : MonoBehaviour
{
	// Token: 0x0600004C RID: 76 RVA: 0x00004A00 File Offset: 0x00002C00
	private void AddAchievement(Achievements.Achievement achievement, int category, int index)
	{
		achievement.CategoryIndex = category;
		achievement.Index = index;
		this.AllAchievements.Add(achievement);
		if (!achievement.Complete())
		{
			this.ActiveAchievements.Add(achievement);
			this.ActiveAchievementsHash.Add(achievement);
		}
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00004A4C File Offset: 0x00002C4C
	public void StoreState()
	{
		if (this.CategoryNames == null)
		{
			return;
		}
		for (int i = 0; i < this.CategoryNames.Length; i++)
		{
			if (this.AchievementCategories.ContainsKey(this.CategoryNames[i]))
			{
				int num = 0;
				List<Achievements.Achievement> list = this.AchievementCategories[this.CategoryNames[i]];
				for (int j = 0; j < list.Count; j++)
				{
					if (!this.ActiveAchievementsHash.Contains(list[j]))
					{
						num |= 1 << j;
					}
				}
				global::PlayerPrefs.SetInt(string.Format("ACH{0}", i), num);
			}
		}
		global::PlayerPrefs.SetInt("AchievementCount", this.AchievementCount);
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00004B14 File Offset: 0x00002D14
	public int CalculateDiamonds()
	{
		int num = 0;
		for (int i = 0; i < 54; i++)
		{
			if (Notifications.CheckNotification(this.AllAchievements[i]))
			{
				num += this.AllAchievements[i].Diamonds;
			}
		}
		return num;
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00004B64 File Offset: 0x00002D64
	private void CreateLoveAchievement(Balance.GirlName girl, Girl.LoveLevel level, string title, string text, string notification, int diamonds, int localization = 0)
	{
		if (!this.AchievementCategories.ContainsKey(girl.ToFriendlyString()))
		{
			Debug.Log(string.Format("Could not create an achievement for {0}", girl.ToFriendlyString()));
			return;
		}
		string achLocalization = string.Format("achievements_{0}", localization.ToString());
		this.AchievementCategories[girl.ToFriendlyString()].Add(new Achievements.Achievement(Achievements.AchievementType.Girl, () => Girl.GetLove(girl) >= (int)level, title, text, notification, diamonds, achLocalization));
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00004C04 File Offset: 0x00002E04
	private void CreateJobAchievement(Requirement.JobType job, int level, string title, string text, string notification, int localization = 0)
	{
		string achLocalization = string.Format("achievements_{0}", localization.ToString());
		this.AchievementCategories[job.ToFriendlyString()].Add(new Achievements.Achievement(Achievements.AchievementType.Girl, () => (Job2.AvailableJobs & job) != Requirement.JobType.None && Job2.GetJob(job).Level >= level, title, text, notification, achLocalization));
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00004C6C File Offset: 0x00002E6C
	public void UpdateAchievementText()
	{
		if (!this.builtAchievements)
		{
			return;
		}
		foreach (Achievements.CategoryMapping categoryMapping in this.AchievementMapping)
		{
			this.ChangeSelection(categoryMapping.Prefab.transform, categoryMapping.CategoryId, 0);
		}
	}

	// Token: 0x06000052 RID: 82 RVA: 0x00004CF0 File Offset: 0x00002EF0
	public void Init(bool buildAchievements = false)
	{
		this.InitInternal();
		if (this._achievementsBundle == null)
		{
			GameState.CurrentState.StartCoroutine(this.InitialLoad(buildAchievements));
		}
		else if (buildAchievements)
		{
			this.BuildAchievements();
		}
	}

	// Token: 0x06000053 RID: 83 RVA: 0x00004D38 File Offset: 0x00002F38
	private void InitInternal()
	{
		this.initialized = true;
		this.ActiveAchievements.Clear();
		this.AllAchievements.Clear();
		this.ActiveAchievementsHash.Clear();
		this.AchievementCategories.Clear();
		Achievements.Categories.Sort();
		this.CategoryNames = new string[Achievements.Categories.Count];
		for (int i = 0; i < Achievements.Categories.Count; i++)
		{
			this.CategoryNames[i] = Achievements.Categories[i].Name;
		}
		this.lastPercentComplete = new float[this.CategoryNames.Length];
		for (int j = 0; j < this.lastPercentComplete.Length; j++)
		{
			this.lastPercentComplete[j] = -1f;
		}
		for (int k = 0; k < this.CategoryNames.Length; k++)
		{
			this.AchievementCategories.Add(this.CategoryNames[k], new List<Achievements.Achievement>());
		}
		int num = 0;
		int num2 = 0;
		short num3 = 1;
		while ((int)num3 <= Universe.Achievements.Count)
		{
			AchievementModel achievement = Universe.Achievements[num3];
			if (achievement.Requirement == AchievementModel.AchievementType.GirlLevel)
			{
				Balance.GirlName girl = (Balance.GirlName)(achievement.Target - 1);
				Girl.LoveLevel level = (Girl.LoveLevel)achievement.Value;
				this.CreateLoveAchievement(girl, level, achievement.Name, achievement.Description, achievement.Notification, achievement.Reward, achievement.Localization);
			}
			else if (achievement.Requirement == AchievementModel.AchievementType.JobLevel)
			{
				Requirement.JobType job = (Requirement.JobType)(1 << (int)(achievement.Target - 1));
				int level2 = (int)achievement.Value;
				this.CreateJobAchievement(job, level2, achievement.Name, achievement.Description, achievement.Notification, achievement.Localization);
			}
			else if (achievement.Requirement == AchievementModel.AchievementType.SoftCurrency && Universe.Resources.ContainsKey(achievement.Target))
			{
				ResourceModel resourceModel = Universe.Resources[achievement.Target];
				string nameToLower = resourceModel.NameToLower;
				switch (nameToLower)
				{
				case "totalmoney":
				{
					int temp1 = num++;
					this.AchievementCategories["Lifetime Earnings"].Add(new Achievements.Achievement(Achievements.AchievementType.Money, () => GameState.TotalIncome.Value >= Achievements.incomeComparison[temp1], achievement));
					Achievements.incomeComparison[temp1] = achievement.Value;
					break;
				}
				case "money":
				{
					int temp2 = num2++;
					this.AchievementCategories["Earnings"].Add(new Achievements.Achievement(Achievements.AchievementType.Money, () => GameState.Money.Value >= Achievements.moneyComparison[temp2], achievement));
					Achievements.moneyComparison[temp2] = achievement.Value;
					break;
				}
				case "totaltime":
				{
					double gameTime = achievement.Value;
					this.AchievementCategories["Game Time"].Add(new Achievements.Achievement(Achievements.AchievementType.Time, () => (double)GameState.TotalTime >= gameTime, achievement));
					break;
				}
				case "totaldates":
				{
					double dateCount = achievement.Value;
					this.AchievementCategories["Dates"].Add(new Achievements.Achievement(Achievements.AchievementType.Girl, () => (double)GameState.DateCount >= dateCount, achievement));
					break;
				}
				case "totalgifts":
				{
					double giftCount = achievement.Value;
					this.AchievementCategories["Gifts"].Add(new Achievements.Achievement(Achievements.AchievementType.Girl, () => (double)GameState.GiftCount >= giftCount, achievement));
					break;
				}
				case "totalhearts":
				{
					double heartCount = achievement.Value;
					this.AchievementCategories["Hearts"].Add(new Achievements.Achievement(Achievements.AchievementType.Girl, () => (double)GameState.HeartCount.Value >= heartCount, achievement));
					break;
				}
				case "multiplierreset":
				{
					double prestigeCount = achievement.Value;
					this.AchievementCategories["Reset"].Add(new Achievements.Achievement(Achievements.AchievementType.Reset, () => (double)GameState.CurrentState.TimeMultiplier.Value >= prestigeCount, achievement));
					break;
				}
				}
			}
			else if (achievement.Requirement == AchievementModel.AchievementType.Hobby)
			{
				this.AchievementCategories["Hobbies"].Add(new Achievements.Achievement(Achievements.AchievementType.Hobby, () => Hobby2.AvailableHobbies.Contains((short)achievement.Value), achievement));
			}
			else if (achievement.Requirement == AchievementModel.AchievementType.Hobby2)
			{
				this.AchievementCategories["Skills"].Add(new Achievements.Achievement(Achievements.AchievementType.Hobby, () => Hobby2.MinimumLevel > (int)achievement.Value - 1, achievement));
			}
			else if (achievement.Requirement == AchievementModel.AchievementType.Special)
			{
				if (achievement.Id != 475)
				{
					if (achievement.Id == 471)
					{
						this.AchievementCategories["Reset"].Insert(0, new Achievements.Achievement(Achievements.AchievementType.Event, () => false, achievement));
					}
					else
					{
						this.AchievementCategories["Misc"].Add(new Achievements.Achievement(Achievements.AchievementType.Event, () => false, achievement));
					}
				}
			}
			num3 += 1;
		}
		for (int l = 0; l < this.CategoryNames.Length; l++)
		{
			if (this.AchievementCategories.ContainsKey(this.CategoryNames[l]) && this.AchievementCategories[this.CategoryNames[l]] != null)
			{
				for (int m = 0; m < this.AchievementCategories[this.CategoryNames[l]].Count; m++)
				{
					this.AddAchievement(this.AchievementCategories[this.CategoryNames[l]][m], l, m);
				}
			}
		}
		if (base.gameObject.activeInHierarchy && this._achievementsBundle != null)
		{
			this.UpdateColor();
		}
	}

	// Token: 0x06000054 RID: 84 RVA: 0x000054D0 File Offset: 0x000036D0
	public void Rebuild()
	{
		this.Init(false);
		if (this.builtAchievements)
		{
			this.selectedAchievement = null;
			for (int i = 0; i < this.lastPercentComplete.Length; i++)
			{
				this.lastPercentComplete[i] = -1f;
			}
			this.builtAchievements = false;
			Transform transform = base.transform.Find("Scroll View/Content Panel");
			for (int j = transform.childCount - 1; j >= 0; j--)
			{
				UnityEngine.Object.DestroyImmediate(transform.GetChild(j).gameObject);
			}
			this.BuildAchievements();
		}
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00005564 File Offset: 0x00003764
	public Achievements.Achievement GetAchievementFromIndex(int index)
	{
		foreach (Achievements.Achievement achievement in this.ActiveAchievements)
		{
			if (achievement.Data != null && (int)achievement.Data.Id == index)
			{
				return achievement;
			}
		}
		return null;
	}

	// Token: 0x06000056 RID: 86 RVA: 0x000055EC File Offset: 0x000037EC
	public static void ForceAchievement(int index)
	{
		GameState.GetAchievements().ForceAchievement(GameState.GetAchievements().GetAchievementFromIndex(index));
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00005604 File Offset: 0x00003804
	public void ForceAchievement(Achievements.Achievement achievement)
	{
		if (this.ActiveAchievements.Contains(achievement))
		{
			Notifications.AddNotification(achievement);
			this.ActiveAchievements.Remove(achievement);
			if (this.ActiveAchievementsHash.Contains(achievement))
			{
				this.ActiveAchievementsHash.Remove(achievement);
			}
			this.StoreState();
			GameState.CurrentState.PendingPrestige.Value += 0.05f;
			if (achievement.Diamonds > 0)
			{
				Utilities.AwardDiamonds(achievement.Diamonds);
			}
		}
	}

	// Token: 0x06000058 RID: 88 RVA: 0x0000568C File Offset: 0x0000388C
	private void TriggerAchievement(Achievements.Achievement achievement, Balance.GirlName girlName = Balance.GirlName.Unknown)
	{
		if (!GameState.Initialized)
		{
			return;
		}
		if (this.affectionMilestone == null)
		{
			this.affectionMilestone = GameState.CurrentState.transform.Find("Popups/Affection Milestone").gameObject;
		}
		Notifications.AddNotification(achievement);
		this.ActiveAchievements.Remove(achievement);
		if (this.ActiveAchievementsHash.Contains(achievement))
		{
			this.ActiveAchievementsHash.Remove(achievement);
		}
		if (achievement.Diamonds > 0)
		{
			string arg = Translations.TranslateGirlName((girlName == Balance.GirlName.Unknown) ? this.GetGirlFromAchievement(achievement) : girlName);
			this.affectionMilestone.transform.Find("Top Text").GetComponent<Text>().text = string.Format(Translations.GetTranslation("everything_else_11_0", "You reached the next stage with {0}"), arg);
			if (achievement.Diamonds == 1)
			{
				this.affectionMilestone.transform.Find("Bottom Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_12_0", "You earned 1 diamond!");
			}
			else
			{
				this.affectionMilestone.transform.Find("Bottom Text").GetComponent<Text>().text = string.Format(Translations.GetTranslation("everything_else_12_1", "You earned {0} diamonds!"), achievement.Diamonds.ToString());
			}
			this.affectionMilestone.SetActive(true);
			this.affectionMilestone.transform.Find("Icon").gameObject.SetActive(false);
			this.affectionMilestone.transform.Find("+1 Diamond").gameObject.SetActive(true);
			Utilities.AwardDiamonds(achievement.Diamonds);
		}
		GameState.CurrentState.PendingPrestige.Value += 0.05f;
		this.StoreState();
		if (base.gameObject.activeInHierarchy && this._achievementsBundle != null)
		{
			this.UpdateColor();
		}
		this.UpdateFreeTime();
		GameState.CurrentState.CheckAchievements();
	}

	// Token: 0x06000059 RID: 89 RVA: 0x0000588C File Offset: 0x00003A8C
	private void UpdateFreeTime()
	{
		int num = this.AchievementCount / 4;
		int num2 = 6;
		if (Girl.FindGirl(Balance.GirlName.QPiddy) != null)
		{
			Girl girl = Girl.FindGirl(Balance.GirlName.QPiddy);
			if (girl.Love >= 1)
			{
				num--;
			}
			if (girl.Love >= 4)
			{
				num--;
			}
			if (girl.Love >= 7)
			{
				num--;
			}
		}
		if (FreeTime.TimeSlots < num2 + num)
		{
			Notifications.AddAchievement();
			global::PlayerPrefs.SetInt("AchievementCount", this.AchievementCount);
		}
		FreeTime.TimeSlots = num2 + num;
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00005918 File Offset: 0x00003B18
	private Balance.GirlName GetGirlFromAchievement(Achievements.Achievement achievement)
	{
		for (int i = 0; i < Universe.Girls.Count; i++)
		{
			string key = ((Balance.GirlName)i).ToFriendlyString();
			if (this.AchievementCategories.ContainsKey(key) && this.AchievementCategories[key].Contains(achievement))
			{
				return (Balance.GirlName)i;
			}
		}
		return Balance.GirlName.Cassie;
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x0600005B RID: 91 RVA: 0x00005974 File Offset: 0x00003B74
	public int AchievementCount
	{
		get
		{
			return this.AllAchievements.Count - this.ActiveAchievements.Count;
		}
	}

	// Token: 0x0600005C RID: 92 RVA: 0x00005990 File Offset: 0x00003B90
	private void OnDestroy()
	{
		this.initialized = false;
	}

	// Token: 0x0600005D RID: 93 RVA: 0x0000599C File Offset: 0x00003B9C
	private void Start()
	{
		if (!this.initialized)
		{
			this.Init(true);
		}
		else
		{
			this._initialBuildPending = true;
		}
	}

	// Token: 0x0600005E RID: 94 RVA: 0x000059BC File Offset: 0x00003BBC
	private void OnEnable()
	{
		if (this._achievementsBundle != null)
		{
			this.UpdateColor();
		}
	}

	// Token: 0x0600005F RID: 95 RVA: 0x000059D8 File Offset: 0x00003BD8
	private IEnumerator InitialLoad(bool buildAchievements)
	{
		if (this._achievementsBundle == null)
		{
			yield return this.LoadAchievementsBundle();
		}
		if (buildAchievements || this._initialBuildPending)
		{
			this.BuildAchievements();
		}
		Kongregate.SubmitStat("AchievementCount", (long)this.AchievementCount);
		this._initialBuildPending = false;
		yield break;
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00005A04 File Offset: 0x00003C04
	private IEnumerator LoadAchievementsBundle()
	{
		if (this._achievementsBundle != null)
		{
			yield break;
		}
		AssetBundleAsync request = GameState.AssetManager.GetBundleAsync("universe/achievements", false);
		yield return request;
		this._achievementsBundle = request.AssetBundle;
		yield break;
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00005A20 File Offset: 0x00003C20
	private Sprite GetSpriteFromAssetBundle(string name)
	{
		name = name.ToLowerInvariant();
		return this._achievementsBundle.LoadAsset<Sprite>("achievements_" + name);
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00005A40 File Offset: 0x00003C40
	private Sprite GetGirlSprite(int girl)
	{
		string text = ((Balance.GirlName)girl).ToFriendlyString();
		if (GameState.GetGirlScreen().IsUnlocked((Balance.GirlName)girl))
		{
			return this.GetSpriteFromAssetBundle(text);
		}
		if (!this.AchievementCategories.ContainsKey(text))
		{
			return this.GetSpriteFromAssetBundle(this.GirlLockedName);
		}
		List<Achievements.Achievement> list = this.AchievementCategories[text];
		if (list == null || list.Count == 0)
		{
			return this.GetSpriteFromAssetBundle(this.GirlLockedName);
		}
		if (this.ActiveAchievementsHash.Contains(list[0]))
		{
			return this.GetSpriteFromAssetBundle(this.GirlLockedName);
		}
		return this.GetSpriteFromAssetBundle(text);
	}

	// Token: 0x06000063 RID: 99 RVA: 0x00005AE0 File Offset: 0x00003CE0
	private Sprite GetGeneralSprite(string name)
	{
		return this.GetSpriteFromAssetBundle(this.GetMiscSpriteFromName(name));
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00005AF0 File Offset: 0x00003CF0
	private string GetMiscSpriteFromName(string name)
	{
		switch (name)
		{
		case "Earnings":
		case "Lifetime Earnings":
			return "money";
		case "Skills":
			return "hobbies";
		case "Gifts":
		case "Hearts":
			return "dates";
		case "Game Time":
			return "time";
		case "Grave Digger":
			return "digger";
		case "Tree Planter":
			return "planter";
		}
		return name.ToLowerInvariant();
	}

	// Token: 0x06000065 RID: 101 RVA: 0x00005BDC File Offset: 0x00003DDC
	private bool HasOutfit(Balance.GirlName girl, Requirement.OutfitType outfit)
	{
		Girl girl2 = Girl.FindGirl(girl);
		return !(girl2 == null) && (girl2.LifetimeOutfits & outfit) != Requirement.OutfitType.None;
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00005C0C File Offset: 0x00003E0C
	private int GetCategoryIdFromGirlId(int girlId)
	{
		if (Achievements.Categories == null || Achievements.Categories.Count == 0)
		{
			return -1;
		}
		string b = ((Balance.GirlName)girlId).ToLowerFriendlyString();
		foreach (Achievements.CategorySort categorySort in Achievements.Categories)
		{
			if (categorySort.Name.ToLowerInvariant() == b)
			{
				return categorySort.Id;
			}
		}
		return -1;
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00005CB0 File Offset: 0x00003EB0
	private void BuildAchievements()
	{
		if (this.builtAchievements)
		{
			return;
		}
		if (Universe.Achievements == null || Universe.Achievements.Count == 0)
		{
			return;
		}
		this._achievementOffset = 0;
		this.AchievementMapping.Clear();
		Transform contentPanel = base.transform.Find("Scroll View/Content Panel");
		List<int> list = new List<int>();
		for (int i = 0; i < Universe.Girls.Count; i++)
		{
			if (i <= 18 || i == 30 || GameState.GetGirlScreen().IsUnlocked((Balance.GirlName)i))
			{
				int categoryIdFromGirlId = this.GetCategoryIdFromGirlId(i);
				if (categoryIdFromGirlId >= 0)
				{
					this.CreateAchievementGO(list, categoryIdFromGirlId, contentPanel, i);
				}
			}
			else
			{
				this._achievementOffset += 9;
			}
		}
		for (int j = 0; j < Achievements.Categories.Count; j++)
		{
			if (!list.Contains(j))
			{
				this.CreateAchievementGO(list, j, contentPanel, -1);
				if (Achievements.Categories[j].Name == "Reset")
				{
					break;
				}
				if (j == 34)
				{
					if (GameState.GetGirlScreen().IsUnlocked(Balance.GirlName.Charlotte))
					{
						this.CreateAchievementGO(list, 51, contentPanel, -1);
					}
					else
					{
						this._achievementOffset += 10;
					}
					if (GameState.GetGirlScreen().IsUnlocked(Balance.GirlName.Suzu))
					{
						this.CreateAchievementGO(list, 67, contentPanel, -1);
					}
					else
					{
						this._achievementOffset += 10;
					}
				}
			}
		}
		this.SetHighestAchievement();
		this.builtAchievements = true;
		int achievementCount = this.GetAchievementCount();
		this.UnlockedCount.text = string.Format(Translations.GetTranslation("everything_else_29_1", "{0} of {1} unlocked"), this.AchievementCount.ToString(), achievementCount.ToString());
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00005E88 File Offset: 0x00004088
	private void CreateAchievementGO(List<int> categoryMapping, int category, Transform contentPanel, int girlId = -1)
	{
		categoryMapping.Add(category);
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.GirlAchievementPrefab, contentPanel);
		gameObject.name = this.CategoryNames[category];
		gameObject.transform.Find("Title").GetComponent<Text>().text = ((category >= this.CategoryNames.Length) ? "Unknown" : this.CategoryNames[category]);
		if (girlId >= 0)
		{
			gameObject.transform.Find("Icon").GetComponent<Image>().sprite = this.GetGirlSprite(girlId);
		}
		else
		{
			gameObject.transform.Find("Icon").GetComponent<Image>().sprite = this.GetGeneralSprite(gameObject.name);
		}
		Transform thisAchievement = gameObject.transform;
		gameObject.transform.Find("Left Arrow").GetComponent<Button>().onClick.RemoveAllListeners();
		gameObject.transform.Find("Left Arrow").GetComponent<Button>().onClick.AddListener(delegate()
		{
			GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.HoverSelect);
			this.ChangeSelection(thisAchievement, category, -1);
		});
		gameObject.transform.Find("Right Arrow").GetComponent<Button>().onClick.RemoveAllListeners();
		gameObject.transform.Find("Right Arrow").GetComponent<Button>().onClick.AddListener(delegate()
		{
			GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.HoverSelect);
			this.ChangeSelection(thisAchievement, category, 1);
		});
		this.AchievementMapping.Add(new Achievements.CategoryMapping
		{
			CategoryId = category,
			CategoryName = this.CategoryNames[category],
			Prefab = gameObject
		});
	}

	// Token: 0x06000069 RID: 105 RVA: 0x0000604C File Offset: 0x0000424C
	private int GetAchievementCount()
	{
		return (this.AllAchievements != null) ? (this.AllAchievements.Count - this._achievementOffset) : 0;
	}

	// Token: 0x0600006A RID: 106 RVA: 0x00006074 File Offset: 0x00004274
	private void ChangeSelection(Transform achievement, int category, int delta)
	{
		this.UpdateAchievementPrefab(achievement, category, Mathf.Max(0, this.selectedAchievement[category] + delta));
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00006090 File Offset: 0x00004290
	private void SetHighestAchievement()
	{
		if (this.selectedAchievement == null)
		{
			this.selectedAchievement = new int[this.CategoryNames.Length];
			for (int i = 0; i < this.selectedAchievement.Length; i++)
			{
				this.selectedAchievement[i] = -1;
			}
		}
		foreach (Achievements.CategoryMapping categoryMapping in this.AchievementMapping)
		{
			if (this.AchievementCategories.ContainsKey(categoryMapping.CategoryName))
			{
				List<Achievements.Achievement> list = this.AchievementCategories[categoryMapping.CategoryName];
				int j;
				for (j = 0; j < list.Count; j++)
				{
					if (this.ActiveAchievementsHash.Contains(list[j]))
					{
						j--;
						break;
					}
				}
				this.UpdateAchievementPrefab(categoryMapping.Prefab.transform, categoryMapping.CategoryId, j + 1);
			}
		}
	}

	// Token: 0x0600006C RID: 108 RVA: 0x000061B8 File Offset: 0x000043B8
	private void UpdateAchievementsPrefabs()
	{
		for (int i = 0; i < this.AchievementMapping.Count; i++)
		{
			Achievements.CategoryMapping categoryMapping = this.AchievementMapping[i];
			if (this.AchievementCategories.ContainsKey(categoryMapping.CategoryName))
			{
				Transform transform = categoryMapping.Prefab.transform;
				this.UpdateAchievementPrefab(transform, categoryMapping.CategoryId, this.selectedAchievement[categoryMapping.CategoryId]);
				if (i < Girl.ActiveGirls.Count && transform.Find("Icon").GetComponent<Image>().sprite != null && transform.Find("Icon").GetComponent<Image>().sprite.name == "achievements_" + this.GirlLockedName && Girls.UnlockedGirlCount > i)
				{
					transform.Find("Icon").GetComponent<Image>().sprite = this.GetGirlSprite(i);
				}
			}
		}
	}

	// Token: 0x0600006D RID: 109 RVA: 0x000062BC File Offset: 0x000044BC
	private void UpdateAchievementPrefab(Transform achievement, int category, int selection)
	{
		if (category < 0 || category >= this.CategoryNames.Length)
		{
			return;
		}
		List<Achievements.Achievement> list = this.AchievementCategories[this.CategoryNames[category]];
		if (selection >= list.Count)
		{
			selection = list.Count - 1;
		}
		if (selection < 0 || (selection < list.Count && this.ActiveAchievementsHash.Contains(list[selection])))
		{
			achievement.GetComponent<Image>().color = this.UnlockedAchievement;
			achievement.transform.Find("Icon").GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
			selection = Mathf.Max(0, selection);
		}
		else
		{
			achievement.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
			achievement.transform.Find("Icon").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
		}
		if (selection >= list.Count)
		{
			selection = list.Count - 1;
			achievement.transform.Find("Progress Background").gameObject.SetActive(false);
		}
		else
		{
			float percentComplete = this.GetPercentComplete(category, selection);
			if (percentComplete != this.lastPercentComplete[category])
			{
				if (percentComplete >= 1f)
				{
					achievement.transform.Find("Progress Background").gameObject.SetActive(false);
				}
				else
				{
					Transform transform = achievement.transform.Find("Progress Background");
					transform.gameObject.SetActive(true);
					transform.Find("ProgressBar").GetComponent<Image>().fillAmount = percentComplete;
					transform.Find("Progress Text").GetComponent<Text>().text = string.Format("{0}%", (percentComplete * 100f).ToString("0.00"));
				}
				this.lastPercentComplete[category] = percentComplete;
			}
		}
		if (selection >= 0 && selection < list.Count)
		{
			achievement.transform.Find("Title").GetComponent<Text>().text = list[selection].Title;
			achievement.transform.Find("Text").GetComponent<Text>().text = list[selection].Text;
			achievement.transform.Find("Checkmark").gameObject.SetActive(false);
			achievement.transform.Find("Index Text").GetComponent<Text>().text = string.Format("{0}/{1}", (selection + 1).ToString(), list.Count.ToString());
			this.selectedAchievement[category] = selection;
		}
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00006588 File Offset: 0x00004788
	private float GetPercentComplete(int category, int selection)
	{
		float b = 1.1f;
		if (selection < 0)
		{
			selection = 0;
		}
		Achievements.Achievement item = this.AchievementCategories[this.CategoryNames[category]][selection];
		if (!this.ActiveAchievementsHash.Contains(item))
		{
			return 1.1f;
		}
		string text = this.CategoryNames[category];
		switch (text)
		{
		case "Earnings":
			b = float.Parse(GameState.Money.Value.ToString()) / (float)this.AchievementCategories["Earnings"][selection].Data.Value;
			break;
		case "Lifetime Earnings":
			b = float.Parse(GameState.TotalIncome.Value.ToString()) / (float)this.AchievementCategories["Lifetime Earnings"][selection].Data.Value;
			break;
		case "Dates":
			b = (float)GameState.DateCount / (float)this.AchievementCategories["Dates"][selection].Data.Value;
			break;
		case "Gifts":
			b = (float)GameState.GiftCount / (float)this.AchievementCategories["Gifts"][selection].Data.Value;
			break;
		case "Hearts":
			b = (float)GameState.HeartCount.Value / (float)this.AchievementCategories["Hearts"][selection].Data.Value;
			break;
		case "Game Time":
			b = (float)GameState.TotalTime / (float)this.AchievementCategories["Game Time"][selection].Data.Value;
			break;
		}
		return Mathf.Max(0f, Mathf.Min(1f, b));
	}

	// Token: 0x0600006F RID: 111 RVA: 0x000067CC File Offset: 0x000049CC
	private void UpdateAchievementLabels()
	{
		if (this.lastAchievementCount != this.AchievementCount)
		{
			this.Countdown.text = string.Format(Translations.GetTranslation("everything_else_29_0", "{0} more achievements until Time Block unlocks."), (4 - this.AchievementCount % 4).ToString());
			int achievementCount = this.GetAchievementCount();
			this.UnlockedCount.text = string.Format(Translations.GetTranslation("everything_else_29_1", "{0} of {1} unlocked"), this.AchievementCount.ToString(), achievementCount.ToString());
			this.lastAchievementCount = this.AchievementCount;
		}
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00006864 File Offset: 0x00004A64
	private void CheckUpdatePrefabCount()
	{
		int num = 0;
		num += this.ActiveAchievements.Count;
		num += this.AllAchievements.Count - this.ActiveAchievements.Count;
		if (num != this.prefabCount)
		{
			this.BuildAchievements();
			Kongregate.SubmitStat("AchievementCount", (long)(this.AllAchievements.Count - this.ActiveAchievements.Count));
			this.prefabCount = num;
		}
	}

	// Token: 0x06000071 RID: 113 RVA: 0x000068D8 File Offset: 0x00004AD8
	private void UpdateProgress(string category)
	{
		Achievements.CategoryMapping categoryMapping = null;
		for (int i = 0; i < this.AchievementMapping.Count; i++)
		{
			if (!(category != this.AchievementMapping[i].CategoryName))
			{
				categoryMapping = this.AchievementMapping[i];
				break;
			}
		}
		if (categoryMapping == null)
		{
			return;
		}
		Transform transform = categoryMapping.Prefab.transform;
		this.UpdateAchievementPrefab(transform, categoryMapping.CategoryId, this.selectedAchievement[categoryMapping.CategoryId]);
		this.UpdateAchievementLabels();
	}

	// Token: 0x06000072 RID: 114 RVA: 0x0000696C File Offset: 0x00004B6C
	private void UpdateColor()
	{
		this.UpdateAchievementsPrefabs();
		this.UpdateAchievementLabels();
		this.CheckUpdatePrefabCount();
	}

	// Token: 0x06000073 RID: 115 RVA: 0x00006980 File Offset: 0x00004B80
	private void UncheckShowButtons()
	{
		base.transform.Find("Achievement Sorting/All Button/Checkmark").gameObject.SetActive(false);
		base.transform.Find("Achievement Sorting/Completed Button/Checkmark").gameObject.SetActive(false);
		base.transform.Find("Achievement Sorting/Incomplete Button/Checkmark").gameObject.SetActive(false);
	}

	// Token: 0x06000074 RID: 116 RVA: 0x000069E0 File Offset: 0x00004BE0
	public void HandleAchievement(string category)
	{
		if (!this.initialized || !this.AchievementCategories.ContainsKey(category))
		{
			return;
		}
		List<Achievements.Achievement> list = this.AchievementCategories[category];
		if (list == null || list.Count == 0)
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (!list[i].Complete())
			{
				if (list[i].Check())
				{
					this.TriggerAchievement(list[i], Balance.GirlName.Unknown);
					if (base.gameObject.activeInHierarchy)
					{
						this.UpdateProgress(category);
					}
				}
				else
				{
					if (base.gameObject.activeInHierarchy)
					{
						this.UpdateProgress(category);
					}
					if (category != "Reset")
					{
						break;
					}
				}
			}
		}
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00006AC4 File Offset: 0x00004CC4
	public static void HandleTotalIncome(double totalIncome)
	{
		GameState.GetAchievements().HandleAchievement("Lifetime Earnings");
	}

	// Token: 0x06000076 RID: 118 RVA: 0x00006AD8 File Offset: 0x00004CD8
	public static void HandleIncome(double income)
	{
		GameState.GetAchievements().HandleAchievement("Earnings");
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00006AEC File Offset: 0x00004CEC
	public static void TriggerLoveAchievement(Balance.GirlName girlName)
	{
		GameState.GetAchievements().HandleAchievement(girlName.ToFriendlyString());
	}

	// Token: 0x06000078 RID: 120 RVA: 0x00006B00 File Offset: 0x00004D00
	public static void HandleDates(int totalDates)
	{
		GameState.GetAchievements().HandleAchievement("Dates");
	}

	// Token: 0x06000079 RID: 121 RVA: 0x00006B14 File Offset: 0x00004D14
	public static void HandleGifts(int totalDates)
	{
		GameState.GetAchievements().HandleAchievement("Gifts");
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00006B28 File Offset: 0x00004D28
	public static void HandleHearts(long totalDates)
	{
		GameState.GetAchievements().HandleAchievement("Hearts");
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00006B3C File Offset: 0x00004D3C
	public static void HandleJob(Requirement.JobType job)
	{
		GameState.GetAchievements().HandleAchievement(job.ToFriendlyString());
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00006B50 File Offset: 0x00004D50
	public static void HandleGameTime()
	{
		GameState.GetAchievements().HandleAchievement("Game Time");
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00006B64 File Offset: 0x00004D64
	public static void HandleReset()
	{
		GameState.GetAchievements().HandleAchievement("Reset");
	}

	// Token: 0x0600007E RID: 126 RVA: 0x00006B78 File Offset: 0x00004D78
	public static void HandleUnlockedHobbies()
	{
		GameState.GetAchievements().HandleAchievement("Hobbies");
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00006B8C File Offset: 0x00004D8C
	public static void HandleHobbiesMinLevel()
	{
		GameState.GetAchievements().HandleAchievement("Skills");
	}

	// Token: 0x04000037 RID: 55
	public List<Achievements.Achievement> ActiveAchievements = new List<Achievements.Achievement>();

	// Token: 0x04000038 RID: 56
	public List<Achievements.Achievement> AllAchievements = new List<Achievements.Achievement>();

	// Token: 0x04000039 RID: 57
	public HashSet<Achievements.Achievement> ActiveAchievementsHash = new HashSet<Achievements.Achievement>();

	// Token: 0x0400003A RID: 58
	public static ListWithKey<string, Achievements.CategorySort> Categories = new ListWithKey<string, Achievements.CategorySort>();

	// Token: 0x0400003B RID: 59
	public Sprite[] AchievementIcons;

	// Token: 0x0400003C RID: 60
	private AssetBundle _achievementsBundle;

	// Token: 0x0400003D RID: 61
	private string[] CategoryNames;

	// Token: 0x0400003E RID: 62
	private Dictionary<string, List<Achievements.Achievement>> AchievementCategories = new Dictionary<string, List<Achievements.Achievement>>();

	// Token: 0x0400003F RID: 63
	private static double[] incomeComparison = new double[10];

	// Token: 0x04000040 RID: 64
	private static double[] moneyComparison = new double[8];

	// Token: 0x04000041 RID: 65
	private GameObject affectionMilestone;

	// Token: 0x04000042 RID: 66
	public Color UnlockedAchievement;

	// Token: 0x04000043 RID: 67
	public Text Countdown;

	// Token: 0x04000044 RID: 68
	public Text UnlockedCount;

	// Token: 0x04000045 RID: 69
	public Scrollbar Scrollbar;

	// Token: 0x04000046 RID: 70
	private bool initialized;

	// Token: 0x04000047 RID: 71
	private bool _initialBuildPending;

	// Token: 0x04000048 RID: 72
	private int prefabCount;

	// Token: 0x04000049 RID: 73
	public GameObject GirlAchievementPrefab;

	// Token: 0x0400004A RID: 74
	private bool builtAchievements;

	// Token: 0x0400004B RID: 75
	public readonly string GirlLockedName = "girl";

	// Token: 0x0400004C RID: 76
	private List<Achievements.CategoryMapping> AchievementMapping = new List<Achievements.CategoryMapping>();

	// Token: 0x0400004D RID: 77
	private int _achievementOffset;

	// Token: 0x0400004E RID: 78
	private int[] selectedAchievement;

	// Token: 0x0400004F RID: 79
	private float[] lastPercentComplete;

	// Token: 0x04000050 RID: 80
	private int lastAchievementCount = -1;

	// Token: 0x02000015 RID: 21
	public class CategorySort : IComparable<Achievements.CategorySort>
	{
		// Token: 0x06000082 RID: 130 RVA: 0x00006BA8 File Offset: 0x00004DA8
		public CategorySort(string name, int id)
		{
			this.Name = name;
			this.Id = id;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00006BC0 File Offset: 0x00004DC0
		int IComparable<Achievements.CategorySort>.CompareTo(Achievements.CategorySort other)
		{
			return this.Id.CompareTo(other.Id);
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00006BE4 File Offset: 0x00004DE4
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00006BEC File Offset: 0x00004DEC
		public string Name { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00006BF8 File Offset: 0x00004DF8
		// (set) Token: 0x06000087 RID: 135 RVA: 0x00006C00 File Offset: 0x00004E00
		public int Id { get; set; }

		// Token: 0x06000088 RID: 136 RVA: 0x00006C0C File Offset: 0x00004E0C
		public override string ToString()
		{
			return string.Format("{0} at {1}", this.Name, this.Id.ToString());
		}
	}

	// Token: 0x02000016 RID: 22
	public enum AchievementType
	{
		// Token: 0x04000059 RID: 89
		Money,
		// Token: 0x0400005A RID: 90
		Time,
		// Token: 0x0400005B RID: 91
		Girl,
		// Token: 0x0400005C RID: 92
		Hobby,
		// Token: 0x0400005D RID: 93
		Job,
		// Token: 0x0400005E RID: 94
		Event,
		// Token: 0x0400005F RID: 95
		Reset
	}

	// Token: 0x02000017 RID: 23
	public class Achievement
	{
		// Token: 0x06000089 RID: 137 RVA: 0x00006C38 File Offset: 0x00004E38
		public Achievement(Achievements.AchievementType type, Achievements.CheckAchievement check, AchievementModel achievement) : this(type, check, achievement.Name, achievement.Description, achievement.Notification, achievement.Reward, "achievements_" + achievement.Localization.ToString())
		{
			this.Data = achievement;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00006C84 File Offset: 0x00004E84
		public Achievement(Achievements.AchievementType type, Achievements.CheckAchievement check, string title, string text, string notification, string achLocalization = "") : this(type, check, title, text, notification, 0, achLocalization)
		{
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00006CA4 File Offset: 0x00004EA4
		public Achievement(Achievements.AchievementType type, Achievements.CheckAchievement check, string title, string text, string notification, int diamonds, string achLocalization = "")
		{
			this.Check = check;
			this.englishTitle = title;
			this.englishText = text;
			this.englishNotification = notification;
			this.Type = type;
			this.Diamonds = diamonds;
			this.Localization = achLocalization;
			this.titleId = string.Format("{0}_0", this.Localization);
			this.textId = string.Format("{0}_1", this.Localization);
			this.notificationId = string.Format("{0}_2", this.Localization);
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00006D4C File Offset: 0x00004F4C
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00006D54 File Offset: 0x00004F54
		public AchievementModel Data { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00006D60 File Offset: 0x00004F60
		public string Title
		{
			get
			{
				return Translations.GetTranslation(this.titleId, this.englishTitle);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00006D74 File Offset: 0x00004F74
		public string Text
		{
			get
			{
				if (this.englishText == this.englishNotification)
				{
					return Translations.GetTranslation(this.notificationId, this.englishNotification);
				}
				return Translations.GetTranslation(this.textId, this.englishText);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00006DB0 File Offset: 0x00004FB0
		public string Notification
		{
			get
			{
				return Translations.GetTranslation(this.notificationId, this.englishNotification);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00006DC4 File Offset: 0x00004FC4
		public string SaveHash
		{
			get
			{
				if (string.IsNullOrEmpty(this._cachedHash))
				{
					if (this.englishNotification.StartsWith("A year!?"))
					{
						this._cachedHash = string.Format("ACH{0}", "You've played for a year. I kind of hope you cheated to get here.".GetHashCode().ToString());
					}
					else
					{
						this._cachedHash = string.Format("ACH{0}", this.englishNotification.GetHashCode().ToString());
					}
				}
				return this._cachedHash;
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00006E48 File Offset: 0x00005048
		public bool Complete()
		{
			if (global::PlayerPrefs.HasKey(this.SaveHash) && global::PlayerPrefs.GetInt(this.SaveHash, 0) == 1)
			{
				global::PlayerPrefs.DeleteKey(this.SaveHash, false);
				int num = global::PlayerPrefs.GetInt(this._prefName, 0);
				num |= 1 << this.Index;
				global::PlayerPrefs.SetInt(this._prefName, num);
				return true;
			}
			int @int = global::PlayerPrefs.GetInt(this._prefName, 0);
			return (@int & 1 << this.Index) != 0;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00006ED0 File Offset: 0x000050D0
		// (set) Token: 0x06000094 RID: 148 RVA: 0x00006ED8 File Offset: 0x000050D8
		public int Index { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00006EE4 File Offset: 0x000050E4
		// (set) Token: 0x06000096 RID: 150 RVA: 0x00006EEC File Offset: 0x000050EC
		public int CategoryIndex
		{
			get
			{
				return this._categoryIndex;
			}
			set
			{
				this._categoryIndex = value;
				this._prefName = "ACH" + value.ToString();
			}
		}

		// Token: 0x04000060 RID: 96
		private string titleId;

		// Token: 0x04000061 RID: 97
		private string textId;

		// Token: 0x04000062 RID: 98
		private string notificationId;

		// Token: 0x04000063 RID: 99
		public Achievements.CheckAchievement Check;

		// Token: 0x04000064 RID: 100
		public Achievements.AchievementType Type;

		// Token: 0x04000065 RID: 101
		public int Diamonds;

		// Token: 0x04000066 RID: 102
		public string Localization;

		// Token: 0x04000067 RID: 103
		private string _cachedHash = string.Empty;

		// Token: 0x04000068 RID: 104
		private string englishText;

		// Token: 0x04000069 RID: 105
		private string englishTitle;

		// Token: 0x0400006A RID: 106
		private string englishNotification;

		// Token: 0x0400006B RID: 107
		private string _prefName = string.Empty;

		// Token: 0x0400006C RID: 108
		private int _categoryIndex = -1;
	}

	// Token: 0x02000018 RID: 24
	private class CategoryMapping
	{
		// Token: 0x0400006F RID: 111
		public GameObject Prefab;

		// Token: 0x04000070 RID: 112
		public string CategoryName;

		// Token: 0x04000071 RID: 113
		public int CategoryId;
	}

	// Token: 0x02000234 RID: 564
	// (Invoke) Token: 0x060011D4 RID: 4564
	public delegate bool CheckAchievement();
}
