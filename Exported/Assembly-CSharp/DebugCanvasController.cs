using System;
using System.Collections;
using System.Collections.Generic;
using BlayFap;
using BlayFapShared;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000091 RID: 145
public class DebugCanvasController : MonoBehaviour
{
	// Token: 0x0600024D RID: 589 RVA: 0x00010854 File Offset: 0x0000EA54
	private void Start()
	{
		this._buttonTemplate = base.transform.Find("Template Button").gameObject;
		this._titleTemplate = base.transform.Find("Title Template").gameObject;
		this._checkboxTemplate = base.transform.Find("Checkbox Template").gameObject;
		this._spacingTemplate = base.transform.Find("Spacing Template").gameObject;
		this._inputFieldTemplate = base.transform.Find("Input Field Template").gameObject;
		this._cellphone = GameState.GetCellphone();
		this._panelTemplate = base.transform.Find("Debug Panels/Panel Template").gameObject;
		this._cellphone = GameState.GetCellphone();
		GameObject parentPanel = this.CreatePanel(this.PanelToActivate.transform, "Debug", true);
		Vector2 cellSize = new Vector2(460f, 460f);
		RectOffset rectOffset = new RectOffset(7, 0, 0, 0);
		Vector2 spacing = new Vector2(0f, 0f);
		cellSize = new Vector2(330f, 100f);
		rectOffset.left = 12;
		spacing = new Vector2(10f, 0f);
		Transform transform = this.AddGridLayout(parentPanel, cellSize, rectOffset, spacing).transform;
		this.CreateSaveAndLoadPanel(transform);
		this.CreateLtePanel(transform);
		this.CreateRuntimeAndWidgetsPanel(transform);
		this.CreateMiscPanel(transform);
		this.CreateAdsPanel(transform);
		this.CreateNotificationsPanel(transform);
		this.CreateDangerZone(transform);
		this.HideFeedbackPopup();
	}

	// Token: 0x0600024E RID: 590 RVA: 0x000109D8 File Offset: 0x0000EBD8
	private void CreateSaveAndLoadPanel(Transform mainPanelLayoutTransform)
	{
		GameObject saveLoadPanel = this.CreatePanel(this.PanelToActivate.transform, "Save and Load", false);
		Transform transform = this.AddVerticalLayout(saveLoadPanel, 5f).transform;
		this.MakeDebugButton(mainPanelLayoutTransform, "Save\nLoad", delegate
		{
			saveLoadPanel.SetActive(true);
		}, true, true);
		this.CreateSaveLoadButtons(transform, ref this._loadSlot1Button, ref this._eraseSlot1Button, 1);
		this.CreateSpacing(transform, 75f, 10f);
		this.CreateSaveLoadButtons(transform, ref this._loadSlot2Button, ref this._eraseSlot2Button, 2);
	}

	// Token: 0x0600024F RID: 591 RVA: 0x00010A74 File Offset: 0x0000EC74
	public void HandleLteWidget(bool enableWidget)
	{
		if (global::PlayerPrefs.GetInt("DebugWidgetIndex") > 0)
		{
			return;
		}
		Transform transform = base.transform.Find("Debug Button/Widget");
		if (enableWidget)
		{
			this.EnableLTEWidget(transform);
		}
		else
		{
			this.ResetWidget(transform);
		}
	}

	// Token: 0x06000250 RID: 592 RVA: 0x00010ABC File Offset: 0x0000ECBC
	private void CreateLtePanel(Transform mainPanelLayoutTransform)
	{
		GameObject ltePanel = this.CreatePanel(this.PanelToActivate.transform, "LTE", false);
		Transform transform = this.AddVerticalLayout(ltePanel, 5f).transform;
		this.CreateLteSkipButtons(transform);
		this.MakeDebugButton(transform, "Clear Purchases and LTE", delegate
		{
			this.ClearPurchasesAndLte();
		}, false, false);
		this.CreateSpacing(transform, 50f, 15f);
		this.MakeTitle(transform, "Tools", null);
		this.MakeInputField(transform, "Check LTE", "Check", delegate(string text)
		{
			short num;
			if (short.TryParse(text, out num))
			{
				string arg = (!TaskManager.IsCompletedEventSet((int)num)) ? "Not completed" : "Completed";
				this.ShowTemporaryPopup("Result", string.Format("The Event with ID: {0} is {1}", num, arg), 5f, 0f, null);
			}
			else
			{
				this.ShowTemporaryPopup("Error", "Something went wrong!", 2f, 0f, null);
			}
		});
		this.MakeInputField(transform, "Claim LTE", "Claim", delegate(string text)
		{
			short num;
			if (short.TryParse(text, out num))
			{
				if (TaskManager.IsCompletedEventSet((int)num))
				{
					this.ShowTemporaryPopup("Info", string.Format("The Event with ID: {0} has already been claimed!", num), 5f, 0f, null);
				}
				else
				{
					GameState.GetTaskSystem().DebugClaim(num);
					this.DisablePanel();
				}
			}
			else
			{
				this.ShowTemporaryPopup("Error", "Something went wrong!", 2f, 0f, null);
			}
		});
		this.MakeInputField(transform, "Has Eq. Event", "Check", delegate(string text)
		{
			short num;
			if (short.TryParse(text, out num))
			{
				string arg = (!GameState.GetTaskSystem().HasEquivalent(num)) ? "does NOT have" : "has";
				this.ShowTemporaryPopup("Info", string.Format("The Event with ID: {0} {1} an Equivalent Event completed!", num, arg), 5f, 0f, null);
			}
			else
			{
				this.ShowTemporaryPopup("Error", "Something went wrong!", 2f, 0f, null);
			}
		});
		this.MakeDebugButton(mainPanelLayoutTransform, "LTE", delegate
		{
			ltePanel.SetActive(true);
			this.UpdateLteData();
		}, true, true);
	}

