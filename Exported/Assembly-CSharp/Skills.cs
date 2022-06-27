using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000126 RID: 294
public class Skills : MonoBehaviour
{
	// Token: 0x0600074A RID: 1866 RVA: 0x0003E09C File Offset: 0x0003C29C
	public static void StoreState()
	{
		if (Skills.skillCache == null)
		{
			Skills.skillCache = new string[12];
			for (int i = 0; i < Skills.skillCache.Length; i++)
			{
				Skills.skillCache[i] = string.Format("Skill{0}", i);
			}
		}
		for (int j = 0; j < 12; j++)
		{
			if (Skills.CachedSkillLevels[j] != Skills.SkillLevel[j])
			{
				global::PlayerPrefs.SetInt(Skills.skillCache[j], Skills.SkillLevel[j]);
				Skills.CachedSkillLevels[j] = Skills.SkillLevel[j];
			}
		}
		global::PlayerPrefs.SetInt("SkillGender", (!Skills.Gender) ? 0 : 1);
		global::PlayerPrefs.SetInt("SkillHat", Skills.hatSelection);
		global::PlayerPrefs.SetInt("SkillHair", Skills.hairSelection);
		global::PlayerPrefs.SetInt("SkillPlushy", ((!Skills._mioActive) ? 0 : 1) | ((!Skills._quillActive) ? 0 : 2));
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x0003E19C File Offset: 0x0003C39C
	private static void LoadState()
	{
		for (int i = 0; i < 12; i++)
		{
			int num = Mathf.Min(75, global::PlayerPrefs.GetInt("Skill" + i, 0));
			Skills.CachedSkillLevels[i] = num;
			Skills.SkillLevel[i] = num;
		}
		Skills.Gender = (global::PlayerPrefs.GetInt("SkillGender", 0) == 1);
		Skills.hatSelection = global::PlayerPrefs.GetInt("SkillHat", -1);
		Skills.hairSelection = global::PlayerPrefs.GetInt("SkillHair", -1);
		int @int = global::PlayerPrefs.GetInt("SkillPlushy", 0);
		Skills._mioActive = ((@int & 1) != 0);
		Skills._quillActive = ((@int & 2) != 0);
		Skills.UpdateAvatar();
	}

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x0600074C RID: 1868 RVA: 0x0003E24C File Offset: 0x0003C44C
	public Sprite[] SkillAvatars
	{
		get
		{
			return (!Skills.Gender) ? this.MaleSkillAvatars : this.FemaleSkillAvatars;
		}
	}

	// Token: 0x0600074D RID: 1869 RVA: 0x0003E26C File Offset: 0x0003C46C
	public void UpdateGender(bool gender)
	{
		Skills.Gender = gender;
		Skills.UpdateAvatar();
		if (Skills.Gender)
		{
			this.FemaleButton.interactable = false;
			this.MaleButton.interactable = true;
		}
		else
		{
			this.FemaleButton.interactable = true;
			this.MaleButton.interactable = false;
		}
		if (global::PlayerPrefs.GetInt("SkillGender", 0) == 1 != gender)
		{
			GameState.CurrentState.QueueSave();
		}
		GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.Select);
	}

