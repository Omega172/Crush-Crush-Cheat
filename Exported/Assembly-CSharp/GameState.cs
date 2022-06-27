using System;
using System.Collections;
using System.Collections.Generic;
using AssetBundles;
using BlayFap;
using BlayFapShared;
using SadPanda.Platforms;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000095 RID: 149
public class GameState : MonoBehaviour
{
	// Token: 0x17000030 RID: 48
	// (get) Token: 0x0600029E RID: 670 RVA: 0x00012C84 File Offset: 0x00010E84
	// (set) Token: 0x0600029F RID: 671 RVA: 0x00012C8C File Offset: 0x00010E8C
	public static int DateCount
	{
		get
		{
			return GameState._dateCount;
		}
		set
		{
			GameState._dateCount = ((value >= 0) ? value : int.MaxValue);
			Achievements.HandleDates(value);
		}
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x060002A0 RID: 672 RVA: 0x00012CAC File Offset: 0x00010EAC
	// (set) Token: 0x060002A1 RID: 673 RVA: 0x00012CB4 File Offset: 0x00010EB4
	public static int GiftCount
	{
		get
		{
			return GameState._giftCount;
		}
		set
		{
			GameState._giftCount = ((value >= 0) ? value : int.MaxValue);
			Achievements.HandleGifts(value);
		}
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x060002A2 RID: 674 RVA: 0x00012CD4 File Offset: 0x00010ED4
	// (set) Token: 0x060002A3 RID: 675 RVA: 0x00012CDC File Offset: 0x00010EDC
	public static int PokeCount
	{
		get
		{
			return GameState._pokeCount;
		}
		set
		{
			GameState._pokeCount = ((value >= 0) ? value : int.MaxValue);
		}
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x060002A4 RID: 676 RVA: 0x00012CF8 File Offset: 0x00010EF8
	public static GameState CurrentState
	{
		get
		{
			return GameState.currentState;
		}
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x00012D00 File Offset: 0x00010F00
	public static Transform FindChild(string path)
	{
		Transform transform = GameState.CurrentState.transform.Find(path);
		if (transform == null)
		{
			Debug.LogException(new Exception("Child is null: " + path));
		}
		return transform;
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x00012D40 File Offset: 0x00010F40
	public static T FindChild<T>(string path) where T : Component
	{
		Transform transform = GameState.FindChild(path);
		if (transform == null)
		{
			return (T)((object)null);
		}
		T component = transform.GetComponent<T>();
		if (component == null)
		{
			Debug.LogException(new Exception("Component not found at: " + path));
		}
		return component;
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x060002A7 RID: 679 RVA: 0x00012D98 File Offset: 0x00010F98
	public static Voiceover Voiceover
	{
		get
		{
			if (GameState.currentState == null)
			{
				return GameObject.Find("Canvas").GetComponent<Voiceover>();
			}
			return GameState.currentState.GetComponent<Voiceover>();
		}
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x00012DD0 File Offset: 0x00010FD0
	public static IntroScreen GetIntroScreen()
	{
		if (GameState.currentState == null || GameState.currentState.Intro == null)
		{
			return GameObject.Find("Canvas").transform.Find("Intro Tutorial").GetComponent<IntroScreen>();
		}
		return GameState.currentState.Intro.GetComponent<IntroScreen>();
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x00012E30 File Offset: 0x00011030
	public static Girls GetGirlScreen()
	{
		if (GameState.currentState == null || GameState.currentState.GirlScreen == null)
		{
			return GameObject.Find("Canvas").transform.Find("Girls").GetComponent<Girls>();
		}
		return GameState.currentState.GirlScreen.GetComponent<Girls>();
	}

	// Token: 0x060002AA RID: 682 RVA: 0x00012E90 File Offset: 0x00011090
	public static TaskManager GetTaskSystem()
	{
		if (GameState.currentState == null || GameState.currentState.TaskSystem == null)
		{
			return GameObject.Find("Canvas").transform.Find("Task System").GetComponent<TaskManager>();
		}
		return GameState.currentState.TaskSystem;
	}

	// Token: 0x060002AB RID: 683 RVA: 0x00012EEC File Offset: 0x000110EC
	public static Cellphone GetCellphone()
	{
		if (GameState.currentState == null || GameState.currentState.Cellphone == null)
		{
			return GameObject.Find("Canvas").transform.Find("Popups/Cellphone").GetComponent<Cellphone>();
		}
		return GameState.currentState.Cellphone;
	}

	// Token: 0x060002AC RID: 684 RVA: 0x00012F48 File Offset: 0x00011148
	public static Achievements GetAchievements()
	{
		if (GameState.currentState == null || GameState.currentState.Achievements == null)
		{
			return GameObject.Find("Canvas").transform.Find("Achievements").GetComponent<Achievements>();
		}
		return GameState.currentState.Achievements;
	}

	// Token: 0x060002AD RID: 685 RVA: 0x00012FA4 File Offset: 0x000111A4
	public static IntroProvider GetCutSceneProvider()
	{
		Transform transform = (!(GameState.currentState == null)) ? GameState.currentState.transform : GameObject.Find("Canvas").transform;
		return transform.transform.Find("Popups/Girl Introduction").GetComponent<IntroProvider>();
	}

	// Token: 0x060002AE RID: 686 RVA: 0x00012FF8 File Offset: 0x000111F8
	public void Prestige()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		long[] array = new long[Album.CacheSize];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = global::PlayerPrefs.GetLong("album" + i, 0L);
		}
		string value = GameState.TotalIncome.Value.ToString();
		short[] array2 = new short[Universe.CellphoneGirls.Count];
		string[] array3 = new string[Universe.CellphoneGirls.Count];
		long[] array4 = new long[Universe.CellphoneGirls.Count];
		int num = 0;
		foreach (KeyValuePair<short, PhoneModel> keyValuePair in Universe.CellphoneGirls)
		{
			array2[num] = keyValuePair.Key;
			array4[num] = keyValuePair.Value.DatePref.GetLong(0L);
			array3[num] = keyValuePair.Value.PathPref.GetString(string.Empty);
			num++;
		}
		this.PrepareReset();
		base.transform.Find("Popups/Task System").GetComponent<TaskManager>().Prestige();
		dictionary.Add("PendingTimelord", global::PlayerPrefs.GetInt("PendingTimelord", 0));
		dictionary.Add("NutakuItems2019", global::PlayerPrefs.GetLong("NutakuItems2019", 0L));
		dictionary.Add("GameStateCreated", global::PlayerPrefs.GetLong("GameStateCreated", 0L));
		dictionary.Add("AyanoChibi2017", global::PlayerPrefs.GetInt("AyanoChibi2017", 0));
		dictionary.Add("AyanoTimeBlock", global::PlayerPrefs.GetInt("AyanoTimeBlock", 0));
		dictionary.Add("Completed2019Events", global::PlayerPrefs.GetLong("Completed2019Events", 0L));
		dictionary.Add("Completed2020Events", global::PlayerPrefs.GetLong("Completed2020Events", 0L));
		foreach (KeyValuePair<short, EventData> keyValuePair2 in Universe.Events)
		{
			int @int = global::PlayerPrefs.GetInt(string.Format("Event{0}Tokens", keyValuePair2.Key.ToString()), 0);
			if (@int != 0)
			{
				dictionary.Add(string.Format("Event{0}Tokens", keyValuePair2.Key.ToString()), @int);
			}
		}
		dictionary.Add("GameStateCovid2020", global::PlayerPrefs.GetInt("GameStateCovid2020", 0));
		dictionary.Add("TutorialAffection", 1);
		dictionary.Add("Tutorial", 255);
		dictionary.Add("GameStateTotalIncome", value);
		dictionary.Add("PurchasedTime", FreeTime.PurchasedTime);
		dictionary.Add("GameStatePurchasedMultiplier", (float)GameState.purchasedMultiplier);
		dictionary.Add("GameStateTotalTime", GameState.TotalTime);
		dictionary.Add("CurrentPanel", "Girl");
		dictionary.Add("GameStateDateCount", GameState.DateCount);
		dictionary.Add("GameStateGiftCount", GameState.GiftCount);
		dictionary.Add("GameStateHeartCount", GameState.HeartCount.Value);
		dictionary.Add("GameStatePokeCount", GameState.PokeCount);
		dictionary.Add("PlayfabInventory", (int)Playfab.InventoryObjects);
		dictionary.Add("Prereg", 1);
		dictionary.Add("PlayfabAwardedItems", (long)Playfab.AwardedItems);
		dictionary.Add("PlayfabFlingPurchases", (long)Playfab.FlingPurchases);
		dictionary.Add("GameStateDiamonds", GameState.Diamonds.Value.ToString());
		float num2 = Mathf.Max(this.TimeMultiplier.Value, this.TimeMultiplier.Value + this.PendingPrestige.Value);
		if (this.PendingPrestige.Value == 0f || this.TimeMultiplier.Value >= 2048f)
		{
			Achievements.ForceAchievement(471);
		}
		this.PendingPrestige.Value = 0f;
		dictionary.Add("TimeMultiplier", num2);
		if (global::PlayerPrefs.HasKey(TaskManager.NEWCOMER_LTE_START_DATETIME))
		{
			dictionary.Add(TaskManager.NEWCOMER_LTE_START_DATETIME, global::PlayerPrefs.GetLong(TaskManager.NEWCOMER_LTE_START_DATETIME, 0L));
		}
		if (global::PlayerPrefs.HasKey(TaskManager.EVENT_NEWCOMER_ID))
		{
			dictionary.Add(TaskManager.EVENT_NEWCOMER_ID, global::PlayerPrefs.GetInt(TaskManager.EVENT_NEWCOMER_ID, 0));
		}
		global::PlayerPrefs.DeleteAll();
		Girl.Reset(true);
		Settings.SaveState();
		for (int j = 0; j < array2.Length; j++)
		{
			PhoneModel phoneModel = Universe.CellphoneGirls[array2[j]];
			phoneModel.DatePref.SetLong(array4[j]);
			phoneModel.PathPref.SetString(array3[j]);
		}
		Transform transform = base.transform.Find("Hobbies/Scroll View/Content Panel");
		for (int k = 0; k < transform.childCount; k++)
		{
			transform.GetChild(k).GetComponent<Hobby2>().Reset();
			transform.GetChild(k).GetComponent<Hobby2>().StoreState();
		}
		Transform transform2 = base.transform.Find("Jobs/Scroll View/Content Panel");
		for (int l = 0; l < transform2.childCount; l++)
		{
			transform2.GetChild(l).GetComponent<Job2>().Reset();
			transform2.GetChild(l).GetComponent<Job2>().StoreState();
		}
		Achievements.HandleReset();
		this.Achievements.StoreState();
		for (int m = 0; m < array.Length; m++)
		{
			global::PlayerPrefs.SetLong("album" + m, array[m]);
		}
		this.TaskSystem.StoreState();
		foreach (KeyValuePair<string, object> keyValuePair3 in dictionary)
		{
			if (keyValuePair3.Value is int)
			{
				global::PlayerPrefs.SetInt(keyValuePair3.Key, (int)keyValuePair3.Value);
			}
			else if (keyValuePair3.Value is long)
			{
				global::PlayerPrefs.SetLong(keyValuePair3.Key, (long)keyValuePair3.Value);
			}
			else if (keyValuePair3.Value is float)
			{
				global::PlayerPrefs.SetFloat(keyValuePair3.Key, (float)keyValuePair3.Value);
			}
			else if (keyValuePair3.Value is string)
			{
				global::PlayerPrefs.SetString(keyValuePair3.Key, (string)keyValuePair3.Value);
			}
			else
			{
				Utilities.SendAnalytic(Utilities.AnalyticType.Exception, "Tried to process " + keyValuePair3.Value.GetType().ToString() + " in persistentPrefs. " + keyValuePair3.Key);
				Debug.LogError("Tried to process " + keyValuePair3.Value.GetType().ToString() + " in persistentPrefs. " + keyValuePair3.Key);
			}
		}
		Skills.StoreState();
		global::PlayerPrefs.Save();
		Utilities.SendAnalytic(Utilities.AnalyticType.Prestige, num2.ToString());
		GameState.Initialized = false;
		SceneManager.LoadScene(0);
	}

	// Token: 0x060002AF RID: 687 RVA: 0x000137B4 File Offset: 0x000119B4
	public void ResetAll()
	{
		int @int = global::PlayerPrefs.GetInt("NutakuItems2019", 0);
		string @string = global::PlayerPrefs.GetString("CompletedEvents", string.Empty);
		this.PrepareReset();
		int num = 0;
		GameState.GiftCount = num;
		num = num;
		GameState.DateCount = num;
		GameState.PokeCount = num;
		GameState.TotalIncome.Value = 0.0;
		GameState.TotalTime = 0;
		GameState.HeartCount.Value = 0L;
		GameState.Diamonds.Value = 0;
		GameState.purchasedMultiplier = 1;
		this.TimeMultiplier.Value = 1f;
		this.Achievements.AllAchievements.Clear();
		this.Achievements.ActiveAchievements.Clear();
		this.Achievements.ActiveAchievementsHash.Clear();
		Girl.Reset(false);
		global::PlayerPrefs.DeleteAll();
		Settings.SaveState();
		global::PlayerPrefs.SetInt("Prereg", 1);
		global::PlayerPrefs.SetInt("PlayfabInventory", (int)Playfab.InventoryObjects);
		global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
		Playfab.FlingPurchases = (Playfab.PhoneFlingPurchases)0L;
		global::PlayerPrefs.SetInt("NutakuItems2019", @int);
		global::PlayerPrefs.SetString("CompletedEvents", @string);
		global::PlayerPrefs.Save();
		Utilities.SendAnalytic(Utilities.AnalyticType.Reset, string.Empty);
		GameState.Initialized = false;
		GameState.IsHardReset = true;
		SceneManager.LoadScene(0);
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x000138E8 File Offset: 0x00011AE8
	public void AwardAnniversary()
	{
		BlayFapClient.Instance.AwardPlayfabItem(Playfab.PlayfabItems.Anniversary2022, delegate(BlayFapResponse response)
		{
			if (response.Error == null)
			{
				Playfab.AwardedItems |= Playfab.PlayfabItems.Anniversary2022;
			}
			else
			{
				Notifications.AddNotification(Notifications.NotificationType.Message, Translations.GetTranslation("achievements_409_0", "There was a problem adding the purchased bundle to your inventory.  Please contact support@sadpandastudios.com!"));
			}
		});
	}

	// Token: 0x060002B1 RID: 689 RVA: 0x00013928 File Offset: 0x00011B28
	public static void ResetShownPromo()
	{
		GameState.shownPromo = false;
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x00013930 File Offset: 0x00011B30
	private void LaunchPromotionCallback(bool loaded)
	{
		if (!loaded)
		{
			return;
		}
		BlayFapIntegration.BlayFapLoaded -= new ReactiveProperty<bool>.Changed(this.LaunchPromotionCallback);
		this.LaunchPromotion();
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x00013968 File Offset: 0x00011B68
	public void LaunchPromotion()
	{
		if (string.IsNullOrEmpty(Playfab.Promotion) || !BlayFapIntegration.BlayFapLoaded.Value)
		{
			return;
		}
		if (GameState.updatedStore)
		{
			return;
		}
		if (GameState.TotalTime < 15)
		{
			return;
		}
		if (GameState.shownPromo)
		{
			return;
		}
		if (Playfab.Promotion.Contains("sirinapopup") && !GameState.GetGirlScreen().IsUnlocked(Balance.GirlName.Sirina) && base.transform.Find("Popups/Sirina Popup") != null)
		{
			base.transform.Find("Popups/Sirina Popup").gameObject.SetActive(true);
			GameState.shownPromo = true;
		}
		if (Playfab.Promotion.Contains("junesale") && base.transform.Find("Popups/June Sale") != null)
		{
			base.transform.Find("Popups/June Sale").gameObject.SetActive(true);
			base.transform.Find("Top UI/Sale").gameObject.SetActive(true);
			GameState.shownPromo = true;
		}
		if (string.IsNullOrEmpty(Playfab.JsonDataUrl))
		{
			base.StartCoroutine(this.WaitForEventURL());
		}
		else
		{
			this.CheckShowEvents();
		}
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x00013AAC File Offset: 0x00011CAC
	private IEnumerator WaitForEventURL()
	{
		float timeout = 60f;
		while (Playfab.JsonDataUrl == string.Empty || GameState.GetTaskSystem().CurrentEvent == null)
		{
			timeout -= Time.deltaTime;
			if (timeout <= 0f)
			{
				yield break;
			}
			yield return null;
		}
		this.CheckShowEvents();
		yield break;
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x00013AC8 File Offset: 0x00011CC8
	private void CheckShowEvents()
	{
		if (!this._isDestroyed && !string.IsNullOrEmpty(Playfab.JsonDataUrl) && Playfab.JsonDataUrl != "No Events")
		{
			GameState.shownPromo = true;
			EventsPopup eventsPopup = base.gameObject.AddComponent<EventsPopup>();
			eventsPopup.InitEventPopup(Playfab.JsonDataUrl);
		}
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x00013B28 File Offset: 0x00011D28
	public void ShowSignIn()
	{
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x00013B2C File Offset: 0x00011D2C
	public void GotoSignIn()
	{
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x00013B30 File Offset: 0x00011D30
	public void LoadGameFromBlayFap()
	{
		BlayFapIntegration.LoadSaveFromUserData();
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x00013B38 File Offset: 0x00011D38
	public void CopyBlayfapID()
	{
		TextEditor textEditor = new TextEditor();
		textEditor.text = BlayFapClient.BlayFapId.ToString("X");
		textEditor.SelectAll();
		textEditor.Copy();
		Notifications.AddNotification(Notifications.NotificationType.Message, Translations.GetTranslation("everything_else_102_0", "Playfab ID was copied to your clipboard!").Replace("Playfab", "SPS"));
	}

	// Token: 0x060002BA RID: 698 RVA: 0x00013B90 File Offset: 0x00011D90
	public void ShowBlayFapID()
	{
		if (base.transform.Find("More") != null && base.transform.Find("More/SPS ID") != null && base.transform.Find("More/SPS ID").GetComponent<Text>() != null)
		{
			base.transform.Find("More/SPS ID").GetComponent<Text>().text = "SPS ID: " + ((!BlayFapClient.LoggedIn) ? "Login Failure" : BlayFapClient.BlayFapId.ToString("X"));
			base.transform.Find("More/SPS Copy").gameObject.SetActive(BlayFapClient.LoggedIn);
		}
	}

	// Token: 0x060002BB RID: 699 RVA: 0x00013C5C File Offset: 0x00011E5C
	public void InitAchievements()
	{
		if (this.platformAchievements != null)
		{
			return;
		}
		this.platformAchievements = new SteamStatsAndAchievements();
		this.platformAchievements.Init();
		this.platformAchievements.CheckAchievements();
	}

	// Token: 0x060002BC RID: 700 RVA: 0x00013C8C File Offset: 0x00011E8C
	public void CheckAchievements()
	{
		if (this.platformAchievements == null)
		{
			return;
		}
		this.platformAchievements.CheckAchievements();
	}

	// Token: 0x060002BD RID: 701 RVA: 0x00013CA8 File Offset: 0x00011EA8
	public void ShowAchievements()
	{
		if (this.platformAchievements == null)
		{
			return;
		}
		this.platformAchievements.ShowAchievements();
	}

	// Token: 0x060002BE RID: 702 RVA: 0x00013CC4 File Offset: 0x00011EC4
	public static void InitializeAssetManager(Action onComplete)
	{
		if (GameState.AssetManager != null)
		{
			if (onComplete != null)
			{
				onComplete();
			}
			return;
		}
		GameState.AssetManager = new AssetBundleManager();
		GameState.AssetManager.SetPrioritizationStrategy(AssetBundleManager.PrioritizationStrategy.PrioritizeStreamingAssets);
		GameState.AssetManager.SetBaseUri("https://www.sadpandastudios.com/assets/crushcrush");
		GameState.AssetManager.Initialize(delegate(bool complete)
		{
			if (onComplete != null)
			{
				onComplete();
			}
		});
	}

	// Token: 0x060002BF RID: 703 RVA: 0x00013D3C File Offset: 0x00011F3C
	public void PostInit()
	{
		if ((Playfab.AwardedItems & Playfab.PlayfabItems.StarterPack) != (Playfab.PlayfabItems)0L && Mathf.RoundToInt(global::PlayerPrefs.GetFloat("GameStatePurchasedMultiplier", 1f)) < 2)
		{
			FreeTime.PurchasedTime += 5;
			GameState.PurchasedMultiplier = 2;
			global::PlayerPrefs.SetFloat("GameStatePurchasedMultiplier", 2f);
			global::PlayerPrefs.SetInt("PurchasedTime", FreeTime.PurchasedTime);
			Utilities.AwardDiamonds(130);
		}
		base.GetComponent<PanelScript>().LoadStartupScreen();
		if (Girl.GetLove(Balance.GirlName.Cassie) <= 1 && global::PlayerPrefs.GetInt("Tutorial") == 0)
		{
			Girl.FindGirl(Balance.GirlName.Cassie).Data.CheckIfLoaded();
			GameState.GetCutSceneProvider().Initialize(new CassieIntroduction(Girl.FindGirl(Balance.GirlName.Cassie)));
			Girl.FindGirl(Balance.GirlName.Cassie).Hearts = 1L;
			GameState.TotalIncome.Value = -1.0;
			base.transform.Find("Girls/Girl Information/Requirements").gameObject.SetActive(false);
			base.transform.Find("Girls/Girl Information/Interaction Buttons").gameObject.SetActive(false);
			base.transform.Find("Girls/Girl Information").gameObject.SetActive(false);
			base.transform.Find("Girls").gameObject.SetActive(false);
			base.transform.Find("Jobs").gameObject.SetActive(false);
			base.transform.Find("Hobbies").gameObject.SetActive(false);
			base.transform.Find("Stats").gameObject.SetActive(false);
			GameState.GetAchievements().gameObject.SetActive(false);
			base.transform.Find("Store Revamp").gameObject.SetActive(false);
			if (base.transform.Find("Popups/More") != null)
			{
				base.transform.Find("Popups/More").gameObject.SetActive(false);
			}
			else
			{
				base.transform.Find("More").gameObject.SetActive(false);
			}
			if (base.transform.Find("Top UI/Starter Pack") != null)
			{
				base.transform.Find("Top UI/Starter Pack").gameObject.SetActive(false);
			}
			for (int i = 1; i < this.BottomUI.transform.childCount; i++)
			{
				this.BottomUI.transform.GetChild(i).GetComponent<Button>().interactable = false;
				this.BottomUI.transform.GetChild(i).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.2f);
				if (this.BottomUI.transform.GetChild(i).Find("Icon") != null)
				{
					this.BottomUI.transform.GetChild(i).Find("Icon").GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.2f);
				}
			}
			global::PlayerPrefs.SetInt("Tutorial", 1);
			this.GirlScreen.GetComponent<Girls>().StoreState();
			GameState.CurrentState.QueueSave();
		}
		else
		{
			this.BottomUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
			base.transform.Find("Girls/Girl Information/Requirements").gameObject.SetActive(true);
			base.transform.Find("Girls/Girl Information/Interaction Buttons").gameObject.SetActive(true);
			base.transform.Find("Girls/Girl Information").gameObject.SetActive(false);
			GameState.CurrentState.transform.Find("Top UI/Task Blocker").gameObject.SetActive(true);
			base.GetComponent<PanelScript>().ClickGirl();
			IntroScreen.TutorialState = IntroScreen.State.HobbiesActive;
			Girls.UnlockedGirlCount = Mathf.Max(1, Girls.UnlockedGirlCount);
		}
		this.IsLoaded = true;
		base.GetComponent<Audio>().SetVolume();
		base.GetComponent<Audio>().Music.loop = true;
		base.GetComponent<Audio>().Music.clip = ((!(DateTime.UtcNow < new DateTime(2021, 1, 1))) ? base.GetComponent<Audio>().Music1 : base.GetComponent<Audio>().HolidayMusic);
		if (Settings.MusicVolume > 0f)
		{
			base.GetComponent<Audio>().Music.Play();
		}
		this.PendingPrestige += delegate(float prestige)
		{
			if (prestige > 1f && !base.transform.Find("Popups/Girl Introduction").gameObject.activeSelf)
			{
				GameState.GetIntroScreen().PrestigeStart();
			}
		};
		global::PlayerPrefs.SetInt("Prereg", 1);
		if (BlayFapIntegration.IsTestDevice)
		{
			GameState.CurrentState.transform.Find("Popups/Debug Init").GetComponent<DebugInitController>().InitDebug();
			Utilities.ForceCheckCachedServerTime();
		}
		BlayFapIntegration.AwardItems(Playfab.AwardedItems);
		GameState.IsHardReset = false;
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x00014228 File Offset: 0x00012428
	public void ShowAppTransparencyNativePopup()
	{
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x0001422C File Offset: 0x0001242C
	public void ResetLTEDay()
	{
		Utilities.DateOffset = 0;
		global::PlayerPrefs.SetInt("GameStateDateOffset", Utilities.DateOffset);
		this.QueueSave();
		Utilities.ForceCheckCachedServerTime();
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x0001425C File Offset: 0x0001245C
	public void NextLTEDay()
	{
		Utilities.DateOffset++;
		global::PlayerPrefs.SetInt("GameStateDateOffset", Utilities.DateOffset);
		this.QueueSave();
		Utilities.ForceCheckCachedServerTime();
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x00014290 File Offset: 0x00012490
	public void PrevLTEDay()
	{
		Utilities.DateOffset--;
		global::PlayerPrefs.SetInt("GameStateDateOffset", Utilities.DateOffset);
		this.QueueSave();
		Utilities.ForceCheckCachedServerTime();
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x060002C4 RID: 708 RVA: 0x000142C4 File Offset: 0x000124C4
	// (set) Token: 0x060002C5 RID: 709 RVA: 0x000142CC File Offset: 0x000124CC
	public bool IsLoaded { get; private set; }

	// Token: 0x060002C6 RID: 710 RVA: 0x000142D8 File Offset: 0x000124D8
	public void Init()
	{
		this.IsLoaded = false;
		GameState.BuildDetails = string.Format("{0}_{1}", GameState.CurrentVersion.ToString(), AssetBundleSettings.Manifest.Substring(1, 6));
		int num = global::PlayerPrefs.GetInt("WinterItems2018", 0);
		num |= 32;
		global::PlayerPrefs.SetInt("WinterItems2018", num);
		if (base.transform.Find("Loading Screen") != null)
		{
			base.transform.Find("Loading Screen").gameObject.SetActive(true);
		}
		GameState.Money.RemoveAllListeners();
		GameState.TotalIncome.RemoveAllListeners();
		GameState.HeartCount.RemoveAllListeners();
		GameState.Money += delegate(double value)
		{
			this.MoneyText.text = Utilities.ToPrefixedNumber(value, false, false);
		};
		GameState.TotalIncome += new ReactiveProperty<double>.Changed(Achievements.HandleTotalIncome);
		GameState.Money += new ReactiveProperty<double>.Changed(Achievements.HandleIncome);
		GameState.HeartCount += new ReactiveProperty<long>.Changed(Achievements.HandleHearts);
		GameState.Diamonds.RemoveAllListeners();
		GameState.Diamonds += delegate(int value)
		{
			this.DiamondText.text = value.ToString("n0");
		};
		Girl.AssetQueue = (Girl.AssetsLoaded = 0);
		GameState.GetTaskSystem().LoadCompletedEvents();
		GameState.NSFW = (global::PlayerPrefs.GetInt("GameStateNSFW", 0) == 1);
		base.transform.Find("More/Version Info").GetComponent<Text>().text = string.Format("{0} V.0.{1} Copyright 2022 Sad Panda Studios Ltd.", Translations.GetTranslation("everything_else_44_0", "Thanks for playing our game!"), GameState.CurrentVersion);
		GameState.BuildDetails = string.Format("{0}_{1}", GameState.CurrentVersion.ToString(), AssetBundleSettings.Manifest.Substring(1, 6));
		base.transform.Find("Mute Buttons/Close Button").gameObject.SetActive(true);
		Girls.UnlockedGirlCount = 0;
		try
		{
			Translations.CurrentLanguage = new ReactiveProperty<int>(0);
			GameState.InitializeAssetManager(delegate
			{
				GameState.Initialized = true;
				FreeTime.TimeSlots = GameState.initialTimeBlocks + global::PlayerPrefs.GetInt("AchievementCount") / 4;
				FreeTime.PurchasedTime = global::PlayerPrefs.GetInt("PurchasedTime", 0);
				this.TimeMultiplier.Value = Mathf.Min(2048f, global::PlayerPrefs.GetFloat("TimeMultiplier", 1f));
				global::PlayerPrefs.SetFloat("TimeMultiplier", this.TimeMultiplier.Value);
				base.StartCoroutine(base.GetComponent<Universe>().Init(delegate
				{
					Settings.LoadState();
					Translations.Init(delegate
					{
						Utilities.SendAnalytic(Utilities.AnalyticType.Login, ((Translations.Language)Translations.CurrentLanguage.Value).ToString());
						base.transform.Find("Achievements").GetComponent<Achievements>().Init(false);
						Skills.Init();
						Girl.Init();
						this.LoadState();
						this.GirlScreen.GetComponent<Girls>().LoadState();
						this.CheckCellphoneUnlock();
						this.CalculateOfflineProgress();
						Achievements.HandleReset();
						this.PostInit();
						if (base.transform.Find("Loading Screen") != null)
						{
							base.transform.Find("Loading Screen").gameObject.SetActive(false);
						}
						if (SteamManager.Initialized)
						{
							if (SteamApps.BIsDlcInstalled(new AppId_t(948680U)))
							{
								GameState.NSFWAllowed = true;
								Playfab.InventoryObjects |= (Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
							}
							if (SteamApps.BIsSubscribedApp(new AppId_t(1336600U)))
							{
								Playfab.AwardedItems |= Playfab.PlayfabItems.Mallory;
							}
						}
						GameObject gameObject = GameObject.Find("Steam");
						if (gameObject == null)
						{
							gameObject = new GameObject("Steam");
						}
						if (gameObject.GetComponent<SteamManager>() == null)
						{
							BlayFapClient.Instance.OnBlayFapLogin += BlayFapIntegration.OnBlayFapLogin;
							SteamManager.InitializeSteam(new Action<string>(this.OnSteamError));
						}
						else
						{
							if (Store2.DiamondItems == null)
							{
								BlayFapClient.Instance.OnBlayFapLogin += BlayFapIntegration.OnBlayFapLogin;
							}
							if (BlayFapClient.UsingBlayfap)
							{
								BlayFapIntegration.CheckLTE();
							}
						}
						GameState.Voiceover.Init(true);
						GameState.UniverseReady.Value = true;
					});
				}));
			});
		}
		catch (Exception ex)
		{
			base.transform.Find("Loading Screen/Loading Text").GetComponent<Text>().text = "An error occurred. " + ex.Message;
			Debug.Log("InitializeAssetManager: " + ex.Message);
			Debug.LogException(ex);
		}
		Utilities.UpdateLoginLoadSaveButtons();
		this.ShowBlayFapID();
		QualitySettings.vSyncCount = 1;
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x00014558 File Offset: 0x00012758
	private void OnSteamError(string error)
	{
		Transform transform = base.transform.Find("Popups/Error Dialog");
		transform.Find("Error Text").GetComponent<Text>().text = "Error: " + error;
		transform.gameObject.SetActive(true);
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x000145A4 File Offset: 0x000127A4
	public void RedeemCoupon(Text textField)
	{
		Transform couponPopup = GameState.CurrentState.transform.Find("More/Redeem Coupon Popup");
		InputField inputText = couponPopup.Find("Dialog/InputField").GetComponent<InputField>();
		Button acceptButton = couponPopup.Find("Dialog/Accept Button").GetComponent<Button>();
		acceptButton.interactable = false;
		string text = textField.text.ToLowerInvariant();
		text = text.Trim(new char[]
		{
			'\r',
			'\n',
			' ',
			'\t',
			'​'
		});
		if (BlayFapClient.LoggedIn)
		{
			RedeemCouponRequest request = new RedeemCouponRequest
			{
				CouponCode = text
			};
			BlayFapClient.Instance.RedeemCoupon(request, delegate(BlayFapResponse result)
			{
				if (result.Error == null)
				{
					BlayFapIntegration.ConsumeBlayFapInventory();
					couponPopup.gameObject.SetActive(false);
				}
				else
				{
					inputText.text = result.Error.ErrorType.ToString();
					acceptButton.interactable = true;
				}
			});
		}
	}

	// Token: 0x060002C9 RID: 713 RVA: 0x00014668 File Offset: 0x00012868
	public void ShowAd()
	{
	}

	// Token: 0x060002CA RID: 714 RVA: 0x0001466C File Offset: 0x0001286C
	public void Show2xAd()
	{
	}

	// Token: 0x060002CB RID: 715 RVA: 0x00014670 File Offset: 0x00012870
	private void Start()
	{
		GameState.currentState = this;
		this.Intro = base.transform.Find("Popups/Intro Tutorial").gameObject;
		this.TaskSystem = base.transform.Find("Popups/Task System").GetComponent<TaskManager>();
		this.Achievements = base.transform.Find("Achievements").GetComponent<Achievements>();
		this.Cellphone = base.transform.Find("Popups/Cellphone").GetComponent<Cellphone>();
		this.BottomUI = base.transform.Find("Bottom UI").gameObject;
		this.GirlScreen = base.transform.Find("Girls").gameObject;
		this.Init();
		GameState.inInit = false;
	}

	// Token: 0x060002CC RID: 716 RVA: 0x00014734 File Offset: 0x00012934
	public static void ShowJobsNotification()
	{
		if (GameState.inInit || GameState.currentState == null || GameState.currentState.BottomUI == null || IntroScreen.TutorialState < IntroScreen.State.JobsActive)
		{
			return;
		}
		GameState.currentState.BottomUI.transform.Find("JobsBTN/Notification").gameObject.SetActive(true);
	}

	// Token: 0x060002CD RID: 717 RVA: 0x000147A0 File Offset: 0x000129A0
	public static void HideJobsNotification()
	{
		if (GameState.inInit || GameState.currentState == null || GameState.currentState.BottomUI == null)
		{
			return;
		}
		GameState.currentState.BottomUI.transform.Find("JobsBTN/Notification").gameObject.SetActive(false);
	}

	// Token: 0x060002CE RID: 718 RVA: 0x00014804 File Offset: 0x00012A04
	public static void HideStoreNotification()
	{
	}

	// Token: 0x060002CF RID: 719 RVA: 0x00014808 File Offset: 0x00012A08
	public static void ShowHobbyNotification()
	{
		if (GameState.inInit || GameState.currentState == null || GameState.currentState.BottomUI == null || IntroScreen.TutorialState < IntroScreen.State.HobbiesActive)
		{
			return;
		}
		GameState.currentState.BottomUI.transform.Find("HobbyBTN/Notification").gameObject.SetActive(true);
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x00014874 File Offset: 0x00012A74
	public static void HideHobbyNotification()
	{
		if (GameState.inInit || GameState.currentState == null || GameState.currentState.BottomUI == null)
		{
			return;
		}
		GameState.currentState.BottomUI.transform.Find("HobbyBTN/Notification").gameObject.SetActive(false);
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x000148D8 File Offset: 0x00012AD8
	public static void HideGirlNotification()
	{
		if (GameState.inInit || GameState.currentState == null || GameState.currentState.BottomUI == null)
		{
			return;
		}
		GameState.currentState.BottomUI.transform.Find("GirlsBTN/Notification").gameObject.SetActive(false);
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x0001493C File Offset: 0x00012B3C
	public static void UpdatePanels(GameState.UpdateType updateType)
	{
		GameState.updatePanels |= updateType;
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x0001494C File Offset: 0x00012B4C
	private void UpdatePanelsInternal()
	{
		GameObject.Find("Canvas").transform.Find("Girls").GetComponent<Girls>().SetGirl();
		if ((byte)(GameState.updatePanels & GameState.UpdateType.Skill) != 0)
		{
			Skills.UpdateAvatar();
			Transform transform = base.transform.Find("Jobs/Scroll View/Content Panel");
			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).GetComponent<Job2>().CheckLock();
			}
		}
		if ((byte)(GameState.updatePanels & GameState.UpdateType.Skill) != 0 || (byte)(GameState.updatePanels & GameState.UpdateType.Job) != 0)
		{
			this.CheckCellphoneUnlock();
		}
		if ((byte)(GameState.updatePanels & GameState.UpdateType.Money) != 0)
		{
			GameState.GetGirlScreen().UpdateSpeedRequirements();
		}
		if (base.transform.Find("Stats").gameObject.activeInHierarchy)
		{
			base.transform.Find("Stats").GetComponent<Skills>().UpdateStats();
		}
		GameState.updatePanels = GameState.UpdateType.None;
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x060002D4 RID: 724 RVA: 0x00014A40 File Offset: 0x00012C40
	// (set) Token: 0x060002D5 RID: 725 RVA: 0x00014A48 File Offset: 0x00012C48
	public static int PurchasedMultiplier
	{
		get
		{
			return GameState.purchasedMultiplier;
		}
		set
		{
			GameState.purchasedMultiplier = Math.Max(1, Math.Min(8192, value));
		}
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x060002D6 RID: 726 RVA: 0x00014A60 File Offset: 0x00012C60
	public static int PurchasedAdMultiplier
	{
		get
		{
			return GameState.purchasedMultiplier;
		}
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x00014A74 File Offset: 0x00012C74
	public void CheckCellphoneUnlock()
	{
		foreach (KeyValuePair<short, PhoneModel> keyValuePair in Universe.CellphoneGirls)
		{
			if (Cellphone.IsUnlocked(keyValuePair.Key) || Cellphone.IsVisible(keyValuePair.Key))
			{
				this.cellphoneUnlocked = true;
			}
		}
		if (this.cellphoneUnlocked)
		{
			base.transform.Find("Top UI/Cellphone").gameObject.SetActive(true);
		}
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x00014B24 File Offset: 0x00012D24
	private void Update()
	{
		if (!GameState.UniverseReady.Value)
		{
			return;
		}
		base.transform.Find("Saving").gameObject.SetActive(this.queueSave);
		base.transform.Find("Saving/Icon").eulerAngles = new Vector3(0f, 0f, base.transform.Find("Saving/Icon").eulerAngles.z - Time.deltaTime * 180f);
		DateTime now = DateTime.Now;
		float num = (float)(now - this.lastTime).TotalSeconds;
		if (num < 0f)
		{
			num = Time.deltaTime;
		}
		this.lastTime = now;
		GameState.seconds += num;
		while (GameState.seconds > 60f)
		{
			GameState.seconds -= 60f;
			GameState.TotalTime++;
			Achievements.HandleGameTime();
			if (GameState.TotalTime % 5 == 0)
			{
				this.QueueSave();
			}
		}
		if (this.cellphoneUnlocked && Time.frameCount % 60 == 0 && GameState.updatePanels != GameState.UpdateType.None)
		{
			if (this.cellphone == null)
			{
				this.cellphone = base.transform.Find("Popups/Cellphone").GetComponent<Cellphone>();
			}
			this.cellphone.UpdateNotifications();
		}
		if (GameState.updatePanels != GameState.UpdateType.None)
		{
			this.UpdatePanelsInternal();
		}
		GameState.UpdateType updateType = GameState.UpdateType.None;
		float dt = num * this.TimeMultiplier.Value * (float)Mathf.Max(1, GameState.PurchasedAdMultiplier) * Time.timeScale;
		for (int i = 0; i < GameState.FrameUpdates.Count; i++)
		{
			IUpdateable updateable = GameState.FrameUpdates[i];
			GameState.UpdateType updateType2 = updateable.PerformUpdate(dt);
			updateType |= updateType2;
			if (i < GameState.FrameUpdates.Count && GameState.FrameUpdates[i] != updateable)
			{
				i--;
			}
		}
		if (updateType != GameState.UpdateType.None)
		{
			GameState.UpdatePanels(updateType);
		}
		if (this.unlockedHobbies != GameState.UnlockedHobbies)
		{
			if (this.unlockedHobbies < 1 && global::PlayerPrefs.GetInt("Tutorial") < 4)
			{
				this.Intro.GetComponent<IntroScreen>().HobbyStart();
			}
			if (GameState.UnlockedHobbies > 0)
			{
				this.unlockedHobbies = Mathf.Max(0, this.unlockedHobbies);
				Transform transform = base.transform.Find("Hobbies/Scroll View/Content Panel");
				int num2 = this.unlockedHobbies;
				while (num2 < GameState.UnlockedHobbies && num2 < transform.childCount)
				{
					transform.GetChild(num2).GetComponent<Hobby2>().Unlock(true);
					num2++;
				}
			}
			this.unlockedHobbies = GameState.UnlockedHobbies;
			Kongregate.SubmitStat("UnlockedHobbies", (long)this.unlockedHobbies);
			global::PlayerPrefs.SetInt("GameStateHobbies", this.unlockedHobbies);
		}
		if (this.queueSave && this.timeSinceLastQueue <= 0f)
		{
			this.StoreState(true);
			this.queueSave = false;
			this.queueCloudSave = true;
			global::PlayerPrefs.Save();
		}
		else if (this.timeSinceLastQueue > 0f)
		{
			this.timeSinceLastQueue -= Time.deltaTime;
		}
		if (this.queueCloudSave && Time.timeSinceLevelLoad - this.lastCloudSave > 300f)
		{
			this.CloudSave(false);
			this.queueCloudSave = false;
		}
		if (this.consumeItems < 0f)
		{
			if (this.consumeItems + Time.deltaTime >= 0f)
			{
				BlayFapIntegration.ConsumeBlayFapInventory();
				this.consumeItems = 0f;
			}
			this.consumeItems += Time.deltaTime;
		}
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x00014EE4 File Offset: 0x000130E4
	private void LateUpdate()
	{
		if (!GameState.UniverseReady.Value)
		{
			return;
		}
		QueuedReactiveProperty.ProcessQueue();
	}

	// Token: 0x060002DA RID: 730 RVA: 0x00014EFC File Offset: 0x000130FC
	public void AwardCovid()
	{
		if (!Playfab.Promotion.Contains("covid"))
		{
			return;
		}
		if (global::PlayerPrefs.GetInt("GameStateCovid2020", 0) == 0)
		{
			Utilities.AwardDiamonds(20);
			global::PlayerPrefs.SetInt("GameStateCovid2020", 1);
			if (base.transform.Find("Store Revamp/Bundles Tab/Scroll View/Content/Covid/Diamond System") != null)
			{
				base.transform.Find("Store Revamp/Bundles Tab/Scroll View/Content/Covid/Diamond System").GetComponent<ParticleSystem>().Emit(20);
			}
			base.transform.Find("Store Revamp/Bundles Tab/Scroll View/Content/Covid/Button").GetComponent<Button>().interactable = false;
		}
	}

	// Token: 0x060002DB RID: 731 RVA: 0x00014F94 File Offset: 0x00013194
	public void CloudSave(bool notify)
	{
		if (Time.timeSinceLevelLoad - this.lastCloudSave < 60f)
		{
			return;
		}
		this.lastCloudSave = Time.timeSinceLevelLoad;
		if (BlayFapClient.UsingBlayfap)
		{
			if (this.sessionId == 0UL && !this.requestedSessionId)
			{
				this.requestedSessionId = true;
				GetUserSessionRequest request = new GetUserSessionRequest
				{
					BlayFapId = BlayFapClient.BlayFapId
				};
				BlayFapClient.Instance.GetDynamoUserSession(request, delegate(GetDynamoUserSessionResponse response)
				{
					if (response.Error == null)
					{
						this.sessionId = response.SessionID;
						Debug.Log("Got session: " + this.sessionId.ToString());
						this.UpdateCloudSession();
					}
				});
			}
			else if (this.sessionId != 0UL)
			{
				this.UpdateCloudSession();
			}
		}
	}

	// Token: 0x060002DC RID: 732 RVA: 0x00015030 File Offset: 0x00013230
	private void UpdateCloudSession()
	{
		UpdateUserSessionRequest request = new UpdateUserSessionRequest
		{
			SessionID = this.sessionId,
			Session = global::PlayerPrefs.Export(),
			BlayFapId = BlayFapClient.BlayFapId
		};
		BlayFapClient.Instance.UpdateDynamoUserSession(request, delegate(UpdateUserSessionResponse result)
		{
		});
	}

	// Token: 0x060002DD RID: 733 RVA: 0x00015090 File Offset: 0x00013290
	public void QueueQuickSave()
	{
		this.lastCloudSave = Time.timeSinceLevelLoad - 298f;
		this.queueSave = true;
		this.timeSinceLastQueue = 0f;
	}

	// Token: 0x060002DE RID: 734 RVA: 0x000150B8 File Offset: 0x000132B8
	public void QueueSave()
	{
		this.queueSave = true;
		this.timeSinceLastQueue = 5f;
	}

	// Token: 0x060002DF RID: 735 RVA: 0x000150CC File Offset: 0x000132CC
	public void SetNavigationButtonState(bool enabled)
	{
		Transform transform = base.transform.Find("Bottom UI");
		for (int i = 0; i < transform.childCount; i++)
		{
			Button component = transform.GetChild(i).GetComponent<Button>();
			if (!(component == null))
			{
				component.enabled = enabled;
			}
		}
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x00015128 File Offset: 0x00013328
	private void OnApplicationPause(bool pauseState)
	{
		if (GameState.UniverseReady.Value)
		{
			if (pauseState)
			{
				this.StoreState(true);
			}
			else
			{
				this.CalculateOfflineProgress();
			}
			this.FromAdd2xBoostAd = false;
		}
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x00015164 File Offset: 0x00013364
	public static void RegisterUpdate(IUpdateable update)
	{
		if (GameState.FrameUpdates.Contains(update))
		{
			return;
		}
		GameState.FrameUpdates.Add(update);
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x00015184 File Offset: 0x00013384
	public static void UnregisterUpdate(IUpdateable update)
	{
		if (GameState.FrameUpdates.Contains(update))
		{
			GameState.FrameUpdates.Remove(update);
		}
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x000151A4 File Offset: 0x000133A4
	public void Import()
	{
		if (Nutaku.Connected || Johren.Connected)
		{
			return;
		}
		if (BlayFapClient.LoggedIn && BlayFapClient.UsingBlayfap)
		{
			LoadSave.Import();
		}
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x000151E0 File Offset: 0x000133E0
	public void Export()
	{
		if (Nutaku.Connected || Johren.Connected)
		{
			this.CloudSave(true);
		}
		else if (BlayFapClient.LoggedIn && BlayFapClient.UsingBlayfap)
		{
			LoadSave.Export();
		}
		else
		{
			string str = global::PlayerPrefs.Export();
			Debug.Log("Save data: " + str);
		}
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x00015244 File Offset: 0x00013444
	public void StoreState(bool force = false)
	{
		if (!GameState.UniverseReady.Value)
		{
			return;
		}
		if (Time.frameCount % 60 == 0 || force)
		{
			global::PlayerPrefs.SetString("GameStateBuild", GameState.BuildDetails);
			global::PlayerPrefs.SetString("GameStateTotalIncome", GameState.TotalIncome.Value.ToString("F0"));
			global::PlayerPrefs.SetString("GameStateMoney", GameState.Money.Value.ToString("F0"));
			global::PlayerPrefs.SetString("GameStateDiamonds", GameState.Diamonds.Value.ToString());
			global::PlayerPrefs.SetString("GameStateDate", DateTime.Now.ToBinary().ToString());
			global::PlayerPrefs.SetString("GameStateDateUTC", DateTime.UtcNow.ToBinary().ToString());
			global::PlayerPrefs.SetString("GameStateLoginDate", BlayFapClient.LoginTime.ToBinary().ToString());
			global::PlayerPrefs.SetInt("GameStateDateCount", GameState.DateCount);
			global::PlayerPrefs.SetInt("GameStateGiftCount", GameState.GiftCount);
			global::PlayerPrefs.SetLong("GameStateHeartCount", GameState.HeartCount.Value);
			global::PlayerPrefs.SetInt("GameStatePokeCount", GameState.PokeCount);
			global::PlayerPrefs.SetFloat("GameStateSeconds", GameState.seconds);
			global::PlayerPrefs.SetInt("GameStateTotalTime", GameState.TotalTime);
			global::PlayerPrefs.SetFloat("GameStatePurchasedMultiplier", (float)GameState.purchasedMultiplier);
			global::PlayerPrefs.SetFloat("GameStatePendingMultiplier", this.PendingPrestige.Value);
			global::PlayerPrefs.SetLong("GameStateBoost2EndTime", this.Boost2EndTime.ToBinary());
			global::PlayerPrefs.SetLong("GameStateTimeSkip2EndTime", this.TimeSkip2EndTime.ToBinary());
			global::PlayerPrefs.SetLong("GameStateIncreaseIncome2EndTime", this.IncreaseIncome2EndTime.ToBinary());
			global::PlayerPrefs.SetLong("GameStatePhoneFlingAdCooldownTime", this.PhoneFlingAdCooldownTime.ToBinary());
			Skills.StoreState();
			foreach (IUpdateable updateable in GameState.FrameUpdates)
			{
				updateable.SaveCurrentTime();
			}
		}
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x00015478 File Offset: 0x00013678
	private void LoadState()
	{
		if (!GameState.Initialized)
		{
			return;
		}
		if (GameState.currentState == null)
		{
			GameState.currentState = this;
		}
		Job2.AvailableJobs = Requirement.JobType.None;
		try
		{
			GameState.Diamonds.Value = int.Parse(global::PlayerPrefs.GetString("GameStateDiamonds", "0"));
			if (GameState.Diamonds.Value == 0)
			{
				long @long = global::PlayerPrefs.GetLong("GameStateDiamonds", 0L);
				if (@long != 0L)
				{
					GameState.Diamonds.Value = (int)@long;
				}
			}
		}
		catch (Exception ex)
		{
			Utilities.SendAnalytic(Utilities.AnalyticType.Exception, "GameStateDiamonds integer constructor " + ex.Message + " contained " + global::PlayerPrefs.GetString("GameStateDiamonds", "0"));
			GameState.Diamonds.Value = 0;
		}
		try
		{
			GameState.Money.Value = double.Parse(global::PlayerPrefs.GetString("GameStateMoney", "0"));
			GameState.TotalIncome.Value = double.Parse(global::PlayerPrefs.GetString("GameStateTotalIncome", "0"));
		}
		catch (Exception ex2)
		{
			Utilities.SendAnalytic(Utilities.AnalyticType.Exception, "GameStateMoney constructor " + ex2.Message + " contained " + global::PlayerPrefs.GetString("GameStateMoney", "0"));
			Utilities.SendAnalytic(Utilities.AnalyticType.Exception, "GameStateTotalIncome constructor " + ex2.Message + " contained " + global::PlayerPrefs.GetString("GameStateTotalIncome", "0"));
		}
		this.unlockedHobbies = (GameState.UnlockedHobbies = global::PlayerPrefs.GetInt("GameStateHobbies", 0));
		GameState.DateCount = global::PlayerPrefs.GetInt("GameStateDateCount", 0);
		GameState.GiftCount = global::PlayerPrefs.GetInt("GameStateGiftCount", 0);
		GameState.HeartCount.Value = global::PlayerPrefs.GetLong("GameStateHeartCount", 0L);
		if (GameState.HeartCount.Value < 0L)
		{
			GameState.HeartCount.Value = long.MaxValue;
		}
		GameState.PokeCount = global::PlayerPrefs.GetInt("GameStatePokeCount", 0);
		GameState.seconds = global::PlayerPrefs.GetFloat("GameStateSeconds", 0f);
		GameState.TotalTime = Mathf.Max(0, global::PlayerPrefs.GetInt("GameStateTotalTime", 0));
		GameState.purchasedMultiplier = Mathf.RoundToInt(global::PlayerPrefs.GetFloat("GameStatePurchasedMultiplier", 1f));
		this.PendingPrestige.Value = global::PlayerPrefs.GetFloat("GameStatePendingMultiplier", 0f);
		Playfab.FlingPurchases = (Playfab.PhoneFlingPurchases)global::PlayerPrefs.GetLong("PlayfabFlingPurchases", 0L);
		this.Boost2EndTime = DateTime.FromBinary(global::PlayerPrefs.GetLong("GameStateBoost2EndTime", 0L));
		this.TimeSkip2EndTime = DateTime.FromBinary(global::PlayerPrefs.GetLong("GameStateTimeSkip2EndTime", 0L));
		this.IncreaseIncome2EndTime = DateTime.FromBinary(global::PlayerPrefs.GetLong("GameStateIncreaseIncome2EndTime", 0L));
		this.PhoneFlingAdCooldownTime = DateTime.FromBinary(global::PlayerPrefs.GetLong("GameStatePhoneFlingAdCooldownTime", 0L));
		Kongregate.SubmitStat("UnlockedHobbies", (long)this.unlockedHobbies);
		Kongregate.SubmitStat("UnlockedGirls", (long)Girls.UnlockedGirlCount);
		Kongregate.SubmitStat("PokeCount", (long)GameState.PokeCount);
		Kongregate.SubmitStat("DateCount", (long)GameState.DateCount);
		Kongregate.SubmitStat("GiftCount", (long)GameState.GiftCount);
		Transform transform = GameState.currentState.transform.Find("Jobs/Scroll View/Content Panel");
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).GetComponent<Job2>().LoadState();
		}
		Transform transform2 = base.transform.Find("Hobbies/Scroll View/Content Panel");
		for (int j = 0; j < transform2.childCount; j++)
		{
			transform2.GetChild(j).GetComponent<Hobby2>().LoadState();
			if (j < GameState.UnlockedHobbies || Skills.SkillLevel[j] > 0)
			{
				transform2.GetChild(j).GetComponent<Hobby2>().Unlock(false);
			}
		}
		this.CheckAchievements();
	}

	// Token: 0x060002E7 RID: 743 RVA: 0x0001585C File Offset: 0x00013A5C
	private DateTime GetNormalizedDateTime(DateTime currentTime, DateTime targetTime, TimeSpan maxAllowedTimeSpan)
	{
		if (targetTime > currentTime + maxAllowedTimeSpan)
		{
			return currentTime + maxAllowedTimeSpan;
		}
		return targetTime;
	}

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x060002E8 RID: 744 RVA: 0x0001587C File Offset: 0x00013A7C
	// (set) Token: 0x060002E9 RID: 745 RVA: 0x00015884 File Offset: 0x00013A84
	public DateTime SaveDateTime { get; private set; }

	// Token: 0x060002EA RID: 746 RVA: 0x00015890 File Offset: 0x00013A90
	private void CalculateOfflineProgress()
	{
		if (global::PlayerPrefs.GetString("GameStateDate") != string.Empty)
		{
			DateTime now = DateTime.Now;
			long dateData = 0L;
			if (!long.TryParse(global::PlayerPrefs.GetString("GameStateDate"), out dateData))
			{
				dateData = DateTime.Now.ToBinary();
			}
			this.SaveDateTime = DateTime.FromBinary(dateData);
			TimeSpan timeSpan = now.Subtract(this.SaveDateTime);
			float num = (float)((timeSpan.TotalDays < 7.0) ? timeSpan.TotalSeconds : 604800.0);
			if (num < 0f)
			{
				num = 0f;
			}
			GameState.TotalTime += Mathf.FloorToInt(num / 60f);
			float wallTime = num;
			num = this.GetBoosts(num);
			this.UpdateOfflineProgress(wallTime, num, OfflineProgress.ProgressReason.Offline);
		}
		else if (!string.IsNullOrEmpty(Playfab.Promotion) && !this.IsOfflinePopupEnabled())
		{
			this.LaunchPromotion();
		}
	}

	// Token: 0x060002EB RID: 747 RVA: 0x0001598C File Offset: 0x00013B8C
	private float GetBoosts(float seconds)
	{
		float num = (float)(this.Boost2EndTime - this.SaveDateTime).TotalSeconds;
		if (num != 0f)
		{
			float num2 = Mathf.Max(0f, Mathf.Min(seconds, num));
		}
		if (seconds > num)
		{
			global::PlayerPrefs.DeleteKey("GameStateBoost2EndTime", false);
		}
		float num3 = (float)GameState.purchasedMultiplier;
		return seconds * num3;
	}

	// Token: 0x060002EC RID: 748 RVA: 0x000159F4 File Offset: 0x00013BF4
	public void ReturnDiamonds()
	{
		Transform transform = GameState.currentState.transform.Find("Jobs/Scroll View/Content Panel");
		Transform transform2 = base.transform.Find("Hobbies/Scroll View/Content Panel");
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).GetComponent<Job2>().DisableJob();
		}
		for (int j = 0; j < transform2.childCount; j++)
		{
			transform2.GetChild(j).GetComponent<Hobby2>().DisableHobby();
		}
		FreeTime.PurchasedTime = 0;
		for (int k = 0; k < transform2.childCount; k++)
		{
			transform2.GetChild(k).GetComponent<Hobby2>().RemoveMultiplier();
		}
		GameState.purchasedMultiplier = 1;
		this.QueueSave();
		GameState.Diamonds.Value = this.Achievements.CalculateDiamonds();
	}

	// Token: 0x060002ED RID: 749 RVA: 0x00015ACC File Offset: 0x00013CCC
	public void UpdateJobProgress(float dt)
	{
		GameState.UpdateType updateType = GameState.UpdateType.None;
		dt = dt * this.TimeMultiplier.Value * (float)GameState.PurchasedAdMultiplier;
		for (int i = 0; i < GameState.FrameUpdates.Count; i++)
		{
			IUpdateable updateable = GameState.FrameUpdates[i];
			if (updateable is Job2)
			{
				updateType |= GameState.FrameUpdates[i].PerformUpdate(dt);
				if (i < GameState.FrameUpdates.Count && updateable != GameState.FrameUpdates[i])
				{
					i--;
				}
			}
		}
		if (updateType != GameState.UpdateType.None)
		{
			GameState.UpdatePanels(updateType);
		}
	}

	// Token: 0x060002EE RID: 750 RVA: 0x00015B70 File Offset: 0x00013D70
	public void UpdateHobbyProgress(float dt)
	{
		GameState.UpdateType updateType = GameState.UpdateType.None;
		dt = dt * this.TimeMultiplier.Value * (float)GameState.PurchasedAdMultiplier;
		for (int i = 0; i < GameState.FrameUpdates.Count; i++)
		{
			IUpdateable updateable = GameState.FrameUpdates[i];
			if (updateable is Hobby2)
			{
				updateType |= GameState.FrameUpdates[i].PerformUpdate(dt);
				if (i < GameState.FrameUpdates.Count && updateable != GameState.FrameUpdates[i])
				{
					i--;
				}
			}
		}
		if (updateType != GameState.UpdateType.None)
		{
			GameState.UpdatePanels(updateType);
		}
	}

	// Token: 0x060002EF RID: 751 RVA: 0x00015C14 File Offset: 0x00013E14
	public void UpdateProgress(float dt, bool includePurchasedMultiplier = true)
	{
		GameState.UpdateType updateType = GameState.UpdateType.None;
		dt *= this.TimeMultiplier.Value;
		if (includePurchasedMultiplier)
		{
			dt *= (float)GameState.PurchasedAdMultiplier;
		}
		for (int i = 0; i < GameState.FrameUpdates.Count; i++)
		{
			IUpdateable updateable = GameState.FrameUpdates[i];
			updateType |= GameState.FrameUpdates[i].PerformUpdate(dt);
			if (i < GameState.FrameUpdates.Count && updateable != GameState.FrameUpdates[i])
			{
				i--;
			}
		}
		if (updateType != GameState.UpdateType.None)
		{
			GameState.UpdatePanels(updateType);
		}
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x00015CB0 File Offset: 0x00013EB0
	public void UpdateOfflineProgress(float wallTime, float dt, OfflineProgress.ProgressReason reason = OfflineProgress.ProgressReason.None)
	{
		GC.Collect();
		if ((Nutaku.Connected || Johren.Connected) && reason != OfflineProgress.ProgressReason.NutakuRegistration)
		{
			int @int = global::PlayerPrefs.GetInt("GameStateUserGrade", 0);
			Nutaku.NutakuUserGrade nutakuUserGrade = (Nutaku.NutakuUserGrade)((@int != 0) ? @int : ((int)Nutaku.UserGrade));
			if (Nutaku.UserGrade != Nutaku.NutakuUserGrade.Guest && nutakuUserGrade == Nutaku.NutakuUserGrade.Guest)
			{
				global::PlayerPrefs.SetInt("GameStateUserGrade", (int)Nutaku.UserGrade);
				dt += 1800f;
				reason = OfflineProgress.ProgressReason.NutakuRegistration;
			}
		}
		double num = 0.0;
		double num2 = 0.0;
		double value = GameState.Money.Value;
		long sumOfSkills = Skills.SumOfSkills;
		for (int i = 0; i < Girl.ActiveGirls.Count; i++)
		{
			if (Girl.ActiveGirls[i] != null)
			{
				num += (double)Girl.ActiveGirls[i].Hearts;
			}
		}
		this.UpdateProgress(dt, reason != OfflineProgress.ProgressReason.Offline && reason != OfflineProgress.ProgressReason.DoubledProgress);
		for (int j = 0; j < Girl.ActiveGirls.Count; j++)
		{
			if (Girl.ActiveGirls[j] != null)
			{
				num2 += (double)Girl.ActiveGirls[j].Hearts;
			}
		}
		double num3 = GameState.Money.Value - value;
		long num4 = Skills.SumOfSkills - sumOfSkills;
		double num5 = num2 - num;
		if (num4 > 0L)
		{
			GameState.UpdatePanels(GameState.UpdateType.Skill);
		}
		TimeSpan timeSpan = DateTime.Now.Subtract(this.SaveDateTime);
		bool flag = true;
		if ((reason != OfflineProgress.ProgressReason.Offline || (!this.FromAdd2xBoostAd && flag)) && (num3 > 0.0 || num4 > 0L || num5 > 0.0))
		{
			base.transform.Find("Popups/Offline Progress").GetComponent<OfflineProgress>().Init(wallTime, dt, num3, num4, num5, reason);
		}
		else if (!string.IsNullOrEmpty(Playfab.Promotion) && !this.IsOfflinePopupEnabled())
		{
			this.LaunchPromotion();
		}
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x00015ED0 File Offset: 0x000140D0
	public void UpdateOfflineProgressMoney(double moneyValue)
	{
		double num = moneyValue - GameState.Money.Value;
		GameState.Money.Value = moneyValue;
		GameState.TotalIncome.Value += num;
		base.transform.Find("Popups/Offline Progress").GetComponent<OfflineProgress>().Init(0f, 0f, num, 0L, 0.0, OfflineProgress.ProgressReason.KongregateAdIncreaseMoney);
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x00015F38 File Offset: 0x00014138
	private bool IsOfflinePopupEnabled()
	{
		return base.transform != null && base.transform.Find("Popups/Offline Progress").gameObject.activeSelf;
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x00015F74 File Offset: 0x00014174
	public void OpenTwitter()
	{
		Application.OpenURL("https://twitter.com/CrushCrushDX");
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x00015F80 File Offset: 0x00014180
	public void OpenPrivacyPolicy()
	{
		Utilities.OpenPrivacyPolicy();
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x00015F88 File Offset: 0x00014188
	public void OpenTermsOfUse()
	{
		Utilities.OpenTermsOfUse();
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x00015F90 File Offset: 0x00014190
	public void Quit()
	{
		global::PlayerPrefs.Save();
		Application.Quit();
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x00015F9C File Offset: 0x0001419C
	private void OnApplicationFocus(bool focusStatus)
	{
		if (!focusStatus)
		{
			Application.targetFrameRate = 10;
		}
		else
		{
			Application.targetFrameRate = 60;
		}
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x00015FB8 File Offset: 0x000141B8
	private void OnDestroy()
	{
		this._isDestroyed = true;
		base.StopAllCoroutines();
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x00015FC8 File Offset: 0x000141C8
	public void PrepareReset()
	{
		GameState.FrameUpdates.Clear();
		Album.AlbumSprites.Clear();
		if (Job2.ActiveJobs != null)
		{
			foreach (Job2 job in Job2.ActiveJobs)
			{
				job.Reset();
			}
		}
		Job2.ActiveJobs.Clear();
		Job2.AvailableJobs = Requirement.JobType.None;
		if (Hobby2.ActiveHobbies != null)
		{
			foreach (Hobby2 hobby in Hobby2.ActiveHobbies)
			{
				hobby.Reset();
			}
		}
		Hobby2.AvailableHobbies.Clear();
		Hobby2.ActiveHobbies.Clear();
		Skills.Reset();
		FreeTime.EventTime = (FreeTime.HobbyTime = (FreeTime.DateTime = (FreeTime.JobTime = 0)));
		GameState.Money.Value = 0.0;
		GameState.UnlockedHobbies = (this.unlockedHobbies = -1);
		GameState.Initialized = false;
		GameState.UniverseReady.Value = false;
		Translations.isHardReset = true;
		IntroScreen.TutorialState = IntroScreen.State.GirlsActive;
		Notifications.Clear();
		Album.Clear();
		GameState.updatedStore = false;
		TitleScreen.NetworkTimeout = 0f;
		GameState.UniverseReady.Value = false;
	}

	// Token: 0x060002FA RID: 762 RVA: 0x00016150 File Offset: 0x00014350
	public void CheckMaxCapBeforePrestige()
	{
		if (this.TimeMultiplier.Value >= 2048f)
		{
			base.transform.Find("Popups/Confirm Capped Reset").gameObject.SetActive(true);
		}
		else
		{
			this.Prestige();
		}
	}

	// Token: 0x0400030E RID: 782
	public static QueuedReactiveProperty<double> TotalIncome = new QueuedReactiveProperty<double>(-1.0);

	// Token: 0x0400030F RID: 783
	public static QueuedReactiveProperty<double> Money = new QueuedReactiveProperty<double>(0.0);

	// Token: 0x04000310 RID: 784
	public static ReactiveProperty<int> Diamonds = new ReactiveProperty<int>(0);

	// Token: 0x04000311 RID: 785
	private GameObject Intro;

	// Token: 0x04000312 RID: 786
	private GameObject GirlScreen;

	// Token: 0x04000313 RID: 787
	private GameObject BottomUI;

	// Token: 0x04000314 RID: 788
	public TaskManager TaskSystem;

	// Token: 0x04000315 RID: 789
	public Cellphone Cellphone;

	// Token: 0x04000316 RID: 790
	private Achievements Achievements;

	// Token: 0x04000317 RID: 791
	private static int initialTimeBlocks = 6;

	// Token: 0x04000318 RID: 792
	private static int _dateCount;

	// Token: 0x04000319 RID: 793
	private static int _giftCount;

	// Token: 0x0400031A RID: 794
	private static int _pokeCount;

	// Token: 0x0400031B RID: 795
	public static QueuedReactiveProperty<long> HeartCount = new QueuedReactiveProperty<long>(0L);

	// Token: 0x0400031C RID: 796
	private static float seconds;

	// Token: 0x0400031D RID: 797
	public static int TotalTime;

	// Token: 0x0400031E RID: 798
	public Text MoneyText;

	// Token: 0x0400031F RID: 799
	public Text DiamondText;

	// Token: 0x04000320 RID: 800
	public static ReactiveProperty<bool> UniverseReady = new ReactiveProperty<bool>(false);

	// Token: 0x04000321 RID: 801
	public static bool DebugSpeedDating = false;

	// Token: 0x04000322 RID: 802
	public float consumeItems;

	// Token: 0x04000323 RID: 803
	private bool _isDestroyed;

	// Token: 0x04000324 RID: 804
	private static GameState currentState;

	// Token: 0x04000325 RID: 805
	public ReactiveProperty<float> PendingPrestige = new ReactiveProperty<float>(0f);

	// Token: 0x04000326 RID: 806
	public static string BuildDetails;

	// Token: 0x04000327 RID: 807
	private static bool shownPromo = false;

	// Token: 0x04000328 RID: 808
	private static bool updatedStore = false;

	// Token: 0x04000329 RID: 809
	private PlatformAchievements platformAchievements;

	// Token: 0x0400032A RID: 810
	public static int CurrentVersion = 373;

	// Token: 0x0400032B RID: 811
	public static bool NSFW = false;

	// Token: 0x0400032C RID: 812
	public static bool NSFWAllowed = false;

	// Token: 0x0400032D RID: 813
	public static AssetBundleManager AssetManager;

	// Token: 0x0400032E RID: 814
	private static bool inInit = true;

	// Token: 0x0400032F RID: 815
	private static GameState.UpdateType updatePanels = GameState.UpdateType.None;

	// Token: 0x04000330 RID: 816
	public ReactiveProperty<float> TimeMultiplier = new ReactiveProperty<float>(1f);

	// Token: 0x04000331 RID: 817
	private static int purchasedMultiplier = 1;

	// Token: 0x04000332 RID: 818
	public DateTime Boost2EndTime;

	// Token: 0x04000333 RID: 819
	public DateTime TimeSkip2EndTime;

	// Token: 0x04000334 RID: 820
	public DateTime IncreaseIncome2EndTime;

	// Token: 0x04000335 RID: 821
	public DateTime PhoneFlingAdCooldownTime;

	// Token: 0x04000336 RID: 822
	private bool _hasCheckedServerTime;

	// Token: 0x04000337 RID: 823
	[HideInInspector]
	public bool FromAdd2xBoostAd;

	// Token: 0x04000338 RID: 824
	private DateTime lastTime = DateTime.Now;

	// Token: 0x04000339 RID: 825
	private float timeSinceLastQueue;

	// Token: 0x0400033A RID: 826
	private Cellphone cellphone;

	// Token: 0x0400033B RID: 827
	private bool cellphoneUnlocked;

	// Token: 0x0400033C RID: 828
	private float lastCloudSave = 60f;

	// Token: 0x0400033D RID: 829
	private ulong sessionId;

	// Token: 0x0400033E RID: 830
	private bool requestedSessionId;

	// Token: 0x0400033F RID: 831
	public static List<IUpdateable> FrameUpdates = new List<IUpdateable>();

	// Token: 0x04000340 RID: 832
	private int unlockedHobbies = -1;

	// Token: 0x04000341 RID: 833
	public static int UnlockedHobbies = -1;

	// Token: 0x04000342 RID: 834
	public static bool Initialized = false;

	// Token: 0x04000343 RID: 835
	private static bool IsHardReset = false;

	// Token: 0x04000344 RID: 836
	private bool queueSave;

	// Token: 0x04000345 RID: 837
	private bool queueCloudSave;

	// Token: 0x02000096 RID: 150
	[Flags]
	public enum UpdateType : byte
	{
		// Token: 0x0400034B RID: 843
		None = 0,
		// Token: 0x0400034C RID: 844
		Money = 1,
		// Token: 0x0400034D RID: 845
		Skill = 2,
		// Token: 0x0400034E RID: 846
		Job = 4
	}
}