	// Token: 0x06000251 RID: 593 RVA: 0x00010BC8 File Offset: 0x0000EDC8
	private void CreateRuntimeAndWidgetsPanel(Transform mainPanelLayoutTransform)
	{
		GameObject runtimePanel = this.CreatePanel(this.PanelToActivate.transform, "Runtime & Widgets", false);
		Transform transform = this.AddVerticalLayout(runtimePanel, 5f).transform;
		Transform widgetTransform = base.transform.Find("Debug Button/Widget");
		this.MakeTitle(transform, "Runtime", new Color?(this.COLOR_BROWN_DARKER));
		this.MakeCheckboxButton(transform, "Auto Bump Affection", delegate(bool isChecked)
		{
			this._girlsAutoBumpAffection = isChecked;
		});
		this.MakeCheckboxButton(transform, "Skip Phone Messages", delegate(bool isChecked)
		{
			this._cellphoneSkipMessage = isChecked;
		});
		this.CreateSpacing(transform, 50f, 15f);
		this.MakeTitle(transform, "Widgets", new Color?(this.COLOR_BROWN_DARK));
		this.CreateWidgetRadioButton(transform, "Skip LTE", delegate
		{
			this.EnableLTEWidget(widgetTransform);
		});
		this.CreateWidgetRadioButton(transform, "Runtime Automations", delegate
		{
			this.EnableRuntimeAutomationsWidget(widgetTransform);
		});
		this.MakeDebugButton(mainPanelLayoutTransform, "Runtime\n&\nWidgets", delegate
		{
			runtimePanel.SetActive(true);
		}, true, true);
	}

	// Token: 0x06000252 RID: 594 RVA: 0x00010CEC File Offset: 0x0000EEEC
	private void CreateMiscPanel(Transform mainPanelLayoutTransform)
	{
		GameObject miscPanel = this.CreatePanel(this.PanelToActivate.transform, "Miscellaneous", false);
		Transform transform = this.AddVerticalLayout(miscPanel, 5f).transform;
		this.MakeDebugButton(transform, "Clear Asset Cache", delegate
		{
			Caching.CleanCache();
		}, false, true);
		this.MakeDebugButton(transform, "Toggle Speed Dating", delegate
		{
			GameState.DebugSpeedDating = !GameState.DebugSpeedDating;
			if (!GameState.DebugSpeedDating && !GameState.GetGirlScreen().IsUnlocked(Balance.GirlName.Catara))
			{
				for (int i = 0; i < Girls.CurrentGirl.Requirements.Length; i++)
				{
					GameState.GetGirlScreen().Requirements[i + 1].transform.Find("Speed").gameObject.SetActive(false);
				}
			}
		}, false, true);
		this.MakeDebugButton(transform, "Clear Purchased Boost", delegate
		{
			GameState.PurchasedMultiplier = 0;
			GameState.CurrentState.transform.Find("Store Revamp").GetComponent<Store2>().UpdateUI();
		}, false, true);
		this.MakeDebugButton(transform, "Max Out Reset Boost", delegate
		{
			GameState.CurrentState.TimeMultiplier.Value = 2048f;
			GameState.CurrentState.QueueSave();
		}, false, true);
		this.MakeDebugButton(transform, "Reset Boost to 1.0", delegate
		{
			GameState.CurrentState.TimeMultiplier.Value = 1f;
			GameState.CurrentState.QueueSave();
		}, false, true);
		this.MakeDebugButton(mainPanelLayoutTransform, "Misc.", delegate
		{
			miscPanel.SetActive(true);
		}, true, true);
	}

	// Token: 0x06000253 RID: 595 RVA: 0x00010E2C File Offset: 0x0000F02C
	private void CreateAdsPanel(Transform mainPanelLayoutTransform)
	{
	}

	// Token: 0x06000254 RID: 596 RVA: 0x00010E30 File Offset: 0x0000F030
	private void CreateNotificationsPanel(Transform mainPanelLayoutTransform)
	{
	}