	// Token: 0x0600074E RID: 1870 RVA: 0x0003E2F4 File Offset: 0x0003C4F4
	public static void UpdateAvatar()
	{
		if (Skills.skillInstance == null)
		{
			return;
		}
		if (GameState.TotalIncome == null)
		{
			return;
		}
		int num;
		if (GameState.TotalIncome.Value < 1000.0)
		{
			num = 0;
		}
		else if (GameState.TotalIncome.Value < 50000.0)
		{
			num = 1;
		}
		else if (GameState.TotalIncome.Value < 1000000.0)
		{
			num = 2;
		}
		else if (GameState.TotalIncome.Value < 50000000.0)
		{
			num = 3;
		}
		else if (GameState.TotalIncome.Value < 1000000000.0)
		{
			num = 4;
		}
		else
		{
			num = 5;
		}
		long sumOfSkills = Skills.SumOfSkills;
		int num2;
		if (sumOfSkills < 20L)
		{
			num2 = 0;
		}
		else if (sumOfSkills < 100L)
		{
			num2 = 1;
		}
		else if (sumOfSkills < 350L)
		{
			num2 = 2;
		}
		else if (sumOfSkills < 600L)
		{
			num2 = 3;
		}
		else if (sumOfSkills < 850L)
		{
			num2 = 4;
		}
		else
		{
			num2 = 5;
		}
		if (num > Skills.skillInstance.SkillBackgrounds.Length)
		{
			num = 0;
		}
		if (num2 > Skills.skillInstance.SkillAvatars.Length)
		{
			num2 = 0;
		}
		Skills.skillInstance.skillBackground.sprite = Skills.skillInstance.SkillBackgrounds[num];
		Skills.skillInstance.skillAvatar.sprite = Skills.skillInstance.SkillAvatars[Mathf.Max(1, num2)];
		if (Skills.hatSelection >= Skills.skillInstance.HatCustomization.Length)
		{
			Skills.hatSelection = 0;
		}
		Skills.skillInstance.skillHat.gameObject.SetActive(Skills.hatSelection >= 0);
		if (Skills.hatSelection >= 0)
		{
			Skills.skillInstance.skillHat.sprite = Skills.skillInstance.HatCustomization[Skills.hatSelection];
		}
		if (Skills.hairSelection >= Skills.skillInstance.HairCustomization.Length)
		{
			Skills.hairSelection = 0;
		}
		Skills.skillInstance.skillHair.gameObject.SetActive(Skills.hairSelection >= 0);
		if (Skills.hairSelection >= 0)
		{
			Skills.skillInstance.skillHair.sprite = Skills.skillInstance.HairCustomization[Skills.hairSelection];
		}
	}

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x0600074F RID: 1871 RVA: 0x0003E550 File Offset: 0x0003C750
	public static int HighestLevel
	{
		get
		{
			if (Skills.SkillLevel == null)
			{
				return 0;
			}
			int num = 0;
			foreach (int num2 in Skills.SkillLevel)
			{
				if (num2 > num)
				{
					num = num2;
				}
			}
			return num;
		}
	}

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x06000750 RID: 1872 RVA: 0x0003E594 File Offset: 0x0003C794
	public static long SumOfSkills
	{
		get
		{
			if (Skills.SkillLevel == null)
			{
				return 0L;
			}
			long num = 0L;
			foreach (int num2 in Skills.SkillLevel)
			{
				num += (long)num2;
			}
			return num;
		}
	}

