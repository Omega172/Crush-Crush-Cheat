using System;
using System.Collections.Generic;
using BlayFap;
using BlayFapShared;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000115 RID: 277
public class LoadSave : MonoBehaviour
{
	// Token: 0x060006CE RID: 1742 RVA: 0x0003AC0C File Offset: 0x00038E0C
	private void InitEmptySaveSlot(Transform slot, string slotName)
	{
		slot.GetComponent<Button>().interactable = true;
		slot.Find("Time Text").GetComponent<Text>().text = "Available";
		slot.Find("Diamond Text").GetComponent<Text>().text = "0";
		slot.GetComponent<Button>().onClick.RemoveAllListeners();
		slot.GetComponent<Button>().onClick.AddListener(delegate()
		{
			if (BlayFapClient.UsingBlayfap)
			{
				this.BlayFapExportToSlot(slotName, true);
			}
			this.gameObject.SetActive(false);
		});
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x0003AC9C File Offset: 0x00038E9C
	private void InitSaveSlot(Transform slot, Dictionary<string, object> save, string slotName, bool enable = true)
	{
		if (!save.ContainsKey("GameStateDate"))
		{
			return;
		}
		long num = 0L;
		DateTime dateTime = DateTime.Now;
		if (long.TryParse((string)save["GameStateDate"], out num))
		{
			dateTime = DateTime.FromBinary(num);
		}
		string s = (!save.ContainsKey("GameStateDiamonds")) ? "0" : ((string)save["GameStateDiamonds"]);
		slot.Find("Diamond Text").GetComponent<Text>().text = int.Parse(s).ToString("n0");
		if (num == 0L)
		{
			slot.Find("Time Text").GetComponent<Text>().text = "Unknown Save Time";
		}
		else
		{
			slot.Find("Time Text").GetComponent<Text>().text = string.Format(Translations.GetTranslation("everything_else_101_0", "Date {0}") + "/{1}/{2} \n" + Translations.GetTranslation("everything_else_101_1", "Time") + " {3}:{4}", new object[]
			{
				dateTime.Month,
				dateTime.Day,
				dateTime.Year,
				dateTime.Hour,
				((dateTime.Minute >= 10) ? string.Empty : "0") + dateTime.Minute
			});
		}
		slot.GetComponent<Button>().onClick.RemoveAllListeners();
		slot.GetComponent<Button>().onClick.AddListener(delegate()
		{
			if (BlayFapClient.UsingBlayfap)
			{
				this.BlayFapExportToSlot(slotName, true);
			}
			this.gameObject.SetActive(false);
		});
		slot.GetComponent<Button>().interactable = enable;
	}

	// Token: 0x060006D0 RID: 1744 RVA: 0x0003AE68 File Offset: 0x00039068
	public void InitSave(Dictionary<string, string> data)
	{
		base.transform.Find("Dialog/Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_50_0", "Save Game");
		string text = (!data.ContainsKey("save1")) ? "data" : "save1";
		string text2 = (!data.ContainsKey("save2")) ? "data2" : "save2";
		if (data.ContainsKey(text))
		{
			Transform slot = base.transform.Find("Dialog/Save Slot 1");
			this.InitSaveSlot(slot, global::PlayerPrefs.Import(data[text]), text, true);
		}
		else
		{
			this.InitEmptySaveSlot(base.transform.Find("Dialog/Save Slot 1"), text);
		}
		if (data.ContainsKey(text2))
		{
			Transform slot2 = base.transform.Find("Dialog/Save Slot 2");
			this.InitSaveSlot(slot2, global::PlayerPrefs.Import(data[text2]), text2, true);
		}
		else
		{
			this.InitEmptySaveSlot(base.transform.Find("Dialog/Save Slot 2"), text2);
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x060006D1 RID: 1745 RVA: 0x0003AF88 File Offset: 0x00039188
	public void InitLoad(Dictionary<string, string> data)
	{
		base.transform.Find("Dialog/Title").GetComponent<Text>().text = Translations.GetTranslation("everything_else_42_4", "Load Game");
		if (data != null)
		{
			string key = (!data.ContainsKey("save1")) ? "data" : "save1";
			string key2 = (!data.ContainsKey("save2")) ? "data2" : "save2";
			if (data.ContainsKey(key))
			{
				Transform slot = base.transform.Find("Dialog/Save Slot 1");
				this.UpdateSlot("Save Slot 1", slot, global::PlayerPrefs.Import(data[key]));
			}
			else
			{
				base.transform.Find("Dialog/Save Slot 1").GetComponent<Button>().interactable = false;
				base.transform.Find("Dialog/Save Slot 1/Time Text").GetComponent<Text>().text = "Empty";
				base.transform.Find("Dialog/Save Slot 1/Diamond Text").GetComponent<Text>().text = "0";
			}
			if (data.ContainsKey(key2))
			{
				Transform slot2 = base.transform.Find("Dialog/Save Slot 2");
				this.UpdateSlot("Save Slot 2", slot2, global::PlayerPrefs.Import(data[key2]));
			}
			else
			{
				base.transform.Find("Dialog/Save Slot 2").GetComponent<Button>().interactable = false;
				base.transform.Find("Dialog/Save Slot 2/Time Text").GetComponent<Text>().text = "Empty";
				base.transform.Find("Dialog/Save Slot 2/Diamond Text").GetComponent<Text>().text = "0";
			}
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x060006D2 RID: 1746 RVA: 0x0003B138 File Offset: 0x00039338
	public void UpdateSlot(string slotName, Transform slot, Dictionary<string, object> save)
	{
		if (!save.ContainsKey("GameStateDate"))
		{
			return;
		}
		long num = 0L;
		DateTime dateTime = DateTime.Now;
		if (long.TryParse((string)save["GameStateDate"], out num))
		{
			dateTime = DateTime.FromBinary(num);
		}
		string s = (!save.ContainsKey("GameStateDiamonds")) ? "0" : ((string)save["GameStateDiamonds"]);
		slot.Find("Diamond Text").GetComponent<Text>().text = int.Parse(s).ToString("n0");
		if (num == 0L)
		{
			slot.Find("Time Text").GetComponent<Text>().text = "Unknown Save Time";
		}
		else
		{
			slot.Find("Time Text").GetComponent<Text>().text = string.Format(Translations.GetTranslation("everything_else_101_0", "Date {0}") + "/{1}/{2} \n" + Translations.GetTranslation("everything_else_101_1", "Time") + " {3}:{4}", new object[]
			{
				dateTime.Month,
				dateTime.Day,
				dateTime.Year,
				dateTime.Hour,
				((dateTime.Minute >= 10) ? string.Empty : "0") + dateTime.Minute
			});
		}
		slot.GetComponent<Button>().onClick.RemoveAllListeners();
		slot.GetComponent<Button>().onClick.AddListener(delegate()
		{
			if (save == null || save.Count == 0)
			{
				return;
			}
			this.CompareSavedGame(slotName, save);
		});
		slot.GetComponent<Button>().interactable = true;
	}

	// Token: 0x060006D3 RID: 1747 RVA: 0x0003B31C File Offset: 0x0003951C
	public void CompareSavedGame(string slotName, Dictionary<string, object> compare)
	{
		if (slotName == "Save Slot 1")
		{
			slotName = string.Format(Translations.GetTranslation("everything_else_50_3", "Slot {0}"), "1");
		}
		else if (slotName == "Save Slot 2")
		{
			slotName = string.Format(Translations.GetTranslation("everything_else_50_3", "Slot {0}"), "2");
		}
		Transform dialog = GameState.CurrentState.transform.Find("Popups/View Saved Game/Dialog Box");
		string s = (!compare.ContainsKey("GameStateMoney")) ? "0" : ((string)compare["GameStateMoney"]);
		int diamonds = int.Parse((!compare.ContainsKey("GameStateDiamonds")) ? "0" : ((string)compare["GameStateDiamonds"]));
		int num = (!compare.ContainsKey("AchievementCount")) ? 0 : ((int)compare["AchievementCount"]);
		int num2 = (!compare.ContainsKey("GirlsUnlocked")) ? 0 : ((int)compare["GirlsUnlocked"]);
		int num3 = (!compare.ContainsKey("GameStateHobbies")) ? 0 : ((int)compare["GameStateHobbies"]);
		int num4 = 6 + num / 4 + ((!compare.ContainsKey("PurchasedTime")) ? 0 : ((int)compare["PurchasedTime"]));
		int flag = (!compare.ContainsKey("AvailableJobs")) ? 0 : ((int)compare["AvailableJobs"]);
		string lte = (!compare.ContainsKey("CompletedEvents")) ? string.Empty : ((string)compare["CompletedEvents"]);
		dialog.Find("Money/Text").GetComponent<Text>().text = Utilities.ToPrefixedNumber(double.Parse(s), false, false);
		if (diamonds == 1)
		{
			dialog.Find("Diamonds/Text").GetComponent<Text>().text = Translations.GetTranslation("requirements_6_0", "1 Diamond");
		}
		else
		{
			dialog.Find("Diamonds/Text").GetComponent<Text>().text = string.Format(Translations.GetTranslation("requirements_32_0", "{0} Diamonds"), diamonds.ToString("n0"));
		}
		if (num3 == -1)
		{
			int num5 = 0;
			for (int i = 0; i < 12; i++)
			{
				if (compare.ContainsKey("Skill" + i.ToString()) && (int)compare["Skill" + i.ToString()] > 0)
				{
					num5++;
				}
			}
			num3 = num5;
		}
		dialog.Find("Achievements/Text").GetComponent<Text>().text = string.Format("{0} " + Translations.GetTranslation("everything_else_100_3", "Achievements Completed"), num.ToString());
		dialog.Find("Girls/Text").GetComponent<Text>().text = string.Format("{0} " + Translations.GetTranslation("everything_else_100_2", "Girls Unlocked"), num2.ToString());
		dialog.Find("Hobbies/Text").GetComponent<Text>().text = string.Format("{0} " + Translations.GetTranslation("everything_else_100_1", "Hobbies Unlocked"), num3.ToString());
		dialog.Find("TimeBlocks/Text").GetComponent<Text>().text = string.Format("{0} " + Translations.GetTranslation("everything_else_100_4", "Timeblocks Unlocked"), num4.ToString());
		dialog.Find("Jobs/Text").GetComponent<Text>().text = string.Format("{0} " + Translations.GetTranslation("everything_else_25_3", "Jobs Unlocked"), Utilities.CountFromFlag(flag).ToString());
		dialog.Find("Text").GetComponent<Text>().text = slotName;
		dialog.parent.gameObject.SetActive(true);
		dialog.Find("Load Button").GetComponent<Button>().onClick.RemoveAllListeners();
		dialog.Find("Load Button").GetComponent<Button>().onClick.AddListener(delegate()
		{
			SetUserDataRequest request = new SetUserDataRequest
			{
				BlayFapId = BlayFapClient.BlayFapId,
				Data = new Dictionary<string, string>
				{
					{
						"cc_lte",
						lte
					}
				}
			};
			BlayFapClient.Instance.SetUserData(request, delegate(SetUserDataResponse response)
			{
				if (response.Error == null)
				{
					BlayFapIntegration.CompletedEventsBlayfap = lte;
					GameState.Initialized = false;
					GameState.UniverseReady.Value = false;
					GameState.Diamonds.Value = diamonds;
					global::PlayerPrefs.Import(compare, true);
				}
				else
				{
					Notifications.AddNotification(Notifications.NotificationType.Message, "There was an error loading your game.  Please contact try again later, or contact support.");
					dialog.gameObject.SetActive(false);
				}
			});
		});
	}

	// Token: 0x060006D4 RID: 1748 RVA: 0x0003B84C File Offset: 0x00039A4C
	private void BlayFapExportToSlot(string slotName, bool notify)
	{
		GameState.CurrentState.StoreState(true);
		string blayfapSlot = (!(slotName == "data") && !(slotName == "save1")) ? "save2" : "save1";
		string data = global::PlayerPrefs.Export();
		SetUserDataRequest request = new SetUserDataRequest
		{
			Data = new Dictionary<string, string>
			{
				{
					blayfapSlot,
					data
				}
			}
		};
		BlayFapClient.Instance.SetUserData(request, delegate(SetUserDataResponse response)
		{
			if (response.Error == null)
			{
				if (notify)
				{
					Notifications.AddNotification(Notifications.NotificationType.Message, Translations.GetTranslation("achievements_413_0", "Game saved successfully!"));
				}
				if (LoadSave.userDataCache != null)
				{
					if (LoadSave.userDataCache.ContainsKey(blayfapSlot))
					{
						LoadSave.userDataCache[blayfapSlot] = data;
					}
					else
					{
						LoadSave.userDataCache.Add(blayfapSlot, data);
					}
				}
			}
			else
			{
				Notifications.AddNotification(Notifications.NotificationType.Message, "There was an error saving your game.  Error: " + response.Error.ErrorType.ToString());
			}
		});
	}

	// Token: 0x060006D5 RID: 1749 RVA: 0x0003B8F0 File Offset: 0x00039AF0
	public static void Export()
	{
		if (LoadSave.userDataCache != null && (DateTime.Now - LoadSave.lastCache).TotalSeconds < 30.0)
		{
			Debug.Log("Loaded user saves from cache");
			GameState.CurrentState.transform.Find("More/Select Save Slot").GetComponent<LoadSave>().InitSave(LoadSave.userDataCache);
			return;
		}
		GetUserDataRequest request = new GetUserDataRequest
		{
			Keys = new List<string>
			{
				"save1",
				"save2"
			}
		};
		BlayFapClient.Instance.GetUserData(request, delegate(GetUserDataResponse result)
		{
			if (result.Error == null)
			{
				Debug.Log("Loaded user save from blayfap");
				if (result.Data == null)
				{
					result.Data = new Dictionary<string, string>();
				}
				GameState.CurrentState.transform.Find("More/Select Save Slot").GetComponent<LoadSave>().InitSave(result.Data);
				LoadSave.userDataCache = result.Data;
				LoadSave.lastCache = DateTime.Now;
			}
			else
			{
				Debug.Log("Failed to load user data: " + result.Error.ErrorType.ToString());
			}
		});
	}

	// Token: 0x060006D6 RID: 1750 RVA: 0x0003B9B0 File Offset: 0x00039BB0
	public static void Import()
	{
		if (LoadSave.userDataCache != null && (DateTime.Now - LoadSave.lastCache).TotalSeconds < 30.0)
		{
			Debug.Log("Loaded user saves from cache");
			GameState.CurrentState.transform.Find("More/Select Save Slot").GetComponent<LoadSave>().InitLoad(LoadSave.userDataCache);
			return;
		}
		GetUserDataRequest request = new GetUserDataRequest
		{
			Keys = new List<string>
			{
				"save1",
				"save2"
			}
		};
		BlayFapClient.Instance.GetUserData(request, delegate(GetUserDataResponse result)
		{
			if (result.Error == null)
			{
				Debug.Log("Loaded user save from blayfap");
				GameState.CurrentState.transform.Find("More/Select Save Slot").GetComponent<LoadSave>().InitLoad(result.Data);
				LoadSave.userDataCache = result.Data;
				LoadSave.lastCache = DateTime.Now;
			}
			else
			{
				Debug.Log("Failed to load user data: " + result.Error.ErrorType.ToString());
			}
		});
	}

	// Token: 0x040006C0 RID: 1728
	private static Dictionary<string, string> userDataCache;

	// Token: 0x040006C1 RID: 1729
	private static DateTime lastCache = new DateTime(1987, 2, 9);
}
