using System;
using System.Collections.Generic;
using System.Linq;
using BlayFap;
using BlayFapShared;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000037 RID: 55
public static class BlayFapIntegration
{
	// Token: 0x0600016E RID: 366 RVA: 0x0000B194 File Offset: 0x00009394
	private static void OnBlayfapUniverseReady(bool ready)
	{
		if (ready)
		{
			BlayFapIntegration.ConsumeBlayFapInventory();
			BlayFapIntegration.CheckLTE();
			GameState.UniverseReady -= new ReactiveProperty<bool>.Changed(BlayFapIntegration.OnBlayfapUniverseReady);
		}
	}

	// Token: 0x0600016F RID: 367 RVA: 0x0000B1C4 File Offset: 0x000093C4
	public static void VerifyIsCurrentSession(Action onSuccess)
	{
		onSuccess();
	}

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x06000170 RID: 368 RVA: 0x0000B1CC File Offset: 0x000093CC
	public static bool IsTestDevice
	{
		get
		{
			return BlayFapClient.BlayFapId == 3628424457388782080UL || BlayFapClient.BlayFapId == 2779485632976684288UL || BlayFapClient.BlayFapId == 13723328943443131648UL || BlayFapClient.BlayFapId == 13005923926360978432UL || BlayFapClient.BlayFapId == 9164686991252832256UL || BlayFapClient.BlayFapId == 8179096824150971136UL || BlayFapClient.BlayFapId == 11807531974312207872UL || BlayFapClient.BlayFapId == 12706267107936137984UL || BlayFapClient.BlayFapId == 3196258312559433472UL || BlayFapClient.BlayFapId == 3196258312559433472UL || BlayFapClient.BlayFapId == 11868702860729133568UL || BlayFapClient.BlayFapId == 17492112877233917952UL || BlayFapClient.BlayFapId == 7560857218806881024UL || BlayFapClient.BlayFapId == 18044834532926732288UL;
		}
	}