	// Token: 0x06000751 RID: 1873 RVA: 0x0003E5D8 File Offset: 0x0003C7D8
	public static void Reset()
	{
		if (Skills.SkillLevel == null)
		{
			Skills.SkillLevel = new int[12];
			Skills.CachedSkillLevels = new int[12];
		}
		for (int i = 0; i < 12; i++)
		{
			Skills.SkillLevel[i] = 0;
			Skills.CachedSkillLevels[i] = 0;
		}
		Skills.UpdateAvatar();
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x0003E630 File Offset: 0x0003C830
	public static void Init()
	{
		if (Skills.SkillLevel == null)
		{
			Skills.SkillLevel = new int[12];
			Skills.CachedSkillLevels = new int[12];
		}
		Skills.LoadState();
	}

	// Token: 0x06000753 RID: 1875 RVA: 0x0003E65C File Offset: 0x0003C85C
	private void Start()
	{
		if (this.SkillIcons.Length == 0)
		{
			return;
		}
		Skills.skillInstance = this;
		this.UpdateGender(Skills.Gender);
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x0003E680 File Offset: 0x0003C880
	private void Update()
	{
		bool flag = false;
		int num = 0;
		while (Skills.CachedSkillLevels != null && Skills.SkillLevel != null && num < Skills.SkillLevel.Length)
		{
			if (Skills.CachedSkillLevels[num] != Skills.SkillLevel[num])
			{
				flag = true;
			}
			num++;
		}
		if (flag)
		{
			Skills.StoreState();
			this.UpdateGender(Skills.Gender);
		}
	}

	// Token: 0x06000755 RID: 1877 RVA: 0x0003E6E8 File Offset: 0x0003C8E8
	public void UpdateStats()
	{
		if (this.summaryText == null)
		{
			this.summaryText = base.transform.Find("Summary").GetComponent<Text>();
			this.currentBonusText = base.transform.Find("Reset/Bonus Multiplier").GetComponent<Text>();
			this.nextBonusText = base.transform.Find("Reset/Reset Text").GetComponent<Text>();
		}
		if (!this.summaryText.gameObject.activeInHierarchy)
		{
			return;
		}
		string arg = (GameState.GiftCount != int.MaxValue) ? Utilities.ToPrefixedNumber((double)GameState.GiftCount, true, false) : "Way too many!";
		string text = string.Format(string.Concat(new string[]
		{
			Translations.statsTransTime,
			" {0}\n",
			Translations.statsTransMoney,
			" {1}\n",
			Translations.statsTransGirls,
			" {2}\n"
		}), (FreeTime.TimeSlots + FreeTime.PurchasedTime).ToString(), Utilities.ToPrefixedNumber(GameState.TotalIncome.Value, true, false), GameState.GetGirlScreen().TotalUnlockedGirlCount.ToString());
		text += string.Format(Translations.statsTransDates + " {0}\n" + Translations.statsTransGifts + " {1}\n", Utilities.ToPrefixedNumber((double)GameState.DateCount, true, false), arg);
		text += string.Format(Translations.statsTransHearts + " {0}", Utilities.ToPrefixedNumber((double)GameState.HeartCount.Value, true, false));
		this.summaryText.text = text;
		this.UpdateSpeedBoost();
		base.transform.Find("Speed Boost/Text").GetComponent<Text>().text = "x" + GameState.PurchasedMultiplier.ToString();
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x0003E8B0 File Offset: 0x0003CAB0
	private void UpdateSpeedBoost()
	{
		this.currentBonusText.text = ((GameState.CurrentState.TimeMultiplier.Value <= 1000f) ? string.Format(Translations.statsTransResetBoost + " x{0}", GameState.CurrentState.TimeMultiplier.Value.ToString("0.00")) : string.Format(Translations.statsTransResetBoost + " x{0}", GameState.CurrentState.TimeMultiplier.Value.ToString("0.")));
		float num = Mathf.Max(0f, GameState.CurrentState.PendingPrestige.Value);
		if (GameState.CurrentState.TimeMultiplier.Value + GameState.CurrentState.PendingPrestige.Value > 2048f)
		{
			num = 2048f - GameState.CurrentState.TimeMultiplier.Value;
		}
		this.nextBonusText.text = string.Format(Translations.statsTransBonusAdd + " +{0}", num.ToString("0.00"));
		base.transform.Find("Speed Boost/Text").GetComponent<Text>().text = "x" + GameState.PurchasedMultiplier.ToString();
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x0003E9FC File Offset: 0x0003CBFC
	private void OnEnable()
	{
		this.UpdatePlushies();
		this.InitCustomizationPage(this.page);
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x0003EA10 File Offset: 0x0003CC10
	private void UpdatePlushies()
	{
		base.transform.Find("Skills Viewer/Mio").gameObject.SetActive((Playfab.AwardedItems & Playfab.PlayfabItems.MioPlush) != (Playfab.PlayfabItems)0L && Skills._mioActive);
		base.transform.Find("Skills Viewer/Quill").gameObject.SetActive((Playfab.AwardedItems & Playfab.PlayfabItems.QuillPlush) != (Playfab.PlayfabItems)0L && Skills._quillActive);
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x0003EA8C File Offset: 0x0003CC8C
	public void InitCustomizationPage(int page)
	{
		this.page = page;
		int plushyCount = 0;
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.MioPlush) != (Playfab.PlayfabItems)0L)
		{
			plushyCount++;
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.QuillPlush) != (Playfab.PlayfabItems)0L)
		{
			plushyCount++;
		}
		for (int i = 0; i < 4; i++)
		{
			Transform transform = base.transform.Find("Customization/AvatarItem " + (i + 1));
			int p4i = page * 4 + i;
			transform.GetComponent<Button>().onClick.RemoveAllListeners();
			if (p4i >= this.HairCustomization.Length + this.HatCustomization.Length + plushyCount)
			{
				transform.gameObject.SetActive(false);
			}
			else
			{
				int item = (p4i >= plushyCount) ? ((p4i >= this.HairCustomization.Length + plushyCount) ? (p4i - this.HairCustomization.Length - plushyCount) : (p4i - plushyCount)) : p4i;
				bool select = p4i >= plushyCount && ((p4i >= this.HairCustomization.Length + plushyCount) ? (Skills.hatSelection == item) : (Skills.hairSelection == item));
				if (p4i == 0)
				{
					if ((Playfab.AwardedItems & Playfab.PlayfabItems.MioPlush) != (Playfab.PlayfabItems)0L || plushyCount == 2)
					{
						select = Skills._mioActive;
					}
					else
					{
						select = Skills._quillActive;
					}
				}
				else if (p4i == 1 && plushyCount == 2)
				{
					select = Skills._quillActive;
				}
				transform.gameObject.SetActive(true);
				if (p4i < plushyCount)
				{
					if (plushyCount == 1)
					{
						if ((Playfab.AwardedItems & Playfab.PlayfabItems.MioPlush) != (Playfab.PlayfabItems)0L)
						{
							transform.Find("Item").GetComponent<Image>().sprite = this.MioPlushy;
						}
						else
						{
							transform.Find("Item").GetComponent<Image>().sprite = this.QuillPlushy;
						}
					}
					else
					{
						transform.Find("Item").GetComponent<Image>().sprite = ((p4i != 0) ? this.QuillPlushy : this.MioPlushy);
					}
				}
				else
				{
					transform.Find("Item").GetComponent<Image>().sprite = ((p4i >= this.HairCustomization.Length + plushyCount) ? this.HatCustomization[item] : this.HairCustomization[item]);
				}
				transform.GetComponent<Image>().color = ((!select) ? new Color(1f, 1f, 1f) : new Color(0.22352941f, 0.79607844f, 0.92941177f));
				transform.GetComponent<Button>().onClick.AddListener(delegate()
				{
					if (p4i < plushyCount)
					{
						if (plushyCount == 1)
						{
							if ((Playfab.AwardedItems & Playfab.PlayfabItems.MioPlush) != (Playfab.PlayfabItems)0L)
							{
								Skills._mioActive = !Skills._mioActive;
							}
							else
							{
								Skills._quillActive = !Skills._quillActive;
							}
						}
						else if (p4i == 0)
						{
							Skills._mioActive = !Skills._mioActive;
						}
						else
						{
							Skills._quillActive = !Skills._quillActive;
						}
						global::PlayerPrefs.SetInt("SkillPlushy", ((!Skills._mioActive) ? 0 : 1) | ((!Skills._quillActive) ? 0 : 2));
						this.UpdatePlushies();
					}
					else if (p4i < this.HairCustomization.Length + plushyCount)
					{
						Skills.hairSelection = ((!select) ? item : -1);
					}
					else
					{
						Skills.hatSelection = ((!select) ? item : -1);
					}
					Skills.UpdateAvatar();
					this.InitCustomizationPage(page);
					GameState.CurrentState.QueueSave();
				});
			}
		}
		base.transform.Find("Customization/Left Button").GetComponent<Button>().interactable = (page != 0);
		base.transform.Find("Customization/Right Button").GetComponent<Button>().interactable = ((page + 1) * 4 < this.HairCustomization.Length + this.HatCustomization.Length + plushyCount);
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x0003EEB4 File Offset: 0x0003D0B4
	public void NextCustomizationPage()
	{
		int num = 0;
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.MioPlush) != (Playfab.PlayfabItems)0L)
		{
			num++;
		}
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.QuillPlush) != (Playfab.PlayfabItems)0L)
		{
			num++;
		}
		if ((this.page + 1) * 4 >= this.HairCustomization.Length + this.HatCustomization.Length + num)
		{
			return;
		}
		this.InitCustomizationPage(this.page + 1);
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x0003EF24 File Offset: 0x0003D124
	public void PrevCustomizationPage()
	{
		if (this.page == 0)
		{
			return;
		}
		this.InitCustomizationPage(this.page - 1);
	}

	// Token: 0x04000790 RID: 1936
	private static string[] skillCache;

	// Token: 0x04000791 RID: 1937
	public Button MaleButton;

	// Token: 0x04000792 RID: 1938
	public Button FemaleButton;

	// Token: 0x04000793 RID: 1939
	public Sprite[] SkillBackgrounds;

	// Token: 0x04000794 RID: 1940
	public Sprite[] MaleSkillAvatars;

	// Token: 0x04000795 RID: 1941
	public Sprite[] FemaleSkillAvatars;

	// Token: 0x04000796 RID: 1942
	public static bool Gender;

	// Token: 0x04000797 RID: 1943
	private static int hatSelection = -1;

	// Token: 0x04000798 RID: 1944
	private static int hairSelection = -1;

	// Token: 0x04000799 RID: 1945
	public Image skillAvatar;

	// Token: 0x0400079A RID: 1946
	public Image skillBackground;

	// Token: 0x0400079B RID: 1947
	public Image skillHat;

	// Token: 0x0400079C RID: 1948
	public Image skillHair;

	// Token: 0x0400079D RID: 1949
	public Sprite MioPlushy;

	// Token: 0x0400079E RID: 1950
	public Sprite QuillPlushy;

	// Token: 0x0400079F RID: 1951
	public Sprite[] SkillIcons;

	// Token: 0x040007A0 RID: 1952
	public static int[] SkillLevel;

	// Token: 0x040007A1 RID: 1953
	private static int[] CachedSkillLevels;

	// Token: 0x040007A2 RID: 1954
	private static Skills skillInstance;

	// Token: 0x040007A3 RID: 1955
	private Text summaryText;

	// Token: 0x040007A4 RID: 1956
	private Text currentBonusText;

	// Token: 0x040007A5 RID: 1957
	private Text nextBonusText;

	// Token: 0x040007A6 RID: 1958
	public Sprite[] HairCustomization;

	// Token: 0x040007A7 RID: 1959
	public Sprite[] HatCustomization;

	// Token: 0x040007A8 RID: 1960
	public GameObject CustomizationPrefab;

	// Token: 0x040007A9 RID: 1961
	private static bool _mioActive;

	// Token: 0x040007AA RID: 1962
	private static bool _quillActive;

	// Token: 0x040007AB RID: 1963
	private int page;
}
