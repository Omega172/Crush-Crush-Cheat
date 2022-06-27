using System;
using System.Collections;
using System.Collections.Generic;
using AssetBundles;
using BlayFap;
using BlayFapShared;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200015A RID: 346
public static class Utilities
{
	// Token: 0x17000145 RID: 325
	// (get) Token: 0x060009B2 RID: 2482 RVA: 0x00050BC4 File Offset: 0x0004EDC4
	public static bool TimeRequested
	{
		get
		{
			return Utilities.timeRequested;
		}
	}

	// Token: 0x17000146 RID: 326
	// (get) Token: 0x060009B3 RID: 2483 RVA: 0x00050BCC File Offset: 0x0004EDCC
	public static DateTime LastTimeCache
	{
		get
		{
			return Utilities.lastTimeCache;
		}
	}

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x060009B4 RID: 2484 RVA: 0x00050BD4 File Offset: 0x0004EDD4
	public static DateTime ServerTime
	{
		get
		{
			DateTime? dateTime = Utilities.serverTime;
			if (dateTime == null)
			{
				return DateTime.UtcNow;
			}
			DateTime? dateTime2 = Utilities.serverTime;
			return dateTime2.Value;
		}
	}

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x060009B5 RID: 2485 RVA: 0x00050C0C File Offset: 0x0004EE0C
	public static TimeSpan TimeOffset
	{
		get
		{
			return Utilities.lastTimeCache - Utilities.ServerTime;
		}
	}

