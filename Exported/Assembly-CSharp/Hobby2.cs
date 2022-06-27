using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000A0 RID: 160
public class Hobby2 : MonoBehaviour, IUpdateable, IEquatable<Hobby2>
{
	// Token: 0x17000054 RID: 84
	// (get) Token: 0x060003B3 RID: 947 RVA: 0x0001F5C4 File Offset: 0x0001D7C4
	public static int MinimumLevel
	{
		get
		{
			if (Skills.SkillLevel == null)
			{
				Skills.Init();
			}
			int num = int.MaxValue;
			foreach (int num2 in Skills.SkillLevel)
			{
				if (num2 < num)
				{
					num = num2;
				}
			}
			return num;
		}
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x060003B4 RID: 948 RVA: 0x0001F610 File Offset: 0x0001D810
	// (set) Token: 0x060003B5 RID: 949 RVA: 0x0001F618 File Offset: 0x0001D818
	public HobbyModel Data { get; set; }

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x060003B6 RID: 950 RVA: 0x0001F624 File Offset: 0x0001D824
	public bool Locked
	{
		get
		{
			return this.locked;
		}
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x060003B7 RID: 951 RVA: 0x0001F62C File Offset: 0x0001D82C
	public bool Enabled
	{
		get
		{
			return this.isActive;
		}
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x0001F634 File Offset: 0x0001D834
	private double[] CreateHobbyTime()
	{
		double[] array = new double[Hobby2.MaxLevel + 1];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.Data.InitialTime * Math.Pow(this.Data.IncreasePerLevel, (double)i);
		}
		return array;
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x060003B9 RID: 953 RVA: 0x0001F684 File Offset: 0x0001D884
	private double BaseTime
	{
		get
		{
			if (this.states == null)
			{
				this.states = this.CreateHobbyTime();
			}
			if (this.Level.Value < 0 || this.Level.Value >= this.states.Length)
			{
				return double.MaxValue;
			}
			return this.states[this.Level.Value];
		}
	}

	// Token: 0x060003BA RID: 954 RVA: 0x0001F6F0 File Offset: 0x0001D8F0
	private void Start()
	{
		if (this.states == null)
		{
			this.states = this.CreateHobbyTime();
		}
		if (!Hobby2.ActiveHobbies.Contains(this))
		{
			Hobby2.ActiveHobbies.Add(this);
		}
		this.remainingText = base.transform.Find("Remaining Time Text").GetComponent<Text>();
		this.levelText = base.transform.Find("Level Text").GetComponent<Text>();
		this.progressBar = base.transform.Find("Progress Bar").GetComponent<Image>();
		this.progressBar.fillAmount = (float)(this.currentTime / this.BaseTime);
		this.levelText.text = this.Level.Value.ToString();
		this.UpdateText();
		Translations.CurrentLanguage += delegate(int language)
		{
			this.UpdateText();
		};
		if (!this.Gilded)
		{
			base.transform.Find("Plus Button").GetComponent<Button>().onClick.RemoveAllListeners();
			base.transform.Find("Plus Button").GetComponent<Button>().onClick.AddListener(delegate()
			{
				GameState.CurrentState.transform.Find("Popups/Gild Popup").GetComponent<HobbyGild>().Init(this);
			});
		}
	}

	// Token: 0x060003BB RID: 955 RVA: 0x0001F830 File Offset: 0x0001DA30
	public void Unlock(bool spawnParticles = true)
	{
		if (!this.locked)
		{
			return;
		}
		base.GetComponent<Image>().sprite = this.UnlockedBackground;
		base.transform.Find("Level Image").GetComponent<Image>().sprite = this.UnlockedIcon;
		base.transform.Find("Progress Background").GetComponent<Image>().sprite = this.UnlockedProgress;
		base.GetComponent<Button>().interactable = true;
		base.transform.Find("Icon").gameObject.SetActive(true);
		base.transform.Find("Remaining Time Text").gameObject.SetActive(true);
		base.transform.Find("Plus Button").gameObject.SetActive(true);
		if (!this.Gilded)
		{
			base.transform.Find("Plus Button").GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
		}
		if (!this.isActive)
		{
			base.GetComponent<Image>().color = new Color(0.7921569f, 0.2627451f, 0.9411765f, 1f);
		}
		this.playedUnlockParticles = !spawnParticles;
		this.locked = false;
		if (!Hobby2.AvailableHobbies.Contains(this.Data.Id))
		{
			Hobby2.AvailableHobbies.Add(this.Data.Id);
			Achievements.HandleUnlockedHobbies();
		}
		GameState.ShowHobbyNotification();
	}

	// Token: 0x060003BC RID: 956 RVA: 0x0001F9B0 File Offset: 0x0001DBB0
	private void Update()
	{
		if (this.isActive)
		{
			float num = (float)(this.currentTime / this.BaseTime);
			if (Math.Abs(num - this.progressBar.fillAmount) > 0.005f)
			{
				this.progressBar.fillAmount = (float)(this.currentTime / this.BaseTime);
			}
			this.reminderTime = 0f;
		}
		if (!this.locked && !this.playedUnlockParticles)
		{
			base.transform.Find("Unlock System").GetComponent<ParticleSystem>().Play();
			GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.GirlUnlock);
			this.playedUnlockParticles = true;
			Utilities.SendAnalytic(Utilities.AnalyticType.Unlock, this.Data.Resource.Singular.English);
		}
		if (this.reminderTime > 0f)
		{
			this.reminderTime = Mathf.Max(0f, this.reminderTime - Time.deltaTime);
			if (this.reminderTime == 0f)
			{
				this.remainingText.text = string.Empty;
			}
		}
		else
		{
			long num2 = (long)((this.BaseTime - this.currentTime) / (double)((float)((!this.Gilded) ? 1 : 16) * GameState.CurrentState.TimeMultiplier.Value * (float)GameState.PurchasedAdMultiplier));
			if (num2 != this.lastRemaining)
			{
				this.remainingText.text = Hobby2.GetTime(num2);
				this.lastRemaining = num2;
			}
		}
		if (this.iconFade > 0f)
		{
			base.transform.Find("Icon").GetComponent<Image>().color = new Color(1f, 1f, 1f, Mathf.Max(0f, Mathf.Min(1f, this.iconFade)));
			base.transform.Find("Icon").GetComponent<Image>().rectTransform.localScale = new Vector3(1f, 1f, 1f) * Mathf.Max(1f, 2f - this.iconFade);
			this.iconFade -= Time.deltaTime;
		}
	}

	// Token: 0x060003BD RID: 957 RVA: 0x0001FBE4 File Offset: 0x0001DDE4
	public void RemoveMultiplier()
	{
		this.Gilded = false;
		this.StoreState();
		if (this.HobbyAvatar.ActiveHobby.Value == this)
		{
			this.HobbyAvatar.ActiveHobby.Force(this);
		}
		base.transform.Find("Plus Button").GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
		base.transform.Find("Plus Button").GetComponent<Button>().onClick.RemoveAllListeners();
		base.transform.Find("Plus Button").GetComponent<Button>().onClick.AddListener(delegate()
		{
			GameObject.Find("Canvas").transform.Find("Popups/Skip Level Popup").GetComponent<HobbyGild>().Init(this);
		});
	}

	// Token: 0x060003BE RID: 958 RVA: 0x0001FCA8 File Offset: 0x0001DEA8
	public void AddMultiplier()
	{
		this.Gilded = true;
		this.StoreState();
		if (this.HobbyAvatar.ActiveHobby.Value == this)
		{
			this.HobbyAvatar.ActiveHobby.Force(this);
		}
		base.transform.Find("Plus Button").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
		base.transform.Find("Unlock System").GetComponent<ParticleSystem>().Play();
		GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.GirlUnlock);
	}

	// Token: 0x060003BF RID: 959 RVA: 0x0001FD4C File Offset: 0x0001DF4C
	public GameState.UpdateType PerformUpdate(float dt)
	{
		if (!this.isActive)
		{
			return GameState.UpdateType.None;
		}
		dt = ((!this.Gilded) ? dt : (16f * dt));
		double num = (double)dt;
		if (this.currentTime + num >= this.BaseTime)
		{
			while (this.currentTime + num >= this.BaseTime)
			{
				num -= this.BaseTime - this.currentTime;
				this.currentTime = 0.0;
				this.Level.Value = Mathf.Min(Hobby2.MaxLevel, this.Level.Value + 1);
				if (this.Level.Value % 10 == 0 || (this.Level.Value > 50 && this.Level.Value % 5 == 0) || this.Level.Value > 70)
				{
					Utilities.SendAnalytic(Utilities.AnalyticType.Hobby, this.Data.Resource.Singular.English + this.Level.Value.ToString());
				}
				if (this.Level.Value == Hobby2.MaxLevel)
				{
					this.DisableHobby();
					num = 0.0;
				}
			}
			this.currentTime = Math.Min(this.BaseTime, num);
			if (this.progressBar != null)
			{
				this.progressBar.fillAmount = 0f;
			}
			return GameState.UpdateType.Skill;
		}
		this.currentTime = Math.Min(this.BaseTime, this.currentTime + num);
		return GameState.UpdateType.None;
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x0001FEE0 File Offset: 0x0001E0E0
	public void Reset()
	{
		this.isActive = false;
		if (this.progressBar != null)
		{
			this.progressBar.fillAmount = 0f;
		}
		if (this.remainingText != null)
		{
			this.remainingText.text = string.Empty;
		}
		this.currentTime = 0.0;
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x0001FF48 File Offset: 0x0001E148
	private static string GetTime(long time)
	{
		if (time < 0L)
		{
			return string.Empty;
		}
		if (time < 60L)
		{
			return time.ToString() + "s";
		}
		long num = time / 3600L;
		time -= num * 3600L;
		long num2 = time / 60L;
		time -= num2 * 60L;
		long num3 = time;
		if (num > 24L)
		{
			long num4 = (long)Math.Floor((double)num / 24.0);
			num -= num4 * 24L;
			return string.Format("{0}d {1}h {2}m", num4.ToString(), num.ToString(), num2.ToString());
		}
		if (num > 0L)
		{
			return string.Format("{0}h {1}m {2}s", num.ToString(), num2.ToString(), num3.ToString());
		}
		return string.Format("{0}m {1}s", num2.ToString(), num3.ToString());
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x00020028 File Offset: 0x0001E228
	public void UpdateText()
	{
		base.transform.Find("Hobby Title Text").GetComponent<Text>().text = Translations.GetHobbyTitle(this.Data.Id);
		base.transform.Find("Stat Payout Text").GetComponent<Text>().text = string.Format("+ {0}", Translations.TranslateSkill(this.Data.Id).ToLowerInvariant());
		base.transform.Find("Icon").GetComponent<Image>().sprite = ((this.Level.Value < Hobby2.MaxLevel) ? this.PauseIcon : this.Checkbox);
		base.transform.Find("Remaining Time Text").gameObject.SetActive(this.Level.Value < Hobby2.MaxLevel && !this.locked);
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x060003C3 RID: 963 RVA: 0x00020114 File Offset: 0x0001E314
	private HobbyAvatar HobbyAvatar
	{
		get
		{
			if (this.hobbyAvatar == null)
			{
				this.hobbyAvatar = new CachedObject<HobbyAvatar>(GameState.CurrentState.gameObject, "Hobbies");
			}
			return this.hobbyAvatar.Object;
		}
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x00020154 File Offset: 0x0001E354
	private void EnableHobby()
	{
		if (this.isActive)
		{
			return;
		}
		if (this.Level.Value >= Hobby2.MaxLevel)
		{
			base.transform.Find("Icon").GetComponent<Image>().sprite = this.Checkbox;
			base.transform.Find("Remaining Time Text").gameObject.SetActive(false);
			return;
		}
		FreeTime.HobbyTime += this.Data.TimeBlocks;
		this.isActive = true;
		GameState.RegisterUpdate(this);
		this.HobbyAvatar.ActiveHobby.Value = this;
		GameState.UpdatePanels(GameState.UpdateType.Skill);
		base.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
		base.transform.Find("Icon").GetComponent<Image>().sprite = this.PlayIcon;
		base.transform.Find("Icon").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
		this.iconFade = 1f;
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x0002027C File Offset: 0x0001E47C
	public void DisableHobby()
	{
		if (!this.isActive)
		{
			return;
		}
		FreeTime.HobbyTime -= this.Data.TimeBlocks;
		if (this.remainingText != null)
		{
			this.remainingText.text = string.Empty;
		}
		this.isActive = false;
		GameState.UnregisterUpdate(this);
		if (this.HobbyAvatar.ActiveHobby.Value == this)
		{
			this.HobbyAvatar.ActiveHobby.Value = null;
			for (int i = 0; i < base.transform.parent.childCount; i++)
			{
				if (base.transform.parent.GetChild(i).GetComponent<Hobby2>().isActive)
				{
					this.HobbyAvatar.ActiveHobby.Value = base.transform.parent.GetChild(i).GetComponent<Hobby2>();
					break;
				}
			}
		}
		GameState.UpdatePanels(GameState.UpdateType.Skill);
		base.GetComponent<Image>().color = new Color(0.7921569f, 0.2627451f, 0.9411765f, 1f);
		base.transform.Find("Icon").GetComponent<Image>().sprite = ((this.Level.Value < Hobby2.MaxLevel) ? this.PauseIcon : this.Checkbox);
		base.transform.Find("Remaining Time Text").gameObject.SetActive(this.Level.Value < Hobby2.MaxLevel && !this.locked);
		base.transform.Find("Icon").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
		base.transform.Find("Icon").GetComponent<Image>().rectTransform.localScale = new Vector3(1f, 1f, 1f);
		this.iconFade = 0f;
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x00020488 File Offset: 0x0001E688
	public void ToggleHobby()
	{
		this.HobbyAvatar.LastToggledHobby = this;
		if (this.isActive)
		{
			this.DisableHobby();
		}
		else
		{
			if (FreeTime.Free < this.Data.TimeBlocks)
			{
				this.reminderTime = 2f;
				Utilities.PurchaseTimeBlocks(this.Data.TimeBlocks - FreeTime.Free);
				return;
			}
			this.EnableHobby();
		}
		this.StoreState();
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x00020500 File Offset: 0x0001E700
	public void LoadState()
	{
		if (!Hobby2.ActiveHobbies.Contains(this))
		{
			Hobby2.ActiveHobbies.Add(this);
		}
		string english = this.Data.Resource.Singular.English;
		string str = string.Format("Hobby{0}", english);
		this.DisableHobby();
		if (global::PlayerPrefs.GetLong(str + "TimeL", 0L) == 0L)
		{
			this.currentTime = (double)global::PlayerPrefs.GetFloat(str + "Time", 0f);
		}
		else
		{
			this.currentTime = (double)global::PlayerPrefs.GetLong(str + "TimeL", 0L);
		}
		this.Gilded = (global::PlayerPrefs.GetInt(str + "MultiplierCount", 0) == 1);
		this.Level.Value = Skills.SkillLevel[(int)(this.Data.Id - 1)];
		if (this.Level.Value >= 75)
		{
			this.currentTime = 0.0;
		}
		else if (global::PlayerPrefs.GetInt(str + "Active", 0) == 1 && FreeTime.Free >= this.Data.TimeBlocks)
		{
			this.EnableHobby();
		}
		if (this.states == null)
		{
			this.states = this.CreateHobbyTime();
		}
		base.transform.Find("Progress Bar").GetComponent<Image>().fillAmount = (float)(this.currentTime / this.BaseTime);
		this.Level.RemoveAllListeners();
		this.Level += delegate(int level)
		{
			if (this.levelText != null)
			{
				this.levelText.text = level.ToString();
			}
			Skills.SkillLevel[(int)(this.Data.Id - 1)] = level;
			Skills.StoreState();
			Achievements.HandleHobbiesMinLevel();
		};
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x00020698 File Offset: 0x0001E898
	public void StoreState()
	{
		if (this.prefActive == null)
		{
			this.prefActive = new CachedPlayerPref(string.Format("Hobby{0}Active", this.Data.Resource.Singular.English));
		}
		if (this.prefMultiplierCount == null)
		{
			this.prefMultiplierCount = new CachedPlayerPref(string.Format("Hobby{0}MultiplierCount", this.Data.Resource.Singular.English));
		}
		if (this.prefTimeL == null)
		{
			this.prefTimeL = new CachedPlayerPref(string.Format("Hobby{0}TimeL", this.Data.Resource.Singular.English));
		}
		this.prefActive.SetInt((!this.isActive) ? 0 : 1);
		this.prefMultiplierCount.SetInt((!this.Gilded) ? 0 : 1);
		this.prefTimeL.SetLong((long)this.currentTime);
		GameState.CurrentState.QueueSave();
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x0002079C File Offset: 0x0001E99C
	public void SaveCurrentTime()
	{
		if (this.prefTimeL == null)
		{
			this.prefTimeL = new CachedPlayerPref(string.Format("Hobby{0}TimeL", this.Data.Resource.Singular.English));
		}
		this.prefTimeL.SetLong((long)this.currentTime);
	}

	// Token: 0x060003CA RID: 970 RVA: 0x000207F0 File Offset: 0x0001E9F0
	public bool Equals(Hobby2 other)
	{
		return this == other;
	}

	// Token: 0x040003E9 RID: 1001
	public static readonly int MaxLevel = 75;

	// Token: 0x040003EA RID: 1002
	public static List<Hobby2> ActiveHobbies = new List<Hobby2>();

	// Token: 0x040003EB RID: 1003
	public static HashSet<short> AvailableHobbies = new HashSet<short>();

	// Token: 0x040003EC RID: 1004
	private Image progressBar;

	// Token: 0x040003ED RID: 1005
	private Text remainingText;

	// Token: 0x040003EE RID: 1006
	private Text levelText;

	// Token: 0x040003EF RID: 1007
	public Sprite HobbySprite1;

	// Token: 0x040003F0 RID: 1008
	public Sprite HobbySprite2;

	// Token: 0x040003F1 RID: 1009
	public Sprite Checkbox;

	// Token: 0x040003F2 RID: 1010
	public Sprite PlayIcon;

	// Token: 0x040003F3 RID: 1011
	public Sprite PauseIcon;

	// Token: 0x040003F4 RID: 1012
	public Sprite UnlockedBackground;

	// Token: 0x040003F5 RID: 1013
	public Sprite UnlockedIcon;

	// Token: 0x040003F6 RID: 1014
	public Sprite UnlockedProgress;

	// Token: 0x040003F7 RID: 1015
	private double[] states;

	// Token: 0x040003F8 RID: 1016
	private float reminderTime;

	// Token: 0x040003F9 RID: 1017
	private float iconFade;

	// Token: 0x040003FA RID: 1018
	private double currentTime;

	// Token: 0x040003FB RID: 1019
	private bool isActive;

	// Token: 0x040003FC RID: 1020
	private bool locked = true;

	// Token: 0x040003FD RID: 1021
	private bool playedUnlockParticles;

	// Token: 0x040003FE RID: 1022
	public ReactiveProperty<int> Level = new ReactiveProperty<int>();

	// Token: 0x040003FF RID: 1023
	public bool Gilded;

	// Token: 0x04000400 RID: 1024
	private long lastRemaining = -1L;

	// Token: 0x04000401 RID: 1025
	private CachedObject<HobbyAvatar> hobbyAvatar;

	// Token: 0x04000402 RID: 1026
	private CachedPlayerPref prefActive;

	// Token: 0x04000403 RID: 1027
	private CachedPlayerPref prefMultiplierCount;

	// Token: 0x04000404 RID: 1028
	private CachedPlayerPref prefTimeL;
}
