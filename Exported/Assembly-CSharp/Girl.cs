using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AssetBundles;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200009B RID: 155
public class Girl : MonoBehaviour, IUpdateable
{
	// Token: 0x1700003A RID: 58
	// (get) Token: 0x0600031D RID: 797 RVA: 0x00017414 File Offset: 0x00015614
	// (set) Token: 0x0600031E RID: 798 RVA: 0x00017428 File Offset: 0x00015628
	public GirlModel Data
	{
		get
		{
			this._data.CheckIfLoaded();
			return this._data;
		}
		set
		{
			this._data = value;
		}
	}

	// Token: 0x0600031F RID: 799 RVA: 0x00017434 File Offset: 0x00015634
	public static Girl FindGirl(Balance.GirlName girl)
	{
		if (Girl.ActiveGirls == null)
		{
			return null;
		}
		if (Girl.ActiveGirls.Count <= (int)girl)
		{
			return null;
		}
		return Girl.ActiveGirls[(int)girl];
	}

	// Token: 0x06000320 RID: 800 RVA: 0x00017460 File Offset: 0x00015660
	public static void Reset(bool keepClothing = false)
	{
		if (Girl.ActiveGirls != null)
		{
			if (keepClothing)
			{
				foreach (Girl girl in Girl.ActiveGirls)
				{
					if (!(girl == null))
					{
						global::PlayerPrefs.SetInt(string.Format("Girl{0}LifeGifts", girl.GirlName.ToFriendlyString()), (int)girl.LifetimeGifts);
						global::PlayerPrefs.SetInt(string.Format("Girl{0}LifeOutfits", girl.GirlName.ToFriendlyString()), (int)girl.LifetimeOutfits);
						girl.clothing = Requirement.OutfitType.None;
						girl.love = Girl.LoveLevel.Adversary;
					}
				}
			}
			Girl.ActiveGirls.Clear();
		}
	}

	// Token: 0x06000321 RID: 801 RVA: 0x00017538 File Offset: 0x00015738
	public static void OnBlayFapLoaded(bool loaded)
	{
		if (!loaded)
		{
			return;
		}
		BlayFapIntegration.BlayFapLoaded -= new ReactiveProperty<bool>.Changed(Girl.OnBlayFapLoaded);
		Girl.InitDeeplinking();
	}

	// Token: 0x06000322 RID: 802 RVA: 0x00017564 File Offset: 0x00015764
	private static bool IsPhoneGirlAndInStore(Balance.GirlName girl)
	{
		return Girl.PhoneGirlStoreList.Contains(girl) && Playfab.Promotion.Contains(girl.ToLowerFriendlyString() + ".");
	}

	// Token: 0x06000323 RID: 803 RVA: 0x00017594 File Offset: 0x00015794
	private static bool IsValueBundleGirlAndInStore(Balance.GirlName girl)
	{
		return Girl.ValueBundleGirls.Contains(girl) && Playfab.Promotion.Contains(girl.ToLowerFriendlyString() + ".");
	}

	// Token: 0x06000324 RID: 804 RVA: 0x000175C4 File Offset: 0x000157C4
	private static bool IsMonsterGirlAndInStore(Balance.GirlName girl)
	{
		return ((girl == Balance.GirlName.Jelle || girl == Balance.GirlName.Quillzone) && Playfab.Promotion.Contains("monsters1")) || ((girl == Balance.GirlName.Bonchovy || girl == Balance.GirlName.Spectrum) && Playfab.Promotion.Contains("monsters2"));
	}

	// Token: 0x06000325 RID: 805 RVA: 0x0001761C File Offset: 0x0001581C
	private static bool IsLteGirlAndInStore(Balance.GirlName girl)
	{
		return Girl.IsLteGirl(girl) && Playfab.Promotion.Contains(girl.ToLowerFriendlyString() + ".");
	}

	// Token: 0x06000326 RID: 806 RVA: 0x00017654 File Offset: 0x00015854
	public static bool WaitingOnFlingCompletion(Balance.GirlName girl)
	{
		if (girl == Balance.GirlName.Wendy)
		{
			if ((Playfab.AwardedItems & Playfab.PlayfabItems.Wendy) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(113))
			{
				return true;
			}
		}
		else if (girl == Balance.GirlName.Ruri)
		{
			if ((Playfab.AwardedItems & Playfab.PlayfabItems.Ruri) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(116))
			{
				return true;
			}
		}
		else if (girl == Balance.GirlName.Generica)
		{
			if ((Playfab.AwardedItems & Playfab.PlayfabItems.Generica) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(119))
			{
				return true;
			}
		}
		else if (girl == Balance.GirlName.Sawyer)
		{
			if ((Playfab.AwardedItems & Playfab.PlayfabItems.Sawyer) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(140))
			{
				return true;
			}
		}
		else if (girl == Balance.GirlName.Renee && ((Playfab.AwardedItems & Playfab.PlayfabItems.Renee) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(161)))
		{
			return true;
		}
		return false;
	}