	// Token: 0x060009B6 RID: 2486 RVA: 0x00050C20 File Offset: 0x0004EE20
	public static void AwardServerItem(string itemId)
	{
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x00050C24 File Offset: 0x0004EE24
	public static void ForceCheckCachedServerTime()
	{
		Utilities.UpdateServerTime(null);
	}

	// Token: 0x060009B8 RID: 2488 RVA: 0x00050C2C File Offset: 0x0004EE2C
	public static void CheckCachedServerTime(Action callback)
	{
		if (Math.Abs((DateTime.UtcNow - Utilities.LastTimeCache).TotalMinutes) > 15.0)
		{
			Utilities.UpdateServerTime(callback);
		}
		else if (callback != null)
		{
			callback();
		}
	}

	// Token: 0x060009B9 RID: 2489 RVA: 0x00050C7C File Offset: 0x0004EE7C
	private static void UpdateServerTime(Action callback)
	{
		if (!BlayFapClient.LoggedIn)
		{
			return;
		}
		if (callback != null)
		{
			List<Action> onTimeReturned = Utilities.OnTimeReturned;
			lock (onTimeReturned)
			{
				Utilities.OnTimeReturned.Add(callback);
			}
		}
		if (Utilities.timeRequested)
		{
			return;
		}
		Utilities.lastTimeCache = DateTime.UtcNow;
		if (BlayFapIntegration.IsTestDevice)
		{
			if (Utilities.DateOffset == -2147483648)
			{
				Utilities.DateOffset = global::PlayerPrefs.GetInt("GameStateDateOffset", 0);
			}
			Utilities.serverTime = new DateTime?(DateTime.UtcNow + TimeSpan.FromDays((double)Utilities.DateOffset));
			Utilities.HandleOnTimeReturnedCallbacks();
			if (BlayFapIntegration.IsTestDevice && GameState.CurrentState.transform.Find("Popups/Debug") != null)
			{
				GameState.CurrentState.transform.Find("Popups/Debug").GetComponent<DebugCanvasController>().UpdateLteData();
			}
			return;
		}
		Utilities.timeRequested = true;
		BlayFapClient.Instance.GetTime(delegate(GetTimeResponse result)
		{
			if (result.Error == null)
			{
				Utilities.serverTime = new DateTime?(result.Time);
			}
			else
			{
				Utilities.serverTime = new DateTime?(default(DateTime));
			}
			Utilities.timeRequested = false;
			if (GameState.CurrentState != null)
			{
				GameState.GetTaskSystem().CheckEvent(true);
			}
			Utilities.HandleOnTimeReturnedCallbacks();
		});
	}

	// Token: 0x060009BA RID: 2490 RVA: 0x00050DB0 File Offset: 0x0004EFB0
	private static void HandleOnTimeReturnedCallbacks()
	{
		Action[] array = null;
		List<Action> onTimeReturned = Utilities.OnTimeReturned;
		lock (onTimeReturned)
		{
			array = Utilities.OnTimeReturned.ToArray();
			Utilities.OnTimeReturned.Clear();
		}
		if (array != null)
		{
			foreach (Action action in array)
			{
				try
				{
					if (action != null)
					{
						action();
					}
				}
				catch (Exception innerException)
				{
					Debug.LogException(new Exception("An UpdateServerTime callback threw an exception.", innerException));
				}
			}
		}
	}

	// Token: 0x060009BB RID: 2491 RVA: 0x00050E70 File Offset: 0x0004F070
	public static void CheckConnection(Action connectedCallback = null, Action offlineCallback = null)
	{
		BlayFapClient.Instance.GetTime(delegate(GetTimeResponse result)
		{
			if (connectedCallback != null && result.Error == null)
			{
				connectedCallback();
			}
			else if (offlineCallback != null && result.Error != null)
			{
				offlineCallback();
			}
		});
	}

	// Token: 0x060009BC RID: 2492 RVA: 0x00050EA8 File Offset: 0x0004F0A8
	public static IEnumerator InstantiateGameObjectAsync(string assetBundleName, string assetName, Sprite[] array, int index)
	{
		AssetBundleAsync bundleRequest = GameState.AssetManager.GetBundleAsync(assetBundleName, false);
		yield return bundleRequest;
		if (bundleRequest.AssetBundle == null)
		{
			yield break;
		}
		Utilities.AsyncSpriteAssetRequest request = new Utilities.AsyncSpriteAssetRequest(bundleRequest.AssetBundle, assetName);
		yield return request.GetSpriteAsync();
		array[index] = request.Sprite;
		yield break;
	}

	// Token: 0x060009BD RID: 2493 RVA: 0x00050EF4 File Offset: 0x0004F0F4
	public static void SendGirlUnlockedEvent(Balance.GirlName girl)
	{
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x00050EF8 File Offset: 0x0004F0F8
	public static void SendGirlLoverEvent(Balance.GirlName girl)
	{
	}

	// Token: 0x060009BF RID: 2495 RVA: 0x00050EFC File Offset: 0x0004F0FC
	public static void SendPFUnlockedEvent(PhoneModel girl)
	{
	}

	// Token: 0x060009C0 RID: 2496 RVA: 0x00050F00 File Offset: 0x0004F100
	private static void SendAdjustToken(string adjustName, int count = 0)
	{
	}

	// Token: 0x060009C1 RID: 2497 RVA: 0x00050F04 File Offset: 0x0004F104
	public static void SendAnalytic(Utilities.AnalyticType type, string data)
	{
		if (GameObject.Find("Google") == null)
		{
			return;
		}
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x00050F1C File Offset: 0x0004F11C
	public static void SendConversion(string id, string data, double price)
	{
		if (GameObject.Find("Google") == null)
		{
			return;
		}
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x00050F34 File Offset: 0x0004F134
	public static string ToDescriptiveString(this Task task)
	{
		switch (task.TaskData.RewardType)
		{
		case TaskManager.TaskRewardType.Cash:
			return Translations.GetTranslation("everything_else_4_0", "Money");
		case TaskManager.TaskRewardType.Hobby:
			return Translations.GetTranslation("limited_time_events_18_1", "Minute\nHobby Skip").Replace("{0}", string.Empty).Trim();
		case TaskManager.TaskRewardType.Reset:
			return Translations.GetTranslation("everything_else_26_0", "Reset Boost");
		case TaskManager.TaskRewardType.Diamond:
			return (task.TaskData.RewardAmount <= 1) ? Translations.GetTranslation("limited_time_events_44_0", "Diamond") : Translations.GetTranslation("limited_time_events_44_1", "Diamonds");
		case TaskManager.TaskRewardType.Time:
			return Translations.GetTranslation("limited_time_events_41_0", "Minute\nTime Skip").Replace("{0}", string.Empty).Trim();
		case TaskManager.TaskRewardType.TimeBlock:
			return (task.TaskData.RewardAmount <= 1) ? Translations.GetTranslation("everything_else_33_1", "1 Time Block").Replace("1", string.Empty).Trim() : "Time Blocks";
		case TaskManager.TaskRewardType.EventToken:
			return Translations.GetTranslation("limited_time_events_3_0", "Event Token");
		case TaskManager.TaskRewardType.Job:
			return Translations.GetTranslation("limited_time_events_18_2", "Hour\nJob Skip").Replace("{0}", string.Empty).Trim();
		default:
			return "Unknown";
		}
	}

	// Token: 0x060009C4 RID: 2500 RVA: 0x00051090 File Offset: 0x0004F290
	public static string ToFriendlyString(this Girl.SpriteType type)
	{
		switch (type)
		{
		case Girl.SpriteType.Happy:
			return "Happy";
		case Girl.SpriteType.Harrassed:
			return "Harrassed";
		case Girl.SpriteType.LikesYou:
			return "LikesYou";
		case Girl.SpriteType.LikesYouLots:
			return "LikesYouLots";
		case Girl.SpriteType.Mad:
			return "Mad";
		case Girl.SpriteType.Neutral:
			return "Neutral";
		case Girl.SpriteType.Tickled:
			return "Tickled";
		case Girl.SpriteType.Scenario:
			return "Scenario";
		default:
			if (type != Girl.SpriteType.Sick)
			{
				return type.ToString();
			}
			return "Sick";
		}
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x00051114 File Offset: 0x0004F314
	public static string ToFriendlyString(this Utilities.AnalyticType type)
	{
		switch (type)
		{
		case Utilities.AnalyticType.Unlock:
			return "Unlock";
		case Utilities.AnalyticType.Girl:
			return "Girl";
		case Utilities.AnalyticType.Prestige:
			return "Prestige";
		case Utilities.AnalyticType.Reset:
			return "Reset";
		case Utilities.AnalyticType.Login:
			return "Login";
		case Utilities.AnalyticType.Gild:
			return "Gild";
		case Utilities.AnalyticType.Hobby:
			return "Hobby";
		case Utilities.AnalyticType.Conversion:
			return "Conversion";
		case Utilities.AnalyticType.Exception:
			return "Exception";
		case Utilities.AnalyticType.SummerItem:
			return "SummerItem";
		case Utilities.AnalyticType.Ayano2017:
			return "Ayano2017";
		}
		return type.ToString();
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x000511AC File Offset: 0x0004F3AC
	public static string ToLowerFriendlyString(this Balance.GirlName name)
	{
		return name.ToFriendlyString().ToLowerInvariant();
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x000511BC File Offset: 0x0004F3BC
	public static Balance.GirlName GirlFromString(string name)
	{
		return (Balance.GirlName)((int)Enum.Parse(typeof(Balance.GirlName), name, true));
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x000511D4 File Offset: 0x0004F3D4
	public static string ToFriendlyString(this Balance.GirlName name)
	{
		switch (name + 1)
		{
		case Balance.GirlName.Cassie:
			return "Unknown";
		case Balance.GirlName.Mio:
			return "Cassie";
		case Balance.GirlName.Quill:
			return "Mio";
		case Balance.GirlName.Elle:
			return "Quill";
		case Balance.GirlName.Nutaku:
			return "Elle";
		case Balance.GirlName.Iro:
			return "Nutaku";
		case Balance.GirlName.Bonnibel:
			return "Iro";
		case Balance.GirlName.Ayano:
			return "Bonnibel";
		case Balance.GirlName.Fumi:
			return "Ayano";
		case Balance.GirlName.Bearverly:
			return "Fumi";
		case Balance.GirlName.Nina:
			return "Bearverly";
		case Balance.GirlName.Alpha:
			return "Nina";
		case Balance.GirlName.Pamu:
			return "Alpha";
		case Balance.GirlName.Luna:
			return "Pamu";
		case Balance.GirlName.Eva:
			return "Luna";
		case Balance.GirlName.Karma:
			return "Eva";
		case Balance.GirlName.Sutra:
			return "Karma";
		case Balance.GirlName.DarkOne:
			return "Sutra";
		case Balance.GirlName.QPiddy:
			return "DarkOne";
		case Balance.GirlName.Darya:
			return "QPiddy";
		case Balance.GirlName.Jelle:
			return "Darya";
		case Balance.GirlName.Quillzone:
			return "Jelle";
		case Balance.GirlName.Bonchovy:
			return "Quillzone";
		case Balance.GirlName.Spectrum:
			return "Bonchovy";
		case Balance.GirlName.Charlotte:
			return "Spectrum";
		case Balance.GirlName.Odango:
			return "Charlotte";
		case Balance.GirlName.Shibuki:
			return "Odango";
		case Balance.GirlName.Sirina:
			return "Shibuki";
		case Balance.GirlName.Catara:
			return "Sirina";
		default:
			if (name != Balance.GirlName.ReneePF)
			{
				if (name != Balance.GirlName.NovaPF)
				{
					return name.ToString();
				}
				return "Nova";
			}
			break;
		case Balance.GirlName.Wendy:
			return "Juliet";
		case Balance.GirlName.Ruri:
			return "Wendy";
		case Balance.GirlName.Generica:
			return "Ruri";
		case Balance.GirlName.Suzu:
			return "Generica";
		case Balance.GirlName.Mallory:
			break;
		}
		return "Renee";
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x0005139C File Offset: 0x0004F59C
	public static string ToFriendlyString(this Girl.LoveLevel level)
	{
		switch (level)
		{
		case Girl.LoveLevel.Adversary:
			return Translations.GetTranslation("everything_else_4_0", "Adversary");
		case Girl.LoveLevel.Nuisance:
			return Translations.GetTranslation("everything_else_4_1", "Nuisance");
		case Girl.LoveLevel.Frenemy:
			return Translations.GetTranslation("everything_else_4_2", "Frenemy");
		case Girl.LoveLevel.Acquaintance:
			return Translations.GetTranslation("everything_else_4_3", "Acquaintance");
		case Girl.LoveLevel.Friendzoned:
			return Translations.GetTranslation("everything_else_4_4", "Friendzoned");
		case Girl.LoveLevel.Awkward_Besties:
			return Translations.GetTranslation("everything_else_4_5", "Awkward Besties");
		case Girl.LoveLevel.Crush:
			return Translations.GetTranslation("everything_else_4_6", "Crush");
		case Girl.LoveLevel.Sweetheart:
			return Translations.GetTranslation("everything_else_4_7", "Sweetheart");
		case Girl.LoveLevel.Girlfriend:
			return Translations.GetTranslation("everything_else_4_8", "Girlfriend");
		case Girl.LoveLevel.Lover:
			return Translations.GetTranslation("everything_else_4_9", "Lover");
		default:
			return level.ToString();
		}
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x0005148C File Offset: 0x0004F68C
	public static string ToFriendlyString(this Requirement.OutfitType outfit)
	{
		if (outfit == Requirement.OutfitType.Christmas)
		{
			return "Christmas";
		}
		if (outfit == Requirement.OutfitType.SchoolUniform)
		{
			return "SchoolUniform";
		}
		if (outfit == Requirement.OutfitType.BathingSuit)
		{
			return "BathingSuit";
		}
		if (outfit == Requirement.OutfitType.Unique)
		{
			return "Unique";
		}
		if (outfit == Requirement.OutfitType.DiamondRing)
		{
			return "DiamondRing";
		}
		if (outfit == Requirement.OutfitType.Lingerie)
		{
			return "Lingerie";
		}
		if (outfit != Requirement.OutfitType.Nude)
		{
			return outfit.ToString();
		}
		return "Nude";
	}

	// Token: 0x060009CB RID: 2507 RVA: 0x00051524 File Offset: 0x0004F724
	public static string ToFriendlyString(this Requirement.GiftType gift)
	{
		switch (gift)
		{
		case Requirement.GiftType.Shell:
			return "Shell";
		case Requirement.GiftType.Rose:
			return "Rose";
		default:
			if (gift == Requirement.GiftType.FruitBasket)
			{
				return "FruitBasket";
			}
			if (gift == Requirement.GiftType.Chocolates)
			{
				return "Chocolates";
			}
			if (gift == Requirement.GiftType.Book)
			{
				return "Book";
			}
			if (gift == Requirement.GiftType.Earrings)
			{
				return "Earrings";
			}
			if (gift == Requirement.GiftType.Drink)
			{
				return "Drink";
			}
			if (gift == Requirement.GiftType.Flowers)
			{
				return "Flowers";
			}
			if (gift == Requirement.GiftType.Cake)
			{
				return "Cake";
			}
			if (gift == Requirement.GiftType.PlushyToy)
			{
				return "PlushyToy";
			}
			if (gift == Requirement.GiftType.TeaSet)
			{
				return "TeaSet";
			}
			if (gift == Requirement.GiftType.Shoes)
			{
				return "Shoes";
			}
			if (gift == Requirement.GiftType.CutePuppy)
			{
				return "CutePuppy";
			}
			if (gift == Requirement.GiftType.Necklace)
			{
				return "Necklace";
			}
			if (gift == Requirement.GiftType.DesignerBag)
			{
				return "DesignerBag";
			}
			if (gift == Requirement.GiftType.NewCar)
			{
				return "NewCar";
			}
			if (gift == Requirement.GiftType.Christmas)
			{
				return "Christmas";
			}
			if (gift == Requirement.GiftType.SchoolUniform)
			{
				return "SchoolUniform";
			}
			if (gift == Requirement.GiftType.BathingSuit)
			{
				return "BathingSuit";
			}
			if (gift == Requirement.GiftType.Unique)
			{
				return "Unique";
			}
			if (gift == Requirement.GiftType.DiamondRing)
			{
				return "DiamondRing";
			}
			if (gift == Requirement.GiftType.USB)
			{
				return "USB";
			}
			if (gift == Requirement.GiftType.Potion)
			{
				return "Potion";
			}
			if (gift == Requirement.GiftType.MagicCandles)
			{
				return "MagicCandles";
			}
			if (gift == Requirement.GiftType.EnchantedScarf)
			{
				return "EnchantedScarf";
			}
			if (gift == Requirement.GiftType.BewitchedJam)
			{
				return "BewitchedJam";
			}
			if (gift == Requirement.GiftType.MysticSlippers)
			{
				return "MysticSlippers";
			}
			if (gift == Requirement.GiftType.Lingerie)
			{
				return "Lingerie";
			}
			if (gift != Requirement.GiftType.Nude)
			{
				return gift.ToString();
			}
			return "Nude";
		case Requirement.GiftType.HandLotion:
			return "HandLotion";
		case Requirement.GiftType.Donut:
			return "Donut";
		}
	}

	// Token: 0x060009CC RID: 2508 RVA: 0x00051748 File Offset: 0x0004F948
	public static string ToFriendlyString(this Requirement.DateType date)
	{
		switch (date)
		{
		case Requirement.DateType.MoonlightStroll:
			return "MoonlightStroll";
		case Requirement.DateType.CoffeeShop:
			return "CoffeeShop";
		default:
			if (date != Requirement.DateType.Beach)
			{
				return date.ToString();
			}
			return "Beach";
		case Requirement.DateType.Sightseeing:
			return "Sightseeing";
		case Requirement.DateType.MovieTheater:
			return "MovieTheater";
		}
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x000517B8 File Offset: 0x0004F9B8
	public static string ToFriendlyString(this Requirement.Skill skill)
	{
		switch (skill)
		{
		case Requirement.Skill.Suave:
			return "Suave";
		case Requirement.Skill.Funny:
			return "Funny";
		case Requirement.Skill.Buff:
			return "Buff";
		case Requirement.Skill.TechSavvy:
			return "TechSavvy";
		case Requirement.Skill.Tenderness:
			return "Tenderness";
		case Requirement.Skill.Motivation:
			return "Motivation";
		case Requirement.Skill.Wisdom:
			return "Wisdom";
		case Requirement.Skill.Badass:
			return "Badass";
		case Requirement.Skill.Smart:
			return "Smart";
		case Requirement.Skill.Angst:
			return "Angst";
		case Requirement.Skill.Mysterious:
			return "Mysterious";
		case Requirement.Skill.Lucky:
			return "Lucky";
		default:
			return skill.ToString();
		}
	}

	// Token: 0x060009CE RID: 2510 RVA: 0x00051858 File Offset: 0x0004FA58
	public static string ToSaveName(this Requirement.JobType job)
	{
		switch (job)
		{
		case Requirement.JobType.Burger:
			return "FAST FOOD";
		case Requirement.JobType.Restaurant:
			return "RESTAURANT";
		default:
			if (job == Requirement.JobType.Art)
			{
				return "ART";
			}
			if (job == Requirement.JobType.Computers)
			{
				return "COMPUTERS";
			}
			if (job == Requirement.JobType.Zoo)
			{
				return "ZOO";
			}
			if (job == Requirement.JobType.Hunting)
			{
				return "HUNTING";
			}
			if (job == Requirement.JobType.Casino)
			{
				return "CASINO";
			}
			if (job == Requirement.JobType.Sports)
			{
				return "SPORTS";
			}
			if (job == Requirement.JobType.Legal)
			{
				return "LEGAL";
			}
			if (job == Requirement.JobType.Movies)
			{
				return "MOVIES";
			}
			if (job == Requirement.JobType.Space)
			{
				return "SPACE";
			}
			if (job == Requirement.JobType.Slaying)
			{
				return "SLAYING";
			}
			if (job == Requirement.JobType.Love)
			{
				return "LOVE";
			}
			if (job == Requirement.JobType.Wizard)
			{
				return "WIZARD";
			}
			if (job == Requirement.JobType.Digger)
			{
				return "DIGGER";
			}
			if (job != Requirement.JobType.Planter)
			{
				return "UNKNOWN";
			}
			return "PLANTER";
		case Requirement.JobType.Cleaning:
			return "CLEANING";
		case Requirement.JobType.Lifeguard:
			return "LIFEGUARD";
		}
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x00051998 File Offset: 0x0004FB98
	public static string ToFriendlyString(this Requirement.JobType job)
	{
		switch (job)
		{
		case Requirement.JobType.None:
			return "None";
		case Requirement.JobType.Burger:
			return "Burger";
		case Requirement.JobType.Restaurant:
			return "Restaurant";
		default:
			if (job == Requirement.JobType.Art)
			{
				return "Art";
			}
			if (job == Requirement.JobType.Computers)
			{
				return "Computers";
			}
			if (job == Requirement.JobType.Zoo)
			{
				return "Zoo";
			}
			if (job == Requirement.JobType.Hunting)
			{
				return "Hunting";
			}
			if (job == Requirement.JobType.Casino)
			{
				return "Casino";
			}
			if (job == Requirement.JobType.Sports)
			{
				return "Sports";
			}
			if (job == Requirement.JobType.Legal)
			{
				return "Legal";
			}
			if (job == Requirement.JobType.Movies)
			{
				return "Movies";
			}
			if (job == Requirement.JobType.Space)
			{
				return "Space";
			}
			if (job == Requirement.JobType.Slaying)
			{
				return "Slaying";
			}
			if (job == Requirement.JobType.Love)
			{
				return "Love";
			}
			if (job == Requirement.JobType.Wizard)
			{
				return "Wizard";
			}
			if (job == Requirement.JobType.Digger)
			{
				return "Grave Digger";
			}
			if (job != Requirement.JobType.Planter)
			{
				return job.ToString();
			}
			return "Tree Planter";
		case Requirement.JobType.Cleaning:
			return "Cleaning";
		case Requirement.JobType.Lifeguard:
			return "Lifeguard";
		}
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x00051AE4 File Offset: 0x0004FCE4
	public static bool AwardDiamonds(int amount)
	{
		bool result;
		try
		{
			GameState.Diamonds.Value += amount;
			GameState.CurrentState.QueueQuickSave();
			result = true;
		}
		catch (Exception)
		{
			result = false;
		}
		return result;
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x00051B44 File Offset: 0x0004FD44
	public static void UpdateLoginLoadSaveButtons()
	{
		Transform transform = GameObject.Find("Canvas").transform;
		transform.Find("More/Buttons/Login Button").gameObject.SetActive(false);
		transform.Find("More/Buttons/Dump Save").gameObject.SetActive(!BlayFapClient.LoggedIn);
		transform.Find("More/Buttons/Save Button").gameObject.SetActive(BlayFapClient.LoggedIn);
		transform.Find("More/Buttons/Load Button").gameObject.SetActive(BlayFapClient.LoggedIn);
	}

	// Token: 0x060009D2 RID: 2514 RVA: 0x00051BC8 File Offset: 0x0004FDC8
	public static void PurchaseDiamonds(int count)
	{
		if (Utilities.diamondPopup == null)
		{
			Utilities.diamondPopup = GameObject.Find("Canvas").transform.Find("Popups/Diamonds Popup");
		}
		Utilities.diamondPopup.Find("Dialog/Top Text").GetComponent<Text>().text = string.Format(Translations.GetTranslation("everything_else_30_0", "You need {0} diamonds to purchase that!"), count.ToString());
		Utilities.diamondPopup.gameObject.SetActive(true);
		GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.NotEnoughTime);
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x00051C58 File Offset: 0x0004FE58
	public static void ConfirmPurchase(int diamonds, Action OnSuccess)
	{
		BlayFapIntegration.VerifyIsCurrentSession(delegate
		{
			Transform transform = GameState.CurrentState.transform;
			Transform confirmPurchase = transform.Find("Popups/Confirm Purchase");
			if (diamonds < 0)
			{
				return;
			}
			if (GameState.Diamonds.Value < diamonds)
			{
				Utilities.PurchaseDiamonds(diamonds);
			}
			else
			{
				confirmPurchase.transform.Find("Dialog/Pay Button/Text").GetComponent<Text>().text = diamonds.ToString();
				confirmPurchase.transform.Find("Dialog/Pay Button").GetComponent<Button>().onClick.RemoveAllListeners();
				confirmPurchase.transform.Find("Dialog/Pay Button").GetComponent<Button>().onClick.AddListener(delegate()
				{
					confirmPurchase.gameObject.SetActive(false);
					if (GameState.Diamonds.Value >= diamonds)
					{
						GameState.Diamonds.Value -= diamonds;
						OnSuccess();
					}
					GameState.CurrentState.QueueQuickSave();
					global::PlayerPrefs.Save();
				});
				confirmPurchase.gameObject.SetActive(true);
			}
		});
	}

	// Token: 0x060009D4 RID: 2516 RVA: 0x00051C8C File Offset: 0x0004FE8C
	public static void PurchaseTimeBlocks(int requiredTimeBlocks)
	{
		if (Utilities.timePurchase == null)
		{
			Utilities.timePurchase = GameObject.Find("Canvas").transform.Find("Popups/No Time Popup").GetComponent<TimePurchase>();
		}
		Utilities.timePurchase.Init(requiredTimeBlocks);
		GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.NotEnoughTime);
	}

	// Token: 0x060009D5 RID: 2517 RVA: 0x00051CE8 File Offset: 0x0004FEE8
	public static int CountFromFlag(int flag)
	{
		int num = 0;
		for (int i = 0; i < 32; i++)
		{
			if ((flag >> i & 1) != 0)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x00051D1C File Offset: 0x0004FF1C
	public static Color HSV2RGB(float h, float s, float v)
	{
		float num;
		for (num = h * 360f; num < 0f; num += 360f)
		{
		}
		while (num >= 360f)
		{
			num -= 360f;
		}
		float b;
		float r;
		float g;
		if (v <= 0f)
		{
			g = (r = (b = 0f));
		}
		else if (s <= 0f)
		{
			b = v;
			g = v;
			r = v;
		}
		else
		{
			float num2 = num / 60f;
			int num3 = (int)Math.Floor((double)num2);
			float num4 = num2 - (float)num3;
			float num5 = v * (1f - s);
			float num6 = v * (1f - s * num4);
			float num7 = v * (1f - s * (1f - num4));
			int num8 = num3;
			switch (num8 + 1)
			{
			case 0:
				r = v;
				g = num5;
				b = num6;
				break;
			case 1:
				r = v;
				g = num7;
				b = num5;
				break;
			case 2:
				r = num6;
				g = v;
				b = num5;
				break;
			case 3:
				r = num5;
				g = v;
				b = num7;
				break;
			case 4:
				r = num5;
				g = num6;
				b = v;
				break;
			case 5:
				r = num7;
				g = num5;
				b = v;
				break;
			case 6:
				r = v;
				g = num5;
				b = num6;
				break;
			case 7:
				r = v;
				g = num7;
				b = num5;
				break;
			default:
				b = v;
				g = v;
				r = v;
				break;
			}
		}
		return new Color(r, g, b);
	}

	// Token: 0x060009D7 RID: 2519 RVA: 0x00051E88 File Offset: 0x00050088
	public static Color HSL2RGB(float h, float sl, float l)
	{
		float r = l;
		float g = l;
		float b = l;
		float num = (l > 0.5f) ? (l + sl - l * sl) : (l * (1f + sl));
		if (num > 0f)
		{
			float num2 = l + l - num;
			float num3 = (num - num2) / num;
			h *= 6f;
			int num4 = (int)h;
			float num5 = h - (float)num4;
			float num6 = num * num3 * num5;
			float num7 = num2 + num6;
			float num8 = num - num6;
			switch (num4)
			{
			case 0:
				r = num;
				g = num7;
				b = num2;
				break;
			case 1:
				r = num8;
				g = num;
				b = num2;
				break;
			case 2:
				r = num2;
				g = num;
				b = num7;
				break;
			case 3:
				r = num2;
				g = num8;
				b = num;
				break;
			case 4:
				r = num7;
				g = num2;
				b = num;
				break;
			case 5:
				r = num;
				g = num2;
				b = num8;
				break;
			}
		}
		return new Color(r, g, b);
	}

	// Token: 0x060009D8 RID: 2520 RVA: 0x00051F80 File Offset: 0x00050180
	public static string CreateCountdown(TimeSpan end)
	{
		int num = 10 + ((end.Days <= 9) ? 0 : 1);
		if (Utilities.countdown == null || Utilities.countdown.Length != num)
		{
			Utilities.countdown = new char[num];
		}
		int num2 = 0;
		if (end.Days >= 10)
		{
			Utilities.countdown[num2++] = (char)(48 + end.Days / 10);
		}
		Utilities.countdown[num2++] = (char)(48 + end.Days % 10);
		Utilities.countdown[num2++] = ':';
		if (end.Hours >= 10)
		{
			Utilities.countdown[num2++] = (char)(48 + end.Hours / 10);
		}
		else
		{
			Utilities.countdown[num2++] = '0';
		}
		Utilities.countdown[num2++] = (char)(48 + end.Hours % 10);
		Utilities.countdown[num2++] = ':';
		if (end.Minutes >= 10)
		{
			Utilities.countdown[num2++] = (char)(48 + end.Minutes / 10);
		}
		else
		{
			Utilities.countdown[num2++] = '0';
		}
		Utilities.countdown[num2++] = (char)(48 + end.Minutes % 10);
		Utilities.countdown[num2++] = ':';
		if (end.Seconds >= 10)
		{
			Utilities.countdown[num2++] = (char)(48 + end.Seconds / 10);
		}
		else
		{
			Utilities.countdown[num2++] = '0';
		}
		Utilities.countdown[num2++] = (char)(48 + end.Seconds % 10);
		return new string(Utilities.countdown);
	}

	// Token: 0x060009D9 RID: 2521 RVA: 0x00052134 File Offset: 0x00050334
	public static string CreateCountdownDay(int seconds)
	{
		int num = 8;
		if (Utilities.countdown == null || Utilities.countdown.Length != num)
		{
			Utilities.countdown = new char[num];
		}
		int num2 = 0;
		int num3 = seconds / 3600;
		seconds -= num3 * 60 * 60;
		int num4 = seconds / 60;
		seconds -= num4 * 60;
		if (num3 >= 10)
		{
			Utilities.countdown[num2++] = (char)(48 + num3 / 10);
		}
		else
		{
			Utilities.countdown[num2++] = '0';
		}
		Utilities.countdown[num2++] = (char)(48 + num3 % 10);
		Utilities.countdown[num2++] = ':';
		if (num4 >= 10)
		{
			Utilities.countdown[num2++] = (char)(48 + num4 / 10);
		}
		else
		{
			Utilities.countdown[num2++] = '0';
		}
		Utilities.countdown[num2++] = (char)(48 + num4 % 10);
		Utilities.countdown[num2++] = ':';
		if (seconds >= 10)
		{
			Utilities.countdown[num2++] = (char)(48 + seconds / 10);
		}
		else
		{
			Utilities.countdown[num2++] = '0';
		}
		Utilities.countdown[num2++] = (char)(48 + seconds % 10);
		return new string(Utilities.countdown);
	}

	// Token: 0x060009DA RID: 2522 RVA: 0x0005226C File Offset: 0x0005046C
	public static string CreateCountdownDay(TimeSpan end)
	{
		int num = 8;
		if (Utilities.countdown == null || Utilities.countdown.Length != num)
		{
			Utilities.countdown = new char[num];
		}
		int num2 = 0;
		if (end.Hours >= 10)
		{
			Utilities.countdown[num2++] = (char)(48 + end.Hours / 10);
		}
		else
		{
			Utilities.countdown[num2++] = '0';
		}
		Utilities.countdown[num2++] = (char)(48 + end.Hours % 10);
		Utilities.countdown[num2++] = ':';
		if (end.Minutes >= 10)
		{
			Utilities.countdown[num2++] = (char)(48 + end.Minutes / 10);
		}
		else
		{
			Utilities.countdown[num2++] = '0';
		}
		Utilities.countdown[num2++] = (char)(48 + end.Minutes % 10);
		Utilities.countdown[num2++] = ':';
		if (end.Seconds >= 10)
		{
			Utilities.countdown[num2++] = (char)(48 + end.Seconds / 10);
		}
		else
		{
			Utilities.countdown[num2++] = '0';
		}
		Utilities.countdown[num2++] = (char)(48 + end.Seconds % 10);
		return new string(Utilities.countdown);
	}

	// Token: 0x060009DB RID: 2523 RVA: 0x000523BC File Offset: 0x000505BC
	public static string ToPrefixedNumber(double number, bool shortHand = false, bool addShortSpace = false)
	{
		bool flag = number < 0.0;
		number = Math.Abs(number);
		if (double.IsInfinity(number) || double.IsNaN(number))
		{
			number = 0.0;
		}
		string text;
		if (number < 1000.0)
		{
			text = ((int)number).ToString();
		}
		else
		{
			int num = (int)Math.Floor(Math.Log10(number));
			int num2 = Math.Max(0, num / 3 - 1);
			float num3 = (float)(number / Math.Pow(10.0, (double)((num2 + 1) * 3)));
			string str = (!shortHand || num3 < 100f) ? num3.ToStringTruncated(2) : num3.ToStringTruncated(1);
			num2 = Math.Min(Utilities.prefixesShort.Length - 1, num2);
			if (shortHand && addShortSpace)
			{
				text = str + " " + Utilities.prefixesShort[num2];
			}
			else if (shortHand)
			{
				text = str + Utilities.prefixesShort[num2];
			}
			else
			{
				text = str + " " + Utilities.prefixes[num2];
			}
		}
		if (flag)
		{
			text = "-" + text;
		}
		return text;
	}

	// Token: 0x060009DC RID: 2524 RVA: 0x000524F0 File Offset: 0x000506F0
	public static string ToStringTruncated(this float number, int decimalPlaces)
	{
		if (decimalPlaces < 0 || decimalPlaces > 6)
		{
			throw new ArgumentOutOfRangeException("decimalPlaces");
		}
		int num = (int)Math.Round((double)(number * Utilities.pow10[decimalPlaces]));
		int num2 = (int)Math.Floor(Math.Log10((double)num)) + 1 - decimalPlaces;
		num2 = Math.Max(1, num2);
		int num3 = num2 + decimalPlaces + 1;
		char[] array;
		try
		{
			if (num3 < Utilities.cachedArrays.Length + 1)
			{
				if (Utilities.cachedArrays[num3 - 1] == null)
				{
					Utilities.cachedArrays[num3 - 1] = new char[num3];
				}
				array = Utilities.cachedArrays[num3 - 1];
			}
			else
			{
				array = new char[num3];
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			array = new char[num3];
		}
		for (int i = num3 - 1; i >= 0; i--)
		{
			if (i == num2)
			{
				array[i] = '.';
			}
			else
			{
				array[i] = (char)(48 + num % 10);
				num /= 10;
			}
		}
		return new string(array);
	}

	// Token: 0x060009DD RID: 2525 RVA: 0x00052604 File Offset: 0x00050804
	public static Transform GetTransformFromPath(Transform rootTransform, string path)
	{
		string[] array = path.Split(new char[]
		{
			'/'
		});
		Transform transform = rootTransform;
		for (int i = 0; i < array.Length; i++)
		{
			transform = Utilities.GetChildWithName(transform, array[i]);
			if (transform == null)
			{
				Debug.LogError("No transform found for: " + path + "  at: " + array[i]);
				break;
			}
		}
		return transform;
	}

	// Token: 0x060009DE RID: 2526 RVA: 0x00052670 File Offset: 0x00050870
	public static Transform GetChildWithName(Transform rootTransform, string transformName)
	{
		for (int i = 0; i < rootTransform.childCount; i++)
		{
			string name = rootTransform.GetChild(i).name;
			if (name == transformName)
			{
				return rootTransform.GetChild(i);
			}
		}
		return null;
	}

	// Token: 0x060009DF RID: 2527 RVA: 0x000526B8 File Offset: 0x000508B8
	public static void OpenPrivacyPolicy()
	{
		Application.OpenURL("http://www.sadpandastudios.com/privacy.html");
	}

	// Token: 0x060009E0 RID: 2528 RVA: 0x000526C4 File Offset: 0x000508C4
	public static void OpenTermsOfUse()
	{
		Application.OpenURL("http://www.sadpandastudios.com/termsofuse.html");
	}

	// Token: 0x060009E1 RID: 2529 RVA: 0x000526D0 File Offset: 0x000508D0
	public static void OpenPrivacyPolicyPartners()
	{
		Application.OpenURL("https://sadpandastudios.com/privacy.html#third-party");
	}

	// Token: 0x060009E2 RID: 2530 RVA: 0x000526DC File Offset: 0x000508DC
	public static void Shuffle<T>(this T[] array)
	{
		for (int i = array.Length - 1; i > 0; i--)
		{
			int num = UnityEngine.Random.Range(0, i);
			T t = array[i];
			array[i] = array[num];
			array[num] = t;
		}
	}

	// Token: 0x060009E3 RID: 2531 RVA: 0x00052728 File Offset: 0x00050928
	public static bool IsValidEmail(string email)
	{
		return !string.IsNullOrEmpty(email) && email.Length >= 3 && email.Contains("@");
	}

	// Token: 0x0400099F RID: 2463
	private static DateTime lastTimeCache;

	// Token: 0x040009A0 RID: 2464
	private static DateTime? serverTime = null;

	// Token: 0x040009A1 RID: 2465
	private static bool timeRequested = false;

	// Token: 0x040009A2 RID: 2466
	public static int DateOffset = int.MinValue;

	// Token: 0x040009A3 RID: 2467
	private static List<Action> OnTimeReturned = new List<Action>();

	// Token: 0x040009A4 RID: 2468
	private static Transform diamondPopup;

	// Token: 0x040009A5 RID: 2469
	private static TimePurchase timePurchase;

	// Token: 0x040009A6 RID: 2470
	private static char[] countdown;

	// Token: 0x040009A7 RID: 2471
	private static string[] prefixes = new string[]
	{
		"Thousand",
		"Million",
		"Billion",
		"Trillion",
		"Quadrillion",
		"Quintillion",
		"Sextillion",
		"Septillion",
		"Octillion",
		"Nonillion",
		"Decillion",
		"Undecillion",
		"Duodecillion",
		"Tredecillion",
		"Quattuordecillion",
		"Quinquadecillion",
		"Sedecillion",
		"Septendecillion",
		"Octodecillion",
		"Novendecillion",
		"Vigintillion",
		"Unvigintillion",
		"Duovigintillion",
		"Tresvigintillion",
		"Quattuorvigintillion",
		"Quinquavigintillion",
		"Sesvigintillion",
		"Septemvigintillion",
		"Octovigintillion",
		"Novemvigintillion",
		"Trigintillion",
		"Untrigintillion",
		"Duotrigintillion",
		"Trestrigintillion",
		"Quattuortrigintillion",
		"Quinquatrigintillion",
		"Sestrigintillion",
		"Septentrigintillion",
		"Octotrigintillion",
		"Noventrigintillion",
		"Quadragintillion"
	};

	// Token: 0x040009A8 RID: 2472
	private static string[] prefixesShort = new string[]
	{
		"k",
		"M",
		"B",
		"T",
		"Q",
		"Qu",
		"S",
		"Se",
		"Oc",
		"No",
		"De",
		"Un",
		"Du",
		"Tr",
		"Qua",
		"Qui",
		"Sed",
		"Sep",
		"Oct",
		"Nov",
		"Vig",
		"Unv",
		"Duo",
		"Tre",
		"Quat",
		"Quin",
		"Ses",
		"Sept",
		"Octov",
		"Novemv",
		"Tri",
		"Unt",
		"Duot",
		"Tres",
		"Quatt",
		"Quinq",
		"Sestrig",
		"Septent",
		"Octotri",
		"Noventri",
		"Quadrag"
	};

	// Token: 0x040009A9 RID: 2473
	private static float[] pow10 = new float[]
	{
		1f,
		10f,
		100f,
		1000f,
		10000f,
		100000f,
		1000000f
	};

	// Token: 0x040009AA RID: 2474
	private static char[][] cachedArrays = new char[10][];

	// Token: 0x0200015B RID: 347
	public class AsyncSpriteAssetRequest
	{
		// Token: 0x060009E5 RID: 2533 RVA: 0x000527B8 File Offset: 0x000509B8
		public AsyncSpriteAssetRequest(AssetBundle bundle, string assetName)
		{
			this.bundle = bundle;
			this.assetName = assetName;
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x000527D0 File Offset: 0x000509D0
		public IEnumerator GetSpriteAsync()
		{
			if (this.bundle == null)
			{
				yield break;
			}
			AssetBundleRequest request = this.bundle.LoadAssetAsync<Sprite>(this.assetName);
			yield return request;
			if (request.asset != null)
			{
				this.Sprite = (request.asset as Sprite);
			}
			yield break;
		}

		// Token: 0x040009AC RID: 2476
		private AssetBundle bundle;

		// Token: 0x040009AD RID: 2477
		private string assetName;

		// Token: 0x040009AE RID: 2478
		public Sprite Sprite;
	}

	// Token: 0x0200015C RID: 348
	public enum AnalyticType
	{
		// Token: 0x040009B0 RID: 2480
		Unlock,
		// Token: 0x040009B1 RID: 2481
		Girl,
		// Token: 0x040009B2 RID: 2482
		Prestige,
		// Token: 0x040009B3 RID: 2483
		Reset,
		// Token: 0x040009B4 RID: 2484
		Login,
		// Token: 0x040009B5 RID: 2485
		Gild,
		// Token: 0x040009B6 RID: 2486
		Hobby,
		// Token: 0x040009B7 RID: 2487
		Conversion,
		// Token: 0x040009B8 RID: 2488
		Exception,
		// Token: 0x040009B9 RID: 2489
		SummerItem,
		// Token: 0x040009BA RID: 2490
		July2017Item,
		// Token: 0x040009BB RID: 2491
		Ayano2017,
		// Token: 0x040009BC RID: 2492
		Winter2018Item,
		// Token: 0x040009BD RID: 2493
		Nutaku2019Item
	}

	// Token: 0x0200015D RID: 349
	public static class CLZF2
	{
		// Token: 0x060009E8 RID: 2536 RVA: 0x00052840 File Offset: 0x00050A40
		public static byte[] Compress(byte[] inputBytes)
		{
			object obj = Utilities.CLZF2.lockObject;
			byte[] result;
			lock (obj)
			{
				int num = inputBytes.Length * 2;
				byte[] src = new byte[num];
				int num2;
				for (num2 = Utilities.CLZF2.lzf_compress(inputBytes, ref src); num2 == 0; num2 = Utilities.CLZF2.lzf_compress(inputBytes, ref src))
				{
					num *= 2;
					src = new byte[num];
				}
				byte[] array = new byte[num2];
				Buffer.BlockCopy(src, 0, array, 0, num2);
				result = array;
			}
			return result;
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x000528D4 File Offset: 0x00050AD4
		public static byte[] Decompress(byte[] inputBytes)
		{
			object obj = Utilities.CLZF2.lockObject;
			byte[] result;
			lock (obj)
			{
				int num = inputBytes.Length * 2;
				byte[] src = new byte[num];
				int num2;
				for (num2 = Utilities.CLZF2.lzf_decompress(inputBytes, ref src); num2 == 0; num2 = Utilities.CLZF2.lzf_decompress(inputBytes, ref src))
				{
					num *= 2;
					src = new byte[num];
				}
				byte[] array = new byte[num2];
				Buffer.BlockCopy(src, 0, array, 0, num2);
				result = array;
			}
			return result;
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x00052968 File Offset: 0x00050B68
		public static int lzf_compress(byte[] input, ref byte[] output)
		{
			int num = input.Length;
			int num2 = output.Length;
			Array.Clear(Utilities.CLZF2.HashTable, 0, (int)Utilities.CLZF2.HSIZE);
			uint num3 = 0U;
			uint num4 = 0U;
			uint num5 = (uint)((int)input[(int)((UIntPtr)num3)] << 8 | (int)input[(int)((UIntPtr)(num3 + 1U))]);
			int num6 = 0;
			for (;;)
			{
				if ((ulong)num3 < (ulong)((long)(num - 2)))
				{
					num5 = (num5 << 8 | (uint)input[(int)((UIntPtr)(num3 + 2U))]);
					long num7 = (long)((ulong)((num5 ^ num5 << 5) >> (int)(24U - Utilities.CLZF2.HLOG - num5 * 5U) & Utilities.CLZF2.HSIZE - 1U));
					long num8 = Utilities.CLZF2.HashTable[(int)(checked((IntPtr)num7))];
					Utilities.CLZF2.HashTable[(int)(checked((IntPtr)num7))] = (long)((ulong)num3);
					long num9;
					if ((num9 = (long)((ulong)num3 - (ulong)num8 - 1UL)) < (long)((ulong)Utilities.CLZF2.MAX_OFF) && (ulong)(num3 + 4U) < (ulong)((long)num) && num8 > 0L && input[(int)(checked((IntPtr)num8))] == input[(int)((UIntPtr)num3)] && input[(int)(checked((IntPtr)(unchecked(num8 + 1L))))] == input[(int)((UIntPtr)(num3 + 1U))] && input[(int)(checked((IntPtr)(unchecked(num8 + 2L))))] == input[(int)((UIntPtr)(num3 + 2U))])
					{
						uint num10 = 2U;
						uint num11 = (uint)(num - (int)num3 - (int)num10);
						num11 = ((num11 <= Utilities.CLZF2.MAX_REF) ? num11 : Utilities.CLZF2.MAX_REF);
						if ((ulong)num4 + (ulong)((long)num6) + 1UL + 3UL >= (ulong)((long)num2))
						{
							break;
						}
						do
						{
							num10 += 1U;
						}
						while (num10 < num11 && input[(int)(checked((IntPtr)(unchecked(num8 + (long)((ulong)num10)))))] == input[(int)((UIntPtr)(num3 + num10))]);
						if (num6 != 0)
						{
							output[(int)((UIntPtr)(num4++))] = (byte)(num6 - 1);
							num6 = -num6;
							do
							{
								output[(int)((UIntPtr)(num4++))] = input[(int)(checked((IntPtr)(unchecked((ulong)num3 + (ulong)((long)num6)))))];
							}
							while (++num6 != 0);
						}
						num10 -= 2U;
						num3 += 1U;
						if (num10 < 7U)
						{
							output[(int)((UIntPtr)(num4++))] = (byte)((num9 >> 8) + (long)((ulong)((ulong)num10 << 5)));
						}
						else
						{
							output[(int)((UIntPtr)(num4++))] = (byte)((num9 >> 8) + 224L);
							output[(int)((UIntPtr)(num4++))] = (byte)(num10 - 7U);
						}
						output[(int)((UIntPtr)(num4++))] = (byte)num9;
						num3 += num10 - 1U;
						num5 = (uint)((int)input[(int)((UIntPtr)num3)] << 8 | (int)input[(int)((UIntPtr)(num3 + 1U))]);
						num5 = (num5 << 8 | (uint)input[(int)((UIntPtr)(num3 + 2U))]);
						Utilities.CLZF2.HashTable[(int)((UIntPtr)((num5 ^ num5 << 5) >> (int)(24U - Utilities.CLZF2.HLOG - num5 * 5U) & Utilities.CLZF2.HSIZE - 1U))] = (long)((ulong)num3);
						num3 += 1U;
						num5 = (num5 << 8 | (uint)input[(int)((UIntPtr)(num3 + 2U))]);
						Utilities.CLZF2.HashTable[(int)((UIntPtr)((num5 ^ num5 << 5) >> (int)(24U - Utilities.CLZF2.HLOG - num5 * 5U) & Utilities.CLZF2.HSIZE - 1U))] = (long)((ulong)num3);
						num3 += 1U;
						continue;
					}
				}
				else if ((ulong)num3 == (ulong)((long)num))
				{
					goto Block_13;
				}
				num6++;
				num3 += 1U;
				if ((long)num6 == (long)((ulong)Utilities.CLZF2.MAX_LIT))
				{
					if ((ulong)(num4 + 1U + Utilities.CLZF2.MAX_LIT) >= (ulong)((long)num2))
					{
						return 0;
					}
					output[(int)((UIntPtr)(num4++))] = (byte)(Utilities.CLZF2.MAX_LIT - 1U);
					num6 = -num6;
					do
					{
						output[(int)((UIntPtr)(num4++))] = input[(int)(checked((IntPtr)(unchecked((ulong)num3 + (ulong)((long)num6)))))];
					}
					while (++num6 != 0);
				}
			}
			return 0;
			Block_13:
			if (num6 != 0)
			{
				if ((ulong)num4 + (ulong)((long)num6) + 1UL >= (ulong)((long)num2))
				{
					return 0;
				}
				output[(int)((UIntPtr)(num4++))] = (byte)(num6 - 1);
				num6 = -num6;
				do
				{
					output[(int)((UIntPtr)(num4++))] = input[(int)(checked((IntPtr)(unchecked((ulong)num3 + (ulong)((long)num6)))))];
				}
				while (++num6 != 0);
			}
			return (int)num4;
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x00052CB0 File Offset: 0x00050EB0
		public static int lzf_decompress(byte[] input, ref byte[] output)
		{
			int num = input.Length;
			int num2 = output.Length;
			uint num3 = 0U;
			uint num4 = 0U;
			for (;;)
			{
				uint num5 = (uint)input[(int)((UIntPtr)(num3++))];
				if (num5 < 32U)
				{
					num5 += 1U;
					if ((ulong)(num4 + num5) > (ulong)((long)num2))
					{
						break;
					}
					do
					{
						output[(int)((UIntPtr)(num4++))] = input[(int)((UIntPtr)(num3++))];
					}
					while ((num5 -= 1U) != 0U);
				}
				else
				{
					uint num6 = num5 >> 5;
					int num7 = (int)(num4 - ((num5 & 31U) << 8) - 1U);
					if (num6 == 7U)
					{
						num6 += (uint)input[(int)((UIntPtr)(num3++))];
					}
					num7 -= (int)input[(int)((UIntPtr)(num3++))];
					if ((ulong)(num4 + num6 + 2U) > (ulong)((long)num2))
					{
						return 0;
					}
					if (num7 < 0)
					{
						return 0;
					}
					output[(int)((UIntPtr)(num4++))] = output[num7++];
					output[(int)((UIntPtr)(num4++))] = output[num7++];
					do
					{
						output[(int)((UIntPtr)(num4++))] = output[num7++];
					}
					while ((num6 -= 1U) != 0U);
				}
				if ((ulong)num3 >= (ulong)((long)num))
				{
					return (int)num4;
				}
			}
			return 0;
		}

		// Token: 0x040009BE RID: 2494
		private static readonly uint HLOG = 14U;

		// Token: 0x040009BF RID: 2495
		private static readonly uint HSIZE = 16384U;

		// Token: 0x040009C0 RID: 2496
		private static readonly uint MAX_LIT = 32U;

		// Token: 0x040009C1 RID: 2497
		private static readonly uint MAX_OFF = 8192U;

		// Token: 0x040009C2 RID: 2498
		private static readonly uint MAX_REF = 264U;

		// Token: 0x040009C3 RID: 2499
		private static readonly long[] HashTable = new long[Utilities.CLZF2.HSIZE];

		// Token: 0x040009C4 RID: 2500
		private static object lockObject = new object();
	}

	// Token: 0x0200015E RID: 350
	public static class Easing
	{
		// Token: 0x060009EC RID: 2540 RVA: 0x00052DB4 File Offset: 0x00050FB4
		public static float Linear(float t, float b, float c, float d)
		{
			return c * t / d + b;
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x00052DC0 File Offset: 0x00050FC0
		public static float ExpoEaseOut(float t, float b, float c, float d)
		{
			return (t != d) ? (c * (-Mathf.Pow(2f, -10f * t / d) + 1f) + b) : (b + c);
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x00052DF0 File Offset: 0x00050FF0
		public static float ExpoEaseIn(float t, float b, float c, float d)
		{
			return (t != 0f) ? (c * Mathf.Pow(2f, 10f * (t / d - 1f)) + b) : b;
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x00052E2C File Offset: 0x0005102C
		public static float ExpoEaseInOut(float t, float b, float c, float d)
		{
			if (t == 0f)
			{
				return b;
			}
			if (t == d)
			{
				return b + c;
			}
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * Mathf.Pow(2f, 10f * (t - 1f)) + b;
			}
			return c / 2f * (-Mathf.Pow(2f, -10f * (t -= 1f)) + 2f) + b;
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x00052EB4 File Offset: 0x000510B4
		public static float ExpoEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Utilities.Easing.ExpoEaseOut(t * 2f, b, c / 2f, d);
			}
			return Utilities.Easing.ExpoEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x00052F04 File Offset: 0x00051104
		public static float CircEaseOut(float t, float b, float c, float d)
		{
			return c * Mathf.Sqrt(1f - (t = t / d - 1f) * t) + b;
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00052F24 File Offset: 0x00051124
		public static float CircEaseIn(float t, float b, float c, float d)
		{
			return -c * (Mathf.Sqrt(1f - (t /= d) * t) - 1f) + b;
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x00052F44 File Offset: 0x00051144
		public static float CircEaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return -c / 2f * (Mathf.Sqrt(1f - t * t) - 1f) + b;
			}
			return c / 2f * (Mathf.Sqrt(1f - (t -= 2f) * t) + 1f) + b;
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x00052FB0 File Offset: 0x000511B0
		public static float CircEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Utilities.Easing.CircEaseOut(t * 2f, b, c / 2f, d);
			}
			return Utilities.Easing.CircEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x00053000 File Offset: 0x00051200
		public static float QuadEaseOut(float t, float b, float c, float d)
		{
			return -c * (t /= d) * (t - 2f) + b;
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x00053018 File Offset: 0x00051218
		public static float QuadEaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t + b;
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x00053028 File Offset: 0x00051228
		public static float QuadEaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t + b;
			}
			return -c / 2f * ((t -= 1f) * (t - 2f) - 1f) + b;
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0005307C File Offset: 0x0005127C
		public static float QuadEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Utilities.Easing.QuadEaseOut(t * 2f, b, c / 2f, d);
			}
			return Utilities.Easing.QuadEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x000530CC File Offset: 0x000512CC
		public static float SineEaseOut(float t, float b, float c, float d)
		{
			return c * Mathf.Sin(t / d * 1.5707964f) + b;
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x000530E0 File Offset: 0x000512E0
		public static float SineEaseIn(float t, float b, float c, float d)
		{
			return -c * Mathf.Cos(t / d * 1.5707964f) + c + b;
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x000530F8 File Offset: 0x000512F8
		public static float SineEaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * Mathf.Sin(3.1415927f * t / 2f) + b;
			}
			return -c / 2f * (Mathf.Cos(3.1415927f * (t -= 1f) / 2f) - 2f) + b;
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x00053164 File Offset: 0x00051364
		public static float SineEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Utilities.Easing.SineEaseOut(t * 2f, b, c / 2f, d);
			}
			return Utilities.Easing.SineEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x000531B4 File Offset: 0x000513B4
		public static float CubicEaseOut(float t, float b, float c, float d)
		{
			return c * ((t = t / d - 1f) * t * t + 1f) + b;
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x000531D0 File Offset: 0x000513D0
		public static float CubicEaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t * t + b;
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x000531E0 File Offset: 0x000513E0
		public static float CubicEaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t * t + b;
			}
			return c / 2f * ((t -= 2f) * t * t + 2f) + b;
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x00053234 File Offset: 0x00051434
		public static float CubicEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Utilities.Easing.CubicEaseOut(t * 2f, b, c / 2f, d);
			}
			return Utilities.Easing.CubicEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x00053284 File Offset: 0x00051484
		public static float QuartEaseOut(float t, float b, float c, float d)
		{
			return -c * ((t = t / d - 1f) * t * t * t - 1f) + b;
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x000532A4 File Offset: 0x000514A4
		public static float QuartEaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t * t * t + b;
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x000532B8 File Offset: 0x000514B8
		public static float QuartEaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t * t * t + b;
			}
			return -c / 2f * ((t -= 2f) * t * t * t - 2f) + b;
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x00053310 File Offset: 0x00051510
		public static float QuartEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Utilities.Easing.QuartEaseOut(t * 2f, b, c / 2f, d);
			}
			return Utilities.Easing.QuartEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x00053360 File Offset: 0x00051560
		public static float QuintEaseOut(float t, float b, float c, float d)
		{
			return c * ((t = t / d - 1f) * t * t * t * t + 1f) + b;
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x00053380 File Offset: 0x00051580
		public static float QuintEaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t * t * t * t + b;
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x00053394 File Offset: 0x00051594
		public static float QuintEaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t * t * t * t + b;
			}
			return c / 2f * ((t -= 2f) * t * t * t * t + 2f) + b;
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x000533F0 File Offset: 0x000515F0
		public static float QuintEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Utilities.Easing.QuintEaseOut(t * 2f, b, c / 2f, d);
			}
			return Utilities.Easing.QuintEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x00053440 File Offset: 0x00051640
		public static float ElasticEaseOut(float t, float b, float c, float d)
		{
			if ((t /= d) == 1f)
			{
				return b + c;
			}
			float num = d * 0.3f;
			float num2 = num / 4f;
			return c * Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * d - num2) * 6.2831855f / num) + c + b;
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0005349C File Offset: 0x0005169C
		public static float ElasticEaseIn(float t, float b, float c, float d)
		{
			if ((t /= d) == 1f)
			{
				return b + c;
			}
			float num = d * 0.3f;
			float num2 = num / 4f;
			return -(c * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t * d - num2) * 6.2831855f / num)) + b;
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x00053500 File Offset: 0x00051700
		public static float ElasticEaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) == 2f)
			{
				return b + c;
			}
			float num = d * 0.45000002f;
			float num2 = num / 4f;
			if (t < 1f)
			{
				return -0.5f * (c * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t * d - num2) * 6.2831855f / num)) + b;
			}
			return c * Mathf.Pow(2f, -10f * (t -= 1f)) * Mathf.Sin((t * d - num2) * 6.2831855f / num) * 0.5f + c + b;
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x000535B4 File Offset: 0x000517B4
		public static float ElasticEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Utilities.Easing.ElasticEaseOut(t * 2f, b, c / 2f, d);
			}
			return Utilities.Easing.ElasticEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x00053604 File Offset: 0x00051804
		public static float BounceEaseOut(float t, float b, float c, float d)
		{
			if ((t /= d) < 0.36363637f)
			{
				return c * (7.5625f * t * t) + b;
			}
			if ((double)t < 0.7272727272727273)
			{
				return c * (7.5625f * (t -= 0.54545456f) * t + 0.75f) + b;
			}
			if ((double)t < 0.9090909090909091)
			{
				return c * (7.5625f * (t -= 0.8181818f) * t + 0.9375f) + b;
			}
			return c * (7.5625f * (t -= 0.95454544f) * t + 0.984375f) + b;
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x000536A8 File Offset: 0x000518A8
		public static float BounceEaseIn(float t, float b, float c, float d)
		{
			return c - Utilities.Easing.BounceEaseOut(d - t, 0f, c, d) + b;
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x000536C0 File Offset: 0x000518C0
		public static float BounceEaseInOut(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Utilities.Easing.BounceEaseIn(t * 2f, 0f, c, d) * 0.5f + b;
			}
			return Utilities.Easing.BounceEaseOut(t * 2f - d, 0f, c, d) * 0.5f + c * 0.5f + b;
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x0005371C File Offset: 0x0005191C
		public static float BounceEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Utilities.Easing.BounceEaseOut(t * 2f, b, c / 2f, d);
			}
			return Utilities.Easing.BounceEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x0005376C File Offset: 0x0005196C
		public static float BackEaseOut(float t, float b, float c, float d)
		{
			float num = t / 1f - 1f;
			return num * num * (1.8f * num + 0.8f) + 1f;
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x000537A0 File Offset: 0x000519A0
		public static float BackEaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t * (2.70158f * t - 1.70158f) + b;
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x000537BC File Offset: 0x000519BC
		public static float BackEaseInOut(float t, float b, float c, float d)
		{
			float num = 1.70158f;
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * (t * t * (((num *= 1.525f) + 1f) * t - num)) + b;
			}
			return c / 2f * ((t -= 2f) * t * (((num *= 1.525f) + 1f) * t + num) + 2f) + b;
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x00053838 File Offset: 0x00051A38
		public static float BackEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return Utilities.Easing.BackEaseOut(t * 2f, b, c / 2f, d);
			}
			return Utilities.Easing.BackEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}
	}
}
