using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020000CE RID: 206
public class Job2 : MonoBehaviour, IUpdateable, IEquatable<Job2>
{
	// Token: 0x06000488 RID: 1160 RVA: 0x00023AE4 File Offset: 0x00021CE4
	public static Job2 GetJob(Requirement.JobType type)
	{
		for (int i = 0; i < Job2.ActiveJobs.Count; i++)
		{
			if (Job2.ActiveJobs[i].JobType == type)
			{
				return Job2.ActiveJobs[i];
			}
		}
		Debug.Log("Invalid job");
		return new Job2();
	}

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x06000489 RID: 1161 RVA: 0x00023B40 File Offset: 0x00021D40
	// (set) Token: 0x0600048A RID: 1162 RVA: 0x00023B48 File Offset: 0x00021D48
	public JobModel Data { get; set; }

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x0600048B RID: 1163 RVA: 0x00023B54 File Offset: 0x00021D54
	public long Experience
	{
		get
		{
			return this.experience;
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x0600048C RID: 1164 RVA: 0x00023B5C File Offset: 0x00021D5C
	public long ExperienceToLevel
	{
		get
		{
			return this.States[this.Level].ExperienceToNextLevel;
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x0600048D RID: 1165 RVA: 0x00023B74 File Offset: 0x00021D74
	public long ExperiencePayout
	{
		get
		{
			if (this.Level == this.States.Length - 1)
			{
				return -1L;
			}
			long income = this.States[this.Level].Income;
			long income2 = this.States[this.Level + 1].Income;
			return income2 - income;
		}
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x0600048E RID: 1166 RVA: 0x00023BCC File Offset: 0x00021DCC
	public float IncomeDiff
	{
		get
		{
			if (this.Level == this.States.Length - 1)
			{
				return -1f;
			}
			float num = (float)this.States[this.Level].Income / this.States[this.Level].TimeToComplete;
			float num2 = (float)this.States[this.Level + 1].Income / this.States[this.Level + 1].TimeToComplete;
			return num2 - num;
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x0600048F RID: 1167 RVA: 0x00023C5C File Offset: 0x00021E5C
	public double IncomePerSecond
	{
		get
		{
			return (double)this.States[this.Level].Income / (double)this.States[this.Level].TimeToComplete;
		}
	}

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x06000490 RID: 1168 RVA: 0x00023C90 File Offset: 0x00021E90
	public int MaxLevel
	{
		get
		{
			return this.States.Length - 1;
		}
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x00023C9C File Offset: 0x00021E9C
	private void LoadStates()
	{
		this.States = new Job2.JobState[this.Data.States.Length];
		for (int i = 0; i < this.States.Length; i++)
		{
			this.States[i] = new Job2.JobState
			{
				ExperienceToNextLevel = this.Data.States[i].CyclesToLevel,
				Income = (long)this.Data.States[i].Money,
				TimeBlocks = this.Data.States[i].TimeBlocks,
				TimeToComplete = (float)this.Data.States[i].TimeToComplete,
				Title = this.Data.States[i].Name
			};
		}
		if (this.Data.UnlockLevel != null && (this.Data.UnlockLevel[0] != 0 || this.Data.UnlockLevel[1] != 0))
		{
			this.requiredSkillCount = this.Data.UnlockLevel;
		}
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x00023DCC File Offset: 0x00021FCC
	private bool IsUnlocked()
	{
		if (this.States == null)
		{
			this.LoadStates();
		}
		if (this.JobType == Requirement.JobType.Digger)
		{
			return (Playfab.AwardedItems & Playfab.PlayfabItems.Charlotte) != (Playfab.PlayfabItems)0L || this.Level != 0 || this.Experience != 0L;
		}
		if (this.JobType == Requirement.JobType.Planter)
		{
			return (Playfab.AwardedItems & Playfab.PlayfabItems.Suzu) != (Playfab.PlayfabItems)0L || this.Level != 0 || this.Experience != 0L;
		}
		if (!this.Data.Available)
		{
			return false;
		}
		if (this.Data.UnlockHobby == null || this.Data.UnlockHobby.Length == 0)
		{
			return true;
		}
		for (int i = 0; i < this.Data.UnlockHobby.Length; i++)
		{
			short num = 0;
			foreach (KeyValuePair<short, HobbyModel> keyValuePair in Universe.Hobbies)
			{
				if (keyValuePair.Value.Resource.Id == this.Data.UnlockHobby[i])
				{
					num = keyValuePair.Value.Id;
				}
			}
			int num2 = Skills.SkillLevel[(int)(num - 1)];
			if (num2 < this.requiredSkillCount[i])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x00023F54 File Offset: 0x00022154
	private void Start()
	{
		if (this.States == null)
		{
			this.LoadStates();
		}
		if (!Job2.ActiveJobs.Contains(this))
		{
			Job2.ActiveJobs.Add(this);
		}
		if (this.moneyText == null)
		{
			this.FindChildren();
		}
		if (base.transform.Find("Money System") != null)
		{
			this.particles = base.transform.Find("Money System").GetComponent<ParticleSystem>();
		}
		else
		{
			GameObject gameObject = base.transform.parent.GetChild(0).Find("Money System").gameObject;
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			gameObject2.transform.SetParent(base.transform);
			gameObject2.transform.localPosition = new Vector3(123f, 79f, 0f);
			this.particles = gameObject2.GetComponent<ParticleSystem>();
		}
		this.UpdateRequirements();
		if (!this.locked)
		{
			base.transform.Find("Background Locked").GetComponent<Image>().color = new Color(0f, 0f, 0f, (!this.IsActive) ? 0.1f : 0f);
		}
		if (this.Level == 0 && !this.IsActive && this.currentTime == 0f)
		{
			this.Reset();
		}
		this.UpdateInfo();
		base.transform.Find("Money System").GetComponent<ParticleLayer>().MagnetPoint = GameObject.Find("Canvas").transform.Find("Top UI/Money Attractor");
		this.progressImage = this.progressBar.transform.Find("ProgressImage").GetComponent<Image>();
		if (this.States[this.Level].TimeToComplete / (GameState.CurrentState.TimeMultiplier.Value * (float)GameState.PurchasedAdMultiplier) < 1f && this.StripeProgress != null)
		{
			if (this.progressImage.sprite != this.StripeProgress)
			{
				this.progressImage.sprite = this.StripeProgress;
				this.progressBar.rectTransform.sizeDelta = new Vector2(144f, 26f);
				this.progressImage.rectTransform.sizeDelta = new Vector2(200f, 26f);
			}
			this.progressPosition = (this.progressPosition + Time.deltaTime * 20f) % 20f;
			this.progressImage.rectTransform.localPosition = new Vector3(this.progressPosition - 20f, 0f, 0f);
		}
		this.progressImage.color = ((this.Level != this.States.Length - 1) ? new Color(1f, 1f, 1f) : new Color(0f, 1f, 0.21568628f));
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x0002426C File Offset: 0x0002246C
	private void UpdateRequirements()
	{
		if (this.Data == null)
		{
			return;
		}
		if (this.Data.UnlockHobby == null || this.Data.UnlockHobby.Length == 0)
		{
			if ((this.Data.Id == 17 || this.Data.Id == 18) && this.jobRequirements == null)
			{
				this.jobRequirements = new GameObject[1];
				this.jobRequirements[0] = (GameObject)UnityEngine.Object.Instantiate(this.RequirementPrefab, base.transform.Find("Requirements"));
				this.jobRequirements[0].transform.localPosition = new Vector3(20f, 0f, 0f);
				if (this.Data.Id == 17)
				{
					this.jobRequirements[0].transform.Find("Text").GetComponent<Text>().text = "Charlotte Bundle";
				}
				else if (this.Data.Id == 18)
				{
					this.jobRequirements[0].transform.Find("Text").GetComponent<Text>().text = "Suzu Bundle";
				}
				this.jobRequirements[0].transform.Find("Check").gameObject.SetActive(false);
			}
			return;
		}
		if (this.jobRequirements == null)
		{
			this.jobRequirements = new GameObject[this.Data.UnlockHobby.Length];
			for (int i = 0; i < this.jobRequirements.Length; i++)
			{
				this.jobRequirements[i] = (GameObject)UnityEngine.Object.Instantiate(this.RequirementPrefab, base.transform.Find("Requirements"));
				this.jobRequirements[i].transform.localPosition = new Vector3((float)(i * 155 + 20), 0f, 0f);
				short num = 0;
				foreach (KeyValuePair<short, HobbyModel> keyValuePair in Universe.Hobbies)
				{
					if (keyValuePair.Value.Resource.Id == this.Data.UnlockHobby[i])
					{
						num = keyValuePair.Value.Id;
					}
				}
				this.jobRequirements[i].transform.Find("Text").GetComponent<Text>().text = string.Format("{0} {1}", this.requiredSkillCount[i].ToString(), Translations.TranslateSkill((Requirement.Skill)(num - 1)));
			}
			Translations.CurrentLanguage += delegate(int language)
			{
				for (int j = 0; j < this.jobRequirements.Length; j++)
				{
					short num5 = 0;
					foreach (KeyValuePair<short, HobbyModel> keyValuePair3 in Universe.Hobbies)
					{
						if (keyValuePair3.Value.Resource.Id == this.Data.UnlockHobby[j])
						{
							num5 = keyValuePair3.Value.Id;
						}
					}
					this.jobRequirements[j].transform.Find("Text").GetComponent<Text>().text = string.Format("{0} {1}", this.requiredSkillCount[j].ToString(), Translations.TranslateSkill((Requirement.Skill)(num5 - 1)));
				}
				this.UpdateInfo();
			};
		}
		if (Skills.SkillLevel == null)
		{
			Skills.Init();
		}
		int num2 = 0;
		while (num2 < this.jobRequirements.Length && this.Data.Available)
		{
			short num3 = 0;
			foreach (KeyValuePair<short, HobbyModel> keyValuePair2 in Universe.Hobbies)
			{
				if (keyValuePair2.Value.Resource.Id == this.Data.UnlockHobby[num2])
				{
					num3 = keyValuePair2.Value.Id;
				}
			}
			int num4 = Skills.SkillLevel[(int)(num3 - 1)];
			this.jobRequirements[num2].transform.Find("Check").gameObject.SetActive(num4 >= this.requiredSkillCount[num2]);
			num2++;
		}
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x0002463C File Offset: 0x0002283C
	private void FindChildren()
	{
		this.moneyText = base.transform.Find("Salary Text").GetComponent<Text>();
		this.timeText = base.transform.Find("Time Text").GetComponent<Text>();
		this.progressBar = base.transform.Find("Progress Bar").GetComponent<Image>();
		this.jobTitle = base.transform.Find("Title Text").GetComponent<Text>();
		this.remainingText = base.transform.Find("Remaining Time Text").GetComponent<Text>();
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x000246D0 File Offset: 0x000228D0
	public void CheckLock()
	{
		this.UpdateRequirements();
		if (this.IsUnlocked())
		{
			this.Unlock();
		}
		else
		{
			this.Lock();
		}
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x00024700 File Offset: 0x00022900
	private void Lock()
	{
		if (this.locked)
		{
			return;
		}
		base.transform.Find("Requirements").gameObject.SetActive(true);
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x00024734 File Offset: 0x00022934
	private void Unlock()
	{
		if (!this.locked)
		{
			return;
		}
		base.transform.Find("Title Text").GetComponent<Text>().color = Color.white;
		if (this.moneyText == null)
		{
			this.FindChildren();
		}
		base.GetComponent<Button>().onClick.RemoveAllListeners();
		for (int i = 0; i < base.transform.childCount; i++)
		{
			base.transform.GetChild(i).gameObject.SetActive(true);
		}
		base.transform.Find("Requirements").gameObject.SetActive(false);
		if (base.transform.Find("Diamond") != null)
		{
			base.transform.Find("Diamond").GetComponent<Image>().color = ((!this.Gilded) ? new Color(1f, 1f, 1f, 0.3f) : new Color(1f, 1f, 1f, 1f));
			if (!this.Gilded)
			{
				base.transform.Find("Diamond").GetComponent<Button>().onClick.RemoveAllListeners();
				base.transform.Find("Diamond").GetComponent<Button>().onClick.AddListener(delegate()
				{
					GameObject.Find("Canvas").transform.Find("Popups/Gild Popup").GetComponent<HobbyGild>().Init(this);
				});
			}
		}
		base.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnJobClicked));
		GameState.UpdatePanels(GameState.UpdateType.Skill);
		if ((Job2.AvailableJobs & this.JobType) == Requirement.JobType.None)
		{
			Job2.AvailableJobs |= this.JobType;
			GameState.ShowJobsNotification();
			global::PlayerPrefs.SetInt("AvailableJobs", (int)Job2.AvailableJobs);
		}
		this.locked = false;
		this.UpdateInfo();
		if (GameState.Initialized)
		{
			Achievements.HandleJob(this.JobType);
		}
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x00024928 File Offset: 0x00022B28
	public void OnJobClicked()
	{
		this.JobAvatar.LastToggledJob = this;
		GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.HoverSelect);
		if (this.IsActive)
		{
			this.DisableJob();
		}
		else
		{
			if (!this.HasEnoughFreeTime())
			{
				this.reminderTime = 2f;
				Utilities.PurchaseTimeBlocks(this.States[this.Level].TimeBlocks - FreeTime.Free);
				return;
			}
			this.EnableJob();
		}
		GameState.CurrentState.QueueSave();
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x000249B4 File Offset: 0x00022BB4
	public bool HasEnoughFreeTime()
	{
		return FreeTime.Free >= this.States[this.Level].TimeBlocks;
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x000249E4 File Offset: 0x00022BE4
	public void AddMultiplier()
	{
		this.Gilded = true;
		this.StoreState();
		this.UpdateInfo();
		if (this.JobAvatar.ActiveJob.Value == this)
		{
			this.JobAvatar.ActiveJob.Force(this);
		}
		base.transform.Find("Diamond").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
		base.transform.Find("Diamond").GetComponent<Button>().onClick.RemoveAllListeners();
		base.transform.Find("Unlock System").GetComponent<ParticleSystem>().Play();
		GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.GirlUnlock);
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x00024AAC File Offset: 0x00022CAC
	private void Update()
	{
		if (this.IsActive)
		{
			if (this.States[this.Level].TimeToComplete / (GameState.CurrentState.TimeMultiplier.Value * (float)GameState.PurchasedAdMultiplier) < 1f && this.StripeProgress != null && this.progressImage != null)
			{
				if (this.progressImage.sprite != this.StripeProgress)
				{
					this.remainingText.text = string.Empty;
					this.progressImage.sprite = this.StripeProgress;
					this.progressBar.rectTransform.sizeDelta = new Vector2(144f, 26f);
					this.progressImage.rectTransform.sizeDelta = new Vector2(200f, 26f);
				}
				if (this.progressImage.rectTransform.sizeDelta.x != 200f)
				{
					this.progressImage.rectTransform.sizeDelta = new Vector2(200f, 26f);
				}
				this.progressPosition = (this.progressPosition + Time.deltaTime * 20f) % 20f;
				this.progressImage.rectTransform.localPosition = new Vector3(this.progressPosition - 20f, 0f, 0f);
			}
			else
			{
				this.progressBar.rectTransform.sizeDelta = new Vector2(this.currentTime / this.States[this.Level].TimeToComplete * 144f, 26f);
				int num = (int)Mathf.Round((this.States[this.Level].TimeToComplete - this.currentTime) / (GameState.CurrentState.TimeMultiplier.Value * (float)GameState.PurchasedAdMultiplier));
				if (num < 60)
				{
					this.remainingText.text = string.Format("{0}s", num.ToString());
				}
				else
				{
					this.remainingText.text = string.Format("{0}m {1}s", (num / 60).ToString(), (num % 60).ToString());
				}
				this.reminderTime = 0f;
			}
			this.timeSinceParticles += Time.deltaTime;
		}
		else if (this.reminderTime > 0f)
		{
			this.reminderTime = Mathf.Max(0f, this.reminderTime - Time.deltaTime);
			if (this.reminderTime == 0f)
			{
				this.remainingText.text = string.Empty;
			}
		}
		else if (!this.locked)
		{
			base.transform.Find("Background Locked").GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.1f);
		}
		if (this.iconFade > 0f)
		{
			base.transform.Find("Icon").GetComponent<Image>().color = new Color(1f, 1f, 1f, Mathf.Max(0f, Mathf.Min(1f, this.iconFade)));
			base.transform.Find("Icon").GetComponent<Image>().rectTransform.localScale = new Vector3(1f, 1f, 1f) * Mathf.Max(1f, 2f - this.iconFade);
			this.iconFade -= Time.deltaTime;
		}
		if (!this.locked && !this.playedUnlockParticles && this.currentTime == 0f)
		{
			base.transform.Find("Unlock System").GetComponent<ParticleSystem>().Play();
			GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.GirlUnlock);
			this.playedUnlockParticles = true;
			Utilities.SendAnalytic(Utilities.AnalyticType.Unlock, this.JobType.ToFriendlyString());
		}
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x00024EC8 File Offset: 0x000230C8
	public void Reset()
	{
		this.IsActive = false;
		this.Level = 0;
		this.experience = 0L;
		this.locked = true;
		this.CheckLock();
		if (this.progressBar != null)
		{
			this.progressBar.rectTransform.sizeDelta = new Vector2(0f, 26f);
		}
		if (this.remainingText != null)
		{
			this.remainingText.text = string.Empty;
		}
		this.currentTime = 0f;
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x00024F54 File Offset: 0x00023154
	private void UpdateInfo()
	{
		if (this.moneyText == null || this.timeText == null || this.jobTitle == null)
		{
			return;
		}
		this.moneyText.text = string.Format("{0}", (this.States[this.Level].Income * ((!this.Gilded) ? 1L : 5L)).ToString("n0"));
		this.timeText.text = string.Format("x{0}", this.States[this.Level].TimeBlocks.ToString());
		string arg = Translations.TranslateJob(this.JobType, 0);
		string arg2 = Translations.TranslateJob(this.JobType, this.Level + 1);
		this.jobTitle.text = string.Format("{0} - {1}", arg, arg2);
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x0002504C File Offset: 0x0002324C
	public GameState.UpdateType PerformUpdate(float dt)
	{
		if (!this.IsActive)
		{
			return GameState.UpdateType.None;
		}
		if (this.States == null)
		{
			this.LoadStates();
		}
		GameState.UpdateType updateType = GameState.UpdateType.None;
		float timeToComplete = this.States[this.Level].TimeToComplete;
		long income = this.States[this.Level].Income;
		long experienceToNextLevel = this.States[this.Level].ExperienceToNextLevel;
		if (this.currentTime + dt >= timeToComplete)
		{
			while (this.currentTime + dt >= timeToComplete)
			{
				long num = experienceToNextLevel - this.experience;
				if (this.Level == this.States.Length - 1)
				{
					num = long.MaxValue;
				}
				long num2 = (long)Mathf.Min((float)num, (this.currentTime + dt) / timeToComplete);
				if (num2 < 0L)
				{
					num2 = 0L;
				}
				if (GameState.TotalIncome.Value == -1.0)
				{
					GameState.TotalIncome.Value = 0.0;
					if (GameState.GetIntroScreen() != null)
					{
						GameState.GetIntroScreen().JobFinished();
					}
				}
				double num3 = (double)income * (double)num2;
				if (this.Gilded)
				{
					num3 *= 5.0;
				}
				GameState.Money.Value += num3;
				GameState.TotalIncome.Value += num3;
				dt -= timeToComplete * (float)num2;
				if (this.Level < this.States.Length - 1)
				{
					this.experience += num2;
					if (this.experience >= experienceToNextLevel)
					{
						FreeTime.JobTime -= this.States[this.Level].TimeBlocks;
						updateType |= GameState.UpdateType.Job;
						this.Level++;
						Achievements.HandleJob(this.JobType);
						this.experience = 0L;
						this.UpdateInfo();
						if (FreeTime.Free < this.States[this.Level].TimeBlocks)
						{
							this.DisableJob();
						}
						FreeTime.JobTime += this.States[this.Level].TimeBlocks;
						if (this.progressImage != null && this.Level == this.States.Length - 1)
						{
							this.progressImage.color = new Color(0f, 1f, 0.21568628f);
						}
						experienceToNextLevel = this.States[this.Level].ExperienceToNextLevel;
						income = this.States[this.Level].Income;
						timeToComplete = this.States[this.Level].TimeToComplete;
					}
				}
			}
			if (this.particles != null && base.gameObject.activeInHierarchy && !Settings.ParticlesDisabled && base.transform.position.y > -175f && base.transform.position.y < 135f && this.timeSinceParticles > 0.25f)
			{
				this.particles.Play();
				Audio component = GameState.CurrentState.GetComponent<Audio>();
				if (component != null)
				{
					component.PlayOnce(Audio.AudioFile.Coins);
				}
				this.timeSinceParticles = 0f;
			}
			updateType |= GameState.UpdateType.Money;
		}
		this.currentTime = Mathf.Min(timeToComplete, this.currentTime + dt);
		if (this.currentTime < 0f)
		{
			this.currentTime = 0f;
		}
		return updateType;
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x060004A0 RID: 1184 RVA: 0x000253F4 File Offset: 0x000235F4
	private JobAvatar JobAvatar
	{
		get
		{
			if (this.jobAvatar == null)
			{
				this.jobAvatar = new CachedObject<JobAvatar>(GameState.CurrentState.gameObject, "Jobs");
			}
			return this.jobAvatar.Object;
		}
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x00025434 File Offset: 0x00023634
	public void EnableJob()
	{
		if (this.IsActive)
		{
			return;
		}
		FreeTime.JobTime += this.States[this.Level].TimeBlocks;
		this.IsActive = true;
		GameState.RegisterUpdate(this);
		this.JobAvatar.ActiveJob.Value = this;
		base.transform.Find("Background Locked").GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
		base.transform.Find("Icon").GetComponent<Image>().sprite = this.PlayIcon;
		base.transform.Find("Icon").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
		this.iconFade = 1f;
		this.StoreState();
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x00025524 File Offset: 0x00023724
	public void DisableJob()
	{
		if (!this.IsActive)
		{
			return;
		}
		FreeTime.JobTime -= this.States[this.Level].TimeBlocks;
		if (this.remainingText != null)
		{
			this.remainingText.text = string.Empty;
		}
		this.IsActive = false;
		GameState.UnregisterUpdate(this);
		if (this.JobAvatar.ActiveJob.Value == this)
		{
			this.JobAvatar.ActiveJob.Value = null;
			for (int i = 0; i < base.transform.parent.childCount; i++)
			{
				if (base.transform.parent.GetChild(i).GetComponent<Job2>().IsActive)
				{
					this.JobAvatar.ActiveJob.Value = base.transform.parent.GetChild(i).GetComponent<Job2>();
					break;
				}
			}
		}
		base.transform.Find("Icon").GetComponent<Image>().sprite = this.PauseIcon;
		base.transform.Find("Icon").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
		base.transform.Find("Icon").GetComponent<Image>().rectTransform.localScale = new Vector3(1f, 1f, 1f);
		this.iconFade = 0f;
		this.StoreState();
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x000256BC File Offset: 0x000238BC
	public void LoadState()
	{
		if (!GameState.Initialized)
		{
			return;
		}
		if (this.States == null)
		{
			this.LoadStates();
		}
		if (!Job2.ActiveJobs.Contains(this))
		{
			Job2.ActiveJobs.Add(this);
		}
		string str = string.Format("Job{0}", this.JobType.ToSaveName());
		this.DisableJob();
		this.Level = Mathf.Max(0, Mathf.Min(this.States.Length - 1, global::PlayerPrefs.GetInt(str + "Level", 0)));
		this.Gilded = (global::PlayerPrefs.GetInt(str + "Gilded", 0) == 1);
		this.experience = global::PlayerPrefs.GetLong(str + "Experience", 0L);
		if (this.experience < 0L)
		{
			this.experience = 0L;
		}
		this.currentTime = global::PlayerPrefs.GetFloat(str + "Time", 0f);
		if (global::PlayerPrefs.GetInt(str + "Locked", 0) == 1 || this.currentTime != 0f)
		{
			this.playedUnlockParticles = true;
		}
		if (this.currentTime != 0f)
		{
			if (this.progressBar == null)
			{
				this.FindChildren();
			}
			this.progressBar.rectTransform.sizeDelta = new Vector2(this.currentTime / this.States[this.Level].TimeToComplete * 144f, 26f);
		}
		this.CheckLock();
		this.UpdateInfo();
		if (global::PlayerPrefs.GetInt(str + "Active", 0) == 1 && FreeTime.Free >= this.States[this.Level].TimeBlocks && this.IsUnlocked())
		{
			this.EnableJob();
		}
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x00025890 File Offset: 0x00023A90
	public void StoreState()
	{
		if (this.prefActive == null)
		{
			this.prefActive = new CachedPlayerPref(string.Format("Job{0}Active", this.JobType.ToSaveName()));
		}
		if (this.prefTime == null)
		{
			this.prefTime = new CachedPlayerPref(string.Format("Job{0}Time", this.JobType.ToSaveName()));
		}
		if (this.prefLocked == null)
		{
			this.prefLocked = new CachedPlayerPref(string.Format("Job{0}Locked", this.JobType.ToSaveName()));
		}
		if (this.prefGilded == null)
		{
			this.prefGilded = new CachedPlayerPref(string.Format("Job{0}Gilded", this.JobType.ToSaveName()));
		}
		this.StoreStateFast();
		this.prefActive.SetInt((!this.IsActive) ? 0 : 1);
		this.prefTime.SetFloat(this.currentTime);
		this.prefLocked.SetInt((!this.locked) ? 1 : 0);
		this.prefGilded.SetInt((!this.Gilded) ? 0 : 1);
		GameState.CurrentState.QueueSave();
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x000259C4 File Offset: 0x00023BC4
	public void StoreStateFast()
	{
		if (this.prefLevel == null)
		{
			this.prefLevel = new CachedPlayerPref(string.Format("Job{0}Level", this.JobType.ToSaveName()));
		}
		if (this.prefExperience == null)
		{
			this.prefExperience = new CachedPlayerPref(string.Format("Job{0}Experience", this.JobType.ToSaveName()));
		}
		this.prefLevel.SetInt(this.Level);
		this.prefExperience.SetLong(this.experience);
		GameState.CurrentState.QueueSave();
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x00025A54 File Offset: 0x00023C54
	public void SaveCurrentTime()
	{
		if (this.prefTime == null)
		{
			this.prefTime = new CachedPlayerPref(string.Format("Job{0}Time", this.JobType.ToSaveName()));
		}
		this.prefTime.SetFloat(this.currentTime);
		this.StoreStateFast();
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x00025AA4 File Offset: 0x00023CA4
	public bool Equals(Job2 other)
	{
		return this == other;
	}

	// Token: 0x04000499 RID: 1177
	public static List<Job2> ActiveJobs = new List<Job2>();

	// Token: 0x0400049A RID: 1178
	public static Requirement.JobType AvailableJobs = Requirement.JobType.None;

	// Token: 0x0400049B RID: 1179
	private Image progressImage;

	// Token: 0x0400049C RID: 1180
	private Image progressBar;

	// Token: 0x0400049D RID: 1181
	private Text jobTitle;

	// Token: 0x0400049E RID: 1182
	private Text moneyText;

	// Token: 0x0400049F RID: 1183
	private Text timeText;

	// Token: 0x040004A0 RID: 1184
	private Text remainingText;

	// Token: 0x040004A1 RID: 1185
	public GameObject RequirementPrefab;

	// Token: 0x040004A2 RID: 1186
	public Requirement.JobType JobType;

	// Token: 0x040004A3 RID: 1187
	public Sprite PlayIcon;

	// Token: 0x040004A4 RID: 1188
	public Sprite PauseIcon;

	// Token: 0x040004A5 RID: 1189
	public Sprite StripeProgress;

	// Token: 0x040004A6 RID: 1190
	public Sprite JobSprite1;

	// Token: 0x040004A7 RID: 1191
	public Sprite JobSprite2;

	// Token: 0x040004A8 RID: 1192
	public int Level;

	// Token: 0x040004A9 RID: 1193
	public bool Gilded;

	// Token: 0x040004AA RID: 1194
	private GameObject[] jobRequirements;

	// Token: 0x040004AB RID: 1195
	private long experience;

	// Token: 0x040004AC RID: 1196
	private float reminderTime;

	// Token: 0x040004AD RID: 1197
	private float currentTime;

	// Token: 0x040004AE RID: 1198
	private bool IsActive;

	// Token: 0x040004AF RID: 1199
	private ParticleSystem particles;

	// Token: 0x040004B0 RID: 1200
	private Job2.JobState[] States;

	// Token: 0x040004B1 RID: 1201
	private int[] requiredSkillCount;

	// Token: 0x040004B2 RID: 1202
	private GameObject LockedPrefab;

	// Token: 0x040004B3 RID: 1203
	private bool locked = true;

	// Token: 0x040004B4 RID: 1204
	private bool playedUnlockParticles;

	// Token: 0x040004B5 RID: 1205
	private float progressPosition;

	// Token: 0x040004B6 RID: 1206
	private float timeSinceParticles;

	// Token: 0x040004B7 RID: 1207
	private float iconFade;

	// Token: 0x040004B8 RID: 1208
	private CachedObject<JobAvatar> jobAvatar;

	// Token: 0x040004B9 RID: 1209
	private CachedPlayerPref prefActive;

	// Token: 0x040004BA RID: 1210
	private CachedPlayerPref prefTime;

	// Token: 0x040004BB RID: 1211
	private CachedPlayerPref prefLocked;

	// Token: 0x040004BC RID: 1212
	private CachedPlayerPref prefGilded;

	// Token: 0x040004BD RID: 1213
	private CachedPlayerPref prefLevel;

	// Token: 0x040004BE RID: 1214
	private CachedPlayerPref prefExperience;

	// Token: 0x020000CF RID: 207
	public struct JobState
	{
		// Token: 0x060004AA RID: 1194 RVA: 0x00025BD8 File Offset: 0x00023DD8
		public JobState(string title, float timeToComplete, int timeBlocks, long income, long experienceToNextLevel)
		{
			this.Title = title;
			this.TimeToComplete = timeToComplete;
			this.TimeBlocks = Mathf.Min(25, timeBlocks);
			this.Income = income;
			this.ExperienceToNextLevel = experienceToNextLevel;
		}

		// Token: 0x040004C0 RID: 1216
		public string Title;

		// Token: 0x040004C1 RID: 1217
		public float TimeToComplete;

		// Token: 0x040004C2 RID: 1218
		public int TimeBlocks;

		// Token: 0x040004C3 RID: 1219
		public long Income;

		// Token: 0x040004C4 RID: 1220
		public long ExperienceToNextLevel;
	}
}