	// Token: 0x06000327 RID: 807 RVA: 0x00017748 File Offset: 0x00015948
	public static void InitDeeplinking()
	{
		if (GameState.CurrentState == null || (IntroScreen.TutorialState < IntroScreen.State.AllActive && GameState.CurrentState.TimeMultiplier.Value == 1f))
		{
			return;
		}
		if (string.IsNullOrEmpty(Playfab.Promotion))
		{
			return;
		}
		Transform transform = GameState.GetGirlScreen().transform.Find("Girl List/Scroll View/Content Panel");
		Girls girlScreen = GameState.GetGirlScreen();
		PanelScript panelScript = GameState.CurrentState.GetComponent<PanelScript>();
		for (int i = 0; i < transform.childCount; i++)
		{
			Girl girl = transform.GetChild(i).GetComponent<Girl>();
			if (!girlScreen.IsUnlocked(girl.GirlName) && (Girl.IsValueBundleGirlAndInStore(girl.GirlName) || Girl.IsPhoneGirlAndInStore(girl.GirlName) || Girl.IsMonsterGirlAndInStore(girl.GirlName) || Girl.IsLteGirlAndInStore(girl.GirlName)))
			{
				if (Girl.IsPhoneGirlAndInStore(girl.GirlName) && Girl.WaitingOnFlingCompletion(girl.GirlName))
				{
					girl.transform.Find("Requirement").gameObject.SetActive(true);
					girl.transform.Find("Store Requirement").gameObject.SetActive(false);
					girl.transform.Find("Store Button").gameObject.SetActive(false);
				}
				else
				{
					Button component = girl.transform.Find("Store Button").GetComponent<Button>();
					component.gameObject.SetActive(true);
					Text component2 = girl.transform.Find("Store Requirement").GetComponent<Text>();
					component2.text = girl.transform.Find("Requirement").GetComponent<Text>().text.Replace("+ LTE", "+ store");
					component2.gameObject.SetActive(true);
					girl.transform.Find("Requirement").gameObject.SetActive(false);
					TaskManager taskSystem = GameState.GetTaskSystem();
					bool flag = Girl.PhoneGirlStoreList.Contains(girl.GirlName);
					component.onClick.AddListener(delegate()
					{
						panelScript.ClickBundles();
						GameState.CurrentState.transform.Find("Store Revamp").GetComponent<Store2>().ScrollToGirl(girl.GirlName.ToFriendlyString(), Girl.IsMonsterGirl(girl.GirlName), false, string.Empty);
					});
					if (flag)
					{
						component2.resizeTextMaxSize = 14;
					}
				}
			}
			else if (!girlScreen.IsUnlocked(girl.GirlName))
			{
				girl.transform.Find("Requirement").gameObject.SetActive(true);
				girl.transform.Find("Store Requirement").gameObject.SetActive(false);
				girl.transform.Find("Store Button").gameObject.SetActive(false);
			}
			else
			{
				girl.transform.Find("Requirement").gameObject.SetActive(false);
				girl.transform.Find("Store Requirement").gameObject.SetActive(false);
				girl.transform.Find("Store Button").gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000328 RID: 808 RVA: 0x00017AE4 File Offset: 0x00015CE4
	public static void Init()
	{
		Girl.ActiveGirls.Clear();
		Girl.LastGirl = Balance.GirlName.Unknown;
		Transform transform = GameState.GetGirlScreen().transform.Find("Girl List/Scroll View/Content Panel");
		for (int i = 0; i < transform.childCount; i++)
		{
			Girl component = transform.GetChild(i).GetComponent<Girl>();
			if (component.GirlName != Balance.GirlName.Unknown)
			{
				component.LoadState();
				while (Girl.ActiveGirls.Count <= (int)component.GirlName)
				{
					Girl.ActiveGirls.Add(null);
				}
				Girl.ActiveGirls[(int)component.GirlName] = component;
				GameState.RegisterUpdate(component);
				Achievements.TriggerLoveAchievement(component.GirlName);
				if ((component.Clothing == Requirement.OutfitType.Nude || component.Clothing == Requirement.OutfitType.Lingerie) && !GameState.NSFW)
				{
					component.RequestClothing(Requirement.OutfitType.None, null);
				}
			}
		}
		BlayFapIntegration.BlayFapLoaded += new ReactiveProperty<bool>.Changed(Girl.OnBlayFapLoaded);
		Translations.UpdateGirlNames();
	}

	// Token: 0x06000329 RID: 809 RVA: 0x00017BE8 File Offset: 0x00015DE8
	public void SetLTECallback()
	{
	}

	// Token: 0x0600032A RID: 810 RVA: 0x00017BEC File Offset: 0x00015DEC
	public static int GetLove(Balance.GirlName name)
	{
		Girl girl = Girl.FindGirl(name);
		if (girl != null)
		{
			return girl.Love;
		}
		return 0;
	}

	// Token: 0x0600032B RID: 811 RVA: 0x00017C14 File Offset: 0x00015E14
	private void LoadState()
	{
		string str = string.Format("Girl{0}", this.GirlName.ToFriendlyString());
		this.DisplayFollowUp = false;
		this.love = (Girl.LoveLevel)global::PlayerPrefs.GetInt(str + "Love", 0);
		this.hearts = global::PlayerPrefs.GetLong(str + "Hearts", 0L);
		this.clothing = (Requirement.OutfitType)global::PlayerPrefs.GetInt(str + "Clothing", 0);
		if (this.clothing == Requirement.OutfitType.Monster)
		{
			this.clothing = Requirement.OutfitType.None;
		}
		if (this.GirlName == Balance.GirlName.Suzu && this.clothing == Requirement.OutfitType.Unique)
		{
			this.clothing = Requirement.OutfitType.Animated;
		}
		if (this.GirlName == Balance.GirlName.Explora && this.clothing == Requirement.OutfitType.Animated)
		{
			this.clothing = Requirement.OutfitType.Animated;
		}
		if (this.GirlName == Balance.GirlName.Mallory && this.clothing == Requirement.OutfitType.Animated)
		{
			this.clothing = Requirement.OutfitType.Animated;
		}
		if (this.Love != 9)
		{
			this.clothing = Requirement.OutfitType.None;
		}
		bool flag = false;
		if (global::PlayerPrefs.HasKey(str + "GiftCount"))
		{
			int @int = global::PlayerPrefs.GetInt(str + "GiftCount", 0);
			this.GiftCount = new int[]
			{
				@int,
				@int,
				@int,
				@int
			};
			global::PlayerPrefs.DeleteKey(str + "GiftCount", false);
			flag = true;
		}
		else
		{
			this.GiftCount = new int[]
			{
				global::PlayerPrefs.GetInt(str + "GiftCount1", 0),
				global::PlayerPrefs.GetInt(str + "GiftCount2", 0),
				global::PlayerPrefs.GetInt(str + "GiftCount3", 0),
				global::PlayerPrefs.GetInt(str + "GiftCount4", 0)
			};
		}
		if (global::PlayerPrefs.HasKey(str + "DateCount"))
		{
			int int2 = global::PlayerPrefs.GetInt(str + "DateCount", 0);
			this.DateCount = new int[]
			{
				int2,
				int2,
				int2,
				int2
			};
			global::PlayerPrefs.DeleteKey(str + "DateCount", false);
			flag = true;
		}
		else
		{
			this.DateCount = new int[]
			{
				global::PlayerPrefs.GetInt(str + "DateCount1", 0),
				global::PlayerPrefs.GetInt(str + "DateCount2", 0),
				global::PlayerPrefs.GetInt(str + "DateCount3", 0),
				global::PlayerPrefs.GetInt(str + "DateCount4", 0)
			};
		}
		this.ReceivedDates = (Requirement.DateType)global::PlayerPrefs.GetInt(str + "Dates", 0);
		this.LifetimeDates = (Requirement.DateType)global::PlayerPrefs.GetInt(str + "LifeDates", 0);
		this.LifetimeGifts = (Requirement.GiftType)global::PlayerPrefs.GetInt(str + "LifeGifts", 0);
		this.LifetimeOutfits = (Requirement.OutfitType)global::PlayerPrefs.GetInt(str + "LifeOutfits", 0);
		if (this.GirlName == Balance.GirlName.Darya || this.GirlName == Balance.GirlName.Charlotte || this.GirlName == Balance.GirlName.Catara || this.GirlName == Balance.GirlName.Suzu || this.GirlName == Balance.GirlName.Mallory)
		{
			this.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
		}
		if (this.GirlName == Balance.GirlName.Suzu || this.GirlName == Balance.GirlName.Mallory)
		{
			this.LifetimeOutfits |= Requirement.OutfitType.Animated;
		}
		if (this.GirlName == Balance.GirlName.Mallory)
		{
			this.LifetimeOutfits |= Requirement.OutfitType.Unique;
		}
		if (GameState.NSFW && this.GirlName == Balance.GirlName.Odango)
		{
			this.LifetimeOutfits |= (Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
		}
		if (GameState.NSFW && this.GirlName == Balance.GirlName.Shibuki)
		{
			this.LifetimeOutfits |= (Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
		}
		if (GameState.NSFW && this.GirlName == Balance.GirlName.Sirina)
		{
			this.LifetimeOutfits |= (Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
		}
		if (GameState.NSFW && this.GirlName == Balance.GirlName.Vellatrix)
		{
			this.LifetimeOutfits |= (Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
		}
		if (GameState.NSFW && this.GirlName == Balance.GirlName.Roxxy)
		{
			this.LifetimeOutfits |= (Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
		}
		if (GameState.NSFW && this.GirlName == Balance.GirlName.Tessa)
		{
			this.LifetimeOutfits |= (Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
		}
		if (this.GirlName == Balance.GirlName.Roxxy && (Playfab.AwardedItems & Playfab.PlayfabItems.Roxxy) == Playfab.PlayfabItems.Roxxy)
		{
			this.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
		}
		if (this.GirlName == Balance.GirlName.Tessa && (Playfab.AwardedItems & Playfab.PlayfabItems.Tessa) == Playfab.PlayfabItems.Tessa)
		{
			this.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
		}
		if ((this.LifetimeDates & Requirement.DateType.Beach) != (Requirement.DateType)0)
		{
			Album.Add(Requirement.DateType.Beach, this);
		}
		if ((this.LifetimeDates & Requirement.DateType.MoonlightStroll) != (Requirement.DateType)0)
		{
			Album.Add(Requirement.DateType.MoonlightStroll, this);
		}
		if ((this.LifetimeDates & Requirement.DateType.MovieTheater) != (Requirement.DateType)0)
		{
			Album.Add(Requirement.DateType.MovieTheater, this);
		}
		if ((this.LifetimeDates & Requirement.DateType.Sightseeing) != (Requirement.DateType)0)
		{
			Album.Add(Requirement.DateType.Sightseeing, this);
		}
		this.LifetimeOutfits |= (Requirement.OutfitType)(this.LifetimeGifts & (Requirement.GiftType.Christmas | Requirement.GiftType.SchoolUniform | Requirement.GiftType.BathingSuit | Requirement.GiftType.Unique | Requirement.GiftType.DiamondRing | Requirement.GiftType.Lingerie | Requirement.GiftType.Nude));
		if (this.love > Girl.LoveLevel.Adversary || this.hearts > 0L)
		{
			Album.Add(this.Data, 0);
		}
		if (this.love >= Girl.LoveLevel.Friendzoned)
		{
			Album.Add(this.Data, 1);
		}
		if (this.love >= Girl.LoveLevel.Sweetheart)
		{
			Album.Add(this.Data, 2);
		}
		if (this.love >= Girl.LoveLevel.Lover)
		{
			Album.Add(this.Data, 3);
		}
		this.SetIcon();
		if (flag)
		{
			this.StoreState();
			if (GameState.CurrentState != null)
			{
				GameState.CurrentState.QueueSave();
			}
		}
	}

	// Token: 0x0600032C RID: 812 RVA: 0x000181DC File Offset: 0x000163DC
	private void StoreStateFast()
	{
		if (string.IsNullOrEmpty(this.girlHeartString))
		{
			this.girlHeartString = string.Format("Girl{0}Hearts", this.GirlName.ToFriendlyString());
		}
		global::PlayerPrefs.SetLong(this.girlHeartString, this.hearts);
	}

	// Token: 0x0600032D RID: 813 RVA: 0x00018228 File Offset: 0x00016428
	public void StoreState()
	{
		string str = string.Format("Girl{0}", this.GirlName.ToFriendlyString());
		global::PlayerPrefs.SetInt(str + "Love", this.Love);
		global::PlayerPrefs.SetLong(str + "Hearts", this.Hearts);
		if (this.GiftCount != null && this.GiftCount.Length == 4)
		{
			global::PlayerPrefs.SetInt(str + "GiftCount1", this.GiftCount[0]);
			global::PlayerPrefs.SetInt(str + "GiftCount2", this.GiftCount[1]);
			global::PlayerPrefs.SetInt(str + "GiftCount3", this.GiftCount[2]);
			global::PlayerPrefs.SetInt(str + "GiftCount4", this.GiftCount[3]);
		}
		if (this.DateCount != null && this.DateCount.Length == 4)
		{
			global::PlayerPrefs.SetInt(str + "DateCount1", this.DateCount[0]);
			global::PlayerPrefs.SetInt(str + "DateCount2", this.DateCount[1]);
			global::PlayerPrefs.SetInt(str + "DateCount3", this.DateCount[2]);
			global::PlayerPrefs.SetInt(str + "DateCount4", this.DateCount[3]);
		}
		global::PlayerPrefs.SetInt(str + "Dates", (int)this.ReceivedDates);
		global::PlayerPrefs.SetInt(str + "Clothing", (int)this.Clothing);
		global::PlayerPrefs.SetInt(str + "LifeDates", (int)this.LifetimeDates);
		global::PlayerPrefs.SetInt(str + "LifeGifts", (int)this.LifetimeGifts);
		global::PlayerPrefs.SetInt(str + "LifeOutfits", (int)this.LifetimeOutfits);
	}

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x0600032E RID: 814 RVA: 0x000183D8 File Offset: 0x000165D8
	// (set) Token: 0x0600032F RID: 815 RVA: 0x000183E0 File Offset: 0x000165E0
	public Sprite CurrentPose { get; internal set; }

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x06000330 RID: 816 RVA: 0x000183EC File Offset: 0x000165EC
	// (set) Token: 0x06000331 RID: 817 RVA: 0x000183F4 File Offset: 0x000165F4
	public Requirement.DateType ReceivedDates { get; set; }

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x06000332 RID: 818 RVA: 0x00018400 File Offset: 0x00016600
	// (set) Token: 0x06000333 RID: 819 RVA: 0x00018408 File Offset: 0x00016608
	public Requirement.DateType LifetimeDates { get; set; }

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x06000334 RID: 820 RVA: 0x00018414 File Offset: 0x00016614
	// (set) Token: 0x06000335 RID: 821 RVA: 0x0001841C File Offset: 0x0001661C
	public Requirement.GiftType LifetimeGifts { get; set; }

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x06000336 RID: 822 RVA: 0x00018428 File Offset: 0x00016628
	// (set) Token: 0x06000337 RID: 823 RVA: 0x00018430 File Offset: 0x00016630
	public Requirement.OutfitType LifetimeOutfits { get; set; }

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x06000338 RID: 824 RVA: 0x0001843C File Offset: 0x0001663C
	// (set) Token: 0x06000339 RID: 825 RVA: 0x00018444 File Offset: 0x00016644
	public int[] GiftCount { get; set; }

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x0600033A RID: 826 RVA: 0x00018450 File Offset: 0x00016650
	// (set) Token: 0x0600033B RID: 827 RVA: 0x00018458 File Offset: 0x00016658
	public int[] DateCount { get; set; }

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x0600033C RID: 828 RVA: 0x00018464 File Offset: 0x00016664
	// (set) Token: 0x0600033D RID: 829 RVA: 0x0001846C File Offset: 0x0001666C
	public float TalkCooldown { get; set; }

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x0600033E RID: 830 RVA: 0x00018478 File Offset: 0x00016678
	// (set) Token: 0x0600033F RID: 831 RVA: 0x00018480 File Offset: 0x00016680
	public float TalkCooldownLength { get; set; }

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x06000340 RID: 832 RVA: 0x0001848C File Offset: 0x0001668C
	// (set) Token: 0x06000341 RID: 833 RVA: 0x00018494 File Offset: 0x00016694
	public float PokeCooldown { get; set; }

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x06000342 RID: 834 RVA: 0x000184A0 File Offset: 0x000166A0
	// (set) Token: 0x06000343 RID: 835 RVA: 0x000184A8 File Offset: 0x000166A8
	public float PokeCooldownLength { get; set; }

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x06000344 RID: 836 RVA: 0x000184B4 File Offset: 0x000166B4
	// (set) Token: 0x06000345 RID: 837 RVA: 0x000184BC File Offset: 0x000166BC
	public float GiftCooldown { get; set; }

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x06000346 RID: 838 RVA: 0x000184C8 File Offset: 0x000166C8
	// (set) Token: 0x06000347 RID: 839 RVA: 0x000184D0 File Offset: 0x000166D0
	public float GiftCooldownLength { get; set; }

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x06000348 RID: 840 RVA: 0x000184DC File Offset: 0x000166DC
	// (set) Token: 0x06000349 RID: 841 RVA: 0x000184E4 File Offset: 0x000166E4
	public float DateCooldown { get; set; }

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x0600034A RID: 842 RVA: 0x000184F0 File Offset: 0x000166F0
	// (set) Token: 0x0600034B RID: 843 RVA: 0x000184F8 File Offset: 0x000166F8
	public float DateCooldownLength { get; set; }

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x0600034C RID: 844 RVA: 0x00018504 File Offset: 0x00016704
	public Requirement.OutfitType Clothing
	{
		get
		{
			return this.clothing;
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x0600034D RID: 845 RVA: 0x0001850C File Offset: 0x0001670C
	public Requirement.OutfitType PendingClothing
	{
		get
		{
			return this._pendingClothing;
		}
	}

	// Token: 0x0600034E RID: 846 RVA: 0x00018514 File Offset: 0x00016714
	public void RequestClothing(Requirement.OutfitType clothing, Action onSuccess)
	{
		if (clothing == Requirement.OutfitType.None)
		{
			this.clothing = clothing;
			this._pendingClothing = clothing;
			this.StoreState();
			GameState.CurrentState.QueueSave();
			if (onSuccess != null)
			{
				onSuccess();
			}
			return;
		}
		this._pendingClothing = clothing;
		this.clothing = clothing;
		this.StoreState();
		GameState.CurrentState.QueueSave();
		if (onSuccess != null)
		{
			onSuccess();
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x0600034F RID: 847 RVA: 0x00018580 File Offset: 0x00016780
	// (set) Token: 0x06000350 RID: 848 RVA: 0x00018598 File Offset: 0x00016798
	public int Love
	{
		get
		{
			return Mathf.Min(9, Mathf.Max(0, (int)this.love));
		}
		set
		{
			if (value > (int)this.love)
			{
				this.ReceivedDates = (Requirement.DateType)0;
				this.GiftCount = new int[4];
				this.DateCount = new int[4];
				StatsModel statsModel = Universe.Stats[(short)(this.GirlName + 1)];
				float num = (float)(Mathf.Max(0, Mathf.Min(9, value)) + 1) * statsModel.PrestigePerLevel;
				GameState.CurrentState.PendingPrestige.Value += num;
				GameState.CurrentState.QueueSave();
			}
			this.love = (Girl.LoveLevel)Mathf.Max(0, Mathf.Min(9, value));
			this.StoreState();
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x06000351 RID: 849 RVA: 0x00018638 File Offset: 0x00016838
	// (set) Token: 0x06000352 RID: 850 RVA: 0x00018640 File Offset: 0x00016840
	public long Hearts
	{
		get
		{
			return this.hearts;
		}
		set
		{
			if (this.Love >= 9)
			{
				return;
			}
			if (value == this.hearts || (value > this.HeartRequirement && this.hearts == this.HeartRequirement))
			{
				return;
			}
			if (value > this.hearts)
			{
				if (9223372036854775807L - GameState.HeartCount.Value > value - this.hearts)
				{
					GameState.HeartCount.Value += value - this.hearts;
				}
				else
				{
					GameState.HeartCount.Value = long.MaxValue;
				}
				if (this.hearts == this.HeartRequirement)
				{
					return;
				}
				if (GameState.GetGirlScreen().gameObject.activeInHierarchy && Girls.CurrentGirl == this)
				{
					GameState.GetGirlScreen().HeartParticles((value - this.hearts <= 25L) ? ((int)(value - this.hearts)) : 25);
					GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.Hearts);
				}
			}
			this.hearts = ((value <= this.HeartRequirement) ? value : this.HeartRequirement);
			this.StoreStateFast();
		}
	}

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x06000353 RID: 851 RVA: 0x00018778 File Offset: 0x00016978
	public GirlModel.GirlLevel CachedLevel
	{
		get
		{
			return this.cachedLevel;
		}
	}

	// Token: 0x06000354 RID: 852 RVA: 0x00018780 File Offset: 0x00016980
	private void CheckCachedRequirements()
	{
		int num = Mathf.Max(0, Mathf.Min(8, this.Love));
		if (this.cachedLevel == null || (int)this.cachedLevel.Level != num + 1 || this.cachedLevel.Language != (Translations.Language)Translations.CurrentLanguage.Value)
		{
			this.cachedLevel = this.Data.Levels[num];
			this.cachedLevel.Language = (Translations.Language)Translations.CurrentLanguage.Value;
			this.cachedHeartReq = this.cachedLevel.Requirements[0].Quantity;
			if (this.cachedRequirements == null || this.cachedRequirements.Length != this.cachedLevel.Requirements.Length - 1)
			{
				this.cachedRequirements = new Requirement[this.cachedLevel.Requirements.Length - 1];
			}
			int i = 0;
			while (i < this.cachedRequirements.Length)
			{
				GirlModel.GirlRequirement girlRequirement = this.cachedLevel.Requirements[i + 1];
				switch (girlRequirement.RequirementType)
				{
				case Requirement.RequirementType.Skill:
					this.cachedRequirements[i] = Requirement.NewSkillRequirement((Requirement.Skill)(girlRequirement.Id - 1), (int)girlRequirement.Quantity);
					break;
				case Requirement.RequirementType.Money:
					this.cachedRequirements[i] = Requirement.NewMoneyRequirement((double)girlRequirement.Quantity, false);
					break;
				case Requirement.RequirementType.Job:
					this.cachedRequirements[i] = Requirement.NewJobRequirement(girlRequirement.Id, (int)girlRequirement.Quantity);
					break;
				case Requirement.RequirementType.Hobby:
				case Requirement.RequirementType.Affection:
				case Requirement.RequirementType.Heart:
					goto IL_383;
				case Requirement.RequirementType.Time:
					this.cachedRequirements[i] = Requirement.NewTimeBlockRequirement((int)girlRequirement.Quantity);
					break;
				case Requirement.RequirementType.Gift:
					this.cachedRequirements[i] = Requirement.NewGiftRequirement(i, this, girlRequirement.Id, (int)girlRequirement.Quantity);
					break;
				case Requirement.RequirementType.Date:
					this.cachedRequirements[i] = Requirement.NewDateRequirement(i, this, girlRequirement.Id, (int)girlRequirement.Quantity);
					break;
				case Requirement.RequirementType.Achievement:
					this.cachedRequirements[i] = Requirement.NewAchievementRequirement(this, (int)girlRequirement.Quantity);
					break;
				case Requirement.RequirementType.Diamond:
					this.cachedRequirements[i] = Requirement.NewDiamondRequirement(this, (int)girlRequirement.Quantity, false);
					break;
				case Requirement.RequirementType.Prestige:
					this.cachedRequirements[i] = Requirement.NewPrestigeRequirement((float)girlRequirement.Quantity, false);
					break;
				case Requirement.RequirementType.PrestigeConsume:
					this.cachedRequirements[i] = Requirement.NewPrestigeRequirement((float)girlRequirement.Quantity, true);
					break;
				case Requirement.RequirementType.Album:
					this.cachedRequirements[i] = Requirement.NewFullAlbumRequirement(this);
					break;
				case Requirement.RequirementType.TotalDates:
					this.cachedRequirements[i] = Requirement.NewDateCountRequirement((int)girlRequirement.Quantity);
					break;
				case Requirement.RequirementType.TotalGifts:
					this.cachedRequirements[i] = Requirement.NewGiftCountRequirement((int)girlRequirement.Quantity);
					break;
				case Requirement.RequirementType.MoneyConsume:
					this.cachedRequirements[i] = Requirement.NewMoneyRequirement((double)girlRequirement.Quantity, true);
					break;
				case Requirement.RequirementType.DiamondConsume:
					this.cachedRequirements[i] = Requirement.NewDiamondRequirement(this, (int)girlRequirement.Quantity, true);
					break;
				case Requirement.RequirementType.JobGild:
					this.cachedRequirements[i] = Requirement.NewJobGildRequirement((int)girlRequirement.Quantity);
					break;
				case Requirement.RequirementType.HobbyGild:
					this.cachedRequirements[i] = Requirement.NewSkillGildRequirement((int)girlRequirement.Quantity);
					break;
				case Requirement.RequirementType.AllJobs:
					this.cachedRequirements[i] = Requirement.NewJobsMaxLevelRequirement();
					break;
				case Requirement.RequirementType.AllHobbies:
					this.cachedRequirements[i] = Requirement.NewAllSkillRequirement((int)girlRequirement.Quantity);
					break;
				case Requirement.RequirementType.GirlsAtLover:
					this.cachedRequirements[i] = Requirement.NewGirlsAtLoverRequirement((int)girlRequirement.Quantity);
					break;
				default:
					goto IL_383;
				}
				IL_39F:
				this.cachedRequirements[i].Index = i;
				i++;
				continue;
				IL_383:
				this.cachedRequirements[i] = Requirement.NewMoneyRequirement(10.0, false);
				goto IL_39F;
			}
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x06000355 RID: 853 RVA: 0x00018B4C File Offset: 0x00016D4C
	public Requirement[] Requirements
	{
		get
		{
			this.CheckCachedRequirements();
			return this.cachedRequirements;
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x06000356 RID: 854 RVA: 0x00018B5C File Offset: 0x00016D5C
	public long HeartRequirement
	{
		get
		{
			this.CheckCachedRequirements();
			return this.cachedHeartReq;
		}
	}

	// Token: 0x06000357 RID: 855 RVA: 0x00018B6C File Offset: 0x00016D6C
	public void SetLove(Girl.LoveLevel value)
	{
		this.love = value;
		this.StoreState();
	}

	// Token: 0x06000358 RID: 856 RVA: 0x00018B7C File Offset: 0x00016D7C
	public void SetHearts(long value, bool playParticles = false)
	{
		if (this.hearts >= this.HeartRequirement)
		{
			return;
		}
		if (value == this.hearts)
		{
			return;
		}
		if (value > this.HeartRequirement)
		{
			value = this.HeartRequirement;
		}
		if (playParticles && GameState.GetGirlScreen().gameObject.activeInHierarchy && Girls.CurrentGirl == this)
		{
			GameState.GetGirlScreen().HeartParticles((value - this.hearts <= 25L) ? ((int)(value - this.hearts)) : 25);
		}
		this.hearts = value;
		this.StoreStateFast();
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x06000359 RID: 857 RVA: 0x00018C20 File Offset: 0x00016E20
	public int FavoriteSkill
	{
		get
		{
			if (this.favoriteSkill == -1)
			{
				this.favoriteSkill = (int)(Universe.Stats[(short)this.GirlName + 1].FavouriteSkill.Id - 1);
			}
			return this.favoriteSkill;
		}
	}

	// Token: 0x0600035A RID: 858 RVA: 0x00018C68 File Offset: 0x00016E68
	public GameState.UpdateType PerformUpdate(float dt)
	{
		if (this.TalkCooldown > 0f)
		{
			this.TalkCooldown -= dt;
		}
		if (this.PokeCooldown > 0f)
		{
			this.PokeCooldown -= dt;
		}
		if (this.GiftCooldown > 0f)
		{
			this.GiftCooldown -= dt;
		}
		if (this.DateCooldown > 0f)
		{
			this.DateCooldown -= dt;
		}
		if ((this.hearts != 0L || this.Love > 0) && Skills.SkillLevel[this.FavoriteSkill] > 0)
		{
			this.currentTime += dt * (float)Skills.SkillLevel[this.FavoriteSkill] / 30f;
			if (this.currentTime > 1f)
			{
				long num = (long)this.currentTime;
				if (this.GirlName == Balance.GirlName.DarkOne)
				{
					num *= 50000L;
					if (num < 0L || this.currentTime > (float)this.Hearts)
					{
						this.SetHearts(1L, false);
						this.currentTime = 0f;
					}
					else
					{
						if (num > this.Hearts)
						{
							num = this.Hearts;
						}
						if (this.Love < 9 && (this.Love > 0 || this.Hearts > 0L))
						{
							this.SetHearts(this.Hearts - num, false);
						}
						if (this.hearts <= 0L)
						{
							this.SetHearts(1L, false);
						}
						this.currentTime -= (float)(num / 50000L);
					}
				}
				else if (num < 0L || this.currentTime > (float)this.HeartRequirement)
				{
					this.SetHearts(this.HeartRequirement, false);
					this.currentTime = 0f;
				}
				else
				{
					if (this.Love < 9 && (this.Love > 0 || this.Hearts > 0L))
					{
						this.SetHearts(this.Hearts + num, false);
					}
					this.currentTime -= (float)num;
				}
				if (Girls.CurrentGirl == this)
				{
					GameState.GetGirlScreen().UpdateHeartText(false);
				}
			}
		}
		return GameState.UpdateType.None;
	}

	// Token: 0x0600035B RID: 859 RVA: 0x00018EAC File Offset: 0x000170AC
	public void SaveCurrentTime()
	{
	}

	// Token: 0x0600035C RID: 860 RVA: 0x00018EB0 File Offset: 0x000170B0
	private void Start()
	{
		Shader.EnableKeyword("UNITY_UI_CLIP_RECT");
		Translations.CurrentLanguage += new ReactiveProperty<int>.Changed(this.UpdateLanguage);
	}

	// Token: 0x0600035D RID: 861 RVA: 0x00018ED8 File Offset: 0x000170D8
	private void UpdateLanguage(int newLanguage)
	{
		base.transform.Find("Requirement").GetComponent<Text>().text = Translations.GetGirlRequirement(this.GirlName);
	}

	// Token: 0x0600035E RID: 862 RVA: 0x00018F0C File Offset: 0x0001710C
	private void OnDestroy()
	{
		Translations.CurrentLanguage -= new ReactiveProperty<int>.Changed(this.UpdateLanguage);
	}

	// Token: 0x0600035F RID: 863 RVA: 0x00018F2C File Offset: 0x0001712C
	private void Update()
	{
		if (this.GirlName == Balance.GirlName.Ayano && (this.hearts > 0L || this.love > Girl.LoveLevel.Adversary))
		{
			Image component = base.transform.Find("Unlocked").GetComponent<Image>();
			if (this.portraitToggle < -1f)
			{
				this.portraitToggle = (float)UnityEngine.Random.Range(40, 120);
			}
			else
			{
				this.portraitToggle -= Time.deltaTime;
			}
			if (component.sprite == this.Portrait && this.portraitToggle <= 0f)
			{
				component.sprite = this.LoverIcon;
			}
			else if (component.sprite == this.LoverIcon && this.portraitToggle > 0f)
			{
				component.sprite = this.Portrait;
			}
		}
	}

	// Token: 0x06000360 RID: 864 RVA: 0x00019014 File Offset: 0x00017214
	public static string GetOutfitName(Requirement.OutfitType outfit)
	{
		switch (outfit)
		{
		case Requirement.OutfitType.Monster:
			return Girl.outfitNames[7];
		case Requirement.OutfitType.Animated:
			return Girl.outfitNames[8];
		default:
			if (outfit == Requirement.OutfitType.Christmas)
			{
				return Girl.outfitNames[5];
			}
			if (outfit == Requirement.OutfitType.SchoolUniform)
			{
				return Girl.outfitNames[1];
			}
			if (outfit == Requirement.OutfitType.BathingSuit)
			{
				return Girl.outfitNames[0];
			}
			if (outfit == Requirement.OutfitType.Unique)
			{
				return Girl.outfitNames[6];
			}
			if (outfit == Requirement.OutfitType.DiamondRing)
			{
				return Girl.outfitNames[2];
			}
			if (outfit == Requirement.OutfitType.Lingerie)
			{
				return Girl.outfitNames[3];
			}
			if (outfit != Requirement.OutfitType.Nude)
			{
				return Girl.outfitNames[0];
			}
			return Girl.outfitNames[4];
		case Requirement.OutfitType.DeluxeWedding:
			return Girl.outfitNames[9];
		}
	}

	// Token: 0x06000361 RID: 865 RVA: 0x000190E8 File Offset: 0x000172E8
	public void SetIcon()
	{
		if (this.LoverIcon == null)
		{
			return;
		}
		if (this.GirlName == Balance.GirlName.Eva && this.Love > 0)
		{
			base.transform.Find("Unlocked").GetComponent<Image>().sprite = this.LoverIcon;
		}
		else if (this.GirlName == Balance.GirlName.Ayano)
		{
			if (this.Portrait != null && (this.Hearts > 0L || this.Love > 0))
			{
				base.transform.Find("Unlocked").GetComponent<Image>().sprite = this.Portrait;
				Translations.UpdateGirlNames();
			}
		}
		else if (this.Love == 9)
		{
			base.transform.Find("Unlocked").GetComponent<Image>().sprite = this.LoverIcon;
		}
	}

	// Token: 0x06000362 RID: 866 RVA: 0x000191D4 File Offset: 0x000173D4
	public bool GetText(out string interactionText, out string buttonText, out string voiceoverId)
	{
		if (this.Love == 0 && this.Hearts < 1L)
		{
			this.CurrentPose = this.GetSprite(Girl.SpriteType.Scenario);
		}
		else if (this.Love < 1)
		{
			this.CurrentPose = this.GetSprite(Girl.SpriteType.Scenario);
		}
		else if (this.Love < 2)
		{
			this.CurrentPose = this.GetSprite((!this.MeetsRequirements()) ? Girl.SpriteType.Mad : Girl.SpriteType.Neutral);
		}
		else if (this.Love < 3)
		{
			this.CurrentPose = this.GetSprite((!this.MeetsRequirements()) ? Girl.SpriteType.Neutral : Girl.SpriteType.LikesYou);
		}
		else if (this.Love < 4)
		{
			this.CurrentPose = this.GetSprite((!this.MeetsRequirements()) ? Girl.SpriteType.Neutral : Girl.SpriteType.LikesYou);
		}
		else if (this.Love < 5)
		{
			this.CurrentPose = this.GetSprite((!this.MeetsRequirements()) ? Girl.SpriteType.Neutral : Girl.SpriteType.Happy);
		}
		else if (this.Love < 6)
		{
			this.CurrentPose = this.GetSprite((!this.MeetsRequirements()) ? Girl.SpriteType.LikesYou : Girl.SpriteType.Happy);
		}
		else if (this.Love < 7)
		{
			this.CurrentPose = this.GetSprite((!this.MeetsRequirements()) ? Girl.SpriteType.Happy : Girl.SpriteType.Tickled);
		}
		else if (this.Love < 8)
		{
			this.CurrentPose = this.GetSprite(Girl.SpriteType.LikesYouLots);
		}
		else
		{
			this.CurrentPose = this.GetSprite(Girl.SpriteType.LikesYouLots);
		}
		interactionText = string.Empty;
		buttonText = "...";
		voiceoverId = string.Empty;
		if (this.MeetsRequirements() && this.Hearts >= this.HeartRequirement)
		{
			GirlModel.GirlText text;
			if (this.DisplayFollowUp)
			{
				text = this.Data.GetText(GirlModel.TextType.LevelText2, Math.Min(8, this.Love));
			}
			else
			{
				text = this.Data.GetText(GirlModel.TextType.LevelText1, Math.Min(8, this.Love));
			}
			voiceoverId = this.GirlName.ToLowerFriendlyString() + "_" + text.ID.ToString();
			interactionText = this.GetDialogueTranslationAndPlay(text, this.GirlName, "GetText" + this.Love.ToString(), 0.5f, Voiceover.BundleType.Girl);
			return true;
		}
		return false;
	}

	// Token: 0x06000363 RID: 867 RVA: 0x00019438 File Offset: 0x00017638
	public bool MeetsRequirements()
	{
		if (Girls.CurrentGirl == null)
		{
			return false;
		}
		if (Girls.CurrentGirl.GirlName == Balance.GirlName.Eva && (this.Requirements == null || this.Requirements.Length == 0) && Girls.CurrentGirl.Love < 9)
		{
			return true;
		}
		if (this.Requirements == null || this.Requirements.Length == 0)
		{
			return false;
		}
		bool result = true;
		foreach (Requirement requirement in this.Requirements)
		{
			if (!requirement.UpdateRequirement())
			{
				result = false;
			}
		}
		return result;
	}

	// Token: 0x06000364 RID: 868 RVA: 0x000194E4 File Offset: 0x000176E4
	public void ResetRequirements()
	{
		this.CheckCachedRequirements();
	}

	// Token: 0x06000365 RID: 869 RVA: 0x000194EC File Offset: 0x000176EC
	private void UnloadOutfit()
	{
		if (this.loadedBundles.ContainsKey(this.outfitBundle.name) && this.outfitBundle != null)
		{
			this.loadedBundles.Remove(this.outfitBundle.name);
			Debug.LogFormat("Unloaded {0}", new object[]
			{
				this.outfitBundle.name
			});
			GameState.AssetManager.UnloadBundle(this.outfitBundle, true);
			this.outfitBundle = null;
			SkeletonGraphic component = GameState.FindChild("Girls/Girl Information/Girl Popup/Girl Popup Pose/Animated").GetComponent<SkeletonGraphic>();
			if (component.skeletonDataAsset != null)
			{
				component.skeletonDataAsset = null;
			}
		}
	}

	// Token: 0x06000366 RID: 870 RVA: 0x0001959C File Offset: 0x0001779C
	public void GetSprite(Girl.SpriteType type, Action<Sprite, Girl> callback)
	{
		List<string> bundles = this.GetBundles(this.Clothing, Girl.SpriteType.Scenario);
		Sprite sprite = null;
		for (int i = bundles.Count - 1; i >= 0; i--)
		{
			if (sprite != null)
			{
				break;
			}
			string bundleName = bundles[i];
			AssetBundle assetBundle = null;
			if (!this.loadedBundles.TryGetValue(bundleName, out assetBundle))
			{
				GameState.AssetManager.GetBundle(bundleName, false, delegate(AssetBundle newBundle)
				{
					if (newBundle == null && this.Clothing != Requirement.OutfitType.None && !bundleName.Contains("nsfw"))
					{
						this.clothing = Requirement.OutfitType.None;
					}
					if (!this.loadedBundles.ContainsKey(bundleName))
					{
						this.loadedBundles.Add(bundleName, newBundle);
					}
					else
					{
						GameState.AssetManager.UnloadBundle(newBundle);
					}
					this.GetSprite(type, callback);
				});
				return;
			}
			if (!(assetBundle == null))
			{
				if (this.Clothing != Requirement.OutfitType.None)
				{
					if (this.outfitBundle != null && this.outfitBundle.name != assetBundle.name)
					{
						this.UnloadOutfit();
					}
					this.outfitBundle = assetBundle;
				}
				if (this.outfitBundle != null && this.clothing == Requirement.OutfitType.Animated)
				{
					this.GetAnimatedAssetsFromBundle(this.outfitBundle);
				}
				string text = string.Format("girl_{0}_{1}", this.GirlNameToAssetName(this.GirlName), this.SpriteTypeToName(type));
				if (this.Clothing != Requirement.OutfitType.None)
				{
					text = Girl.GetOutfitName(this.Clothing) + "_" + text;
				}
				sprite = assetBundle.LoadAsset<Sprite>(text);
				if (!(sprite == null))
				{
					if (this.Clothing == Requirement.OutfitType.None && this.outfitBundle != null)
					{
						this.UnloadOutfit();
						this.outfitBundle = null;
					}
					if (callback != null)
					{
						callback(sprite, this);
					}
					return;
				}
			}
		}
		Debug.Log(string.Format("Could not find type {0} for {1}.  Tried {2} bundles.", type.ToFriendlyString(), this.GirlName.ToFriendlyString(), bundles.Count.ToString()));
		if (callback != null)
		{
			callback(null, this);
		}
	}

	// Token: 0x06000367 RID: 871 RVA: 0x000197D8 File Offset: 0x000179D8
	private void GetAnimatedAssetsFromBundle(AssetBundle bundle)
	{
		if (bundle == null)
		{
			return;
		}
		RectTransform rectTransform = GameState.FindChild<RectTransform>("Girls/Girl Information/Girl Popup/Girl Popup Pose/Animated");
		SkeletonGraphic component = rectTransform.GetComponent<SkeletonGraphic>();
		TextAsset textAsset = null;
		TextAsset textAsset2 = null;
		Texture texture = null;
		string[] allAssetNames = bundle.GetAllAssetNames();
		for (int i = 0; i < allAssetNames.Length; i++)
		{
			if (allAssetNames[i].EndsWith("json"))
			{
				textAsset = bundle.LoadAsset<TextAsset>(allAssetNames[i]);
			}
			else if (allAssetNames[i].EndsWith("txt"))
			{
				textAsset2 = bundle.LoadAsset<TextAsset>(allAssetNames[i]);
			}
			else if (allAssetNames[i].EndsWith("png"))
			{
				texture = bundle.LoadAsset<Texture>(allAssetNames[i]);
			}
		}
		if (textAsset == null || textAsset2 == null || texture == null)
		{
			Debug.LogError("Could not load needed assets for animated girl: " + this.GirlName.ToFriendlyString());
			return;
		}
		if (component.skeletonDataAsset != null && component.skeletonDataAsset.skeletonJSON.Equals(textAsset))
		{
			return;
		}
		SkeletonDataAsset skeletonDataAsset = ScriptableObject.CreateInstance<SkeletonDataAsset>();
		skeletonDataAsset.skeletonJSON = textAsset;
		skeletonDataAsset.scale = 0.01f;
		SpineAtlasAsset spineAtlasAsset = ScriptableObject.CreateInstance<SpineAtlasAsset>();
		component.material.SetTexture("_MainTex", texture);
		spineAtlasAsset.atlasFile = textAsset2;
		spineAtlasAsset.materials = new Material[]
		{
			component.material
		};
		skeletonDataAsset.atlasAssets = new SpineAtlasAsset[]
		{
			spineAtlasAsset
		};
		component.skeletonDataAsset = skeletonDataAsset;
		component.Initialize(true);
		RectTransform rectTransform2 = GameState.FindChild<RectTransform>(string.Format("Girls/Girl Information/Girl Popup/Girl Popup Pose/{0}", this.GirlName.ToFriendlyString()));
		rectTransform.anchoredPosition = rectTransform2.anchoredPosition;
		rectTransform.pivot = rectTransform2.pivot;
		rectTransform.localScale = rectTransform2.localScale;
		rectTransform.anchorMin = rectTransform2.anchorMin;
		rectTransform.anchorMax = rectTransform2.anchorMax;
		rectTransform.sizeDelta = rectTransform2.sizeDelta;
		if (!GameState.FindChild<Image>("Girls/Girl Information/Girl Popup/Girl Popup Pose").enabled)
		{
			rectTransform.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000368 RID: 872 RVA: 0x000199FC File Offset: 0x00017BFC
	private string GirlNameToAssetName(Balance.GirlName girl)
	{
		if (girl != Balance.GirlName.Bonnibel)
		{
			return girl.ToLowerFriendlyString();
		}
		return "bonbon";
	}

	// Token: 0x06000369 RID: 873 RVA: 0x00019A24 File Offset: 0x00017C24
	private string SpriteTypeToName(Girl.SpriteType type)
	{
		switch (type)
		{
		case Girl.SpriteType.Happy:
			return "happy";
		case Girl.SpriteType.Harrassed:
			return "harrassed";
		case Girl.SpriteType.LikesYou:
			return "likesyou";
		case Girl.SpriteType.LikesYouLots:
			return "likesyouLOTS";
		case Girl.SpriteType.Mad:
			return "mad";
		case Girl.SpriteType.Neutral:
			return "neutral";
		case Girl.SpriteType.Tickled:
			return "tickled";
		case Girl.SpriteType.Scenario:
			return "special";
		default:
			if (type != Girl.SpriteType.Sick)
			{
				return "neutral";
			}
			return "sick";
		}
	}

	// Token: 0x0600036A RID: 874 RVA: 0x00019AA4 File Offset: 0x00017CA4
	public Sprite GetSprite(Girl.SpriteType type)
	{
		if (type == Girl.SpriteType.LikesYouLots)
		{
			GameState.GetGirlScreen().ResetLoveAnimation();
		}
		this.CurrentSpriteType = type;
		return null;
	}

	// Token: 0x0600036B RID: 875 RVA: 0x00019AC0 File Offset: 0x00017CC0
	private List<string> GetBundles(Requirement.OutfitType outfit, Girl.SpriteType type = Girl.SpriteType.Scenario)
	{
		List<string> list = new List<string>();
		if (type >= Girl.SpriteType.Moonlight && type <= Girl.SpriteType.Beach)
		{
			list.Add(string.Format("{0}/{0}_dates", this.GirlName.ToLowerFriendlyString()));
			if (this.Love == 9 && (this.GirlName == Balance.GirlName.Bearverly || this.GirlName == Balance.GirlName.Alpha || this.GirlName == Balance.GirlName.Generica))
			{
				list.Add(string.Format("{0}/{0}_dates_human", this.GirlName.ToLowerFriendlyString()));
			}
			if (GameState.NSFW)
			{
				list.Add(string.Format("{0}/{0}_dates_nsfw", this.GirlName.ToLowerFriendlyString()));
			}
		}
		else if (type >= Girl.SpriteType.Intro && type <= Girl.SpriteType.Lover)
		{
			list.Add(string.Format("{0}/{0}_pinups", this.GirlName.ToLowerFriendlyString()));
			if (GameState.NSFW)
			{
				list.Add(string.Format("{0}/{0}_pinups_nsfw", this.GirlName.ToLowerFriendlyString()));
			}
		}
		else if (outfit == Requirement.OutfitType.None)
		{
			list.Add(string.Format("{0}/{0}", this.GirlName.ToLowerFriendlyString()));
			if (this.Love == 9 && (this.GirlName == Balance.GirlName.Bearverly || this.GirlName == Balance.GirlName.Alpha || this.GirlName == Balance.GirlName.Generica))
			{
				list.Add(string.Format("{0}/{0}_human", this.GirlName.ToLowerFriendlyString()));
			}
			if (GameState.NSFW)
			{
				list.Add(string.Format("{0}/{0}_nsfw", this.GirlName.ToLowerFriendlyString()));
			}
		}
		else
		{
			list.Add(string.Format("{0}/{0}_{1}", this.GirlName.ToLowerFriendlyString(), Girl.GetOutfitName(outfit).ToLowerInvariant()));
			if (GameState.NSFW && outfit != Requirement.OutfitType.Nude && outfit != Requirement.OutfitType.Lingerie)
			{
				list.Add(string.Format("{0}/{0}_{1}_nsfw", this.GirlName.ToLowerFriendlyString(), Girl.GetOutfitName(outfit).ToLowerInvariant()));
			}
		}
		return list;
	}

	// Token: 0x0600036C RID: 876 RVA: 0x00019CD4 File Offset: 0x00017ED4
	public void UnloadAssets()
	{
		foreach (KeyValuePair<string, AssetBundle> keyValuePair in this.loadedBundles)
		{
			GameState.AssetManager.UnloadBundle(keyValuePair.Value, true);
		}
		this.loadedBundles.Clear();
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x0600036D RID: 877 RVA: 0x00019D50 File Offset: 0x00017F50
	// (set) Token: 0x0600036E RID: 878 RVA: 0x00019D58 File Offset: 0x00017F58
	public bool PendingLoad { get; private set; }

	// Token: 0x0600036F RID: 879 RVA: 0x00019D64 File Offset: 0x00017F64
	public void LoadAssets(bool fastLoad, bool setWhenLoaded = false)
	{
		base.transform.Find("Store Button").gameObject.SetActive(false);
		base.transform.Find("Store Requirement").gameObject.SetActive(false);
		if (this._loaded)
		{
			return;
		}
		this._loaded = true;
		this.PendingLoad = true;
		List<string> bundles = this.GetBundles(Requirement.OutfitType.None, Girl.SpriteType.Scenario);
		Dictionary<string, AssetBundle> bundlePreload = new Dictionary<string, AssetBundle>();
		for (int i = 0; i < bundles.Count; i++)
		{
			int temp = i;
			string bundleName = bundles[i];
			AssetBundle bundle = null;
			if (!bundlePreload.TryGetValue(bundleName, out bundle))
			{
				GameState.AssetManager.GetBundle(bundleName, false, delegate(AssetBundle bundleCallback)
				{
					if (bundleCallback != null)
					{
						if (!bundlePreload.ContainsKey(bundleName))
						{
							bundlePreload.Add(bundleName, bundleCallback);
						}
						else
						{
							GameState.AssetManager.UnloadBundle(bundleCallback);
						}
						bundle = bundleCallback;
					}
					this.transform.Find("Unlocked").GetComponent<Button>().interactable = true;
					this.transform.GetComponent<Image>().enabled = false;
					if (bundlePreload.Count > 0)
					{
						this.PendingLoad = false;
						foreach (KeyValuePair<string, AssetBundle> keyValuePair in bundlePreload)
						{
							GameState.AssetManager.UnloadBundle(keyValuePair.Value, true);
						}
					}
					if (setWhenLoaded && temp == 0)
					{
						GameState.GetGirlScreen().SetGirl(this);
					}
				});
			}
		}
	}

	// Token: 0x06000370 RID: 880 RVA: 0x00019E68 File Offset: 0x00018068
	public string Poke()
	{
		GirlModel.GirlText text = this.Data.GetText((this.Clothing != Requirement.OutfitType.Nude) ? GirlModel.TextType.Poke : GirlModel.TextType.PokeNude, this.Love);
		if (this.GirlName == Balance.GirlName.Mallory && this.Love >= 6 && this.Love <= 8)
		{
			this.CurrentPose = this.GetSprite(Girl.SpriteType.Sick);
		}
		else if (this.Love < 1)
		{
			this.CurrentPose = this.GetSprite(Girl.SpriteType.Scenario);
		}
		else if (this.Love < 3)
		{
			this.CurrentPose = this.GetSprite(Girl.SpriteType.Mad);
		}
		else if (this.Love < 4)
		{
			this.CurrentPose = this.GetSprite(Girl.SpriteType.Harrassed);
		}
		else if (this.Love < 9)
		{
			this.CurrentPose = this.GetSprite(Girl.SpriteType.Tickled);
		}
		else
		{
			this.CurrentPose = this.GetSprite(Girl.SpriteType.Tickled);
		}
		return this.GetDialogueTranslationAndPlay(text, this.GirlName, "Poke" + this.Love.ToString(), 0.5f, Voiceover.BundleType.Girl);
	}

	// Token: 0x06000371 RID: 881 RVA: 0x00019F88 File Offset: 0x00018188
	public string Talk()
	{
		GirlModel.GirlText text = this.Data.GetText((this.Clothing != Requirement.OutfitType.Nude) ? GirlModel.TextType.Talk : GirlModel.TextType.TalkNude, this.Love);
		if (this.GirlName == Balance.GirlName.Mallory && this.Love >= 6 && this.Love <= 8)
		{
			this.CurrentPose = this.GetSprite(Girl.SpriteType.Sick);
		}
		else if (this.Love < 1)
		{
			this.CurrentPose = this.GetSprite(Girl.SpriteType.Scenario);
		}
		else if (this.Love <= 3)
		{
			this.CurrentPose = this.GetSprite(Girl.SpriteType.Mad);
		}
		else if (this.Love <= 4)
		{
			this.CurrentPose = this.GetSprite(Girl.SpriteType.Neutral);
		}
		else if (this.Love <= 6)
		{
			this.CurrentPose = this.GetSprite(Girl.SpriteType.Happy);
		}
		else if (this.Love < 9)
		{
			this.CurrentPose = this.GetSprite(Girl.SpriteType.LikesYou);
		}
		else
		{
			this.CurrentPose = ((this.Clothing != Requirement.OutfitType.None) ? this.GetSprite(Girl.SpriteType.LikesYou) : this.GetSprite(Girl.SpriteType.Happy));
		}
		return this.GetDialogueTranslationAndPlay(text, this.GirlName, "Talk" + this.Love.ToString(), 0.2f, Voiceover.BundleType.Girl);
	}

	// Token: 0x06000372 RID: 882 RVA: 0x0001A0E0 File Offset: 0x000182E0
	public void GiveGift(Requirement.GiftType gift, int quantity)
	{
		this.gift = gift;
		this.giftQuantity = quantity;
	}

	// Token: 0x06000373 RID: 883 RVA: 0x0001A0F0 File Offset: 0x000182F0
	public void GiveDate(Requirement.DateType date, int quantity)
	{
		this.date = date;
		this.dateQuantity = quantity;
		Album.Add(date, this);
		this.ReceivedDates |= date;
		this.LifetimeDates |= date;
		int num = 0;
		while (this.Requirements != null && num < this.Requirements.Length)
		{
			if (this.Requirements[num].Type == Requirement.RequirementType.Date && this.Requirements[num].Date == date)
			{
				this.DateCount[num] += quantity;
			}
			num++;
		}
		this.StoreState();
	}

	// Token: 0x06000374 RID: 884 RVA: 0x0001A190 File Offset: 0x00018390
	public string Date()
	{
		GirlModel.TextType type = (this.date != Requirement.DateType.Beach) ? ((this.date != Requirement.DateType.MoonlightStroll) ? ((this.date != Requirement.DateType.MovieTheater) ? GirlModel.TextType.Sightseeing : GirlModel.TextType.MovieTheater) : GirlModel.TextType.MoonlightStroll) : GirlModel.TextType.Beach;
		GirlModel.GirlText text = this.Data.GetText(type, this.Love);
		return this.GetDialogueTranslationAndPlay(text, this.GirlName, "Date" + this.Love.ToString(), 0.2f, Voiceover.BundleType.Girl);
	}

	// Token: 0x06000375 RID: 885 RVA: 0x0001A218 File Offset: 0x00018418
	public void FinishDate()
	{
		this.CurrentPose = this.GetRandomGoodSpriteType();
		if (this.CurrentSpriteType == Girl.SpriteType.LikesYouLots)
		{
			this.CurrentPose = this.GetSprite(Girl.SpriteType.LikesYou);
		}
	}

	// Token: 0x06000376 RID: 886 RVA: 0x0001A24C File Offset: 0x0001844C
	private Sprite GetRandomGoodSpriteType()
	{
		int num = UnityEngine.Random.Range(0, 4);
		if (num == 0)
		{
			return this.GetSprite(Girl.SpriteType.Neutral);
		}
		if (num == 1)
		{
			return this.GetSprite(Girl.SpriteType.Happy);
		}
		if (num == 2)
		{
			return this.GetSprite(Girl.SpriteType.LikesYou);
		}
		return this.GetSprite(Girl.SpriteType.LikesYouLots);
	}

	// Token: 0x06000377 RID: 887 RVA: 0x0001A294 File Offset: 0x00018494
	public string Gift()
	{
		this.LifetimeGifts |= this.gift;
		int num = 0;
		while (this.Requirements != null && num < this.Requirements.Length)
		{
			if (this.Requirements[num].Type == Requirement.RequirementType.Gift && this.Requirements[num].Gift == this.gift)
			{
				this.GiftCount[num] += this.giftQuantity;
			}
			num++;
		}
		this.StoreState();
		if (this.GirlName == Balance.GirlName.Explora && this.Love < 9)
		{
			if (this.gift == Requirement.GiftType.FruitBasket)
			{
				this.CurrentPose = this.GetSprite(Girl.SpriteType.Mad);
				return this.GetDialogueTranslationAndPlay(this.Data.GetText(47), this.GirlName, "Gift" + this.Love.ToString(), 0.2f, Voiceover.BundleType.Girl);
			}
			if (this.gift == Requirement.GiftType.Earrings)
			{
				this.CurrentPose = this.GetSprite(Girl.SpriteType.Mad);
				return this.GetDialogueTranslationAndPlay(this.Data.GetText(74), this.GirlName, "Gift" + this.Love.ToString(), 0.2f, Voiceover.BundleType.Girl);
			}
		}
		if (this.GirlName == Balance.GirlName.Mallory && this.gift == Requirement.GiftType.FruitBasket)
		{
			this.CurrentPose = this.GetSprite(Girl.SpriteType.Happy);
			if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
			{
				return this.GetDialogueTranslationAndPlay(this.Data.GetText(68), this.GirlName, "Gift" + this.Love.ToString(), 0.2f, Voiceover.BundleType.Girl);
			}
			return this.GetDialogueTranslationAndPlay(this.Data.GetText(70), this.GirlName, "Gift" + this.Love.ToString(), 0.2f, Voiceover.BundleType.Girl);
		}
		else
		{
			if (this.GirlName == Balance.GirlName.Mallory && this.gift == Requirement.GiftType.USB)
			{
				this.CurrentPose = this.GetSprite(Girl.SpriteType.Happy);
				return this.GetDialogueTranslationAndPlay(this.Data.GetText(73), this.GirlName, "Gift" + this.Love.ToString(), 0.2f, Voiceover.BundleType.Girl);
			}
			if (this.Love < 4)
			{
				this.CurrentPose = this.GetSprite(Girl.SpriteType.Mad);
			}
			else if (this.Love == 4)
			{
				this.CurrentPose = this.GetSprite(Girl.SpriteType.Neutral);
			}
			else if (this.Love < 7)
			{
				this.CurrentPose = this.GetSprite(Girl.SpriteType.Happy);
			}
			else if (this.Love < 9)
			{
				this.CurrentPose = this.GetSprite(Girl.SpriteType.LikesYou);
			}
			else
			{
				this.CurrentPose = this.GetRandomGoodSpriteType();
			}
			GirlModel.GirlText text = this.Data.GetText(GirlModel.TextType.Gift, this.Love);
			return this.GetDialogueTranslationAndPlay(text, this.GirlName, "Gift" + this.Love.ToString(), 0.2f, Voiceover.BundleType.Girl);
		}
	}

	// Token: 0x06000378 RID: 888 RVA: 0x0001A5BC File Offset: 0x000187BC
	public string Gift(Requirement.OutfitType outfit)
	{
		this.LifetimeOutfits |= outfit;
		this.StoreState();
		this.CurrentPose = this.GetSprite(Girl.SpriteType.Happy);
		GirlModel.TextType type = (outfit != Requirement.OutfitType.BathingSuit) ? ((outfit != Requirement.OutfitType.Christmas) ? ((outfit != Requirement.OutfitType.DiamondRing) ? ((outfit != Requirement.OutfitType.Lingerie) ? ((outfit != Requirement.OutfitType.Nude) ? ((outfit != Requirement.OutfitType.SchoolUniform) ? ((outfit != Requirement.OutfitType.Unique) ? ((outfit != Requirement.OutfitType.DeluxeWedding) ? GirlModel.TextType.AnyOutfit : GirlModel.TextType.Wedding) : GirlModel.TextType.Unique) : GirlModel.TextType.Seifuku) : GirlModel.TextType.Nude) : GirlModel.TextType.Lingerie) : GirlModel.TextType.Wedding) : GirlModel.TextType.Xmas) : GirlModel.TextType.BathingSuit;
		GirlModel.GirlText text = this.Data.GetText(type, this.Love);
		if (text == null)
		{
			text = this.Data.GetText(GirlModel.TextType.AnyOutfit, this.Love);
		}
		if (text == null)
		{
			text = this.Data.GetText(GirlModel.TextType.Gift, this.Love);
		}
		return this.GetDialogueTranslationAndPlay(text, this.GirlName, "GiftOutfit" + this.Love.ToString(), 0.2f, Voiceover.BundleType.Girl);
	}

	// Token: 0x06000379 RID: 889 RVA: 0x0001A6E8 File Offset: 0x000188E8
	public string Greeting()
	{
		GirlModel.GirlText girlText = null;
		if (this.GirlName == Balance.GirlName.Mallory && this.Love >= 6 && this.Love <= 8)
		{
			this.CurrentPose = this.GetSprite((!this.MeetsRequirements()) ? Girl.SpriteType.Sick : Girl.SpriteType.Happy);
		}
		else if (this.Love == 0 && this.Hearts < 1L)
		{
			this.CurrentPose = this.GetSprite(Girl.SpriteType.Scenario);
		}
		else if (this.Love < 1)
		{
			this.CurrentPose = this.GetSprite(Girl.SpriteType.Scenario);
		}
		else if (this.Love < 2)
		{
			this.CurrentPose = this.GetSprite((!this.MeetsRequirements()) ? Girl.SpriteType.Mad : Girl.SpriteType.Neutral);
		}
		else if (this.Love < 3)
		{
			this.CurrentPose = this.GetSprite((!this.MeetsRequirements()) ? Girl.SpriteType.Neutral : Girl.SpriteType.LikesYou);
		}
		else if (this.Love < 4)
		{
			this.CurrentPose = this.GetSprite((!this.MeetsRequirements()) ? Girl.SpriteType.Neutral : Girl.SpriteType.LikesYou);
		}
		else if (this.Love < 5)
		{
			this.CurrentPose = this.GetSprite((!this.MeetsRequirements()) ? Girl.SpriteType.Neutral : Girl.SpriteType.Happy);
		}
		else if (this.Love < 6)
		{
			this.CurrentPose = this.GetSprite((!this.MeetsRequirements()) ? Girl.SpriteType.LikesYou : Girl.SpriteType.Happy);
		}
		else if (this.Love < 7)
		{
			this.CurrentPose = this.GetSprite((!this.MeetsRequirements()) ? Girl.SpriteType.Happy : Girl.SpriteType.Tickled);
		}
		else if (this.Love < 8)
		{
			this.CurrentPose = this.GetSprite(Girl.SpriteType.LikesYouLots);
		}
		else
		{
			this.CurrentPose = this.GetSprite(Girl.SpriteType.LikesYouLots);
		}
		if (this.Love > 1 && UnityEngine.Random.value < 0.5f && Girl.LastGirl != Balance.GirlName.Unknown)
		{
			string key = string.Format("idle{0}Talk", Girl.LastGirl.ToFriendlyString());
			if (this.Data.OtherText.ContainsKey(key))
			{
				List<GirlModel.GirlText> list = this.Data.OtherText[key];
				girlText = list[UnityEngine.Random.Range(0, list.Count)];
			}
		}
		if (girlText == null && this.Love > 1 && UnityEngine.Random.value < 0.5f && Girl.LastGirl == Balance.GirlName.Ayano && this.GirlName != Balance.GirlName.Ayano)
		{
			girlText = this.Data.GetText(GirlModel.TextType.Ayano, this.Love);
		}
		if (girlText == null)
		{
			girlText = this.Data.GetText((this.Clothing != Requirement.OutfitType.Nude) ? GirlModel.TextType.Greet : GirlModel.TextType.GreetNude, this.Love);
		}
		return this.GetDialogueTranslationAndPlay(girlText, this.GirlName, "Greeting" + this.Love.ToString(), 0.5f, Voiceover.BundleType.Girl);
	}

	// Token: 0x0600037A RID: 890 RVA: 0x0001A9EC File Offset: 0x00018BEC
	public void AdvanceRelationship()
	{
		foreach (GirlModel.GirlRequirement girlRequirement in this.cachedLevel.Requirements)
		{
			switch (girlRequirement.RequirementType)
			{
			case Requirement.RequirementType.PrestigeConsume:
				GameState.CurrentState.TimeMultiplier.Value -= (float)girlRequirement.Quantity;
				global::PlayerPrefs.SetFloat("TimeMultiplier", GameState.CurrentState.TimeMultiplier.Value);
				break;
			case Requirement.RequirementType.MoneyConsume:
				GameState.Money.Value -= (double)girlRequirement.Quantity;
				break;
			case Requirement.RequirementType.DiamondConsume:
				GameState.Diamonds.Value -= (int)girlRequirement.Quantity;
				break;
			}
		}
		if (this.GirlName == Balance.GirlName.DarkOne && this.Love == 8)
		{
			GameState.CurrentState.TimeMultiplier.Value = 2048f;
			global::PlayerPrefs.SetFloat("TimeMultiplier", GameState.CurrentState.TimeMultiplier.Value);
		}
		if (this.GirlName == Balance.GirlName.Shibuki && this.Love == 8)
		{
			GameState.CurrentState.TimeMultiplier.Value = Mathf.Min(2048f, GameState.CurrentState.TimeMultiplier.Value + 45f);
			global::PlayerPrefs.SetFloat("TimeMultiplier", GameState.CurrentState.TimeMultiplier.Value);
		}
		this.Love++;
		if (this.GirlName == Balance.GirlName.Eva && this.Love == 1)
		{
			this.SetIcon();
		}
		if (this.GirlName == Balance.GirlName.Alpha && this.Love == 9)
		{
			this.SetIcon();
		}
		if (this.GirlName == Balance.GirlName.Bearverly && this.Love == 9)
		{
			this.SetIcon();
		}
		if (this.GirlName == Balance.GirlName.DarkOne && this.Love == 9)
		{
			this.SetIcon();
		}
		if (this.GirlName == Balance.GirlName.QPiddy && this.Love == 9)
		{
			this.SetIcon();
		}
		if (this.GirlName == Balance.GirlName.Generica && this.Love == 9)
		{
			this.SetIcon();
		}
		Achievements.TriggerLoveAchievement(this.GirlName);
	}

	// Token: 0x0600037B RID: 891 RVA: 0x0001AC4C File Offset: 0x00018E4C
	public string GetDialogueTranslationAndPlay(GirlModel.GirlText text, Balance.GirlName girlName, string type, float delay = 0.5f, Voiceover.BundleType bundleType = Voiceover.BundleType.Girl)
	{
		if (text == null)
		{
			Utilities.SendAnalytic(Utilities.AnalyticType.Exception, type + " for " + girlName.ToFriendlyString() + " was null.");
			text = new GirlModel.GirlText
			{
				ID = 0,
				AudioID = null,
				English = "Unknown text..."
			};
		}
		string text2 = girlName.ToLowerFriendlyString() + "_" + text.ID.ToString();
		string id = (!string.IsNullOrEmpty(text.AudioID)) ? text.AudioID : text2;
		GameState.Voiceover.Play(girlName, bundleType, id, delay);
		return Translations.GetDialogueTranslation(text, girlName, type);
	}

	// Token: 0x0600037C RID: 892 RVA: 0x0001ACF0 File Offset: 0x00018EF0
	private static bool IsDlcGirl(Balance.GirlName girlName)
	{
		return Girl.ValueBundleGirls.Contains(girlName);
	}

	// Token: 0x0600037D RID: 893 RVA: 0x0001AD00 File Offset: 0x00018F00
	private static bool IsMonsterGirl(Balance.GirlName girlName)
	{
		bool flag = girlName == Balance.GirlName.Jelle || girlName == Balance.GirlName.Quillzone;
		bool flag2 = girlName == Balance.GirlName.Bonchovy || girlName == Balance.GirlName.Spectrum;
		return flag || flag2;
	}

	// Token: 0x0600037E RID: 894 RVA: 0x0001AD3C File Offset: 0x00018F3C
	public static bool IsLteGirl(Balance.GirlName girlName)
	{
		return girlName > Balance.GirlName.QPiddy && girlName != Balance.GirlName.Peanut && !Girl.ValueBundleGirls.Contains(girlName) && !Girl.IsMonsterGirl(girlName);
	}

	// Token: 0x0600037F RID: 895 RVA: 0x0001AD78 File Offset: 0x00018F78
	public static bool IsCoreGirl(Balance.GirlName girlName)
	{
		return girlName <= Balance.GirlName.QPiddy || girlName == Balance.GirlName.Peanut;
	}

	// Token: 0x04000360 RID: 864
	public static List<Girl> ActiveGirls = new List<Girl>();

	// Token: 0x04000361 RID: 865
	public static Balance.GirlName LastGirl;

	// Token: 0x04000362 RID: 866
	private GirlModel _data;

	// Token: 0x04000363 RID: 867
	public Girl.SpriteType CurrentSpriteType;

	// Token: 0x04000364 RID: 868
	public static int AssetQueue = 0;

	// Token: 0x04000365 RID: 869
	public static int AssetsLoaded = 0;

	// Token: 0x04000366 RID: 870
	private static readonly string[] outfitNames = new string[]
	{
		"BIKINI",
		"SEIFUKU",
		"WEDDING",
		"LINGERIE",
		"NUDE",
		"XMAS",
		"UNIQUE",
		"MONSTER",
		"ANIMATED",
		"WEDDING2"
	};

	// Token: 0x04000367 RID: 871
	public Balance.GirlName GirlName;

	// Token: 0x04000368 RID: 872
	public Sprite LoverIcon;

	// Token: 0x04000369 RID: 873
	public int[] DateOffsets;

	// Token: 0x0400036A RID: 874
	public Sprite Portrait;

	// Token: 0x0400036B RID: 875
	public bool DisplayFollowUp;

	// Token: 0x0400036C RID: 876
	private long hearts;

	// Token: 0x0400036D RID: 877
	private Girl.LoveLevel love;

	// Token: 0x0400036E RID: 878
	private float currentTime;

	// Token: 0x0400036F RID: 879
	private Requirement.GiftType gift;

	// Token: 0x04000370 RID: 880
	private int giftQuantity;

	// Token: 0x04000371 RID: 881
	private int dateQuantity;

	// Token: 0x04000372 RID: 882
	private Requirement.DateType date;

	// Token: 0x04000373 RID: 883
	private string girlHeartString = string.Empty;

	// Token: 0x04000374 RID: 884
	private float portraitToggle = -2f;

	// Token: 0x04000375 RID: 885
	private Requirement.OutfitType clothing;

	// Token: 0x04000376 RID: 886
	private Requirement.OutfitType _pendingClothing;

	// Token: 0x04000377 RID: 887
	private GirlModel.GirlLevel cachedLevel;

	// Token: 0x04000378 RID: 888
	private Requirement[] cachedRequirements;

	// Token: 0x04000379 RID: 889
	private long cachedHeartReq;

	// Token: 0x0400037A RID: 890
	private int favoriteSkill = -1;

	// Token: 0x0400037B RID: 891
	private Dictionary<string, AssetBundle> loadedBundles = new Dictionary<string, AssetBundle>();

	// Token: 0x0400037C RID: 892
	private AssetBundle outfitBundle;

	// Token: 0x0400037D RID: 893
	private bool _loaded;

	// Token: 0x0400037E RID: 894
	private static Balance.GirlName[] PhoneGirlStoreList = new Balance.GirlName[]
	{
		Balance.GirlName.Wendy,
		Balance.GirlName.Ruri,
		Balance.GirlName.Generica,
		Balance.GirlName.Sawyer,
		Balance.GirlName.Renee
	};

	// Token: 0x0400037F RID: 895
	private static Balance.GirlName[] ValueBundleGirls = new Balance.GirlName[]
	{
		Balance.GirlName.Catara,
		Balance.GirlName.Charlotte,
		Balance.GirlName.Darya,
		Balance.GirlName.Suzu,
		Balance.GirlName.Mallory
	};

	// Token: 0x0200009C RID: 156
	public enum LoveLevel
	{
		// Token: 0x04000391 RID: 913
		Adversary,
		// Token: 0x04000392 RID: 914
		Nuisance,
		// Token: 0x04000393 RID: 915
		Frenemy,
		// Token: 0x04000394 RID: 916
		Acquaintance,
		// Token: 0x04000395 RID: 917
		Friendzoned,
		// Token: 0x04000396 RID: 918
		Awkward_Besties,
		// Token: 0x04000397 RID: 919
		Crush,
		// Token: 0x04000398 RID: 920
		Sweetheart,
		// Token: 0x04000399 RID: 921
		Girlfriend,
		// Token: 0x0400039A RID: 922
		Lover
	}

	// Token: 0x0200009D RID: 157
	public enum SpriteType
	{
		// Token: 0x0400039C RID: 924
		Happy,
		// Token: 0x0400039D RID: 925
		Harrassed,
		// Token: 0x0400039E RID: 926
		LikesYou,
		// Token: 0x0400039F RID: 927
		LikesYouLots,
		// Token: 0x040003A0 RID: 928
		Mad,
		// Token: 0x040003A1 RID: 929
		Neutral,
		// Token: 0x040003A2 RID: 930
		Tickled,
		// Token: 0x040003A3 RID: 931
		Scenario,
		// Token: 0x040003A4 RID: 932
		Intro = 16,
		// Token: 0x040003A5 RID: 933
		Friendship,
		// Token: 0x040003A6 RID: 934
		Kiss,
		// Token: 0x040003A7 RID: 935
		Lover,
		// Token: 0x040003A8 RID: 936
		Moonlight,
		// Token: 0x040003A9 RID: 937
		Theatre,
		// Token: 0x040003AA RID: 938
		Sightseeing,
		// Token: 0x040003AB RID: 939
		Beach,
		// Token: 0x040003AC RID: 940
		Sick = 30
	}

	// Token: 0x0200009E RID: 158
	public class LoadImageAsyncRequest
	{
		// Token: 0x06000380 RID: 896 RVA: 0x0001AD8C File Offset: 0x00018F8C
		public LoadImageAsyncRequest(Girl girl, Girl.SpriteType type)
		{
			this.girl = girl;
			this.type = type;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0001ADA4 File Offset: 0x00018FA4
		public IEnumerator GetSpriteAsync()
		{
			List<string> bundles = this.girl.GetBundles(Requirement.OutfitType.None, this.type);
			for (int i = bundles.Count - 1; i >= 0; i--)
			{
				string bundleName = bundles[i];
				AssetBundleAsync bundleRequest = GameState.AssetManager.GetBundleAsync(bundleName, false);
				yield return bundleRequest;
				AssetBundle bundle = bundleRequest.AssetBundle;
				if (!(bundle == null))
				{
					string[] names = bundle.GetAllAssetNames();
					for (int j = 0; j < names.Length; j++)
					{
						string name = names[j];
						string lower = name.ToLowerInvariant();
						lower = lower.Substring(0, names[j].Length - 4);
						lower = lower.Substring(lower.LastIndexOf('/') + 1);
						if (lower.StartsWith("date") || lower.StartsWith("event"))
						{
							AssetBundleRequest spriteRequest = null;
							if (lower.StartsWith("dateimg01_moonlight_") && this.type == Girl.SpriteType.Moonlight)
							{
								spriteRequest = bundle.LoadAssetAsync<Sprite>(name);
							}
							if (lower.StartsWith("dateimg05_theatre_") && this.type == Girl.SpriteType.Theatre)
							{
								spriteRequest = bundle.LoadAssetAsync<Sprite>(name);
							}
							if (lower.StartsWith("dateimg01_sightseeing_") && this.type == Girl.SpriteType.Sightseeing)
							{
								spriteRequest = bundle.LoadAssetAsync<Sprite>(name);
							}
							if (lower.StartsWith("dateimg04_beach_") && this.type == Girl.SpriteType.Beach)
							{
								spriteRequest = bundle.LoadAssetAsync<Sprite>(name);
							}
							if (lower.StartsWith("eventcgs00_") && this.type == Girl.SpriteType.Intro)
							{
								spriteRequest = bundle.LoadAssetAsync<Sprite>(name);
							}
							if (lower.StartsWith("eventcgs01_") && this.type == Girl.SpriteType.Friendship)
							{
								spriteRequest = bundle.LoadAssetAsync<Sprite>(name);
							}
							if (lower.StartsWith("eventcgs02_") && this.type == Girl.SpriteType.Kiss)
							{
								spriteRequest = bundle.LoadAssetAsync<Sprite>(name);
							}
							if (lower.StartsWith("eventcgfinal_") && !lower.Contains("1") && !lower.Contains("2") && this.type == Girl.SpriteType.Lover)
							{
								spriteRequest = bundle.LoadAssetAsync<Sprite>(name);
							}
							if (spriteRequest != null)
							{
								this.Bundle = bundle;
								yield return spriteRequest;
								if (spriteRequest.asset != null)
								{
									this.Sprite = (spriteRequest.asset as Sprite);
								}
								yield break;
							}
						}
					}
					GameState.AssetManager.UnloadBundle(bundle);
				}
			}
			yield break;
		}

		// Token: 0x040003AD RID: 941
		private Girl girl;

		// Token: 0x040003AE RID: 942
		private Girl.SpriteType type;

		// Token: 0x040003AF RID: 943
		public Sprite Sprite;

		// Token: 0x040003B0 RID: 944
		public AssetBundle Bundle;
	}
}