	// Token: 0x06000171 RID: 369 RVA: 0x0000B2E4 File Offset: 0x000094E4
	public static void OnBlayFapLogin(LoginResponse loggedIn)
	{
		if (loggedIn == null)
		{
			return;
		}
		if (loggedIn.Error == null && loggedIn.BlayFapId != 0UL)
		{
			BlayFapClient.Instance.OnBlayFapLogin -= BlayFapIntegration.OnBlayFapLogin;
			if (GameState.CurrentState != null)
			{
				GameState.CurrentState.InitAchievements();
				GameState.CurrentState.ShowBlayFapID();
				Utilities.UpdateLoginLoadSaveButtons();
			}
			GetTitleDataRequest request = new GetTitleDataRequest
			{
				BlayFapId = BlayFapClient.BlayFapId,
				Keys = new List<string>(new string[]
				{
					"steamCatalog",
					"steamPromotion",
					"steamTracking"
				})
			};
			if (Store2.DiamondItems != null)
			{
				if (BlayFapIntegration.IsTestDevice)
				{
					GameState.CurrentState.transform.Find("Popups/Debug Init").GetComponent<DebugInitController>().InitDebug();
					Utilities.ForceCheckCachedServerTime();
				}
				GameState.UniverseReady += new ReactiveProperty<bool>.Changed(BlayFapIntegration.OnBlayfapUniverseReady);
				return;
			}
			BlayFapClient.Instance.GetTitleData(request, delegate(GetTitleDataResponse titleResponse)
			{
				if (titleResponse.Error == null && titleResponse.Data != null)
				{
					titleResponse.Data.TryGetValue("steamPromotion", out Playfab.Promotion);
				}
				titleResponse.Data.TryGetValue("steamTracking", out BlayFapIntegration.Tracking);
				if (string.IsNullOrEmpty(Playfab.JsonDataUrl))
				{
					Playfab.JsonDataUrl = "No Events";
				}
				if (Playfab.Promotion == null)
				{
					Playfab.Promotion = string.Empty;
				}
				if (string.IsNullOrEmpty(Playfab.JsonDataUrl))
				{
					Playfab.JsonDataUrl = "No Events";
				}
				if (BlayFapIntegration.IsTestDevice)
				{
					GameState.CurrentState.transform.Find("Popups/Debug Init").GetComponent<DebugInitController>().InitDebug();
					Utilities.ForceCheckCachedServerTime();
				}
				string[] array = Playfab.Promotion.Split(new char[]
				{
					','
				});
				Playfab.Promotion = string.Empty;
				for (int i = 0; i < array.Length; i++)
				{
					if (!string.IsNullOrEmpty(array[i]))
					{
						string text = array[i];
						if (text == "odango")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "charlotte")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "shibuki")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "sirina")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "vellatrix")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "catara")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "monsters1")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "monsters2")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "roxxy")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "tessa")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "claudia")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "rosa")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "juliet")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "ruri")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "wendy")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "generica")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "sawyer")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "suzu")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "lustat")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "fullvoices2")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "flingvote")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "flingnova")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "flinglake")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "darya")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "explora")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "esper")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "sirinapopup")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "renee")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "mallory")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "lake")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "xmasdates")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "junesale")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
						else if (text == "outfitspopup")
						{
							Playfab.Promotion = Playfab.Promotion + text + ".";
						}
					}
				}
				BlayFapIntegration.PromotionReady.Value = true;
				Playfab.Promotion = Playfab.Promotion.Trim(new char[]
				{
					' '
				});
				string catalogSaleId = null;
				string catalogId = "steam";
				catalogSaleId = ((titleResponse.Error == null && titleResponse.Data != null && titleResponse.Data.Count != 0 && titleResponse.Data.ContainsKey("steamCatalog")) ? titleResponse.Data["steamCatalog"] : null);
				if (Playfab.Promotion.Contains("junesale."))
				{
					catalogSaleId = "steam2";
				}
				GetFilteredCatalogRequest request2 = new GetFilteredCatalogRequest
				{
					BlayFapId = BlayFapClient.BlayFapId,
					Prefix = "ca.sadpanda.crushcrush." + catalogId
				};
				catalogId = "." + catalogId + ".";
				if (!string.IsNullOrEmpty(catalogSaleId))
				{
					catalogSaleId = "." + catalogSaleId + ".";
				}
				List<Store2.BlayfapItem> storeItems = new List<Store2.BlayfapItem>();
				List<Store2.BlayfapItem> bundleItems = new List<Store2.BlayfapItem>();
				BlayFapClient.Instance.GetFilteredCatalog(request2, delegate(GetCatalogResponse catalog)
				{
					bool flag = false;
					if (catalog.Error != null)
					{
						return;
					}
					List<Store2.BlayfapItem> list = new List<Store2.BlayfapItem>();
					List<Store2.BlayfapItem> list2 = new List<Store2.BlayfapItem>();
					Dictionary<string, Store2.BlayfapItem> dictionary = new Dictionary<string, Store2.BlayfapItem>();
					foreach (CatalogItem catalogItem in catalog.Catalog)
					{
						Store2.BlayfapItem blayfapItem = new Store2.BlayfapItem(catalogItem.ItemID, catalogItem.Price, catalogItem.Metadata);
						if (!string.IsNullOrEmpty(catalogSaleId) && catalogItem.ItemID.Contains(catalogSaleId) && catalogSaleId != catalogId)
						{
							flag = true;
							if (catalogItem.ItemID.Contains(".diamond"))
							{
								list2.Add(blayfapItem);
							}
							else if (dictionary.ContainsKey(blayfapItem.Id))
							{
								dictionary[blayfapItem.Id] = blayfapItem;
							}
							else
							{
								dictionary.Add(blayfapItem.Id, blayfapItem);
							}
						}
						else if (catalogItem.ItemID.Contains(catalogId) && catalogItem.ItemID.Contains(".diamond"))
						{
							list.Add(blayfapItem);
						}
						else if (catalogItem.ItemID.Contains(catalogId) && !dictionary.ContainsKey(blayfapItem.Id))
						{
							dictionary.Add(blayfapItem.Id, blayfapItem);
						}
					}
					storeItems = ((list2.Count != 6) ? list : list2);
					if (!string.IsNullOrEmpty(catalogSaleId))
					{
						foreach (KeyValuePair<string, Store2.BlayfapItem> keyValuePair in from u in dictionary
						where u.Key.Contains(catalogSaleId)
						select u)
						{
							bundleItems.Add(keyValuePair.Value);
						}
					}
					KeyValuePair<string, Store2.BlayfapItem> item;
					foreach (KeyValuePair<string, Store2.BlayfapItem> item2 in from u in dictionary
					where u.Key.Contains(catalogId)
					select u)
					{
						item = item2;
						Store2.BlayfapItem blayfapItem2 = bundleItems.SingleOrDefault((Store2.BlayfapItem u) => u.Id == item.Key.Replace(catalogId, catalogSaleId));
						if (blayfapItem2 == null)
						{
							bundleItems.Add(item.Value);
						}
						else
						{
							blayfapItem2.OriginalPrice = item.Value.Price;
						}
					}
					storeItems.Sort();
					Store2.DiamondItems = storeItems.ToArray();
					Store2.BundleItems = bundleItems.ToArray();
					if (GameState.CurrentState != null)
					{
						GameState.CurrentState.transform.Find("Store Revamp").GetComponent<Store2>().InitializePurchasing();
					}
					if (GameState.CurrentState != null)
					{
						GameState.CurrentState.transform.Find("Store Revamp").GetComponent<Store2>().UpdateUI();
						if (flag)
						{
						}
					}
				});
				BlayFapClient.Instance.MigrateUser(delegate(MigrateUserResponse response)
				{
					if (response.Error == null)
					{
						BlayFapClient.UsingBlayfap = true;
					}
					GameState.UniverseReady += new ReactiveProperty<bool>.Changed(BlayFapIntegration.OnBlayfapUniverseReady);
				});
			});
		}
		else
		{
			Playfab.AwardedItems = (Playfab.PlayfabItems)global::PlayerPrefs.GetLong("PlayfabAwardedItems", 0L);
			Playfab.InventoryObjects = (Requirement.OutfitType)global::PlayerPrefs.GetInt("PlayfabInventory", 0);
			if ((Playfab.AwardedItems & (Playfab.PlayfabItems.Darya | Playfab.PlayfabItems.JelleQuillzone | Playfab.PlayfabItems.BonchovySpectrum | Playfab.PlayfabItems.Charlotte | Playfab.PlayfabItems.Odango)) != (Playfab.PlayfabItems)0L)
			{
				GameState.GetGirlScreen().UnlockGirls();
			}
			if ((Playfab.AwardedItems & Playfab.PlayfabItems.NSFW) != (Playfab.PlayfabItems)0L)
			{
				GameState.NSFWAllowed = true;
			}
		}
	}

	// Token: 0x06000172 RID: 370 RVA: 0x0000B468 File Offset: 0x00009668
	public static void CheckLTE()
	{
		if (!BlayFapIntegration._requestedLte)
		{
			BlayFapIntegration._requestedLte = true;
			List<string> keys = new List<string>
			{
				"Completed2017Events",
				"Completed2018Events",
				"Completed2019Events",
				"Completed2020Events",
				"cc_lte"
			};
			GetUserDataRequest request = new GetUserDataRequest
			{
				BlayFapId = BlayFapClient.BlayFapId,
				Keys = keys
			};
			BlayFapClient.Instance.GetUserData(request, delegate(GetUserDataResponse dataResponse)
			{
				if (dataResponse != null && dataResponse.Error == null && dataResponse.Data != null)
				{
					bool flag = false;
					int c = 0;
					long c2 = 0L;
					long c3 = 0L;
					long c4 = 0L;
					foreach (KeyValuePair<string, string> keyValuePair in dataResponse.Data)
					{
						if (keyValuePair.Key == "Completed2017Events" && int.TryParse(keyValuePair.Value, out c))
						{
							flag = true;
						}
						if (keyValuePair.Key == "Completed2018Events" && long.TryParse(keyValuePair.Value, out c2))
						{
							flag = true;
						}
						if (keyValuePair.Key == "Completed2019Events" && long.TryParse(keyValuePair.Value, out c3))
						{
							flag = true;
						}
						if (keyValuePair.Key == "Completed2020Events" && long.TryParse(keyValuePair.Value, out c4))
						{
							flag = true;
						}
						if (dataResponse.Data.ContainsKey("cc_lte"))
						{
							BlayFapIntegration.CompletedEventsBlayfap = ((dataResponse.Data["cc_lte"] == null) ? string.Empty : dataResponse.Data["cc_lte"]);
						}
					}
					if (flag)
					{
						GameState.GetTaskSystem().ConvertSave(c, c2, c3, c4);
						Dictionary<string, string> dictionary = new Dictionary<string, string>();
						dictionary.Add("cc_lte", global::PlayerPrefs.GetString("CompletedEvents", string.Empty));
						SetUserDataRequest request2 = new SetUserDataRequest
						{
							KeysToRemove = new List<string>(new string[]
							{
								"Completed2017Events",
								"Completed2018Events",
								"Completed2019Events",
								"Completed2020Events"
							}),
							Data = dictionary
						};
						BlayFapClient.Instance.SetUserData(request2, delegate(SetUserDataResponse response)
						{
						});
					}
					GameState.GetTaskSystem().Init();
					GameState.GetTaskSystem().CompareAgainstServer();
				}
			});
		}
		else
		{
			GameState.GetTaskSystem().Init();
			GameState.GetTaskSystem().CompareAgainstServer();
		}
	}

	// Token: 0x06000173 RID: 371 RVA: 0x0000B520 File Offset: 0x00009720
	public static void LoadSaveFromUserData()
	{
		if (string.IsNullOrEmpty(BlayFapIntegration._queuedSave))
		{
			return;
		}
		if (!BlayFapClient.LoggedIn)
		{
			return;
		}
		SetUserDataRequest request = new SetUserDataRequest
		{
			KeysToRemove = new List<string>(new string[]
			{
				"cc_save"
			})
		};
		BlayFapClient.Instance.SetUserData(request, delegate(SetUserDataResponse response)
		{
			if (response.Error == null)
			{
				Dictionary<string, object> dictionary = global::PlayerPrefs.Import(BlayFapIntegration._queuedSave);
				int value = int.Parse((!dictionary.ContainsKey("GameStateDiamonds")) ? "0" : ((string)dictionary["GameStateDiamonds"]));
				GameState.UniverseReady.Value = false;
				GameState.Diamonds.Value = value;
				global::PlayerPrefs.Import(dictionary, true);
			}
			else
			{
				Notifications.AddNotification(Notifications.NotificationType.Message, "There was an error while loading your save game.  Please close the game and try again.");
			}
		});
	}

	// Token: 0x06000174 RID: 372 RVA: 0x0000B594 File Offset: 0x00009794
	public static void ClearPurchasesAndLte(Action<bool> callback)
	{
		if (!BlayFapClient.LoggedIn)
		{
			return;
		}
		if (string.IsNullOrEmpty(BlayFapIntegration.CompletedEventsBlayfap))
		{
			BlayFapIntegration.CompletedEventsBlayfap = string.Empty;
		}
		try
		{
			SetUserDataRequest request = new SetUserDataRequest
			{
				BlayFapId = BlayFapClient.BlayFapId,
				KeysToRemove = new List<string>(new string[]
				{
					"cc_lte",
					"Completed2017Events",
					"Completed2018Events",
					"Completed2019Events",
					"Completed2020Events"
				})
			};
			BlayFapClient.Instance.SetUserData(request, delegate(SetUserDataResponse response)
			{
				if (response.Error == null)
				{
					Playfab.AwardedItems = (Playfab.PlayfabItems)0L;
					TaskManager.CompletedEvents = new byte[0];
					BlayFapIntegration.CompletedEventsBlayfap = string.Empty;
					global::PlayerPrefs.DeleteKey("CompletedEvents", false);
					if (GameState.CurrentState != null)
					{
						GameState.GetTaskSystem().CompareAgainstServer();
						GameState.CurrentState.QueueSave();
					}
					if (callback != null)
					{
						callback(true);
					}
				}
				else if (callback != null)
				{
					callback(false);
				}
			});
		}
		catch (Exception)
		{
			if (callback != null)
			{
				callback(false);
			}
		}
	}

	// Token: 0x06000175 RID: 373 RVA: 0x0000B674 File Offset: 0x00009874
	public static void MarkLTEComplete(byte[] array)
	{
		if (!BlayFapClient.LoggedIn)
		{
			return;
		}
		if (string.IsNullOrEmpty(BlayFapIntegration.CompletedEventsBlayfap))
		{
			BlayFapIntegration.CompletedEventsBlayfap = string.Empty;
		}
		try
		{
			byte[] array2 = Convert.FromBase64String(BlayFapIntegration.CompletedEventsBlayfap);
			if (array2.Length < array.Length)
			{
				byte[] array3 = array2;
				array2 = new byte[array.Length];
				Array.Copy(array3, array2, array3.Length);
			}
			for (int i = 0; i < array.Length; i++)
			{
				byte[] array4 = array2;
				int num = i;
				array4[num] |= array[i];
			}
			string newCompletedEvents = Convert.ToBase64String(array2);
			if (!(BlayFapIntegration.CompletedEventsBlayfap == newCompletedEvents))
			{
				SetUserDataRequest request = new SetUserDataRequest
				{
					BlayFapId = BlayFapClient.BlayFapId,
					Data = new Dictionary<string, string>
					{
						{
							"cc_lte",
							newCompletedEvents
						}
					}
				};
				BlayFapClient.Instance.SetUserData(request, delegate(SetUserDataResponse response)
				{
					if (response.Error == null)
					{
						BlayFapIntegration.CompletedEventsBlayfap = newCompletedEvents;
					}
				});
			}
		}
		catch (Exception)
		{
		}
	}

	// Token: 0x06000176 RID: 374 RVA: 0x0000B79C File Offset: 0x0000999C
	public static void ConsumeBlayFapInventory()
	{
		BlayFapClient.Instance.GetUserInventory(delegate(GetUserInventoryResponse inventoryResult)
		{
			if (inventoryResult.Error == null)
			{
				Playfab.AwardedItems = (Playfab.PlayfabItems)0L;
				Playfab.InventoryObjects = Requirement.OutfitType.None;
				GameState.NSFWAllowed = false;
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
				if (inventoryResult.Items != null)
				{
					foreach (InventoryItem inventoryItem in inventoryResult.Items)
					{
						try
						{
							BlayFapIntegration.ConsumeBlayFapItem(inventoryItem);
						}
						catch (Exception)
						{
							Utilities.SendAnalytic(Utilities.AnalyticType.Exception, "Threw exception while processing " + inventoryItem.ItemId);
						}
					}
				}
				global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
				global::PlayerPrefs.SetInt("PlayfabInventory", (int)Playfab.InventoryObjects);
				if (!GameState.NSFWAllowed && GameState.NSFW)
				{
					GameState.NSFW = false;
					foreach (Girl girl in Girl.ActiveGirls)
					{
						if (girl.transform.Find("Unlocked").GetComponent<Button>().interactable)
						{
							girl.LoadAssets(false, false);
						}
					}
					if (Girls.CurrentGirl != null)
					{
						GameState.GetGirlScreen().SetGirl(Girls.CurrentGirl);
					}
				}
				BlayFapIntegration.BlayFapLoaded.Value = true;
			}
		});
	}

	// Token: 0x06000177 RID: 375 RVA: 0x0000B7C8 File Offset: 0x000099C8
	public static void AwardItems(Playfab.PlayfabItems items)
	{
		bool flag = false;
		for (int i = 0; i < 64; i++)
		{
			if ((items & (Playfab.PlayfabItems)(1L << i)) != (Playfab.PlayfabItems)0L)
			{
				flag |= BlayFapIntegration.AwardItem(items & (Playfab.PlayfabItems)(1L << i));
			}
		}
		if (flag)
		{
			GameState.GetGirlScreen().UnlockGirls();
		}
	}

	// Token: 0x06000178 RID: 376 RVA: 0x0000B81C File Offset: 0x00009A1C
	private static bool AwardItem(Playfab.PlayfabItems item)
	{
		Playfab.AwardedItems |= item;
		if (item == Playfab.PlayfabItems.Esper)
		{
			Girl girl = Girl.FindGirl(Balance.GirlName.Esper);
			if (girl != null)
			{
				girl.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
			}
			return true;
		}
		if (item == Playfab.PlayfabItems.Explora)
		{
			Girl girl2 = Girl.FindGirl(Balance.GirlName.Explora);
			if (girl2 != null)
			{
				girl2.LifetimeOutfits |= Requirement.OutfitType.All;
			}
			return true;
		}
		if (item == Playfab.PlayfabItems.Sawyer)
		{
			Girl girl3 = Girl.FindGirl(Balance.GirlName.Sawyer);
			if (girl3 != null)
			{
				girl3.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
			}
			return true;
		}
		if (item == Playfab.PlayfabItems.Lake)
		{
			Girl girl4 = Girl.FindGirl(Balance.GirlName.Lake);
			if (girl4 != null)
			{
				girl4.LifetimeOutfits |= (Requirement.OutfitType.Animated | Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.Unique | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
			}
			return true;
		}
		if (item == Playfab.PlayfabItems.Mallory)
		{
			Girl girl5 = Girl.FindGirl(Balance.GirlName.Mallory);
			if (girl5 != null)
			{
				girl5.LifetimeOutfits |= (Requirement.OutfitType.Animated | Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.Unique | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
			}
			return true;
		}
		if (item == Playfab.PlayfabItems.Renee)
		{
			Girl girl6 = Girl.FindGirl(Balance.GirlName.Renee);
			if (girl6 != null)
			{
				girl6.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
			}
			return true;
		}
		if (item != Playfab.PlayfabItems.Charlotte)
		{
			if (item != Playfab.PlayfabItems.Odango && item != Playfab.PlayfabItems.Shibuki && item != Playfab.PlayfabItems.Sirina && item != Playfab.PlayfabItems.Vellatrix)
			{
				if (item == Playfab.PlayfabItems.Roxxy)
				{
					Girl girl7 = Girl.FindGirl(Balance.GirlName.Roxxy);
					if (girl7 != null)
					{
						girl7.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
					}
					return true;
				}
				if (item == Playfab.PlayfabItems.Tessa)
				{
					Girl girl8 = Girl.FindGirl(Balance.GirlName.Tessa);
					if (girl8 != null)
					{
						girl8.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
					}
					return true;
				}
				if (item != Playfab.PlayfabItems.Catara)
				{
					if (item == Playfab.PlayfabItems.Claudia)
					{
						Girl girl9 = Girl.FindGirl(Balance.GirlName.Claudia);
						if (girl9 != null)
						{
							girl9.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
						}
						return true;
					}
					if (item == Playfab.PlayfabItems.Juliet)
					{
						Girl girl10 = Girl.FindGirl(Balance.GirlName.Juliet);
						if (girl10 != null)
						{
							girl10.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
						}
						return true;
					}
					if (item == Playfab.PlayfabItems.Rosa)
					{
						Girl girl11 = Girl.FindGirl(Balance.GirlName.Rosa);
						if (girl11 != null)
						{
							girl11.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
						}
						return true;
					}
					if (item == Playfab.PlayfabItems.Wendy)
					{
						Girl girl12 = Girl.FindGirl(Balance.GirlName.Wendy);
						if (girl12 != null)
						{
							girl12.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
						}
						return true;
					}
					if (item == Playfab.PlayfabItems.Ruri)
					{
						Girl girl13 = Girl.FindGirl(Balance.GirlName.Ruri);
						if (girl13 != null)
						{
							girl13.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
						}
						return true;
					}
					if (item == Playfab.PlayfabItems.Generica)
					{
						Girl girl14 = Girl.FindGirl(Balance.GirlName.Generica);
						if (girl14 != null)
						{
							girl14.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
						}
						return true;
					}
					if (item == Playfab.PlayfabItems.Suzu)
					{
						Job2[] componentsInChildren = GameState.CurrentState.transform.Find("Jobs").GetComponentsInChildren<Job2>();
						foreach (Job2 job in componentsInChildren)
						{
							if (job.JobType == Requirement.JobType.Planter)
							{
								job.CheckLock();
							}
						}
						return true;
					}
					if (item != Playfab.PlayfabItems.Lustat)
					{
						return false;
					}
					Girl girl15 = Girl.FindGirl(Balance.GirlName.Lustat);
					if (girl15 != null)
					{
						girl15.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
					}
					return true;
				}
			}
			return true;
		}
		Job2[] componentsInChildren2 = GameState.CurrentState.transform.Find("Jobs").GetComponentsInChildren<Job2>();
		foreach (Job2 job2 in componentsInChildren2)
		{
			if (job2.JobType == Requirement.JobType.Digger)
			{
				job2.CheckLock();
			}
		}
		return true;
	}

	// Token: 0x06000179 RID: 377 RVA: 0x0000BC9C File Offset: 0x00009E9C
	public static void ConsumeBlayFapItem(InventoryItem item)
	{
		if (item == null || string.IsNullOrEmpty(item.ItemId))
		{
			return;
		}
		string itemid = item.ItemId.Substring(item.ItemId.LastIndexOf('.') + 1);
		if (itemid == "refunda" || itemid == "refundb")
		{
			ConsumeUserItemRequest request = new ConsumeUserItemRequest
			{
				BlayFapId = BlayFapClient.BlayFapId,
				ItemId = item.ItemId,
				Quantity = item.Quantity
			};
			BlayFapClient.Instance.ConsumeUserItem(request, delegate(ConsumeUserItemResponse response)
			{
				if (response.Error == null)
				{
					if (itemid == "refunda")
					{
						Utilities.AwardDiamonds(item.Quantity);
					}
					else
					{
						BlayFapIntegration.EventTokenRefund += item.Quantity;
						if (GameState.CurrentState != null)
						{
							GameState.GetTaskSystem().RefundTokens();
						}
					}
				}
			});
		}
		else if (itemid.StartsWith("diamond"))
		{
			int diamonds = int.Parse(itemid.Substring(7)) * item.Quantity;
			Debug.Log("Found " + diamonds.ToString() + " diamonds in inventory");
			ConsumeUserItemRequest request2 = new ConsumeUserItemRequest
			{
				BlayFapId = BlayFapClient.BlayFapId,
				ItemId = item.ItemId,
				Quantity = item.Quantity
			};
			BlayFapClient.Instance.ConsumeUserItem(request2, delegate(ConsumeUserItemResponse response)
			{
				if (response.Error == null)
				{
					Utilities.AwardDiamonds(diamonds);
				}
			});
		}
		else
		{
			string itemid2 = itemid;
			switch (itemid2)
			{
			case "outfits_bikini":
			case "bikinis":
				Playfab.InventoryObjects |= Requirement.OutfitType.BathingSuit;
				break;
			case "outfits_wedding":
			case "rings":
				Playfab.InventoryObjects |= Requirement.OutfitType.DiamondRing;
				break;
			case "outfits_school":
			case "seifukus":
				Playfab.InventoryObjects |= Requirement.OutfitType.SchoolUniform;
				break;
			case "preregistration":
			case "lingeries":
				Playfab.InventoryObjects |= Requirement.OutfitType.Lingerie;
				break;
			case "nudies":
				Playfab.InventoryObjects |= Requirement.OutfitType.Nude;
				break;
			case "outfits_xmas":
			case "christmas":
				Playfab.InventoryObjects |= Requirement.OutfitType.Christmas;
				break;
			case "nsfw":
			case "pandapass":
				GameState.NSFWAllowed = true;
				Playfab.AwardedItems |= Playfab.PlayfabItems.NSFW;
				break;
			case "easter2017reward":
				Playfab.AwardedItems |= Playfab.PlayfabItems.Easter2017;
				global::PlayerPrefs.SetInt("EasterEggs2017", 1023);
				break;
			case "summer2017reward":
				Playfab.AwardedItems |= Playfab.PlayfabItems.Summer2017;
				global::PlayerPrefs.SetInt("SummerItems2017", 1048575);
				break;
			case "starterpack":
				Playfab.AwardedItems |= Playfab.PlayfabItems.StarterPack;
				if (Mathf.RoundToInt(global::PlayerPrefs.GetFloat("GameStatePurchasedMultiplier", 1f)) < 2)
				{
					FreeTime.PurchasedTime += 5;
					GameState.PurchasedMultiplier = 2;
					global::PlayerPrefs.SetFloat("GameStatePurchasedMultiplier", 2f);
					global::PlayerPrefs.SetInt("PurchasedTime", FreeTime.PurchasedTime);
					Utilities.AwardDiamonds(130);
				}
				break;
			case "july2017reward":
				Playfab.AwardedItems |= Playfab.PlayfabItems.July2017;
				global::PlayerPrefs.SetInt("JulyItems2017", 1048575);
				Girl.FindGirl(Balance.GirlName.Mio).LifetimeOutfits |= Requirement.OutfitType.Unique;
				break;
			case "backtoschool2017reward":
				Playfab.AwardedItems |= Playfab.PlayfabItems.BackToSchool2017;
				Girl.FindGirl(Balance.GirlName.Quill).LifetimeOutfits |= Requirement.OutfitType.Unique;
				break;
			case "ayano2017reward":
				Playfab.AwardedItems |= Playfab.PlayfabItems.Ayano2017;
				break;
			case "darya":
				Playfab.AwardedItems |= Playfab.PlayfabItems.Darya;
				GameState.GetGirlScreen().UnlockGirls();
				break;
			case "jellequillzone":
				Playfab.AwardedItems |= Playfab.PlayfabItems.JelleQuillzone;
				GameState.GetGirlScreen().UnlockGirls();
				break;
			case "bonchovyspectrum":
				Playfab.AwardedItems |= Playfab.PlayfabItems.BonchovySpectrum;
				GameState.GetGirlScreen().UnlockGirls();
				break;
			case "winter2018":
				Playfab.AwardedItems |= Playfab.PlayfabItems.Winter2018;
				break;
			case "nutaku2019":
				Playfab.AwardedItems |= Playfab.PlayfabItems.Nutaku2019;
				break;
			case "quillunique":
				Girl.FindGirl(Balance.GirlName.Quill).LifetimeOutfits |= Requirement.OutfitType.Unique;
				break;
			case "miounique":
				Girl.FindGirl(Balance.GirlName.Mio).LifetimeOutfits |= Requirement.OutfitType.Unique;
				break;
			case "ayanounique":
				Girl.FindGirl(Balance.GirlName.Ayano).LifetimeOutfits |= Requirement.OutfitType.Unique;
				break;
			case "ninaunique":
				Girl.FindGirl(Balance.GirlName.Nina).LifetimeOutfits |= Requirement.OutfitType.Unique;
				break;
			case "nutakuunique":
				Girl.FindGirl(Balance.GirlName.Nutaku).LifetimeOutfits |= Requirement.OutfitType.Unique;
				break;
			case "cassiedeluxe":
				Girl.FindGirl(Balance.GirlName.Cassie).LifetimeOutfits |= Requirement.OutfitType.DeluxeWedding;
				break;
			case "miodeluxe":
				Girl.FindGirl(Balance.GirlName.Mio).LifetimeOutfits |= Requirement.OutfitType.DeluxeWedding;
				break;
			case "quilldeluxe":
				Girl.FindGirl(Balance.GirlName.Quill).LifetimeOutfits |= Requirement.OutfitType.DeluxeWedding;
				break;
			case "charlotte":
			{
				Playfab.AwardedItems |= Playfab.PlayfabItems.Charlotte;
				GameState.GetGirlScreen().UnlockGirls();
				Job2[] componentsInChildren = GameState.CurrentState.transform.Find("Jobs").GetComponentsInChildren<Job2>();
				foreach (Job2 job in componentsInChildren)
				{
					if (job.JobType == Requirement.JobType.Digger)
					{
						job.CheckLock();
					}
				}
				break;
			}
			case "odango":
				Playfab.AwardedItems |= Playfab.PlayfabItems.Odango;
				GameState.GetGirlScreen().UnlockGirls();
				break;
			case "shibuki":
				Playfab.AwardedItems |= Playfab.PlayfabItems.Shibuki;
				GameState.GetGirlScreen().UnlockGirls();
				break;
			case "sirina":
				Playfab.AwardedItems |= Playfab.PlayfabItems.Sirina;
				GameState.GetGirlScreen().UnlockGirls();
				break;
			case "catara":
				Playfab.AwardedItems |= Playfab.PlayfabItems.Catara;
				GameState.GetGirlScreen().UnlockGirls();
				break;
			case "vellatrix":
				Playfab.AwardedItems |= Playfab.PlayfabItems.Vellatrix;
				GameState.GetGirlScreen().UnlockGirls();
				break;
			case "roxxy":
			{
				Playfab.AwardedItems |= Playfab.PlayfabItems.Roxxy;
				GameState.GetGirlScreen().UnlockGirls();
				Girl girl = Girl.FindGirl(Balance.GirlName.Roxxy);
				if (girl != null)
				{
					girl.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
				}
				break;
			}
			case "tessa":
			{
				Playfab.AwardedItems |= Playfab.PlayfabItems.Tessa;
				GameState.GetGirlScreen().UnlockGirls();
				Girl girl2 = Girl.FindGirl(Balance.GirlName.Tessa);
				if (girl2 != null)
				{
					girl2.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
				}
				break;
			}
			case "claudia":
			{
				Playfab.AwardedItems |= Playfab.PlayfabItems.Claudia;
				GameState.GetGirlScreen().UnlockGirls();
				Girl girl3 = Girl.FindGirl(Balance.GirlName.Claudia);
				if (girl3 != null)
				{
					girl3.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
				}
				break;
			}
			case "rosa":
			{
				Playfab.AwardedItems |= Playfab.PlayfabItems.Rosa;
				GameState.GetGirlScreen().UnlockGirls();
				Girl girl4 = Girl.FindGirl(Balance.GirlName.Rosa);
				if (girl4 != null)
				{
					girl4.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
				}
				break;
			}
			case "juliet":
			{
				Playfab.AwardedItems |= Playfab.PlayfabItems.Juliet;
				GameState.GetGirlScreen().UnlockGirls();
				Girl girl5 = Girl.FindGirl(Balance.GirlName.Juliet);
				if (girl5 != null)
				{
					girl5.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
				}
				break;
			}
			case "wendy":
			{
				Playfab.AwardedItems |= Playfab.PlayfabItems.Wendy;
				GameState.GetGirlScreen().UnlockGirls();
				Girl girl6 = Girl.FindGirl(Balance.GirlName.Wendy);
				if (girl6 != null)
				{
					girl6.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
				}
				break;
			}
			case "ruri":
			{
				Playfab.AwardedItems |= Playfab.PlayfabItems.Ruri;
				GameState.GetGirlScreen().UnlockGirls();
				Girl girl7 = Girl.FindGirl(Balance.GirlName.Ruri);
				if (girl7 != null)
				{
					girl7.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
				}
				break;
			}
			case "generica":
			{
				Playfab.AwardedItems |= Playfab.PlayfabItems.Generica;
				GameState.GetGirlScreen().UnlockGirls();
				Girl girl8 = Girl.FindGirl(Balance.GirlName.Generica);
				if (girl8 != null)
				{
					girl8.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
				}
				break;
			}
			case "suzu":
			{
				Playfab.AwardedItems |= Playfab.PlayfabItems.Suzu;
				GameState.GetGirlScreen().UnlockGirls();
				Job2[] componentsInChildren2 = GameState.CurrentState.transform.Find("Jobs").GetComponentsInChildren<Job2>();
				foreach (Job2 job2 in componentsInChildren2)
				{
					if (job2.JobType == Requirement.JobType.Planter)
					{
						job2.CheckLock();
					}
				}
				break;
			}
			case "fullvoices":
				Playfab.AwardedItems |= Playfab.PlayfabItems.FullVoices;
				break;
			case "lustat":
			{
				Playfab.AwardedItems |= Playfab.PlayfabItems.Lustat;
				GameState.GetGirlScreen().UnlockGirls();
				Girl girl9 = Girl.FindGirl(Balance.GirlName.Lustat);
				if (girl9 != null)
				{
					girl9.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
				}
				break;
			}
			case "winter2020":
				Playfab.AwardedItems |= Playfab.PlayfabItems.Winter2020;
				break;
			case "anniversary2021":
				Playfab.AwardedItems |= (Playfab.PlayfabItems)((ulong)int.MinValue);
				break;
			case "anniversary2022":
				Playfab.AwardedItems |= Playfab.PlayfabItems.Anniversary2022;
				break;
			case "sawyer":
			{
				Playfab.AwardedItems |= Playfab.PlayfabItems.Sawyer;
				GameState.GetGirlScreen().UnlockGirls();
				Girl girl10 = Girl.FindGirl(Balance.GirlName.Sawyer);
				if (girl10 != null)
				{
					girl10.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
				}
				break;
			}
			case "renee":
			{
				Playfab.AwardedItems |= Playfab.PlayfabItems.Renee;
				GameState.GetGirlScreen().UnlockGirls();
				Girl girl11 = Girl.FindGirl(Balance.GirlName.Renee);
				if (girl11 != null)
				{
					girl11.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
				}
				break;
			}
			case "explora":
			{
				Playfab.AwardedItems |= Playfab.PlayfabItems.Explora;
				GameState.GetGirlScreen().UnlockGirls();
				Girl girl12 = Girl.FindGirl(Balance.GirlName.Explora);
				if (girl12 != null)
				{
					girl12.LifetimeOutfits |= Requirement.OutfitType.All;
				}
				break;
			}
			case "esper":
			{
				Playfab.AwardedItems |= Playfab.PlayfabItems.Esper;
				GameState.GetGirlScreen().UnlockGirls();
				Girl girl13 = Girl.FindGirl(Balance.GirlName.Esper);
				if (girl13 != null)
				{
					girl13.LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
				}
				break;
			}
			case "mallory":
			{
				Playfab.AwardedItems |= Playfab.PlayfabItems.Mallory;
				GameState.GetGirlScreen().UnlockGirls();
				Girl girl14 = Girl.FindGirl(Balance.GirlName.Mallory);
				if (girl14 != null)
				{
					girl14.LifetimeOutfits |= (Requirement.OutfitType.Animated | Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.Unique | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
				}
				break;
			}
			case "lake":
			{
				Playfab.AwardedItems |= Playfab.PlayfabItems.Lake;
				GameState.GetGirlScreen().UnlockGirls();
				Girl girl15 = Girl.FindGirl(Balance.GirlName.Lake);
				if (girl15 != null)
				{
					girl15.LifetimeOutfits |= (Requirement.OutfitType.Animated | Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.Unique | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
				}
				break;
			}
			case "mio_plushie":
				Playfab.AwardedItems |= Playfab.PlayfabItems.MioPlush;
				break;
			case "quill_plushie":
				Playfab.AwardedItems |= Playfab.PlayfabItems.QuillPlush;
				break;
			}
		}
	}

	// Token: 0x040001C7 RID: 455
	public static ReactiveProperty<bool> BlayFapLoaded = new ReactiveProperty<bool>(false);

	// Token: 0x040001C8 RID: 456
	public static ReactiveProperty<bool> PromotionReady = new ReactiveProperty<bool>(false);

	// Token: 0x040001C9 RID: 457
	public static string LastError = string.Empty;

	// Token: 0x040001CA RID: 458
	public static string Tracking = string.Empty;

	// Token: 0x040001CB RID: 459
	private static string _queuedSave = string.Empty;

	// Token: 0x040001CC RID: 460
	public static string CompletedEventsBlayfap;

	// Token: 0x040001CD RID: 461
	private static bool _requestedLte = false;

	// Token: 0x040001CE RID: 462
	public static int EventTokenRefund = 0;
}
