using System;
using System.Collections.Generic;
using BlayFap;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000E5 RID: 229
public class TaskManager : MonoBehaviour
{
	// Token: 0x1700006E RID: 110
	// (get) Token: 0x060004FF RID: 1279 RVA: 0x00027444 File Offset: 0x00025644
	public EventData CurrentEvent
	{
		get
		{
			return this.currentEvent;
		}
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x0002744C File Offset: 0x0002564C
	private void OnDisable()
	{
		if (BlayFapIntegration.IsTestDevice)
		{
			UnityEngine.Object.FindObjectOfType<DebugCanvasController>().HandleLteWidget(false);
		}
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x00027464 File Offset: 0x00025664
	private static string GetStreamingAssetsPath()
	{
		if (Application.isEditor)
		{
			return Environment.CurrentDirectory.Replace("\\", "/");
		}
		return Application.streamingAssetsPath;
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x00027498 File Offset: 0x00025698
	private void LoadEvent()
	{
		Utilities.CheckCachedServerTime(delegate
		{
			this.CheckEvent(true);
		});
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x000274AC File Offset: 0x000256AC
	public void CompareAgainstServer()
	{
		if (!GameState.UniverseReady.Value)
		{
			return;
		}
		TaskManager.CompletedEvents = TaskManager.FromBase64Safely(global::PlayerPrefs.GetString("CompletedEvents", string.Empty));
		if (!string.IsNullOrEmpty(BlayFapIntegration.CompletedEventsBlayfap))
		{
			byte[] array = TaskManager.FromBase64Safely(BlayFapIntegration.CompletedEventsBlayfap);
			if (array.Length > TaskManager.CompletedEvents.Length)
			{
				byte[] completedEvents = TaskManager.CompletedEvents;
				TaskManager.CompletedEvents = new byte[array.Length];
				Array.Copy(completedEvents, TaskManager.CompletedEvents, completedEvents.Length);
			}
			short num = 0;
			while ((int)num < array.Length * 8)
			{
				byte[] completedEvents2 = TaskManager.CompletedEvents;
				short num2 = num / 8;
				completedEvents2[(int)num2] = (completedEvents2[(int)num2] | array[(int)(num / 8)]);
				num += 1;
			}
			global::PlayerPrefs.SetString("CompletedEvents", Convert.ToBase64String(TaskManager.CompletedEvents));
		}
		this.pendingAchievementRebuild = false;
		short num3 = 0;
		while ((int)num3 < TaskManager.CompletedEvents.Length * 8)
		{
			if (TaskManager.IsCompletedEventSet((int)num3))
			{
				this.Claim(num3, false, false, false);
			}
			num3 += 1;
		}
		if (this.pendingAchievementRebuild)
		{
			GameState.GetAchievements().Rebuild();
		}
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x000275BC File Offset: 0x000257BC
	public void OpenLTEPanel()
	{
		if (!BlayFapClient.LoggedIn)
		{
			Notifications.AddNotification(Notifications.NotificationType.Message, Translations.GetTranslation("limited_time_events_26_0", "Could not connect to our backend server.  Do you have internet access?"));
			return;
		}
		if (this.currentEvent == null)
		{
			this.LoadEventInternal();
		}
		if (this.currentEvent == null)
		{
			Notifications.AddNotification(Notifications.NotificationType.Message, Translations.GetTranslation("limited_time_events_27_0", "Could not load the current event."));
			return;
		}
		if (BlayFapIntegration.IsTestDevice && UnityEngine.Object.FindObjectOfType<DebugCanvasController>() != null)
		{
			UnityEngine.Object.FindObjectOfType<DebugCanvasController>().HandleLteWidget(true);
		}
		base.gameObject.SetActive(true);
		this.tick = 1f;
		string str = (!(base.transform.Find("Dialog") != null)) ? string.Empty : "Dialog/";
		base.transform.Find(str + "Rewards/Image/Already Owned").gameObject.SetActive(this.HasEquivalent(this.currentEvent.EventID));
		this.SetUpPreviousReward();
		this.UpdateGirlOutfits();
		this.CompareAgainstServer();
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x000276D4 File Offset: 0x000258D4
	public void UpdateGirlOutfits()
	{
		if ((Girl.FindGirl(Balance.GirlName.Odango).LifetimeOutfits & Requirement.OutfitType.BathingSuit) != Requirement.OutfitType.None)
		{
			TaskManager.SetCompletedEventBit(123);
		}
		if ((Girl.FindGirl(Balance.GirlName.Shibuki).LifetimeOutfits & Requirement.OutfitType.BathingSuit) != Requirement.OutfitType.None)
		{
			TaskManager.SetCompletedEventBit(138);
		}
		if ((Girl.FindGirl(Balance.GirlName.Sirina).LifetimeOutfits & Requirement.OutfitType.BathingSuit) != Requirement.OutfitType.None)
		{
			TaskManager.SetCompletedEventBit(132);
		}
		if ((Girl.FindGirl(Balance.GirlName.Vellatrix).LifetimeOutfits & Requirement.OutfitType.BathingSuit) != Requirement.OutfitType.None)
		{
			TaskManager.SetCompletedEventBit(126);
		}
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x00027760 File Offset: 0x00025960
	public void RefundTokens()
	{
		int num = BlayFapIntegration.EventTokenRefund;
		Debug.Log(string.Format("Found event token refund of {0}", num.ToString()));
		if (num == 0)
		{
			return;
		}
		if (this.currentEvent == null || this.EventTasks == null || this.EventTasks.Length == 0)
		{
			this.LoadEventInternal();
			if (this.currentEvent == null)
			{
				return;
			}
		}
		int num2 = 0;
		while (num2 * 3 < this.EventTasks.Length && num > 0)
		{
			if (!this.EventTasks[num2 * 3].Claimed)
			{
				this.EventTasks[num2 * 3].Complete = (this.EventTasks[num2 * 3].Claimed = true);
				num--;
			}
			num2++;
		}
		this.StoreState();
		BlayFapIntegration.EventTokenRefund = 0;
		string str = (!(base.transform.Find("Dialog") != null)) ? "Tasks/" : "Dialog/Tasks/Layout/";
		TimeSpan timeOffset = Utilities.TimeOffset;
		this.SetTask(this.EventTasks[this.CurrentDay * 3], base.transform.Find(str + "Task 1"), timeOffset);
		this.UpdateDayIcons();
	}

	// Token: 0x06000507 RID: 1287 RVA: 0x000278A0 File Offset: 0x00025AA0
	public void CheckEvent(bool loadEvent = false)
	{
		if (!GameState.CurrentState.IsLoaded)
		{
			Debug.LogError("Universe was not ready yet");
			return;
		}
		Utilities.CheckCachedServerTime(null);
		if (Utilities.TimeRequested)
		{
			return;
		}
		if (Utilities.ServerTime.Year == 1)
		{
			if (GameState.CurrentState != null)
			{
				GameState.CurrentState.transform.Find("Top UI/Task Blocker").gameObject.SetActive(false);
			}
			this.currentEvent = null;
			base.gameObject.SetActive(false);
			return;
		}
		TimeSpan timeOffset = Utilities.TimeOffset;
		EventData evt = this.currentEvent;
		this.currentEvent = null;
		DateTime dateTime = DateTime.UtcNow - timeOffset;
		foreach (KeyValuePair<short, EventData> keyValuePair in Universe.Events)
		{
			if (dateTime > keyValuePair.Value.StartTimeUTC && dateTime < keyValuePair.Value.EndTimeUTC)
			{
				this.currentEvent = keyValuePair.Value;
				this.SetNewcomerEvents(dateTime);
				break;
			}
		}
		if (this.currentEvent == null || evt == this.currentEvent)
		{
			return;
		}
		if (loadEvent)
		{
			this.LoadEventInternal();
		}
		if (this.currentEvent.RewardType.Contains(TaskManager.EventRewardType.NewGirl))
		{
			for (int i = 0; i < this.currentEvent.RewardGirlsID.Length; i++)
			{
				Girl girl = Girl.FindGirl((Balance.GirlName)(this.currentEvent.RewardGirlsID[i] - 1));
				if (girl != null)
				{
					girl.SetLTECallback();
				}
			}
		}
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x00027A7C File Offset: 0x00025C7C
	private void SetUpPreviousReward()
	{
		string prefix = (!(base.transform.Find("Dialog") != null)) ? string.Empty : "Dialog/";
		bool flag = Universe.Events.ContainsKey(this.currentEvent.PreviousEventID) && TaskManager.IsCompletedEventSet((int)this.currentEvent.PreviousEventID);
		flag |= this.HasEquivalent(this.currentEvent.PreviousEventID);
		base.transform.Find(prefix + "Upcoming Events").gameObject.SetActive(this.currentEvent.PreviousEventID < 0 || flag);
		base.transform.Find(prefix + "Miss Reward").gameObject.SetActive(this.currentEvent.PreviousEventID >= 0 && !flag);
		if (this.currentEvent.PreviousEventID >= 0 && Universe.Events.ContainsKey(this.currentEvent.PreviousEventID))
		{
			EventData previousEvent = Universe.Events[this.currentEvent.PreviousEventID];
			int finishCost = previousEvent.FinishCost;
			int num = Math.Max(0, global::PlayerPrefs.GetInt(string.Format("Event{0}Tokens", previousEvent.EventID.ToString()), 0));
			finishCost -= num * previousEvent.CostPerToken;
			finishCost = Math.Max(0, Math.Min(previousEvent.FinishCost, finishCost));
			if (previousEvent.Icon == null)
			{
				GameState.AssetManager.GetBundle("universe/" + previousEvent.AssetBundleName, false, delegate(AssetBundle bundle)
				{
					if (bundle != null)
					{
						previousEvent.Icon = bundle.LoadAsset<Sprite>(previousEvent.IconName);
						this.transform.Find(prefix + "Miss Reward/Icon").GetComponent<Image>().sprite = previousEvent.Icon;
						GameState.AssetManager.UnloadBundle(bundle);
					}
				});
			}
			else
			{
				base.transform.Find(prefix + "Miss Reward/Icon").GetComponent<Image>().sprite = previousEvent.Icon;
			}
			if (previousEvent.RewardText.StartsWith("You've unlocked"))
			{
				base.transform.Find(prefix + "Miss Reward/Text").GetComponent<Text>().text = "You missed " + previousEvent.RewardText.Substring(16);
			}
			else
			{
				base.transform.Find(prefix + "Miss Reward/Text").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_6_0", "You missed the event!");
			}
			base.transform.Find(prefix + "Miss Reward/Purchase Last Event/Cost").GetComponent<Text>().text = finishCost.ToString();
			Button component = base.transform.Find(prefix + "Miss Reward/Purchase Last Event").GetComponent<Button>();
			component.onClick.RemoveAllListeners();
			component.onClick.AddListener(delegate()
			{
				this.ConfirmMissedPurchase(finishCost, previousEvent.Name, delegate
				{
					this.Claim(previousEvent.EventID, true, true, true);
					this.transform.Find(prefix + "Miss Reward").gameObject.SetActive(false);
					this.transform.Find(prefix + "Upcoming Events").gameObject.SetActive(true);
				});
			});
		}
	}

	// Token: 0x06000509 RID: 1289 RVA: 0x00027DE4 File Offset: 0x00025FE4
	public void LoadEventInternal()
	{
		this.CheckEvent(false);
		if (this.currentEvent == null)
		{
			return;
		}
		string prefix = (!(base.transform.Find("Dialog") != null)) ? string.Empty : "Dialog/";
		string text = this.currentEvent.Name.Replace("\\n", "\n");
		if (!text.Contains("\n"))
		{
			text = text + "\n" + string.Format(Translations.GetTranslation("limited_time_events_21_0", "Collect {0} Tokens!"), this.currentEvent.TokenRequirement);
		}
		base.transform.Find(prefix + "Title").GetComponent<Text>().text = text;
		this.newDailyTasksTimer = base.transform.Find(prefix + "Tasks/New Tasks Timer").GetComponent<Text>();
		this.nextEventTimer = base.transform.Find(prefix + "Rewards/Next Event Timer").GetComponent<Text>();
		this.offerExpiresTimer = base.transform.Find(prefix + "Miss Reward/Offer Ends").GetComponent<Text>();
		this.GetTasks();
		this.LoadState();
		this.StoreEventDiscount();
		Transform transform = base.transform.Find(prefix + "Rewards/Days");
		for (int i = transform.childCount - 1; i >= 0; i--)
		{
			UnityEngine.Object.DestroyImmediate(transform.GetChild(i).gameObject);
		}
		int num = 0;
		while ((double)num < this.currentEvent.timeSpan.TotalDays)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.TaskDay, transform);
			gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
			gameObject.transform.Find("Day").GetComponent<Text>().text = (num + 1).ToString();
			num++;
		}
		if (this.currentEvent.RewardSprite == null)
		{
			GameState.AssetManager.GetBundle("universe/" + this.currentEvent.AssetBundleName, false, delegate(AssetBundle bundle)
			{
				if (bundle != null)
				{
					if (this.currentEvent.RewardType.Count == 1 && this.currentEvent.RewardType[0] == TaskManager.EventRewardType.NewGirl)
					{
						Girl girl = Girl.FindGirl((Balance.GirlName)(this.currentEvent.RewardGirlsID[0] - 1));
						Sprite sprite = girl.transform.Find("Unlocked").GetComponent<Image>().sprite;
						this.currentEvent.RewardSprite = ((!(girl.LoverIcon == null)) ? girl.LoverIcon : sprite);
					}
					else
					{
						this.currentEvent.RewardSprite = bundle.LoadAsset<Sprite>(this.currentEvent.RewardSpriteName);
					}
					this.currentEvent.Icon = bundle.LoadAsset<Sprite>(this.currentEvent.IconName);
					if (Universe.Events[this.currentEvent.EventID] != this.currentEvent)
					{
						Universe.Events[this.currentEvent.EventID].RewardSprite = this.currentEvent.RewardSprite;
						Universe.Events[this.currentEvent.EventID].Icon = this.currentEvent.Icon;
					}
					this.transform.Find(prefix + "Rewards/Image").GetComponent<Image>().sprite = this.currentEvent.RewardSprite;
					GameState.AssetManager.UnloadBundle(bundle);
				}
			});
		}
		else
		{
			base.transform.Find(prefix + "Rewards/Image").GetComponent<Image>().sprite = this.currentEvent.RewardSprite;
		}
		Text component = base.transform.Find(prefix + "Rewards/Image/Text").GetComponent<Text>();
		component.gameObject.SetActive(this.currentEvent.RewardSpriteName.StartsWith("LTEreward_outfits"));
		if (this.currentEvent.RewardSpriteName.StartsWith("LTEreward_outfits"))
		{
			component.text = string.Format("{0}'s Outfits", Translations.TranslateGirlName((Balance.GirlName)(this.currentEvent.RewardGirlsID[0] - 1)));
		}
		Text component2 = base.transform.Find(prefix + "Rewards/Image/Name").GetComponent<Text>();
		component2.gameObject.SetActive(this.currentEvent.RewardType.Count == 1 && this.currentEvent.RewardType[0] == TaskManager.EventRewardType.NewGirl);
		if (this.currentEvent.RewardType.Count == 1 && this.currentEvent.RewardType[0] == TaskManager.EventRewardType.NewGirl)
		{
			component2.text = Translations.TranslateGirlName((Balance.GirlName)(this.currentEvent.RewardGirlsID[0] - 1)).ToUpperInvariant();
			component2.color = Universe.Girls[this.currentEvent.RewardGirlsID[0]].TextColor;
		}
		base.transform.Find(prefix + "Rewards/Image/Already Owned").gameObject.SetActive(this.HasEquivalent(this.currentEvent.EventID));
		this.UpdateDayIcons();
		this.SetUpPreviousReward();
		this.RefundTokens();
		if (this.currentEvent != null)
		{
			GameState.CurrentState.transform.Find("Top UI/Task Blocker/Task Button").GetComponent<TaskCountdown>().Init(this.currentEvent);
		}
		else
		{
			GameState.CurrentState.transform.Find("Top UI/Task Blocker/Task Button").gameObject.SetActive(false);
		}
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x00028284 File Offset: 0x00026484
	private void SetNewcomerEvents(DateTime currentDate)
	{
		if (Girl.GetLove(Balance.GirlName.Cassie) <= 1 && IntroScreen.TutorialState < IntroScreen.State.JobsActive && !global::PlayerPrefs.HasKey(TaskManager.NEWCOMER_LTE_START_DATETIME))
		{
			if (global::PlayerPrefs.GetInt("GirlCassieLove", 0) > 1 || global::PlayerPrefs.GetInt("EventID", 0) != 0)
			{
				return;
			}
			global::PlayerPrefs.SetLong(TaskManager.NEWCOMER_LTE_START_DATETIME, currentDate.Date.ToBinary());
			global::PlayerPrefs.SetInt(TaskManager.EVENT_NEWCOMER_ID, (int)this.currentEvent.EventID);
		}
		else if (!global::PlayerPrefs.HasKey(TaskManager.NEWCOMER_LTE_START_DATETIME))
		{
			return;
		}
		short num = (short)global::PlayerPrefs.GetInt(TaskManager.EVENT_NEWCOMER_ID, -1);
		EventData eventData = Universe.Events[num];
		if (eventData == null)
		{
			return;
		}
		List<EventData> list = new List<EventData>();
		DateTime dateTime = DateTime.FromBinary(global::PlayerPrefs.GetLong(TaskManager.NEWCOMER_LTE_START_DATETIME, -1L));
		int num2 = Mathf.CeilToInt((float)(eventData.EndTimeUTC - dateTime).TotalDays);
		EventData eventData2 = null;
		EventData eventData3 = null;
		EventData eventData4 = null;
		bool flag = eventData.RewardType.Contains(TaskManager.EventRewardType.AllOutfits);
		if (num2 == eventData.TokenRequirement)
		{
			if (flag)
			{
				if (Universe.Events.TryGetValue(num - 1, out eventData2))
				{
					eventData2 = eventData2.Clone();
				}
				if (Universe.Events.TryGetValue(num + 1, out eventData3))
				{
					eventData3 = eventData3.Clone();
				}
			}
			else
			{
				eventData2 = eventData.Clone();
			}
		}
		else
		{
			if (!Universe.Events.TryGetValue(TaskManager.GetNewcomerEventId(), out eventData2))
			{
				return;
			}
			eventData2 = eventData2.Clone();
			bool flag2 = Universe.Events.ContainsKey(num + 1) && Universe.Events[num + 1] != null && Universe.Events[num + 1].RewardType.Contains(TaskManager.EventRewardType.AllOutfits);
			if (eventData.RewardType.Contains(TaskManager.EventRewardType.NewGirl) && flag2)
			{
				eventData3 = eventData.Clone();
				if (Universe.Events.TryGetValue(num + 2, out eventData4))
				{
					eventData4 = eventData4.Clone();
				}
			}
			else if (Universe.Events.TryGetValue(num + 1, out eventData3))
			{
				eventData3 = eventData3.Clone();
			}
		}
		int finishCost = (eventData2.EventID != TaskManager.GetNewcomerEventId()) ? eventData2.FinishCost : (eventData2.CostPerToken * num2);
		if (eventData2.EventID == TaskManager.GetNewcomerEventId() && this.currentEvent != null && this.currentEvent.EventID != eventData2.EventID)
		{
			EventData eventData5 = Universe.Events[TaskManager.GetNewcomerEventId()];
			eventData5.ModifyEvent(eventData5.StartTimeUTC, eventData5.EndTimeUTC, eventData5.TokenRequirement, finishCost, -1);
		}
		eventData2.ModifyEvent(dateTime, eventData.EndTimeUTC, num2, finishCost, -1);
		list.Add(eventData2);
		if (eventData3 != null)
		{
			list.Add(eventData3);
			EventData eventData6 = Universe.Events[num + 1];
			finishCost = Universe.Events[eventData3.EventID].FinishCost;
			eventData3.ModifyEvent(eventData6.StartTimeUTC, eventData6.EndTimeUTC, eventData6.TokenRequirement, finishCost, eventData2.EventID);
		}
		if (eventData4 != null)
		{
			list.Add(eventData4);
			EventData eventData7 = Universe.Events[num + 2];
			finishCost = Universe.Events[eventData4.EventID].FinishCost;
			eventData4.ModifyEvent(eventData7.StartTimeUTC, eventData7.EndTimeUTC, eventData7.TokenRequirement, finishCost, eventData3.EventID);
		}
		EventData evt = null;
		for (int i = 0; i < list.Count; i++)
		{
			EventData eventData8 = list[i];
			if (currentDate > eventData8.StartTimeUTC && currentDate < eventData8.EndTimeUTC)
			{
				evt = eventData8;
				break;
			}
		}
		if (evt != null)
		{
			this.currentEvent = evt;
		}
		else
		{
			if ((int)num + list.Count < Universe.Events.Count)
			{
				this.currentEvent = Universe.Events[(short)((int)num + list.Count)];
			}
			TaskManager.DeleteNewcomerKeys();
		}
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x000286D4 File Offset: 0x000268D4
	public static void DeleteNewcomerKeys()
	{
		global::PlayerPrefs.DeleteKey(TaskManager.NEWCOMER_LTE_START_DATETIME, false);
		global::PlayerPrefs.DeleteKey(TaskManager.EVENT_NEWCOMER_ID, false);
		GameState.CurrentState.QueueQuickSave();
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x00028704 File Offset: 0x00026904
	public static short GetNewcomerEventId()
	{
		return 118;
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x00028718 File Offset: 0x00026918
	private void ConfirmMissedPurchase(int diamonds, string eventText, Action OnSuccess)
	{
		Transform confirmPurchase = base.transform.Find("Confirm Previous Purchase");
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
			confirmPurchase.transform.Find("Dialog/Event").GetComponent<Text>().text = string.Format("Missed Event: {0}", eventText);
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
			});
			confirmPurchase.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x0002883C File Offset: 0x00026A3C
	private int UpdateDayIcons()
	{
		string prefix = (!(base.transform.Find("Dialog") != null)) ? string.Empty : "Dialog/";
		int tokens = 0;
		Transform transform = base.transform.Find(prefix + "Rewards/Days");
		int num = 0;
		while ((double)num < this.currentEvent.timeSpan.TotalDays)
		{
			Transform child = transform.GetChild(num);
			child.transform.Find("Complete").gameObject.SetActive(this.EventTasks[num * 3].Claimed);
			child.transform.Find("Missed").gameObject.SetActive(!this.EventTasks[num * 3].Claimed && num < this.CurrentDay);
			if (this.EventTasks[num * 3].Claimed)
			{
				tokens++;
			}
			child.GetComponent<Image>().color = ((this.CurrentDay != num) ? Color.white : new Color(0.5019608f, 1f, 0.60784316f));
			num++;
		}
		if (tokens >= this.currentEvent.TokenRequirement && !TaskManager.IsCompletedEventSet((int)this.currentEvent.EventID))
		{
			this.Claim(this.currentEvent.EventID, true, true, true);
		}
		bool flag = this.HasEquivalent(this.currentEvent.EventID);
		int cost = (tokens >= this.currentEvent.TokenRequirement) ? 0 : (this.currentEvent.FinishCost - this.currentEvent.CostPerToken * tokens);
		if (cost < 0 || flag)
		{
			cost = 0;
		}
		base.transform.Find(prefix + "Rewards/Progress Text").GetComponent<Text>().text = string.Format("{0} {1}/{2}", Translations.GetTranslation("limited_time_events_20_0", "Progress:"), tokens.ToString(), this.currentEvent.TokenRequirement.ToString());
		base.transform.Find(prefix + "Rewards/Purchase Now/Cost").GetComponent<Text>().text = cost.ToString();
		Button component = base.transform.Find(prefix + "Rewards/Purchase Now").GetComponent<Button>();
		component.onClick.RemoveAllListeners();
		component.onClick.AddListener(delegate()
		{
			Utilities.ConfirmPurchase(cost, delegate
			{
				int num2 = this.currentEvent.TokenRequirement - tokens;
				int num3 = 0;
				while ((double)num3 < this.currentEvent.timeSpan.TotalDays && num2 > 0)
				{
					if (!this.EventTasks[num3 * 3].Claimed)
					{
						this.EventTasks[num3 * 3].Complete = (this.EventTasks[num3 * 3].Claimed = true);
						num2--;
					}
					num3++;
				}
				this.Claim(this.currentEvent.EventID, true, true, true);
				Utilities.SendAnalytic(Utilities.AnalyticType.Conversion, "Event" + this.currentEvent.EventID.ToString());
				this.StoreState();
				TimeSpan timeOffset = Utilities.TimeOffset;
				this.SetTask(this.EventTasks[this.CurrentDay * 3], this.transform.Find(prefix + "Tasks/Task 1"), timeOffset);
				this.UpdateDayIcons();
				this.transform.Find(prefix + "Rewards/Image/Already Owned").gameObject.SetActive(true);
			});
		});
		component.interactable = (tokens < this.currentEvent.TokenRequirement && cost > 0 && !flag);
		base.transform.Find(prefix + "Rewards/Information Text").gameObject.SetActive(this.currentEvent.timeSpan.TotalDays <= 7.0);
		this.UpdateTimeBlockRequirement();
		return tokens;
	}

	// Token: 0x0600050F RID: 1295 RVA: 0x00028BA0 File Offset: 0x00026DA0
	private void Start()
	{
		string prefix = (!(base.transform.Find("Dialog") != null)) ? string.Empty : "Dialog/";
		base.transform.Find(prefix + "Tasks/Task 1/Description").GetComponent<Button>().onClick.AddListener(delegate()
		{
			this.EnableTask(0, this.transform.Find(prefix + "Tasks/Task 1"));
		});
		base.transform.Find(prefix + "Tasks/Task 2/Description").GetComponent<Button>().onClick.AddListener(delegate()
		{
			this.EnableTask(1, this.transform.Find(prefix + "Tasks/Task 2"));
		});
		base.transform.Find(prefix + "Tasks/Task 3/Description").GetComponent<Button>().onClick.AddListener(delegate()
		{
			this.EnableTask(2, this.transform.Find(prefix + "Tasks/Task 3"));
		});
		base.transform.Find(prefix + "Tasks/Task 1/Purchase").GetComponent<Button>().onClick.AddListener(delegate()
		{
			this.PurchaseTask(0, this.transform.Find(prefix + "Tasks/Task 1"));
		});
		base.transform.Find(prefix + "Tasks/Task 2/Purchase").GetComponent<Button>().onClick.AddListener(delegate()
		{
			this.PurchaseTask(1, this.transform.Find(prefix + "Tasks/Task 2"));
		});
		base.transform.Find(prefix + "Tasks/Task 3/Purchase").GetComponent<Button>().onClick.AddListener(delegate()
		{
			this.PurchaseTask(2, this.transform.Find(prefix + "Tasks/Task 3"));
		});
		base.transform.Find(prefix + "Upcoming Events").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_19_0", "More events are coming soon!");
		base.transform.Find(prefix + "Upcoming Events/Text").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_40_0", "Pin-Ups, Outfits and More!");
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x00028D98 File Offset: 0x00026F98
	public void Init()
	{
		if (!this.initialized)
		{
			this.LoadEvent();
			this.initialized = true;
		}
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x00028DB4 File Offset: 0x00026FB4
	public void UpdateTranslations()
	{
		if (this.currentEvent == null || !this.initialized || this.EventTasks.Length == 0)
		{
			return;
		}
		TimeSpan timeOffset = Utilities.TimeOffset;
		TimeSpan timeSpan = this.currentEvent.EndTimeUTC - DateTime.UtcNow + timeOffset;
		int num = (int)(this.currentEvent.timeSpan.TotalDays - timeSpan.TotalDays);
		this.SetTask(this.EventTasks[num * 3], base.transform.Find("Tasks/Task 1"), timeOffset);
		this.SetTask(this.EventTasks[num * 3 + 1], base.transform.Find("Tasks/Task 2"), timeOffset);
		this.SetTask(this.EventTasks[num * 3 + 2], base.transform.Find("Tasks/Task 3"), timeOffset);
		this.UpdateDayIcons();
		base.transform.Find("Upcoming Events").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_19_0", "More events are coming soon!");
		base.transform.Find("Upcoming Events/Text").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_40_0", "Pin-Ups, Outfits and More!");
		string text = this.currentEvent.Name.Replace("\\n", "\n");
		if (!text.Contains("\n"))
		{
			text = text + "\n" + string.Format(Translations.GetTranslation("limited_time_events_21_0", "Collect At Least {0} Tokens!"), this.currentEvent.TokenRequirement);
		}
		base.transform.Find("Title").GetComponent<Text>().text = text;
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x00028F60 File Offset: 0x00027160
	private void Update()
	{
		if (this.currentEvent == null && !this.initialized)
		{
			if (GameState.UniverseReady.Value && !this.initialized)
			{
				this.LoadEvent();
				this.initialized = true;
			}
			return;
		}
		Utilities.CheckCachedServerTime(null);
		if (Utilities.TimeRequested)
		{
			return;
		}
		TimeSpan timeOffset = Utilities.TimeOffset;
		this.tick += Time.deltaTime;
		if (this.tick > 1f)
		{
			this.tick -= 1f;
			if (this.currentEvent == null || DateTime.UtcNow - timeOffset > this.currentEvent.EndTimeUTC || DateTime.UtcNow - timeOffset < this.currentEvent.StartTimeUTC)
			{
				GameState.CurrentState.transform.Find("Popups/Confirm Purchase").gameObject.SetActive(false);
				this.LoadEventInternal();
			}
			if (this.currentEvent == null)
			{
				return;
			}
			TimeSpan end = this.currentEvent.EndTimeUTC - DateTime.UtcNow + timeOffset;
			string arg = Utilities.CreateCountdown(end);
			this.nextEventTimer.text = string.Format("{0}\n{1}", Translations.GetTranslation("limited_time_events_2_0", "Next event in:"), arg);
			this.newDailyTasksTimer.text = Utilities.CreateCountdownDay(end);
			this.offerExpiresTimer.text = string.Format("{0} {1}", Translations.GetTranslation("limited_time_events_6_1", "Offer Expires:"), arg);
			int num = (int)(this.currentEvent.timeSpan.TotalDays - end.TotalDays);
			if (this.EventTasks == null || this.EventTasks.Length == 0)
			{
				this.GetTasks();
			}
			string str = (!(base.transform.Find("Dialog") != null)) ? "Tasks/" : "Dialog/Tasks/Layout/";
			if (this.task1 != this.EventTasks[num * 3])
			{
				this.task1 = this.EventTasks[num * 3];
				this.SetTask(this.EventTasks[num * 3], base.transform.Find(str + "Task 1"), timeOffset);
				this.SetTask(this.EventTasks[num * 3 + 1], base.transform.Find(str + "Task 2"), timeOffset);
				this.SetTask(this.EventTasks[num * 3 + 2], base.transform.Find(str + "Task 3"), timeOffset);
				this.UpdateDayIcons();
			}
			this.UpdateTask(this.EventTasks[num * 3], base.transform.Find(str + "Task 1"), timeOffset);
			this.UpdateTask(this.EventTasks[num * 3 + 1], base.transform.Find(str + "Task 2"), timeOffset);
			this.UpdateTask(this.EventTasks[num * 3 + 2], base.transform.Find(str + "Task 3"), timeOffset);
		}
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x0002928C File Offset: 0x0002748C
	private void GetTasks()
	{
		this.EventTasks = new Task[this.currentEvent.TaskIDs.Length * 3 / 2];
		for (int i = 0; i < this.currentEvent.TaskIDs.Length / 2; i++)
		{
			TaskData taskData;
			Universe.Tasks.TryGetValue(this.currentEvent.TaskIDs[2 * i], out taskData);
			TaskData taskData2;
			Universe.Tasks.TryGetValue(this.currentEvent.TaskIDs[2 * i + 1], out taskData2);
			this.EventTasks[i * 3] = new Task(this.currentEvent.EpicTask);
			this.EventTasks[i * 3 + 1] = new Task(taskData);
			this.EventTasks[i * 3 + 2] = new Task(taskData2);
		}
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x00029350 File Offset: 0x00027550
	private void UpdateTask(Task task, Transform ui, TimeSpan timeOffset)
	{
		if (!task.Claimed && !task.Complete && task.DateStart != 0L)
		{
			DateTime dateTime = DateTime.UtcNow - timeOffset;
			if (dateTime.Ticks < task.DateStart)
			{
				Debug.Log("Moved backwards through time...");
				task.DateStart = dateTime.Ticks;
			}
			long num = (dateTime.Ticks - task.DateStart) / 10000000L;
			float fillAmount = (float)num / (float)task.TaskData.Duration.TotalSeconds;
			int num2 = (int)task.TaskData.Duration.TotalSeconds - (int)num;
			ui.Find("ProgressBar/Fill").GetComponent<Image>().fillAmount = fillAmount;
			ui.Find("ProgressBar/Text").GetComponent<Text>().text = Utilities.CreateCountdownDay(num2);
			int num3 = (task.TaskData.RewardType != TaskManager.TaskRewardType.Diamond) ? Mathf.CeilToInt((float)num2 / 300f) : task.TaskData.RewardAmount;
			ui.Find("Purchase/Cost").GetComponent<Text>().text = num3.ToString();
			if (num2 <= 0)
			{
				this.CompleteTask(task, ui);
			}
		}
	}

	// Token: 0x06000515 RID: 1301 RVA: 0x00029488 File Offset: 0x00027688
	private void SetTask(Task task, Transform ui, TimeSpan timeOffset)
	{
		if (task == null || task.TaskData == null)
		{
			return;
		}
		ui.Find("Title").GetComponent<Text>().text = Translations.GetTranslation(Translations.GetLTETaskId(task.TaskData.TaskID), task.TaskData.Name);
		ui.Find("Reward/Icon").GetComponent<Image>().sprite = this.TaskIcons[(int)task.TaskData.RewardType];
		ui.Find("Reward/Text").GetComponent<Text>().text = ((task.TaskData.RewardType != TaskManager.TaskRewardType.Cash) ? ((task.TaskData.RewardType != TaskManager.TaskRewardType.Job) ? task.TaskData.RewardAmount.ToString() : (task.TaskData.RewardAmount / 60).ToString()) : "$$");
		ui.Find("Reward/Small Text").GetComponent<Text>().text = task.ToDescriptiveString();
		string arg = (task.TaskData.Duration.Minutes < 10) ? ("0" + task.TaskData.Duration.Minutes.ToString()) : task.TaskData.Duration.Minutes.ToString();
		ui.Find("Description/Text").GetComponent<Text>().text = string.Format("x{0} / {1}:{2}:00", task.TaskData.TimeBlockRequirement.ToString(), task.TaskData.Duration.Hours.ToString(), arg);
		ui.Find("Description").gameObject.SetActive(true);
		ui.Find("ProgressBar").gameObject.SetActive(false);
		ui.Find("Purchase").gameObject.SetActive(false);
		ui.Find("Purchase/Text").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_13_0", "Insta-Finish").Replace('-', '\n');
		ui.Find("Purchase/Text").GetComponent<Text>().fontSize = ((Translations.PreferredLanguage != Translations.Language.Japanese) ? 16 : 12);
		if (task.Claimed)
		{
			this.ClaimTaskUI(task, ui);
		}
		else if (task.Complete)
		{
			this.CompleteTask(task, ui);
		}
		else if (!task.Complete && task.DateStart != 0L)
		{
			ui.Find("Description").gameObject.SetActive(false);
			ui.Find("ProgressBar").gameObject.SetActive(true);
			if (task.TaskData.Duration.TotalSeconds > 10.0)
			{
				ui.Find("Purchase").gameObject.SetActive(true);
			}
		}
		else
		{
			ui.Find("Description").GetComponent<Image>().sprite = ((task.TaskData.TaskID != this.currentEvent.EpicTask.TaskID) ? this.StandardSprite : this.EpicSprite);
			ui.Find("Description").GetComponent<Button>().interactable = true;
			ui.Find("Description/Start").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_15_0", "START");
			ui.Find("Reward").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
			ui.Find("Description/Text").gameObject.SetActive(true);
			ui.Find("Description/Icon").gameObject.SetActive(true);
			ui.Find("Description/Start").GetComponent<Text>().rectTransform.localPosition = new Vector3(-4f, -9f, 0f);
			ui.Find("Description").GetComponent<Button>().onClick.RemoveAllListeners();
			ui.Find("Description").GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.EnableTask(task, ui);
			});
		}
	}

	// Token: 0x06000516 RID: 1302 RVA: 0x000299D8 File Offset: 0x00027BD8
	private void PurchaseTask(int i, Transform ui)
	{
		TimeSpan timeOffset = Utilities.TimeOffset;
		DateTime dateTime = DateTime.UtcNow - timeOffset;
		Task task = this.EventTasks[this.CurrentDay * 3 + i];
		long num = (dateTime.Ticks - task.DateStart) / 10000000L;
		int num2 = (int)task.TaskData.Duration.TotalSeconds - (int)num;
		int diamonds = (task.TaskData.RewardType != TaskManager.TaskRewardType.Diamond) ? Mathf.CeilToInt((float)num2 / 300f) : task.TaskData.RewardAmount;
		Utilities.ConfirmPurchase(diamonds, delegate
		{
			this.CompleteTask(task, ui);
			Utilities.SendAnalytic(Utilities.AnalyticType.Conversion, "Task" + task.TaskData.TaskID.ToString());
		});
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x00029AB4 File Offset: 0x00027CB4
	private void EnableTask(int i, Transform ui)
	{
		this.EnableTask(this.EventTasks[this.CurrentDay * 3 + i], ui);
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x00029AD0 File Offset: 0x00027CD0
	private void EnableTask(Task task, Transform ui)
	{
		Utilities.CheckCachedServerTime(delegate
		{
			if (task.TaskData.TimeBlockRequirement > FreeTime.Free)
			{
				Utilities.PurchaseTimeBlocks(task.TaskData.TimeBlockRequirement - FreeTime.Free);
				return;
			}
			TimeSpan timeOffset = Utilities.TimeOffset;
			DateTime dateTime = DateTime.UtcNow - timeOffset;
			task.DateStart = dateTime.Ticks;
			ui.Find("Description/Start").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_17_0", "Wait...");
			ui.Find("Description").GetComponent<Button>().interactable = false;
			this.StoreState();
			this.UpdateTimeBlockRequirement();
			if (task.TaskData.TaskID != 0)
			{
				global::PlayerPrefs.SaveCallbacks.Add(delegate
				{
					this.EnableTaskUI(task, ui);
				});
			}
			else
			{
				this.EnableTaskUI(task, ui);
			}
			if (GameState.CurrentState != null)
			{
				GameState.CurrentState.QueueQuickSave();
			}
		});
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x00029B0C File Offset: 0x00027D0C
	private void EnableTaskUI(Task task, Transform ui)
	{
		if (task.TaskData.Duration.TotalSeconds <= 10.0)
		{
			this.CompleteTask(task, ui);
		}
		else
		{
			ui.Find("Description").gameObject.SetActive(false);
			ui.Find("Description/Start").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_15_0", "START");
			ui.Find("Description").GetComponent<Button>().interactable = true;
			ui.Find("ProgressBar").gameObject.SetActive(true);
			if (task.TaskData.Duration.TotalSeconds > 10.0)
			{
				ui.Find("Purchase").gameObject.SetActive(true);
			}
			this.UpdateTask(task, ui, Utilities.TimeOffset);
		}
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x00029BF4 File Offset: 0x00027DF4
	private void CompleteTask(Task task, Transform ui)
	{
		task.Complete = true;
		ui.Find("Description").gameObject.SetActive(true);
		ui.Find("ProgressBar").gameObject.SetActive(false);
		ui.Find("Purchase").gameObject.SetActive(false);
		ui.Find("Description").GetComponent<Button>().onClick.RemoveAllListeners();
		ui.Find("Description").GetComponent<Button>().onClick.AddListener(delegate()
		{
			this.ClaimTask(task, ui);
			this.Claim(task.TaskData);
		});
		ui.Find("Description").GetComponent<Image>().sprite = this.ClaimSprite;
		ui.Find("Description/Start").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_24_0", "Claim!");
		ui.Find("Description/Start").GetComponent<Text>().rectTransform.localPosition = new Vector3(-4f, 5f, 0f);
		ui.Find("Description/Text").gameObject.SetActive(false);
		ui.Find("Description/Icon").gameObject.SetActive(false);
		ui.Find("Description").GetComponent<Button>().interactable = true;
		this.StoreState();
		this.UpdateTimeBlockRequirement();
		if (GameState.CurrentState != null)
		{
			GameState.CurrentState.QueueSave();
		}
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x00029DB8 File Offset: 0x00027FB8
	private void ClaimTask(Task task, Transform ui)
	{
		task.Claimed = true;
		this.StoreState();
		ui.Find("Description/Start").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_17_0", "Wait...");
		ui.Find("Description").GetComponent<Button>().interactable = false;
		global::PlayerPrefs.SaveCallbacks.Add(delegate
		{
			this.ClaimTaskUI(task, ui);
		});
	}

	// Token: 0x0600051C RID: 1308 RVA: 0x00029E4C File Offset: 0x0002804C
	private void ClaimTaskUI(Task task, Transform ui)
	{
		ui.Find("Description").GetComponent<Image>().sprite = this.CompleteSprite;
		ui.Find("Description/Start").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_16_0", "Completed!");
		ui.Find("Description/Start").GetComponent<Text>().rectTransform.localPosition = new Vector3(-4f, 5f, 0f);
		ui.Find("Description/Text").gameObject.SetActive(false);
		ui.Find("Description/Icon").gameObject.SetActive(false);
		ui.Find("Description").GetComponent<Button>().interactable = false;
		ui.Find("Reward/Small Text").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_14_0", "Claimed");
		ui.Find("Reward").GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
		this.StoreState();
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x00029F60 File Offset: 0x00028160
	public bool HasEquivalent(short eventId)
	{
		if (Universe.Events == null)
		{
			return false;
		}
		if (eventId < 1)
		{
			return false;
		}
		EventData eventData = Universe.Events[eventId];
		foreach (KeyValuePair<short, EventData> keyValuePair in Universe.Events)
		{
			EventData value = keyValuePair.Value;
			if (value.RewardType.Count == eventData.RewardType.Count && value.RewardGirlsID.Length == eventData.RewardGirlsID.Length && TaskManager.RewardGirlIdsMatch(eventData, value) && this.currentEvent.RewardAmount.Count == value.RewardAmount.Count && TaskManager.RewardAmountsMatch(eventData, value))
			{
				bool flag = true;
				for (int i = 0; i < eventData.RewardType.Count; i++)
				{
					if (eventData.RewardType[i] != value.RewardType[i])
					{
						flag = false;
						break;
					}
					switch (eventData.RewardType[i])
					{
					case TaskManager.EventRewardType.UniqueOutfit:
					case TaskManager.EventRewardType.MonsterOutfit:
					case TaskManager.EventRewardType.HolidayOutfit:
					case TaskManager.EventRewardType.BikiniOutfit:
					case TaskManager.EventRewardType.SchoolOutfit:
					case TaskManager.EventRewardType.WeddingOutfit:
					case TaskManager.EventRewardType.AllOutfits:
						flag = TaskManager.HasOutfits(eventData, eventData.RewardType[i]);
						break;
					case TaskManager.EventRewardType.ExclusivePinup:
						flag = TaskManager.IsCompletedEventSet((int)value.EventID);
						break;
					case TaskManager.EventRewardType.NewGirl:
						flag = TaskManager.CheckGirlsUnlocked(eventData);
						break;
					default:
						flag = false;
						break;
					}
				}
				if (flag)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x0002A134 File Offset: 0x00028334
	private static bool RewardGirlIdsMatch(EventData currentEvent, EventData universeEvent)
	{
		for (int i = 0; i < currentEvent.RewardGirlsID.Length; i++)
		{
			if (currentEvent.RewardGirlsID[i] != universeEvent.RewardGirlsID[i])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x0002A174 File Offset: 0x00028374
	private static bool RewardAmountsMatch(EventData currentEvent, EventData universeEvent)
	{
		for (int i = 0; i < currentEvent.RewardGirlsID.Length; i++)
		{
			if (currentEvent.RewardAmount[i] != universeEvent.RewardAmount[i])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x0002A1BC File Offset: 0x000283BC
	private static bool IsEquivalentEvent(EventData e1, EventData e2)
	{
		if (e1.EventID == e2.EventID)
		{
			return true;
		}
		if (e1.RewardType.Count != e2.RewardType.Count)
		{
			return false;
		}
		for (int i = 0; i < e1.RewardType.Count; i++)
		{
			if (e1.RewardType[i] != e2.RewardType[i])
			{
				return false;
			}
		}
		if (e1.RewardGirlsID != null && e2.RewardGirlsID != null)
		{
			if (e1.RewardGirlsID.Length != e2.RewardGirlsID.Length)
			{
				return false;
			}
			for (int j = 0; j < e1.RewardGirlsID.Length; j++)
			{
				if (e1.RewardGirlsID[j] != e2.RewardGirlsID[j])
				{
					return false;
				}
			}
		}
		if (e1.RewardAmount != null && e2.RewardAmount != null)
		{
			if (e1.RewardAmount.Count != e2.RewardAmount.Count)
			{
				return false;
			}
			for (int k = 0; k < e1.RewardAmount.Count; k++)
			{
				if (e1.RewardAmount[k] != e2.RewardAmount[k])
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x0002A300 File Offset: 0x00028500
	private static bool CheckGirlsUnlocked(EventData currentEvent)
	{
		if (TaskManager.IsCompletedEventSet((int)currentEvent.EventID))
		{
			return true;
		}
		foreach (KeyValuePair<short, EventData> keyValuePair in Universe.Events)
		{
			if (TaskManager.IsEquivalentEvent(keyValuePair.Value, currentEvent) && TaskManager.IsCompletedEventSet((int)keyValuePair.Value.EventID))
			{
				return true;
			}
		}
		Balance.GirlName girlName = (Balance.GirlName)(currentEvent.RewardGirlsID[0] - 1);
		if (girlName == Balance.GirlName.Wendy && (Playfab.AwardedItems & Playfab.PlayfabItems.Wendy) != (Playfab.PlayfabItems)0L)
		{
			return true;
		}
		if (girlName == Balance.GirlName.Ruri && (Playfab.AwardedItems & Playfab.PlayfabItems.Ruri) != (Playfab.PlayfabItems)0L)
		{
			return true;
		}
		if (girlName == Balance.GirlName.Sawyer && (Playfab.AwardedItems & Playfab.PlayfabItems.Sawyer) != (Playfab.PlayfabItems)0L)
		{
			return true;
		}
		if (girlName == Balance.GirlName.Renee && (Playfab.AwardedItems & Playfab.PlayfabItems.Renee) != (Playfab.PlayfabItems)0L)
		{
			return true;
		}
		foreach (short key in currentEvent.RewardGirlsID)
		{
			Balance.GirlName girl = Utilities.GirlFromString(Universe.Girls[key].Name);
			if (!GameState.GetGirlScreen().IsUnlocked(girl))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x0002A474 File Offset: 0x00028674
	private static bool HasOutfits(EventData currentEvent, TaskManager.EventRewardType rewardType)
	{
		if (!TaskManager.eventToOutfitDic.ContainsKey(rewardType))
		{
			return false;
		}
		foreach (short key in currentEvent.RewardGirlsID)
		{
			string name = Universe.Girls[key].Name;
			Girl girl = Girl.FindGirl(Utilities.GirlFromString(name));
			if (girl == null)
			{
				return false;
			}
			if (rewardType == TaskManager.EventRewardType.AllOutfits)
			{
				Requirement.OutfitType outfitType = Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing;
				if ((girl.LifetimeOutfits & outfitType) != outfitType)
				{
					return false;
				}
			}
			else if ((girl.LifetimeOutfits & TaskManager.eventToOutfitDic[rewardType]) == Requirement.OutfitType.None)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x0002A520 File Offset: 0x00028720
	public static bool IsCompletedEventSet(int index)
	{
		int num = index / 8;
		int num2 = index % 8;
		return num < TaskManager.CompletedEvents.Length && ((int)TaskManager.CompletedEvents[num] & 1 << num2) != 0;
	}

	// Token: 0x06000524 RID: 1316 RVA: 0x0002A558 File Offset: 0x00028758
	public static bool IsAnyEventCompleted(params int[] indexes)
	{
		for (int i = 0; i < indexes.Length; i++)
		{
			if (TaskManager.IsCompletedEventSet(indexes[i]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x0002A58C File Offset: 0x0002878C
	private static void SetCompletedEventBit(int index)
	{
		int num = index / 8;
		int num2 = index % 8;
		if (num >= TaskManager.CompletedEvents.Length)
		{
			byte[] array = new byte[num + 1];
			Array.Copy(TaskManager.CompletedEvents, array, TaskManager.CompletedEvents.Length);
			TaskManager.CompletedEvents = array;
		}
		byte[] completedEvents = TaskManager.CompletedEvents;
		int num3 = num;
		completedEvents[num3] |= (byte)(1 << num2);
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x0002A5E8 File Offset: 0x000287E8
	private void MaybeGetGirlAndAssignOutfits(Balance.GirlName girlName, Requirement.OutfitType outfit)
	{
		Girl girl = Girl.FindGirl(girlName);
		if (girl == null)
		{
			return;
		}
		girl.LifetimeOutfits |= outfit;
		girl.StoreState();
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x0002A620 File Offset: 0x00028820
	private void RewardOutfit(short[] ids, Requirement.OutfitType outfitType)
	{
		foreach (short key in ids)
		{
			string name = Universe.Girls[key].Name;
			this.MaybeGetGirlAndAssignOutfits(Utilities.GirlFromString(name), outfitType);
		}
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x0002A668 File Offset: 0x00028868
	public void DebugClaim(short eventId)
	{
		if (!BlayFapIntegration.IsTestDevice)
		{
			Debug.LogError("Error: trying to claim task while not being a Debug Device!");
			return;
		}
		this.Claim(eventId, true, true, true);
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x0002A68C File Offset: 0x0002888C
	private void Claim(short eventId, bool displayPopup = true, bool sendPlayfabUpdate = true, bool rebuildAchievements = true)
	{
		if (Universe.Events == null)
		{
			return;
		}
		if (!Universe.Events.ContainsKey(eventId))
		{
			return;
		}
		if (eventId < 1)
		{
			return;
		}
		string text = Translations.GetTranslation("limited_time_events_29_0", "You got an exclusive pin-up!");
		Sprite sprite = null;
		if (Universe.Events.ContainsKey(eventId))
		{
			EventData eventData = Universe.Events[eventId];
			text = eventData.RewardText;
			if (eventData.Icon == null)
			{
				GameState.AssetManager.GetBundle("universe/" + this.currentEvent.AssetBundleName, false, delegate(AssetBundle bundle)
				{
					if (bundle != null)
					{
						GameObject gameObject2 = GameState.CurrentState.transform.Find("Popups/Affection Milestone").gameObject;
						gameObject2.transform.Find("Icon").GetComponent<Image>().sprite = bundle.LoadAsset<Sprite>(this.currentEvent.IconName);
						GameState.AssetManager.UnloadBundle(bundle);
						if (displayPopup)
						{
							gameObject2.gameObject.SetActive(true);
						}
					}
				});
			}
			else
			{
				sprite = eventData.Icon;
			}
		}
		TaskManager.SetCompletedEventBit((int)eventId);
		global::PlayerPrefs.SetString("CompletedEvents", Convert.ToBase64String(TaskManager.CompletedEvents));
		if (GameState.CurrentState != null)
		{
			GameState.CurrentState.QueueSave();
		}
		if (sendPlayfabUpdate)
		{
			BlayFapIntegration.MarkLTEComplete(TaskManager.CompletedEvents);
		}
		if (this.currentEvent != null && eventId == this.currentEvent.EventID)
		{
			string str = (!(base.transform.Find("Dialog") != null)) ? string.Empty : "Dialog/";
			base.transform.Find(str + "Rewards/Image/Already Owned").gameObject.SetActive(this.HasEquivalent(eventId));
		}
		switch (eventId)
		{
		case 1:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Nutaku, Requirement.OutfitType.Unique);
			break;
		case 3:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Elle, Requirement.OutfitType.Monster);
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Iro, Requirement.OutfitType.Monster);
			GameState.GetGirlScreen().UnlockGirls();
			break;
		case 5:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Quill, Requirement.OutfitType.Monster);
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Bonnibel, Requirement.OutfitType.Monster);
			GameState.GetGirlScreen().UnlockGirls();
			break;
		case 8:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Cassie, Requirement.OutfitType.Christmas);
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Mio, Requirement.OutfitType.Christmas);
			break;
		case 9:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Quill, Requirement.OutfitType.Christmas);
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Elle, Requirement.OutfitType.Christmas);
			break;
		case 10:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Nutaku, Requirement.OutfitType.Christmas);
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Iro, Requirement.OutfitType.Christmas);
			break;
		case 12:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Bonnibel, Requirement.OutfitType.Christmas);
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Ayano, Requirement.OutfitType.Christmas);
			break;
		case 13:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Fumi, Requirement.OutfitType.Christmas);
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Bearverly, Requirement.OutfitType.Christmas);
			break;
		case 14:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Nina, Requirement.OutfitType.Christmas);
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Alpha, Requirement.OutfitType.Christmas);
			break;
		case 15:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Pamu, Requirement.OutfitType.Christmas);
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Luna, Requirement.OutfitType.Christmas);
			break;
		case 16:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Eva, Requirement.OutfitType.Christmas);
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Karma, Requirement.OutfitType.Christmas);
			break;
		case 17:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Sutra, Requirement.OutfitType.Christmas);
			break;
		case 28:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Cassie, Requirement.OutfitType.BathingSuit);
			break;
		case 29:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Mio, Requirement.OutfitType.BathingSuit);
			break;
		case 30:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Quill, Requirement.OutfitType.BathingSuit);
			break;
		case 32:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Mio, Requirement.OutfitType.Unique);
			break;
		case 33:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Elle, Requirement.OutfitType.BathingSuit);
			break;
		case 34:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Iro, Requirement.OutfitType.BathingSuit);
			break;
		case 35:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Nutaku, Requirement.OutfitType.BathingSuit);
			break;
		case 36:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Bonnibel, Requirement.OutfitType.BathingSuit);
			break;
		case 37:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Ayano, Requirement.OutfitType.BathingSuit);
			break;
		case 38:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Fumi, Requirement.OutfitType.BathingSuit);
			break;
		case 39:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Bearverly, Requirement.OutfitType.BathingSuit);
			break;
		case 40:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Nina, Requirement.OutfitType.BathingSuit);
			break;
		case 41:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Alpha, Requirement.OutfitType.BathingSuit);
			break;
		case 42:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Pamu, Requirement.OutfitType.BathingSuit);
			break;
		case 43:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Luna, Requirement.OutfitType.BathingSuit);
			break;
		case 46:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Cassie, Requirement.OutfitType.SchoolUniform);
			break;
		case 47:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Mio, Requirement.OutfitType.SchoolUniform);
			break;
		case 48:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Quill, Requirement.OutfitType.Unique);
			break;
		case 49:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Elle, Requirement.OutfitType.SchoolUniform);
			break;
		case 50:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Nutaku, Requirement.OutfitType.SchoolUniform);
			break;
		case 51:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Iro, Requirement.OutfitType.SchoolUniform);
			break;
		case 54:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Cassie, Requirement.OutfitType.Christmas);
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Mio, Requirement.OutfitType.Christmas);
			break;
		case 55:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Quill, Requirement.OutfitType.Christmas);
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Elle, Requirement.OutfitType.Christmas);
			break;
		case 56:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Nutaku, Requirement.OutfitType.Christmas);
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Iro, Requirement.OutfitType.Christmas);
			break;
		case 57:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Bonnibel, Requirement.OutfitType.Christmas);
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Ayano, Requirement.OutfitType.Christmas);
			break;
		case 58:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Fumi, Requirement.OutfitType.Christmas);
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Bearverly, Requirement.OutfitType.Christmas);
			break;
		case 59:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Nina, Requirement.OutfitType.Christmas);
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Alpha, Requirement.OutfitType.Christmas);
			break;
		case 61:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Pamu, Requirement.OutfitType.Christmas);
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Luna, Requirement.OutfitType.Christmas);
			break;
		case 62:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Eva, Requirement.OutfitType.Christmas);
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Karma, Requirement.OutfitType.Christmas);
			break;
		case 63:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Sutra, Requirement.OutfitType.Christmas);
			break;
		case 64:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Nutaku, Requirement.OutfitType.Unique);
			break;
		case 77:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Cassie, Requirement.OutfitType.BathingSuit);
			break;
		case 78:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Mio, Requirement.OutfitType.BathingSuit);
			break;
		case 79:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Quill, Requirement.OutfitType.BathingSuit);
			break;
		case 81:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Mio, Requirement.OutfitType.Unique);
			break;
		case 82:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Elle, Requirement.OutfitType.BathingSuit);
			break;
		case 83:
		case 122:
		case 173:
			Playfab.AwardedItems |= Playfab.PlayfabItems.Odango;
			GameState.GetGirlScreen().UnlockGirls();
			global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
			GameState.GetAchievements().Rebuild();
			break;
		case 84:
		case 123:
		case 174:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Odango, Requirement.OutfitType.All);
			break;
		case 85:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Iro, Requirement.OutfitType.BathingSuit);
			break;
		case 87:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Nutaku, Requirement.OutfitType.BathingSuit);
			break;
		case 88:
			Playfab.AwardedItems |= Playfab.PlayfabItems.Shibuki;
			GameState.GetGirlScreen().UnlockGirls();
			global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
			GameState.GetAchievements().Rebuild();
			break;
		case 89:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Shibuki, Requirement.OutfitType.All);
			break;
		case 91:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Quill, Requirement.OutfitType.Unique);
			break;
		case 92:
		case 131:
		case 182:
			Playfab.AwardedItems |= Playfab.PlayfabItems.Sirina;
			GameState.GetGirlScreen().UnlockGirls();
			global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
			GameState.GetAchievements().Rebuild();
			break;
		case 93:
		case 132:
		case 183:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Sirina, Requirement.OutfitType.All);
			break;
		case 95:
		case 125:
		case 170:
			Playfab.AwardedItems |= Playfab.PlayfabItems.Vellatrix;
			GameState.GetGirlScreen().UnlockGirls();
			global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
			GameState.GetAchievements().Rebuild();
			break;
		case 96:
		case 126:
		case 171:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Vellatrix, Requirement.OutfitType.All);
			break;
		case 98:
		case 176:
			GameState.GetGirlScreen().UnlockGirls();
			GameState.GetAchievements().Rebuild();
			break;
		case 99:
		case 177:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Roxxy, Requirement.OutfitType.All);
			break;
		case 101:
		case 134:
			GameState.GetGirlScreen().UnlockGirls();
			GameState.GetAchievements().Rebuild();
			break;
		case 102:
		case 135:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Tessa, Requirement.OutfitType.All);
			break;
		case 104:
		case 164:
			GameState.GetGirlScreen().UnlockGirls();
			GameState.GetAchievements().Rebuild();
			break;
		case 105:
		case 165:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Claudia, Requirement.OutfitType.All);
			break;
		case 107:
		case 149:
			GameState.GetGirlScreen().UnlockGirls();
			GameState.GetAchievements().Rebuild();
			break;
		case 108:
		case 150:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Rosa, Requirement.OutfitType.All);
			break;
		case 110:
		case 153:
			GameState.GetGirlScreen().UnlockGirls();
			GameState.GetAchievements().Rebuild();
			break;
		case 111:
		case 154:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Juliet, Requirement.OutfitType.All);
			break;
		case 113:
			GameState.GetGirlScreen().UnlockGirls();
			GameState.GetAchievements().Rebuild();
			break;
		case 114:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Wendy, Requirement.OutfitType.All);
			break;
		case 116:
			GameState.GetGirlScreen().UnlockGirls();
			GameState.GetAchievements().Rebuild();
			break;
		case 117:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Ruri, Requirement.OutfitType.All);
			break;
		case 119:
			GameState.GetGirlScreen().UnlockGirls();
			GameState.GetAchievements().Rebuild();
			break;
		case 120:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Generica, Requirement.OutfitType.All);
			break;
		case 128:
		case 167:
			GameState.GetGirlScreen().UnlockGirls();
			GameState.GetAchievements().Rebuild();
			break;
		case 129:
		case 168:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Lustat, Requirement.OutfitType.All);
			break;
		case 137:
			GameState.GetGirlScreen().UnlockGirls();
			GameState.GetAchievements().Rebuild();
			break;
		case 138:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Shibuki, Requirement.OutfitType.All);
			break;
		case 140:
			GameState.GetGirlScreen().UnlockGirls();
			GameState.GetAchievements().Rebuild();
			break;
		case 141:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Sawyer, Requirement.OutfitType.All);
			break;
		case 143:
			GameState.GetGirlScreen().UnlockGirls();
			GameState.GetAchievements().Rebuild();
			break;
		case 144:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Explora, Requirement.OutfitType.All);
			break;
		case 146:
			GameState.GetGirlScreen().UnlockGirls();
			GameState.GetAchievements().Rebuild();
			break;
		case 147:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Esper, Requirement.OutfitType.All);
			break;
		case 148:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Mio, Requirement.OutfitType.Unique);
			break;
		case 156:
		case 157:
		case 158:
		case 159:
			GameState.GetGirlScreen().UnlockGirls();
			GameState.GetAchievements().Rebuild();
			break;
		case 161:
			GameState.GetGirlScreen().UnlockGirls();
			GameState.GetAchievements().Rebuild();
			break;
		case 162:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Renee, Requirement.OutfitType.All);
			break;
		case 179:
			GameState.GetGirlScreen().UnlockGirls();
			GameState.GetAchievements().Rebuild();
			break;
		case 180:
			this.MaybeGetGirlAndAssignOutfits(Balance.GirlName.Lake, Requirement.OutfitType.All);
			break;
		}
		if (displayPopup)
		{
			GameObject gameObject = GameState.CurrentState.transform.Find("Popups/Affection Milestone").gameObject;
			gameObject.transform.Find("Top Text").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_30_0", "Event Complete!");
			gameObject.transform.Find("Icon").GetComponent<Image>().sprite = sprite;
			gameObject.transform.Find("Icon").gameObject.SetActive(true);
			gameObject.transform.Find("+1 Diamond").gameObject.SetActive(false);
			gameObject.transform.Find("Bottom Text").GetComponent<Text>().text = text;
			gameObject.SetActive(sprite != null);
		}
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x0002B4A8 File Offset: 0x000296A8
	private void Claim(TaskData task)
	{
		string rewardText = string.Empty;
		switch (task.RewardType)
		{
		case TaskManager.TaskRewardType.Cash:
		{
			double num;
			switch (task.RewardAmount)
			{
			case 2:
				num = TaskManager.cashAwards[0];
				goto IL_B4;
			case 3:
				num = TaskManager.cashAwards[1];
				goto IL_B4;
			case 4:
				num = TaskManager.cashAwards[2];
				goto IL_B4;
			case 5:
				num = TaskManager.cashAwards[3];
				goto IL_B4;
			case 7:
				num = TaskManager.cashAwards[4];
				goto IL_B4;
			}
			num = TaskManager.cashAwards[0];
			IL_B4:
			rewardText = string.Format(Translations.GetTranslation("limited_time_events_31_0", "You earned ${0}"), Utilities.ToPrefixedNumber(num, true, false));
			GameState.Money.Value += num;
			break;
		}
		case TaskManager.TaskRewardType.Hobby:
			rewardText = string.Format(Translations.GetTranslation("limited_time_events_42_3", "You earned a {0}m hobby skip!"), task.RewardAmount.ToString());
			GameState.CurrentState.UpdateHobbyProgress((float)task.RewardAmount * 60f);
			break;
		case TaskManager.TaskRewardType.Reset:
			rewardText = string.Format(Translations.GetTranslation("limited_time_events_42_2", "You earned a +{0} reset boost!"), task.RewardAmount.ToString());
			GameState.CurrentState.TimeMultiplier.Value = Mathf.Min(2048f, GameState.CurrentState.TimeMultiplier.Value + (float)task.RewardAmount);
			global::PlayerPrefs.SetFloat("TimeMultiplier", GameState.CurrentState.TimeMultiplier.Value);
			break;
		case TaskManager.TaskRewardType.Diamond:
			if (task.RewardAmount > 1)
			{
				rewardText = string.Format(Translations.GetTranslation("limited_time_events_42_5", "You earned {0} diamonds!"), task.RewardAmount.ToString());
			}
			else
			{
				rewardText = Translations.GetTranslation("everything_else_12_0", "You earned 1 diamond!");
			}
			Utilities.AwardDiamonds(task.RewardAmount);
			break;
		case TaskManager.TaskRewardType.Time:
			rewardText = string.Format(Translations.GetTranslation("limited_time_events_42_0", "You earned a {0}m time skip!"), task.RewardAmount.ToString());
			GameState.CurrentState.UpdateProgress((float)task.RewardAmount * 60f, true);
			break;
		case TaskManager.TaskRewardType.TimeBlock:
			if (task.RewardAmount == 1)
			{
				rewardText = Translations.GetTranslation("limited_time_events_42_4", "You earned a time block!");
			}
			else
			{
				rewardText = string.Format(Translations.GetTranslation("limited_time_events_43_0", "You earned {0} time blocks!"), task.RewardAmount.ToString());
			}
			FreeTime.PurchasedTime += task.RewardAmount;
			global::PlayerPrefs.SetInt("PurchasedTime", FreeTime.PurchasedTime);
			break;
		case TaskManager.TaskRewardType.EventToken:
		{
			rewardText = Translations.GetTranslation("everything_else_32_0", "You earned an event token!");
			int num2 = this.UpdateDayIcons();
			this.StoreState();
			if (num2 == this.currentEvent.TokenRequirement)
			{
				this.Claim(this.currentEvent.EventID, true, true, true);
				return;
			}
			GameState.CurrentState.CloudSave(false);
			break;
		}
		case TaskManager.TaskRewardType.Job:
			rewardText = string.Format(Translations.GetTranslation("limited_time_events_42_1", "You earned a {0}h job skip!"), (task.RewardAmount / 60).ToString());
			GameState.CurrentState.UpdateJobProgress((float)task.RewardAmount * 60f);
			break;
		}
		GameState.CurrentState.QueueQuickSave();
		this.ClaimUI(task, rewardText);
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x0002B7F8 File Offset: 0x000299F8
	private void ClaimUI(TaskData task, string rewardText)
	{
		GameObject gameObject = GameState.CurrentState.transform.Find("Popups/Affection Milestone").gameObject;
		gameObject.transform.Find("Top Text").GetComponent<Text>().text = Translations.GetTranslation("limited_time_events_33_0", "Task Complete!");
		gameObject.transform.Find("Icon").GetComponent<Image>().sprite = this.TaskIcons[(int)task.RewardType];
		gameObject.transform.Find("Icon").gameObject.SetActive(true);
		gameObject.transform.Find("+1 Diamond").gameObject.SetActive(false);
		gameObject.transform.Find("Bottom Text").GetComponent<Text>().text = rewardText;
		gameObject.SetActive(true);
	}

	// Token: 0x1700006F RID: 111
	// (get) Token: 0x0600052C RID: 1324 RVA: 0x0002B8C8 File Offset: 0x00029AC8
	public int CurrentDay
	{
		get
		{
			TimeSpan timeOffset = Utilities.TimeOffset;
			TimeSpan end = this.currentEvent.EndTimeUTC - DateTime.UtcNow + timeOffset;
			this.nextEventTimer.text = string.Format("{0}\n{1}", Translations.GetTranslation("limited_time_events_2_0", "Next event in:"), Utilities.CreateCountdown(end));
			return (int)(this.currentEvent.timeSpan.TotalDays - end.TotalDays);
		}
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x0002B93C File Offset: 0x00029B3C
	private void UpdateTimeBlockRequirement()
	{
		FreeTime.EventTime = 0;
		for (int i = 0; i < 3; i++)
		{
			Task task = this.EventTasks[this.CurrentDay * 3 + i];
			if (task.DateStart != 0L && !task.Complete)
			{
				FreeTime.EventTime += task.TaskData.TimeBlockRequirement;
			}
		}
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x0002B9A0 File Offset: 0x00029BA0
	private void StoreEventDiscount()
	{
		int num = 0;
		for (int i = 0; i < this.EventTasks.Length; i++)
		{
			string taskPrefix = "Task" + i;
			this.EventTasks[i].StoreState(taskPrefix);
			if (this.EventTasks[i].Claimed && this.EventTasks[i].TaskData.RewardType == TaskManager.TaskRewardType.EventToken)
			{
				num++;
			}
		}
		global::PlayerPrefs.SetInt(string.Format("Event{0}Tokens", this.currentEvent.EventID.ToString()), num);
		for (int j = 0; j < (int)this.currentEvent.PreviousEventID; j++)
		{
			global::PlayerPrefs.DeleteKey(string.Format("Event{0}Tokens", j.ToString()), false);
		}
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x0002BA6C File Offset: 0x00029C6C
	public void StoreState()
	{
		if (this.currentEvent == null)
		{
			global::PlayerPrefs.SetInt("EventID", this.lastEventId);
			for (int i = 0; i < 42; i++)
			{
				string str = "Task" + i;
				global::PlayerPrefs.SetLong(str + "Start", this.lastEventDate[i]);
				global::PlayerPrefs.SetInt(str + "Complete", (!this.lastEventComplete[i]) ? 0 : 1);
				global::PlayerPrefs.SetInt(str + "Claimed", (!this.lastEventClaimed[i]) ? 0 : 1);
			}
			return;
		}
		global::PlayerPrefs.SetInt("EventID", (int)this.currentEvent.EventID);
		global::PlayerPrefs.SetString("CompletedEvents", Convert.ToBase64String(TaskManager.CompletedEvents));
		this.StoreEventDiscount();
		GameState.CurrentState.QueueSave();
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x0002BB58 File Offset: 0x00029D58
	public void ConvertSave(int c2017, long c2018, long c2019, long c2020)
	{
		TaskManager.CompletedEvents = TaskManager.FromBase64Safely(global::PlayerPrefs.GetString("CompletedEvents", string.Empty));
		int num = global::PlayerPrefs.GetInt("Completed2017Events", 0);
		num |= c2017;
		long num2 = global::PlayerPrefs.GetLong("Completed2018Events", 0L);
		num2 |= c2018;
		long num3 = global::PlayerPrefs.GetLong("Completed2019Events", 0L);
		num3 |= c2019;
		long num4 = global::PlayerPrefs.GetLong("Completed2020Events", 0L);
		num4 |= c2020;
		if (num == 0 && num2 == 0L && num3 == 0L && num4 == 0L)
		{
			return;
		}
		for (int i = 0; i < 31; i++)
		{
			short num5 = (short)(i + 1);
			if (((long)num & 1L << i) != 0L && num5 < 13)
			{
				TaskManager.SetCompletedEventBit((int)num5);
			}
		}
		for (int j = 0; j < 63; j++)
		{
			short num6 = (short)(j + 13);
			if ((num2 & 1L << j) != 0L && num6 < 61)
			{
				TaskManager.SetCompletedEventBit((int)num6);
			}
		}
		for (int k = 0; k < 63; k++)
		{
			short num7 = (short)(k + 61);
			if ((num3 & 1L << k) != 0L && num7 < 102)
			{
				TaskManager.SetCompletedEventBit((int)num7);
			}
		}
		for (int l = 0; l < 63; l++)
		{
			short num8 = (short)(l + 102);
			if ((num4 & 1L << l) != 0L && num8 < 134)
			{
				TaskManager.SetCompletedEventBit((int)num8);
			}
		}
		global::PlayerPrefs.DeleteKey("Completed2017Events", false);
		global::PlayerPrefs.DeleteKey("Completed2018Events", false);
		global::PlayerPrefs.DeleteKey("Completed2019Events", false);
		global::PlayerPrefs.DeleteKey("Completed2020Events", false);
		global::PlayerPrefs.SetString("CompletedEvents", Convert.ToBase64String(TaskManager.CompletedEvents));
		if (GameState.CurrentState != null)
		{
			GameState.CurrentState.QueueSave();
		}
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x0002BD30 File Offset: 0x00029F30
	public void LoadCompletedEvents()
	{
		if (!global::PlayerPrefs.HasKey("CompletedEvents"))
		{
			this.ConvertSave(0, 0L, 0L, 0L);
		}
		else
		{
			TaskManager.CompletedEvents = TaskManager.FromBase64Safely(global::PlayerPrefs.GetString("CompletedEvents", string.Empty));
		}
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x0002BD78 File Offset: 0x00029F78
	public static byte[] FromBase64Safely(string s)
	{
		byte[] result;
		try
		{
			result = Convert.FromBase64String(s);
		}
		catch (Exception)
		{
			result = new byte[0];
		}
		return result;
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x0002BDC8 File Offset: 0x00029FC8
	public void Prestige()
	{
		if (this.currentEvent == null)
		{
			this.lastEventId = global::PlayerPrefs.GetInt("EventID", 0);
			Array.Clear(this.lastEventClaimed, 0, this.lastEventClaimed.Length);
			Array.Clear(this.lastEventComplete, 0, this.lastEventComplete.Length);
			Array.Clear(this.lastEventDate, 0, this.lastEventDate.Length);
			for (int i = 0; i < 42; i++)
			{
				string str = "Task" + i;
				this.lastEventDate[i] = global::PlayerPrefs.GetLong(str + "Start", 0L);
				this.lastEventComplete[i] = (global::PlayerPrefs.GetInt(str + "Complete", 0) == 1);
				this.lastEventClaimed[i] = (global::PlayerPrefs.GetInt(str + "Claimed", 0) == 1);
			}
		}
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x0002BEAC File Offset: 0x0002A0AC
	public void LoadState()
	{
		this.LoadCompletedEvents();
		if ((int)this.currentEvent.EventID == global::PlayerPrefs.GetInt("EventID", 0))
		{
			for (int i = 0; i < this.EventTasks.Length; i++)
			{
				string taskPrefix = "Task" + i;
				this.EventTasks[i].LoadState(taskPrefix);
			}
			this.UpdateTimeBlockRequirement();
		}
		else
		{
			for (int j = 0; j < 42; j++)
			{
				string str = "Task" + j.ToString();
				global::PlayerPrefs.DeleteKey(str + "Start", true);
				global::PlayerPrefs.DeleteKey(str + "Complete", false);
				global::PlayerPrefs.DeleteKey(str + "Claimed", false);
			}
			global::PlayerPrefs.SetInt("EventID", (int)this.currentEvent.EventID);
			GameState.CurrentState.QueueSave();
		}
	}

	// Token: 0x04000517 RID: 1303
	private EventData currentEvent;

	// Token: 0x04000518 RID: 1304
	private Text nextEventTimer;

	// Token: 0x04000519 RID: 1305
	private Text newDailyTasksTimer;

	// Token: 0x0400051A RID: 1306
	private Text offerExpiresTimer;

	// Token: 0x0400051B RID: 1307
	public GameObject TaskDay;

	// Token: 0x0400051C RID: 1308
	public static string NEWCOMER_LTE_START_DATETIME = "NewcomerLteStartDatetime";

	// Token: 0x0400051D RID: 1309
	public static string EVENT_NEWCOMER_ID = "NewcomerEventID";

	// Token: 0x0400051E RID: 1310
	private static bool shownLTEPromo = false;

	// Token: 0x0400051F RID: 1311
	private bool _isNewcomerLte;

	// Token: 0x04000520 RID: 1312
	private float tick = 1f;

	// Token: 0x04000521 RID: 1313
	private bool initialized;

	// Token: 0x04000522 RID: 1314
	private Task[] EventTasks;

	// Token: 0x04000523 RID: 1315
	private Task task1;

	// Token: 0x04000524 RID: 1316
	public Sprite[] TaskIcons;

	// Token: 0x04000525 RID: 1317
	public Sprite CompleteSprite;

	// Token: 0x04000526 RID: 1318
	public Sprite EpicSprite;

	// Token: 0x04000527 RID: 1319
	public Sprite StandardSprite;

	// Token: 0x04000528 RID: 1320
	public Sprite ClaimSprite;

	// Token: 0x04000529 RID: 1321
	private static Dictionary<TaskManager.EventRewardType, Requirement.OutfitType> eventToOutfitDic = new Dictionary<TaskManager.EventRewardType, Requirement.OutfitType>
	{
		{
			TaskManager.EventRewardType.AllOutfits,
			Requirement.OutfitType.All
		},
		{
			TaskManager.EventRewardType.BikiniOutfit,
			Requirement.OutfitType.BathingSuit
		},
		{
			TaskManager.EventRewardType.HolidayOutfit,
			Requirement.OutfitType.Christmas
		},
		{
			TaskManager.EventRewardType.MonsterOutfit,
			Requirement.OutfitType.Monster
		},
		{
			TaskManager.EventRewardType.SchoolOutfit,
			Requirement.OutfitType.SchoolUniform
		},
		{
			TaskManager.EventRewardType.UniqueOutfit,
			Requirement.OutfitType.Unique
		},
		{
			TaskManager.EventRewardType.WeddingOutfit,
			Requirement.OutfitType.DiamondRing
		}
	};

	// Token: 0x0400052A RID: 1322
	public static byte[] CompletedEvents = new byte[1];

	// Token: 0x0400052B RID: 1323
	private bool pendingAchievementRebuild;

	// Token: 0x0400052C RID: 1324
	private static double[] cashAwards = new double[]
	{
		500.0,
		7500.0,
		1500000.0,
		750000000.0,
		15000000000.0
	};

	// Token: 0x0400052D RID: 1325
	private int lastEventId;

	// Token: 0x0400052E RID: 1326
	private bool[] lastEventClaimed = new bool[42];

	// Token: 0x0400052F RID: 1327
	private bool[] lastEventComplete = new bool[42];

	// Token: 0x04000530 RID: 1328
	private long[] lastEventDate = new long[42];

	// Token: 0x020000E6 RID: 230
	public enum TaskRewardType : byte
	{
		// Token: 0x04000532 RID: 1330
		Cash,
		// Token: 0x04000533 RID: 1331
		Hobby,
		// Token: 0x04000534 RID: 1332
		Reset,
		// Token: 0x04000535 RID: 1333
		Diamond,
		// Token: 0x04000536 RID: 1334
		Time,
		// Token: 0x04000537 RID: 1335
		TimeBlock,
		// Token: 0x04000538 RID: 1336
		EventToken,
		// Token: 0x04000539 RID: 1337
		Job
	}

	// Token: 0x020000E7 RID: 231
	public enum EventRewardType : byte
	{
		// Token: 0x0400053B RID: 1339
		UniqueOutfit,
		// Token: 0x0400053C RID: 1340
		ExclusivePinup,
		// Token: 0x0400053D RID: 1341
		NewGirl,
		// Token: 0x0400053E RID: 1342
		MonsterOutfit,
		// Token: 0x0400053F RID: 1343
		HolidayOutfit,
		// Token: 0x04000540 RID: 1344
		BikiniOutfit,
		// Token: 0x04000541 RID: 1345
		SchoolOutfit,
		// Token: 0x04000542 RID: 1346
		WeddingOutfit,
		// Token: 0x04000543 RID: 1347
		AllOutfits
	}
}