	// Token: 0x06000255 RID: 597 RVA: 0x00010E34 File Offset: 0x0000F034
	private void CreateDangerZone(Transform mainPanelLayoutTransform)
	{
		this.CreateSpacing(mainPanelLayoutTransform, 1f, 1f);
		this.CreateSpacing(mainPanelLayoutTransform, 1f, 1f);
		if (this._mainPanelButtonCount % 2 != 0)
		{
			this.CreateSpacing(mainPanelLayoutTransform, 1f, 1f);
		}
		this.MakeDebugButton(mainPanelLayoutTransform, "Reset\nGame", delegate
		{
			this.LoadDebugSave(global::PlayerPrefs.Export());
		}, true, true);
		this.MakeDebugButton(mainPanelLayoutTransform, "Destroy\nDebug", delegate
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}, true, true);
	}

	// Token: 0x06000256 RID: 598 RVA: 0x00010EBC File Offset: 0x0000F0BC
	private GameObject CreatePanel(Transform parentTransform, string title, bool isMainPanel = false)
	{
		GameObject go = (GameObject)UnityEngine.Object.Instantiate(this._panelTemplate, parentTransform);
		go.name = title + " Panel";
		go.transform.Find("Title Parent/Title").GetComponent<Text>().text = title;
		Button component = go.transform.Find("Title Parent/Back Button").GetComponent<Button>();
		go.transform.Find("Title Parent").GetComponent<Image>().color = ((!isMainPanel) ? this.COLOR_BROWN_DARKEST : this.COLOR_BROWN_DARKER);
		component.onClick.AddListener(delegate()
		{
			if (isMainPanel)
			{
				this.DisablePanel();
			}
			else
			{
				go.SetActive(false);
			}
		});
		if (isMainPanel)
		{
			UnityEngine.Object.Destroy(go.GetComponent<BackButtonView>());
			go.SetActive(true);
		}
		return go;
	}

	// Token: 0x06000257 RID: 599 RVA: 0x00010FC4 File Offset: 0x0000F1C4
	private GridLayoutGroup AddGridLayout(GameObject parentPanel, Vector2 cellSize, RectOffset padding, Vector2 spacing)
	{
		GridLayoutGroup gridLayoutGroup = parentPanel.transform.Find("Scroll View/Viewport/Content").gameObject.AddComponent<GridLayoutGroup>();
		gridLayoutGroup.cellSize = cellSize;
		gridLayoutGroup.padding = padding;
		gridLayoutGroup.spacing = spacing;
		return gridLayoutGroup;
	}

	// Token: 0x06000258 RID: 600 RVA: 0x00011004 File Offset: 0x0000F204
	private VerticalLayoutGroup AddVerticalLayout(GameObject parentPanel, float spacing)
	{
		RectOffset padding = new RectOffset(0, 0, 50, 0);
		padding = new RectOffset(0, 0, 10, 0);
		VerticalLayoutGroup verticalLayoutGroup = parentPanel.transform.Find("Scroll View/Viewport/Content").gameObject.AddComponent<VerticalLayoutGroup>();
		verticalLayoutGroup.padding = padding;
		verticalLayoutGroup.spacing = spacing;
		verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
		return verticalLayoutGroup;
	}

	// Token: 0x06000259 RID: 601 RVA: 0x00011058 File Offset: 0x0000F258
	private GameObject CreateSpacing(Transform parentPanel, float spacingMobile, float spacingWebGL)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this._spacingTemplate, parentPanel.transform);
		gameObject.name = "Spacing: " + spacingMobile;
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.sizeDelta = new Vector2(component.sizeDelta.x, spacingWebGL);
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x0600025A RID: 602 RVA: 0x000110C0 File Offset: 0x0000F2C0
	private GameObject MakeTitle(Transform parentTransform, string text, Color? color = null)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this._titleTemplate, parentTransform);
		gameObject.name = text + " Title";
		gameObject.GetComponentInChildren<Text>().text = text;
		if (color != null)
		{
			gameObject.GetComponent<Image>().color = color.Value;
		}
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x0600025B RID: 603 RVA: 0x00011124 File Offset: 0x0000F324
	private GameObject MakeDebugButton(Transform parentTransform, string text, Action callback, bool isMainPanel = false, bool enableWordWrapping = true)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this._buttonTemplate, parentTransform);
		gameObject.name = text + " Button";
		text = text.Replace("\n", " ");
		gameObject.GetComponentInChildren<Text>().text = text;
		gameObject.GetComponent<Button>().onClick.AddListener(delegate()
		{
			callback();
		});
		if (isMainPanel)
		{
			this._mainPanelButtonCount++;
		}
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x0600025C RID: 604 RVA: 0x000111C0 File Offset: 0x0000F3C0
	private void MakeCheckboxButton(Transform parentTransform, string text, Action<bool> callback)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this._checkboxTemplate, parentTransform);
		gameObject.name = text + " Button";
		gameObject.GetComponentInChildren<Text>().text = text;
		CheckBox checkbox = gameObject.GetComponent<CheckBox>();
		checkbox.Checked = false;
		gameObject.GetComponent<Button>().onClick.AddListener(delegate()
		{
			callback(checkbox.Checked);
		});
		gameObject.SetActive(true);
	}

	// Token: 0x0600025D RID: 605 RVA: 0x00011244 File Offset: 0x0000F444
	private void CreateLteSkipButtons(Transform panelTransform)
	{
		RectTransform component = this.MakeTitle(panelTransform, "LTE Date", new Color?(Color.clear)).GetComponent<RectTransform>();
		this.UpdateLteData();
		GameObject gameObject = this.MakeDebugButton(panelTransform, "Prev.", delegate
		{
			GameState.CurrentState.PrevLTEDay();
		}, false, true);
		GameObject gameObject2 = this.MakeDebugButton(panelTransform, "Next", delegate
		{
			GameState.CurrentState.NextLTEDay();
		}, false, true);
		GameObject gameObject3 = this.MakeDebugButton(panelTransform, "Reset", delegate
		{
			GameState.CurrentState.ResetLTEDay();
		}, false, true);
		List<GameObject> objectlist = new List<GameObject>
		{
			gameObject.gameObject,
			gameObject2.gameObject,
			gameObject3.gameObject
		};
		this.MakeHorizontalLayout(panelTransform, "LTE Layout", gameObject.GetComponent<RectTransform>().sizeDelta.y, objectlist);
	}

	// Token: 0x0600025E RID: 606 RVA: 0x0001134C File Offset: 0x0000F54C
	public void UpdateLteData()
	{
		if (!this._hasInit)
		{
			return;
		}
		string text = Utilities.ServerTime.ToShortDateString();
		base.transform.Find("Debug Panels/LTE Panel/Scroll View/Viewport/Content/LTE Date Title/Title").GetComponent<Text>().text = text;
		if (base.transform.Find("Debug Button/Widget/Label").GetComponent<Text>().text == "LTE Skip")
		{
			base.transform.Find("Debug Button/Widget/Feedback Text").GetComponent<Text>().text = text;
		}
	}

	// Token: 0x0600025F RID: 607 RVA: 0x000113D4 File Offset: 0x0000F5D4
	private void MakeInputField(Transform parentTransform, string label, string buttonText, Action<string> callback)
	{
		Debug.Log("TODO");
	}

	// Token: 0x06000260 RID: 608 RVA: 0x000113E0 File Offset: 0x0000F5E0
	private void MakeHorizontalLayout(Transform parentTransform, string name, float height, List<GameObject> objectlist)
	{
		GameObject gameObject = new GameObject(name);
		gameObject.transform.SetParent(parentTransform);
		gameObject.transform.localScale = Vector3.one;
		RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
		float num = 600f;
		rectTransform.sizeDelta = new Vector2(num, height);
		HorizontalLayoutGroup horizontalLayoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
		float x = (num - 5f * (float)objectlist.Count) / (float)objectlist.Count;
		foreach (GameObject gameObject2 in objectlist)
		{
			gameObject2.GetComponent<RectTransform>().sizeDelta = new Vector2(x, gameObject2.GetComponent<RectTransform>().sizeDelta.y);
			gameObject2.transform.SetParent(gameObject.transform);
		}
	}

	// Token: 0x06000261 RID: 609 RVA: 0x000114D8 File Offset: 0x0000F6D8
	private void CreateNotificationButtons(Transform targetTransform)
	{
		MobileNotificationProxy mobileNotificationProxy = base.GetComponent<MobileNotificationProxy>();
		GameObject gameObject = this.MakeDebugButton(targetTransform, "5''", delegate
		{
			mobileNotificationProxy.SendTestNotification(5);
		}, false, true);
		GameObject item = this.MakeDebugButton(targetTransform, "30''", delegate
		{
			mobileNotificationProxy.SendTestNotification(30);
		}, false, true);
		GameObject item2 = this.MakeDebugButton(targetTransform, "Cancel", delegate
		{
			mobileNotificationProxy.CancelTestNotification();
		}, false, true);
		List<GameObject> objectlist = new List<GameObject>
		{
			gameObject,
			item,
			item2
		};
		this.MakeHorizontalLayout(targetTransform, "Notifications", gameObject.GetComponent<RectTransform>().sizeDelta.y, objectlist);
	}

	// Token: 0x06000262 RID: 610 RVA: 0x00011590 File Offset: 0x0000F790
	private void CreateSaveLoadButtons(Transform runtimePanel, ref Button loadButton, ref Button eraseButton, int slot)
	{
		this.MakeTitle(runtimePanel, "Save Slot: " + slot, new Color?((slot != 1) ? this.COLOR_BROWN_DARK : this.COLOR_BROWN_DARKER));
		string text = (slot != 1) ? this._saveSlot2 : this._saveSlot1;
		loadButton = this.MakeDebugButton(runtimePanel, "Load", delegate
		{
			this.UpdateSaveSlots(delegate
			{
				this.LoadDebugSave((slot != 1) ? this._saveSlot2 : this._saveSlot1);
			});
		}, false, true).GetComponent<Button>();
		eraseButton = this.MakeDebugButton(runtimePanel, "Erase", delegate
		{
			this.EraseSave(slot);
		}, false, true).GetComponent<Button>();
		GameObject gameObject = this.MakeDebugButton(runtimePanel, "Save", delegate
		{
			string text2 = global::PlayerPrefs.Export();
			this.BackupSave(slot, text2);
			if (slot == 1)
			{
				this._saveSlot1 = text2;
			}
			else
			{
				this._saveSlot2 = text2;
			}
			this.UpdateSaveButtons();
		}, false, true);
		List<GameObject> objectlist = new List<GameObject>
		{
			gameObject,
			loadButton.gameObject,
			eraseButton.gameObject
		};
		this.MakeHorizontalLayout(runtimePanel, "Save Layout " + slot, gameObject.GetComponent<RectTransform>().sizeDelta.y, objectlist);
	}

	// Token: 0x06000263 RID: 611 RVA: 0x000116CC File Offset: 0x0000F8CC
	private void LoadDebugSave(string save)
	{
		Dictionary<string, object> dictionary = global::PlayerPrefs.Import(save);
		int value = int.Parse((!dictionary.ContainsKey("GameStateDiamonds")) ? "0" : ((string)dictionary["GameStateDiamonds"]));
		GameState.Initialized = false;
		GameState.UniverseReady.Value = false;
		GameState.Diamonds.Value = value;
		if (BlayFapIntegration.IsTestDevice)
		{
			string @string = global::PlayerPrefs.GetString("DO-NOT-BACKUPscheduledNotificationsJson", string.Empty);
			if (!string.IsNullOrEmpty(@string))
			{
				if (dictionary.ContainsKey("DO-NOT-BACKUPscheduledNotificationsJson"))
				{
					dictionary["DO-NOT-BACKUPscheduledNotificationsJson"] = @string;
				}
				else
				{
					dictionary.Add("DO-NOT-BACKUPscheduledNotificationsJson", @string);
				}
			}
			if (!dictionary.ContainsKey("DO-NOT-BACKUPMNM_SEEN_PP"))
			{
				dictionary.Add("DO-NOT-BACKUPMNM_SEEN_PP", 1);
			}
			GameState.ResetShownPromo();
		}
		global::PlayerPrefs.Import(dictionary, true);
	}

	// Token: 0x06000264 RID: 612 RVA: 0x000117AC File Offset: 0x0000F9AC
	private void BackupSave(int slot, string save)
	{
		SetUserDataRequest request = new SetUserDataRequest
		{
			Data = new Dictionary<string, string>
			{
				{
					"cc_save_debug" + slot,
					save
				}
			}
		};
		BlayFapClient.Instance.SetUserData(request, delegate(SetUserDataResponse response)
		{
		});
	}

	// Token: 0x06000265 RID: 613 RVA: 0x00011810 File Offset: 0x0000FA10
	private void EraseSave(int slot)
	{
		SetUserDataRequest request = new SetUserDataRequest
		{
			KeysToRemove = new List<string>(new string[]
			{
				"cc_save_debug" + slot
			})
		};
		BlayFapClient.Instance.SetUserData(request, delegate(SetUserDataResponse response)
		{
			if (response.Error == null)
			{
				if (slot == 1)
				{
					this._saveSlot1 = string.Empty;
				}
				else
				{
					this._saveSlot2 = string.Empty;
				}
				this.UpdateSaveButtons();
			}
		});
	}

	// Token: 0x06000266 RID: 614 RVA: 0x0001187C File Offset: 0x0000FA7C
	private void ResetWidget(Transform parentTransform)
	{
		parentTransform.Find("Label").GetComponent<Text>().text = string.Empty;
		parentTransform.Find("Right Button").gameObject.SetActive(false);
		parentTransform.Find("Right Button").GetComponent<Image>().color = this.COLOR_WIDGET_BUTTON;
		parentTransform.Find("Right Button/Image").gameObject.SetActive(false);
		parentTransform.Find("Left Button").gameObject.SetActive(false);
		parentTransform.Find("Left Button/Image").gameObject.SetActive(false);
		parentTransform.Find("Left Button").GetComponent<Image>().color = this.COLOR_WIDGET_BUTTON;
		parentTransform.Find("Up Button").gameObject.SetActive(false);
		parentTransform.Find("Up Button/Image").gameObject.SetActive(false);
		parentTransform.Find("Up Button").GetComponent<Image>().color = this.COLOR_WIDGET_BUTTON;
		parentTransform.Find("Down Button").gameObject.SetActive(false);
		parentTransform.Find("Down Button/Image").gameObject.SetActive(false);
		parentTransform.Find("Down Button").GetComponent<Image>().color = this.COLOR_WIDGET_BUTTON;
		parentTransform.Find("Feedback Text").GetComponent<Text>().text = string.Empty;
	}

	// Token: 0x06000267 RID: 615 RVA: 0x000119DC File Offset: 0x0000FBDC
	private void SetWidgetButton(Transform parentTransform, string buttonPath, string label, Action callback, bool isToggle = false)
	{
		Button button = parentTransform.Find(buttonPath).GetComponent<Button>();
		button.onClick.RemoveAllListeners();
		if (callback != null)
		{
			button.onClick.AddListener(delegate()
			{
				callback();
				button.GetComponent<Image>().color = ((!isToggle || !(button.GetComponent<Image>().color == this.COLOR_WIDGET_BUTTON)) ? this.COLOR_WIDGET_BUTTON : this.COLOR_WIDGET_PRESSED_BUTTON);
			});
		}
		parentTransform.Find(buttonPath).gameObject.SetActive(callback != null);
		parentTransform.Find(buttonPath + "/Text").GetComponent<Text>().text = ((!string.IsNullOrEmpty(label)) ? label : string.Empty);
		parentTransform.Find(buttonPath + "/Image").gameObject.SetActive(!string.IsNullOrEmpty(label));
	}

	// Token: 0x06000268 RID: 616 RVA: 0x00011AC4 File Offset: 0x0000FCC4
	private void CreateWidgetRadioButton(Transform parentTransform, string text, Action widgetCallback)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this._checkboxTemplate, parentTransform);
		gameObject.name = text + " Button";
		gameObject.GetComponentInChildren<Text>().text = text;
		CheckBox checkbox = gameObject.GetComponent<CheckBox>();
		checkbox.Checked = false;
		int widgetIndex = this._radioToCallback.Count;
		gameObject.GetComponent<Button>().onClick.AddListener(delegate()
		{
			this.TriggerWidget(checkbox, widgetIndex);
		});
		gameObject.SetActive(true);
		this._radioToCallback.Add(checkbox, widgetCallback);
		if (widgetIndex == global::PlayerPrefs.GetInt("DebugWidgetIndex", 0) - 1)
		{
			checkbox.Checked = true;
			this.TriggerWidget(checkbox, widgetIndex);
		}
	}

	// Token: 0x06000269 RID: 617 RVA: 0x00011BA4 File Offset: 0x0000FDA4
	private void EnableLTEWidget(Transform widgetTransform)
	{
		this.ResetWidget(widgetTransform);
		widgetTransform.Find("Label").GetComponent<Text>().text = "LTE Skip";
		Text component = widgetTransform.Find("Feedback Text").GetComponent<Text>();
		component.text = Utilities.ServerTime.ToShortDateString();
		this.SetWidgetButton(widgetTransform, "Right Button", "Next", delegate
		{
			GameState.CurrentState.NextLTEDay();
		}, false);
		this.SetWidgetButton(widgetTransform, "Left Button", "Previous", delegate
		{
			GameState.CurrentState.PrevLTEDay();
		}, false);
		this.SetWidgetButton(widgetTransform, "Up Button", "Reset", delegate
		{
			GameState.CurrentState.ResetLTEDay();
		}, false);
		widgetTransform.gameObject.SetActive(true);
	}

	// Token: 0x0600026A RID: 618 RVA: 0x00011C90 File Offset: 0x0000FE90
	private void EnableRuntimeAutomationsWidget(Transform widgetTransform)
	{
		this.ResetWidget(widgetTransform);
		widgetTransform.Find("Label").GetComponent<Text>().text = "Automations";
		this.SetWidgetButton(widgetTransform, "Up Button", "Bump Love", delegate
		{
			this._girlsAutoBumpAffection = !this._girlsAutoBumpAffection;
		}, true);
		this.SetWidgetButton(widgetTransform, "Left Button", "Auto Skip Msgs", delegate
		{
			this._cellphoneSkipMessage = !this._cellphoneSkipMessage;
		}, true);
		this.SetWidgetButton(widgetTransform, "Right Button", "Skip Next Msg", delegate
		{
			this._cellphone.Debug_SkipMessage();
		}, false);
		widgetTransform.gameObject.SetActive(true);
	}

	// Token: 0x0600026B RID: 619 RVA: 0x00011D24 File Offset: 0x0000FF24
	public void Init()
	{
		if (this._hasInit)
		{
			return;
		}
		this._hasInit = true;
		this.UpdateSaveSlots(null);
	}

	// Token: 0x0600026C RID: 620 RVA: 0x00011D40 File Offset: 0x0000FF40
	private void UpdateSaveSlots(Action callback = null)
	{
		GetUserDataRequest request = new GetUserDataRequest
		{
			BlayFapId = BlayFapClient.BlayFapId,
			Keys = new List<string>
			{
				"cc_save_debug1",
				"cc_save_debug2"
			}
		};
		BlayFapClient.Instance.GetUserData(request, delegate(GetUserDataResponse dataResponse)
		{
			if (dataResponse != null && dataResponse.Error == null && dataResponse.Data != null)
			{
				foreach (KeyValuePair<string, string> keyValuePair in dataResponse.Data)
				{
					if (keyValuePair.Key == "cc_save_debug1" && !string.IsNullOrEmpty(keyValuePair.Value))
					{
						this._saveSlot1 = keyValuePair.Value;
					}
					if (keyValuePair.Key == "cc_save_debug2" && !string.IsNullOrEmpty(keyValuePair.Value))
					{
						this._saveSlot2 = keyValuePair.Value;
					}
				}
				this.UpdateSaveButtons();
			}
			if (callback != null)
			{
				callback();
			}
		});
	}

	// Token: 0x0600026D RID: 621 RVA: 0x00011DB0 File Offset: 0x0000FFB0
	public void EnablePanel()
	{
		this.PanelToActivate.SetActive(true);
		this.UpdateSaveButtons();
	}

	// Token: 0x0600026E RID: 622 RVA: 0x00011DC4 File Offset: 0x0000FFC4
	private void UpdateSaveButtons()
	{
		if (this._loadSlot1Button == null)
		{
			return;
		}
		this._loadSlot1Button.interactable = !string.IsNullOrEmpty(this._saveSlot1);
		this._eraseSlot1Button.interactable = !string.IsNullOrEmpty(this._saveSlot1);
		this._loadSlot2Button.interactable = !string.IsNullOrEmpty(this._saveSlot2);
		this._eraseSlot2Button.interactable = !string.IsNullOrEmpty(this._saveSlot2);
	}

	// Token: 0x0600026F RID: 623 RVA: 0x00011E48 File Offset: 0x00010048
	public void DisablePanel()
	{
		this.PanelToActivate.SetActive(false);
	}

	// Token: 0x06000270 RID: 624 RVA: 0x00011E58 File Offset: 0x00010058
	public void SetActivePanel(Button fromButton)
	{
		foreach (Button button in this._tabButtonsToPanelDic.Keys)
		{
			button.GetComponent<Image>().color = ((!(fromButton == button)) ? (Color.white * 0.75f) : Color.white);
			this._tabButtonsToPanelDic[button].gameObject.SetActive(fromButton == button);
		}
	}

	// Token: 0x06000271 RID: 625 RVA: 0x00011F0C File Offset: 0x0001010C
	private void TriggerWidget(CheckBox checkbox, int index)
	{
		foreach (CheckBox checkBox in this._radioToCallback.Keys)
		{
			if (checkBox != checkbox)
			{
				checkBox.Checked = false;
			}
		}
		int value = 0;
		if (checkbox.Checked)
		{
			this._radioToCallback[checkbox]();
			value = index + 1;
		}
		else
		{
			this.ResetWidget(base.transform.Find("Debug Button/Widget"));
		}
		global::PlayerPrefs.SetInt("DebugWidgetIndex", value);
		GameState.CurrentState.QueueSave();
	}

	// Token: 0x06000272 RID: 626 RVA: 0x00011FD8 File Offset: 0x000101D8
	private void ShowFeedbackPopup(string title, string text)
	{
		if (this._feedbackPopup == null)
		{
			return;
		}
		this._feedbackPopup.transform.Find("Dialog/Title Parent/Title").GetComponent<Text>().text = title;
		Text component = this._feedbackPopup.transform.Find("Dialog/Text").GetComponent<Text>();
		component.text = text;
		Debug.Log("TODO");
		this._feedbackPopup.SetActive(true);
	}

	// Token: 0x06000273 RID: 627 RVA: 0x00012050 File Offset: 0x00010250
	private void HideFeedbackPopup()
	{
		if (this._feedbackPopup != null)
		{
			this._feedbackPopup.SetActive(false);
		}
	}

	// Token: 0x06000274 RID: 628 RVA: 0x00012070 File Offset: 0x00010270
	public void ShowTemporaryPopup(string title, string text, float durationInSeconds, float delay = 0f, Action callback = null)
	{
		GameState.CurrentState.StartCoroutine(this.DoShowTemporaryPopup(this._temporaryPopup, title, text, durationInSeconds, delay, callback));
	}

	// Token: 0x06000275 RID: 629 RVA: 0x0001209C File Offset: 0x0001029C
	public void ShowTemporaryFeedbackPopup(string title, string text, float durationInSeconds, float delay = 0f, Action callback = null)
	{
		GameState.CurrentState.StartCoroutine(this.DoShowTemporaryPopup(this._feedbackPopup, title, text, durationInSeconds, delay, callback));
	}

	// Token: 0x06000276 RID: 630 RVA: 0x000120C8 File Offset: 0x000102C8
	private IEnumerator DoShowTemporaryPopup(GameObject prefab, string title, string text, float durationInSeconds, float delay, Action callback)
	{
		while ((delay -= Time.deltaTime) > 0f)
		{
			yield return null;
		}
		base.transform.SetAsLastSibling();
		GameObject popup = (GameObject)UnityEngine.Object.Instantiate(prefab, base.transform);
		bool isTemporaryPopup = prefab == this._temporaryPopup;
		RectTransform rectTransform = popup.transform.Find("Dialog").GetComponent<RectTransform>();
		if (isTemporaryPopup && this._temporaryPopupCount > 0)
		{
			rectTransform.anchoredPosition += new Vector2(0f, (float)this._temporaryPopupCount * rectTransform.sizeDelta.y);
		}
		if (isTemporaryPopup)
		{
			this._temporaryPopupCount++;
		}
		popup.transform.Find("Dialog/Title Parent/Title").GetComponent<Text>().text = title;
		Text textTMP = popup.transform.Find("Dialog/Text").GetComponent<Text>();
		textTMP.text = text;
		Debug.Log("TODO");
		popup.SetActive(true);
		bool callbackExecuted = false;
		while ((durationInSeconds -= Time.deltaTime) > 0f)
		{
			yield return null;
			if (!popup.activeSelf && callback != null)
			{
				callbackExecuted = true;
				UnityEngine.Object.Destroy(popup);
				callback();
				if (isTemporaryPopup)
				{
					this._temporaryPopupCount--;
				}
				yield break;
			}
		}
		UnityEngine.Object.Destroy(popup);
		if (isTemporaryPopup)
		{
			this._temporaryPopupCount--;
		}
		if (!callbackExecuted && callback != null)
		{
			callback();
		}
		yield break;
	}

	// Token: 0x06000277 RID: 631 RVA: 0x00012140 File Offset: 0x00010340
	private void ClearPurchasesAndLte()
	{
		this.ShowFeedbackPopup("Erasing LTE", "Please wait while we reach Blayfap");
		BlayFapIntegration.ClearPurchasesAndLte(new Action<bool>(this.EraseLTECallback));
	}

	// Token: 0x06000278 RID: 632 RVA: 0x00012164 File Offset: 0x00010364
	private void EraseLTECallback(bool result)
	{
		GameState.CurrentState.ResetLTEDay();
		this.LoadDebugSave(global::PlayerPrefs.Export());
	}

	// Token: 0x06000279 RID: 633 RVA: 0x0001217C File Offset: 0x0001037C
	public void AddScheduledNotification()
	{
		GameObject gameObject = this.NotificationsList[0].gameObject;
		GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject, gameObject.transform.parent);
		this.NotificationsList.Add(gameObject2.transform);
		gameObject2.transform.SetSiblingIndex(gameObject2.transform.GetSiblingIndex() - 1);
		LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject2.GetComponent<RectTransform>());
	}

	// Token: 0x0600027A RID: 634 RVA: 0x000121E8 File Offset: 0x000103E8
	public void RemoveScheduledNotification()
	{
		if (this.NotificationsList.Count <= 1)
		{
			return;
		}
		Transform transform = this.NotificationsList[this.NotificationsList.Count - 1];
		this.NotificationsList.Remove(transform);
		UnityEngine.Object.Destroy(transform.gameObject);
	}

	// Token: 0x0600027B RID: 635 RVA: 0x00012238 File Offset: 0x00010438
	public BundleEventData GetScheduledNotificationsBundleData()
	{
		string @string = global::PlayerPrefs.GetString("DO-NOT-BACKUPscheduledNotificationsJson", string.Empty);
		if (string.IsNullOrEmpty(@string))
		{
			return null;
		}
		BundleEventData bundleEventData = JsonUtility.FromJson<BundleEventData>(@string);
		if (bundleEventData == null && string.IsNullOrEmpty(@string))
		{
			this.ShowTemporaryPopup("Error", "Could not load Scheduled notifications from Json: " + @string, 5f, 0f, null);
			return null;
		}
		return bundleEventData;
	}

	// Token: 0x0600027C RID: 636 RVA: 0x000122A0 File Offset: 0x000104A0
	private void Update()
	{
		if (this._cellphoneSkipMessage && Time.frameCount % 15 == 0)
		{
			this._cellphone.Debug_SkipMessage();
		}
		if (this._girlsAutoBumpAffection && Time.frameCount % 3 == 0)
		{
			GameState.GetGirlScreen().BumpAffection();
		}
	}

	// Token: 0x0600027D RID: 637 RVA: 0x000122F4 File Offset: 0x000104F4
	public void SetCellphoneSkip(bool skip)
	{
		this._cellphoneSkipMessage = skip;
	}

	// Token: 0x0600027E RID: 638 RVA: 0x00012300 File Offset: 0x00010500
	public void SetBumpAfection(bool bumpAffection)
	{
		this._girlsAutoBumpAffection = bumpAffection;
	}

	// Token: 0x0600027F RID: 639 RVA: 0x0001230C File Offset: 0x0001050C
	private void ResetAllAds()
	{
	}

	// Token: 0x040002C9 RID: 713
	public const string PrefsWidgetEnabled = "DebugWidgetIndex";

	// Token: 0x040002CA RID: 714
	public const string PrefsSavePrefix = "cc_save_debug";

	// Token: 0x040002CB RID: 715
	private const string SCHEDULED_NOTIFICATIONS_KEY = "DO-NOT-BACKUPscheduledNotificationsJson";

	// Token: 0x040002CC RID: 716
	[SerializeField]
	private GameObject PanelToActivate;

	// Token: 0x040002CD RID: 717
	private GameObject _buttonTemplate;

	// Token: 0x040002CE RID: 718
	private GameObject _titleTemplate;

	// Token: 0x040002CF RID: 719
	private GameObject _feedbackPopup;

	// Token: 0x040002D0 RID: 720
	private GameObject _temporaryPopup;

	// Token: 0x040002D1 RID: 721
	private GameObject _checkboxTemplate;

	// Token: 0x040002D2 RID: 722
	private GameObject _panelTemplate;

	// Token: 0x040002D3 RID: 723
	private GameObject _spacingTemplate;

	// Token: 0x040002D4 RID: 724
	private GameObject _inputFieldTemplate;

	// Token: 0x040002D5 RID: 725
	private Cellphone _cellphone;

	// Token: 0x040002D6 RID: 726
	private bool _cellphoneSkipMessage;

	// Token: 0x040002D7 RID: 727
	private bool _girlsAutoBumpAffection;

	// Token: 0x040002D8 RID: 728
	private Dictionary<Button, Transform> _tabButtonsToPanelDic;

	// Token: 0x040002D9 RID: 729
	private List<CheckBox> _radioList = new List<CheckBox>();

	// Token: 0x040002DA RID: 730
	private Dictionary<CheckBox, Action> _radioToCallback = new Dictionary<CheckBox, Action>();

	// Token: 0x040002DB RID: 731
	private string _saveSlot1;

	// Token: 0x040002DC RID: 732
	private string _saveSlot2;

	// Token: 0x040002DD RID: 733
	private Button _loadSlot1Button;

	// Token: 0x040002DE RID: 734
	private Button _loadSlot2Button;

	// Token: 0x040002DF RID: 735
	private Button _eraseSlot1Button;

	// Token: 0x040002E0 RID: 736
	private Button _eraseSlot2Button;

	// Token: 0x040002E1 RID: 737
	private bool _hasInit;

	// Token: 0x040002E2 RID: 738
	private readonly Color COLOR_BROWN_DARKEST = new Color(0.4705882f, 0.2039216f, 0.02745098f);

	// Token: 0x040002E3 RID: 739
	private readonly Color COLOR_BROWN_DARKER = new Color(0.757f, 0.459f, 0.263f);

	// Token: 0x040002E4 RID: 740
	private readonly Color COLOR_BROWN_DARK = new Color(0.906f, 0.627f, 0.447f);

	// Token: 0x040002E5 RID: 741
	private readonly Color COLOR_BROWN_LIGHT = new Color(1f, 0.796f, 0.659f);

	// Token: 0x040002E6 RID: 742
	private readonly Color COLOR_WIDGET_PRESSED_BUTTON = new Color(0.9f, 0.5f, 0.8f, 0.7f);

	// Token: 0x040002E7 RID: 743
	private readonly Color COLOR_WIDGET_BUTTON = new Color(1f, 1f, 1f, 0.7f);

	// Token: 0x040002E8 RID: 744
	private int _mainPanelButtonCount;

	// Token: 0x040002E9 RID: 745
	private int _temporaryPopupCount;

	// Token: 0x040002EA RID: 746
	private List<Transform> NotificationsList = new List<Transform>();
}
