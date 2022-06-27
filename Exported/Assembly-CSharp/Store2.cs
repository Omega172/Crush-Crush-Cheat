using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using BlayFap;
using BlayFapShared;
using SadPanda;
using SadPanda.Platforms;
using Steamworks;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;

// Token: 0x02000127 RID: 295
public class Store2 : MonoBehaviour, IStoreListener
{
	// Token: 0x0600075E RID: 1886 RVA: 0x0003F074 File Offset: 0x0003D274
	private bool IsInitialized()
	{
		return Store2.m_StoreController != null && Store2.m_StoreExtensionProvider != null;
	}

	// Token: 0x0600075F RID: 1887 RVA: 0x0003F090 File Offset: 0x0003D290
	public void RealMoneyPurchase(string productId)
	{
		if (!this.IsInitialized())
		{
			Debug.LogError("IAP is not initialized.");
			return;
		}
		this.ProcessRealMoneyPurchase(productId);
	}

	// Token: 0x06000760 RID: 1888 RVA: 0x0003F0B0 File Offset: 0x0003D2B0
	private void ProcessRealMoneyPurchase(string productId)
	{
		Product product = Store2.m_StoreController.products.WithID(productId);
		try
		{
			if (product != null && product.availableToPurchase)
			{
				Debug.Log(string.Format("Purchasing product: [{0}]", product.definition.id));
				Store2.m_StoreController.InitiatePurchase(product);
			}
			else
			{
				Debug.LogError(string.Format("Unable to find product [{0}] in the IAP catalog.", productId));
			}
		}
		catch (Exception ex)
		{
			Utilities.SendAnalytic(Utilities.AnalyticType.Exception, "Store2.RealMoneyPurchase " + ex.Message);
		}
	}

	// Token: 0x06000761 RID: 1889 RVA: 0x0003F158 File Offset: 0x0003D358
	public void RestorePurchases()
	{
		if (!this.IsInitialized())
		{
			Debug.Log("RestorePurchases FAIL. Not initialized.");
			return;
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
		{
			Debug.Log("RestorePurchases started ...");
			IAppleExtensions extension = Store2.m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
			extension.RestoreTransactions(delegate(bool result)
			{
				Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
			});
		}
		else
		{
			Debug.Log(string.Format("RestorePurchases FAIL. Not supported on platform [{0}]", Application.platform));
		}
	}

	// Token: 0x06000762 RID: 1890 RVA: 0x0003F1E8 File Offset: 0x0003D3E8
	public void InitializePurchasing()
	{
		if (this.IsInitialized())
		{
			return;
		}
		if (Store2.DiamondItems == null)
		{
			return;
		}
		SadPandaPurchasingModule sadPandaPurchasingModule = SadPandaPurchasingModule.Instance();
		ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(sadPandaPurchasingModule, new IPurchasingModule[0]);
		string appStore = sadPandaPurchasingModule.appStore;
		Debug.LogFormat("UnityIAP: AppStore appears to be [{0}]", new object[]
		{
			appStore
		});
		foreach (Store2.BlayfapItem blayfapItem in Store2.DiamondItems)
		{
			configurationBuilder.AddProduct(blayfapItem.Id, ProductType.Consumable, new IDs
			{
				{
					blayfapItem.Id,
					new string[]
					{
						appStore
					}
				},
				{
					blayfapItem.Id,
					new object[]
					{
						AppStore.fake
					}
				}
			});
		}
		if (Store2.BundleItems != null)
		{
			foreach (Store2.BlayfapItem blayfapItem2 in Store2.BundleItems)
			{
				if (blayfapItem2.Id.Contains("starter") || blayfapItem2.Id.Contains("darya") || blayfapItem2.Id.Contains("charlotte") || blayfapItem2.Id.Contains("catara") || blayfapItem2.Id.Contains("suzu") || blayfapItem2.Id.Contains("explora") || blayfapItem2.Id.Contains("mallory"))
				{
					configurationBuilder.AddProduct(blayfapItem2.Id, ProductType.Consumable, new IDs
					{
						{
							blayfapItem2.Id,
							new string[]
							{
								appStore
							}
						},
						{
							blayfapItem2.Id,
							new object[]
							{
								AppStore.fake
							}
						}
					});
				}
			}
		}
		UnityPurchasing.Initialize(this, configurationBuilder);
	}

	// Token: 0x06000763 RID: 1891 RVA: 0x0003F3CC File Offset: 0x0003D5CC
	private void InitTabs()
	{
		bool flag = true;
		if (this.boostsTab == null)
		{
			this.boostsTab = base.transform.Find((!flag) ? "Content/Special Content/Speed Boosts" : "Boosts Tab");
			this.timeBlocksTab = base.transform.Find((!flag) ? "Content/Special Content/Time Blocks" : "Time Blocks Tab");
			this.diamondsTab = base.transform.Find((!flag) ? "Content/Currency Content/Diamonds/Content/Diamonds Tab" : "Diamonds Tab");
			this.skipResetTab = base.transform.Find((!flag) ? "Content/Special Content/Skip Reset" : "Skip Reset Tab");
			this.bundlesTab = base.transform.Find((!flag) ? "Content/Bundles Content/Regular Bundles Scroll View" : "Bundles Tab");
			this.outfitsTab = base.transform.Find((!flag) ? "Content/No Connection" : "Outfits Tab");
			this.noConnection = base.transform.Find((!flag) ? "Content/No Connection" : "No Connection");
		}
	}

	// Token: 0x06000764 RID: 1892 RVA: 0x0003F4F8 File Offset: 0x0003D6F8
	private void OnEnable()
	{
		this.InitTabs();
		this.DisableBundles();
	}

	// Token: 0x06000765 RID: 1893 RVA: 0x0003F508 File Offset: 0x0003D708
	private void Start()
	{
		Transform transform = base.transform.Find("Navigation");
		for (int i = 0; i < transform.childCount; i++)
		{
			int tabId = i;
			transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate()
			{
				this.SelectTab(tabId);
			});
		}
		this.InitializePurchasing();
		this.UpdateUI();
		this.SelectTab(this.SelectTabOnLoad);
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x0003F58C File Offset: 0x0003D78C
	private void Update()
	{
		if (this.disabledTime > 0f)
		{
			this.disabledTime -= Time.deltaTime;
			if (this.disabledTime <= 0f && this.diamonds != null)
			{
				foreach (Button button in this.diamonds)
				{
					button.interactable = true;
				}
				this.DisableBundles();
			}
		}
	}

	// Token: 0x06000767 RID: 1895 RVA: 0x0003F604 File Offset: 0x0003D804
	public void DisableBundles()
	{
		if (this.disabledTime > 0f)
		{
			return;
		}
		if (this.darya == null)
		{
			return;
		}
		if (this._girlButtonPairs == null)
		{
			this._girlButtonPairs = new List<KeyValuePair<Balance.GirlName, Button>>
			{
				new KeyValuePair<Balance.GirlName, Button>(Balance.GirlName.Darya, this.darya),
				new KeyValuePair<Balance.GirlName, Button>(Balance.GirlName.Charlotte, this.charlotte),
				new KeyValuePair<Balance.GirlName, Button>(Balance.GirlName.Odango, this.odango),
				new KeyValuePair<Balance.GirlName, Button>(Balance.GirlName.Shibuki, this.shibuki),
				new KeyValuePair<Balance.GirlName, Button>(Balance.GirlName.Sirina, this.sirina),
				new KeyValuePair<Balance.GirlName, Button>(Balance.GirlName.Catara, this.catara),
				new KeyValuePair<Balance.GirlName, Button>(Balance.GirlName.Roxxy, this.roxxy),
				new KeyValuePair<Balance.GirlName, Button>(Balance.GirlName.Tessa, this.tessa),
				new KeyValuePair<Balance.GirlName, Button>(Balance.GirlName.Claudia, this.claudia),
				new KeyValuePair<Balance.GirlName, Button>(Balance.GirlName.Rosa, this.rosa),
				new KeyValuePair<Balance.GirlName, Button>(Balance.GirlName.Juliet, this.juliet),
				new KeyValuePair<Balance.GirlName, Button>(Balance.GirlName.Wendy, this.wendy),
				new KeyValuePair<Balance.GirlName, Button>(Balance.GirlName.Ruri, this.ruri),
				new KeyValuePair<Balance.GirlName, Button>(Balance.GirlName.Suzu, this.suzu),
				new KeyValuePair<Balance.GirlName, Button>(Balance.GirlName.Vellatrix, this.vellatrix),
				new KeyValuePair<Balance.GirlName, Button>(Balance.GirlName.Generica, this.generica),
				new KeyValuePair<Balance.GirlName, Button>(Balance.GirlName.Lustat, this.lustat),
				new KeyValuePair<Balance.GirlName, Button>(Balance.GirlName.Sawyer, this.sawyer),
				new KeyValuePair<Balance.GirlName, Button>(Balance.GirlName.Explora, this.explora),
				new KeyValuePair<Balance.GirlName, Button>(Balance.GirlName.Esper, this.esper),
				new KeyValuePair<Balance.GirlName, Button>(Balance.GirlName.Renee, this.renee),
				new KeyValuePair<Balance.GirlName, Button>(Balance.GirlName.Mallory, this.mallory),
				new KeyValuePair<Balance.GirlName, Button>(Balance.GirlName.Lake, this.lake)
			};
		}
		foreach (KeyValuePair<Balance.GirlName, Button> keyValuePair in this._girlButtonPairs)
		{
			this.UnlockGirlButton(keyValuePair.Key, keyValuePair.Value);
		}
		if (this.monsters1 != null && this.monsterItem1 != null && this.monsterItem1.Price > 0U)
		{
			Girl girl = Girl.FindGirl(Balance.GirlName.Elle);
			Girl girl2 = Girl.FindGirl(Balance.GirlName.Quill);
			if (girl == null || girl2 == null)
			{
				this.monsters1.transform.parent.gameObject.SetActive(false);
			}
			else
			{
				this.monsters1.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("monsters1."));
				bool flag = (girl.LifetimeOutfits & Requirement.OutfitType.Monster) != Requirement.OutfitType.None;
				flag &= ((girl2.LifetimeOutfits & Requirement.OutfitType.Monster) != Requirement.OutfitType.None);
				this.monsters1.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.JelleQuillzone) == (Playfab.PlayfabItems)0L && BlayFapClient.LoggedIn && !flag);
				this.monsters1.transform.Find("Icon").GetComponent<Image>().color = ((!this.monsters1.interactable) ? new Color(1f, 1f, 1f, 0.5f) : Color.white);
			}
		}
		if (this.monsters2 != null && this.monsterItem2 != null && this.monsterItem2.Price > 0U)
		{
			Girl girl3 = Girl.FindGirl(Balance.GirlName.Iro);
			Girl girl4 = Girl.FindGirl(Balance.GirlName.Bonnibel);
			if (girl3 == null || girl4 == null)
			{
				this.monsters2.transform.parent.gameObject.SetActive(false);
			}
			else
			{
				this.monsters2.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("monsters2."));
				bool flag2 = (girl3.LifetimeOutfits & Requirement.OutfitType.Monster) != Requirement.OutfitType.None;
				flag2 &= ((girl4.LifetimeOutfits & Requirement.OutfitType.Monster) != Requirement.OutfitType.None);
				this.monsters2.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.BonchovySpectrum) == (Playfab.PlayfabItems)0L && BlayFapClient.LoggedIn && !flag2);
				this.monsters2.transform.Find("Icon").GetComponent<Image>().color = ((!this.monsters2.interactable) ? new Color(1f, 1f, 1f, 0.5f) : Color.white);
			}
		}
		if (this.timelord != null)
		{
			this.timelord.interactable = true;
		}
		this.HandleOutfits();
	}

	// Token: 0x06000768 RID: 1896 RVA: 0x0003FB28 File Offset: 0x0003DD28
	private void UnlockGirlButton(Balance.GirlName girlName, Button girlBtn)
	{
		if (girlBtn == null)
		{
			return;
		}
		bool flag = GameState.GetGirlScreen().IsUnlocked(girlName);
		bool flag2 = !(Girl.FindGirl(girlName) == null) && (Girl.FindGirl(girlName).LifetimeOutfits & Requirement.OutfitType.BathingSuit) != Requirement.OutfitType.None;
		if (girlName == Balance.GirlName.Ruri)
		{
			flag |= ((Playfab.AwardedItems & Playfab.PlayfabItems.Ruri) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(116));
		}
		else if (girlName == Balance.GirlName.Wendy)
		{
			flag |= ((Playfab.AwardedItems & Playfab.PlayfabItems.Wendy) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(113));
		}
		else if (girlName == Balance.GirlName.Generica)
		{
			flag |= ((Playfab.AwardedItems & Playfab.PlayfabItems.Generica) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(119));
		}
		else if (girlName == Balance.GirlName.Sawyer)
		{
			flag |= ((Playfab.AwardedItems & Playfab.PlayfabItems.Sawyer) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(140));
		}
		else if (girlName == Balance.GirlName.Esper)
		{
			flag |= ((Playfab.AwardedItems & Playfab.PlayfabItems.Esper) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(146));
		}
		else if (girlName == Balance.GirlName.Renee)
		{
			flag |= ((Playfab.AwardedItems & Playfab.PlayfabItems.Renee) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(161));
		}
		else if (girlName == Balance.GirlName.Lake)
		{
			flag |= ((Playfab.AwardedItems & Playfab.PlayfabItems.Lake) != (Playfab.PlayfabItems)0L || TaskManager.IsCompletedEventSet(179));
		}
		girlBtn.interactable = (!flag || !flag2);
		if (girlBtn.transform.Find("Icon") != null)
		{
			girlBtn.transform.Find("Icon").GetComponent<Image>().color = ((!girlBtn.interactable) ? new Color(1f, 1f, 1f, 0.5f) : Color.white);
		}
	}

	// Token: 0x06000769 RID: 1897 RVA: 0x0003FD34 File Offset: 0x0003DF34
	private string PriceToString(Store2.BlayfapItem item)
	{
		if (!string.IsNullOrEmpty(item.LocalizedPrice))
		{
			return item.LocalizedPrice;
		}
		if (item.Price % 100U == 0U)
		{
			return string.Format("${0}", (item.Price / 100f).ToString("n0"));
		}
		return string.Format("${0}.{1}", ((item.Price - item.Price % 100U) / 100f).ToString("n0"), (item.Price % 100U).ToString("D2"));
	}

	// Token: 0x0600076A RID: 1898 RVA: 0x0003FDD4 File Offset: 0x0003DFD4
	public void UpdateUI()
	{
		bool flag = true;
		if (this.boostsTab == null)
		{
			return;
		}
		Transform transform = base.transform.Find((!flag) ? "Content/Special Content/Speed Boosts/Parent/Layout/Selection" : "Boosts Tab/Speed Boosts/Selection");
		this.speedBoosts = new Button[transform.childCount];
		for (int i = 0; i < this.speedBoosts.Length; i++)
		{
			Store2.PurchaseType purchaseType = Store2.PurchaseType.SpeedBoost2 + i;
			int purchaseCost = this.speedBoostCosts[i];
			this.speedBoosts[i] = transform.GetChild(i).GetComponent<Button>();
			this.speedBoosts[i].transform.Find("Item").GetComponent<Text>().text = string.Format("x{0}", this.speedBoostAmounts[i].ToString());
			this.speedBoosts[i].transform.Find("Item").GetComponent<Text>().color = this.BoostColor;
			this.speedBoosts[i].transform.Find("Button/Price").GetComponent<Text>().text = this.speedBoostCosts[i].ToString();
			this.speedBoosts[i].onClick.RemoveAllListeners();
			this.speedBoosts[i].onClick.AddListener(delegate()
			{
				this.PurchaseItem(purchaseType, purchaseCost);
			});
		}
		Transform transform2 = base.transform.Find("Boosts Tab/Time Skips/Selection");
		transform2 = base.transform.Find((!flag) ? "Content/Special Content/Time Skips/Parent/Layout/Selection" : "Boosts Tab/Time Skips/Selection");
		this.timeSkips = new Button[transform2.childCount];
		for (int j = 0; j < this.timeSkips.Length; j++)
		{
			Store2.PurchaseType purchaseType = Store2.PurchaseType.JumpFourHours + j;
			int purchaseCost = this.timeSkipCosts[j];
			int num = this.timeSkipAmounts[j];
			this.timeSkips[j] = transform2.GetChild(j).GetComponent<Button>();
			this.timeSkips[j].transform.Find("Item").GetComponent<Text>().color = this.TimeColor;
			this.timeSkips[j].transform.Find("Button/Price").GetComponent<Text>().text = this.timeSkipCosts[j].ToString();
			this.timeSkips[j].onClick.RemoveAllListeners();
			this.timeSkips[j].onClick.AddListener(delegate()
			{
				this.PurchaseItem(purchaseType, purchaseCost);
			});
			this.timeSkips[j].transform.Find("Unit").GetComponent<Text>().text = ((num <= 24) ? ((num != 24) ? Translations.GetTranslation("everything_else_146_1", "Hours") : Translations.GetTranslation("everything_else_146_2", "Day")) : Translations.GetTranslation("everything_else_146_3", "Days"));
			this.timeSkips[j].transform.Find("Unit").GetComponent<Text>().color = this.TimeColor;
			this.timeSkips[j].transform.Find("Unit").gameObject.SetActive(true);
			if (num >= 24)
			{
				num /= 24;
			}
			this.timeSkips[j].transform.Find("Item").GetComponent<Text>().text = num.ToString();
			this.timeSkips[j].transform.Find("Item").GetComponent<Text>().transform.localPosition = new Vector3(1f, 40f, 0f);
		}
		this.timeBlocks = new Button[this.timeBlocksTab.childCount];
		for (int k = 0; k < this.timeBlocks.Length; k++)
		{
			Store2.PurchaseType purchaseType = Store2.PurchaseType.Timeblock1 + k;
			int purchaseCost = this.timeBlockCosts[k];
			this.timeBlocks[k] = this.timeBlocksTab.GetChild(k).GetComponent<Button>();
			string format = (this.timeBlockAmounts[k] != 1) ? Translations.GetTranslation("everything_else_33_3", "{0} Time Blocks") : Translations.GetTranslation("everything_else_33_1", "1 Time Block");
			this.timeBlocks[k].transform.Find("Background/Title").GetComponent<Text>().text = string.Format(format, this.timeBlockAmounts[k].ToString());
			format = ((this.timeBlockAmounts[k] != 1) ? Translations.GetTranslation("everything_else_33_4", "Adds {0} Time Blocks to total capacity.") : Translations.GetTranslation("everything_else_33_2", "Adds 1 Time Block to total capacity."));
			this.timeBlocks[k].transform.Find("Background/Description").GetComponent<Text>().text = string.Format(format, this.timeBlockAmounts[k].ToString());
			this.timeBlocks[k].transform.Find("Background/Button/Price").GetComponent<Text>().text = this.timeBlockCosts[k].ToString();
			this.timeBlocks[k].onClick.RemoveAllListeners();
			this.timeBlocks[k].onClick.AddListener(delegate()
			{
				this.PurchaseItem(purchaseType, purchaseCost);
			});
		}
		this.skipResetTab.Find("Skip Reset/Purchase/Button/Price").GetComponent<Text>().text = Store2.SkipResetCost.ToString();
		this.skipResetTab.Find("Skip Reset/Purchase").GetComponent<Button>().onClick.RemoveAllListeners();
		this.skipResetTab.Find("Skip Reset/Purchase").GetComponent<Button>().onClick.AddListener(delegate()
		{
			this.PurchaseItem(Store2.PurchaseType.SkipReset, Store2.SkipResetCost);
		});
		this.diamonds = new Button[6];
		this.diamonds[0] = this.diamondsTab.Find("First Panel 1/Small Diamond Purchase 1").GetComponent<Button>();
		this.diamonds[1] = this.diamondsTab.Find("First Panel 1/Small Diamond Purchase 2").GetComponent<Button>();
		this.diamonds[2] = this.diamondsTab.Find("Large Diamond Purchase 1").GetComponent<Button>();
		this.diamonds[3] = this.diamondsTab.Find("First Panel 2/Small Diamond Purchase 1").GetComponent<Button>();
		this.diamonds[4] = this.diamondsTab.Find("First Panel 2/Small Diamond Purchase 2").GetComponent<Button>();
		this.diamonds[5] = this.diamondsTab.Find("Large Diamond Purchase 2").GetComponent<Button>();
		this.darya = this.bundlesTab.Find("Scroll View/Content/Darya Bundle/Button").GetComponent<Button>();
		this.charlotte = this.bundlesTab.Find("Scroll View/Content/Charlotte Bundle/Button").GetComponent<Button>();
		this.catara = this.bundlesTab.Find("Scroll View/Content/Catara Bundle/Button").GetComponent<Button>();
		this.odango = this.bundlesTab.Find("Scroll View/Content/Odango Bundle/Button").GetComponent<Button>();
		this.shibuki = this.bundlesTab.Find("Scroll View/Content/Shibuki Bundle/Button").GetComponent<Button>();
		this.sirina = this.bundlesTab.Find("Scroll View/Content/Sirina Bundle/Button").GetComponent<Button>();
		this.vellatrix = this.bundlesTab.Find("Scroll View/Content/Vellatrix Bundle/Button").GetComponent<Button>();
		this.roxxy = this.bundlesTab.Find("Scroll View/Content/Roxxy Bundle/Button").GetComponent<Button>();
		this.tessa = this.bundlesTab.Find("Scroll View/Content/Tessa Bundle/Button").GetComponent<Button>();
		this.claudia = this.bundlesTab.Find("Scroll View/Content/Claudia Bundle/Button").GetComponent<Button>();
		this.rosa = this.bundlesTab.Find("Scroll View/Content/Rosa Bundle/Button").GetComponent<Button>();
		this.juliet = this.bundlesTab.Find("Scroll View/Content/Juliet Bundle/Button").GetComponent<Button>();
		this.wendy = this.bundlesTab.Find("Scroll View/Content/Wendy Bundle/Button").GetComponent<Button>();
		this.ruri = this.bundlesTab.Find("Scroll View/Content/Ruri Bundle/Button").GetComponent<Button>();
		this.generica = this.bundlesTab.Find("Scroll View/Content/Generica Bundle/Button").GetComponent<Button>();
		this.bundle1 = this.bundlesTab.Find("Scroll View/Content/Time Lord Bundle/Button").GetComponent<Button>();
		this.monsters1 = this.bundlesTab.Find("Scroll View/Content/Jelle and Quillzone/Button").GetComponent<Button>();
		this.monsters2 = this.bundlesTab.Find("Scroll View/Content/Bonchovy and Spectrum/Button").GetComponent<Button>();
		this.suzu = this.bundlesTab.Find("Scroll View/Content/Suzu Bundle/Button").GetComponent<Button>();
		this.lustat = this.bundlesTab.Find("Scroll View/Content/Lustat Bundle/Button").GetComponent<Button>();
		this.lake = this.bundlesTab.Find("Scroll View/Content/Lake Bundle/Button").GetComponent<Button>();
		this.sawyer = this.bundlesTab.Find("Scroll View/Content/Sawyer Bundle/Button").GetComponent<Button>();
		this.explora = this.bundlesTab.Find("Scroll View/Content/Explora Bundle/Button").GetComponent<Button>();
		this.timelord = this.bundlesTab.Find("Scroll View/Content/Time Lord Bundle/Button").GetComponent<Button>();
		this.starter = this.bundlesTab.Find("Scroll View/Content/Starter Pack/Button").GetComponent<Button>();
		this.esper = this.bundlesTab.Find("Scroll View/Content/Esper Bundle/Button").GetComponent<Button>();
		this.renee = this.bundlesTab.Find("Scroll View/Content/Renee Bundle/Button").GetComponent<Button>();
		this.mallory = this.bundlesTab.Find("Scroll View/Content/Mallory Bundle/Button").GetComponent<Button>();
		this.weddingButton = this.outfitsTab.Find("Scroll View/Content/Wedding Bundle/Button").GetComponent<Button>();
		this.bikiniButton = this.outfitsTab.Find("Scroll View/Content/Bikini Bundle/Button").GetComponent<Button>();
		this.schoolButton = this.outfitsTab.Find("Scroll View/Content/School Uniform Bundle/Button").GetComponent<Button>();
		this.xmasButton = this.outfitsTab.Find("Scroll View/Content/Xmas Outfit Bundle/Button").GetComponent<Button>();
		this.HandleInfoButtons();
		this.HandleMonsterInfoButtons();
		if (Store2.DiamondItems != null && Store2.DiamondItems[0] != null)
		{
			float num2 = (float)Store2.DiamondItems[0].Diamonds / Store2.DiamondItems[0].Price;
			bool flag2 = false;
			for (int l = 0; l < this.diamonds.Length; l++)
			{
				Store2.BlayfapItem storeItem = Store2.DiamondItems[l];
				int num3 = Mathf.RoundToInt(100f * ((float)Store2.DiamondItems[l].Diamonds / Store2.DiamondItems[l].Price) / num2 - 100f);
				this.diamonds[l].transform.Find("Image").GetComponent<Image>().sprite = this.DiamondIcons[l];
				this.diamonds[l].transform.Find("Title").GetComponent<Text>().text = storeItem.Diamonds.ToString();
				if (storeItem.Diamonds >= 1000 && l != 2 && l != 5)
				{
					this.diamonds[l].transform.Find("Title").GetComponent<Text>().fontSize = 40;
				}
				if (storeItem.Diamonds != this.defaultDiamonds[l] || (ulong)storeItem.Price != (ulong)((long)this.defaultPrices[l]))
				{
					flag2 = true;
					this.diamonds[l].transform.Find("Promo").gameObject.SetActive(storeItem.Diamonds != this.defaultDiamonds[l]);
					if (storeItem.Diamonds != this.defaultDiamonds[l])
					{
						this.diamonds[l].transform.Find("Promo").GetComponent<Text>().text = this.defaultDiamonds[l].ToString();
					}
				}
				this.diamonds[l].transform.Find("Button/Price").GetComponent<Text>().text = this.PriceToString(storeItem);
				if (Store2.DiamondItems[l].Diamonds == this.defaultDiamonds[l] && (ulong)Store2.DiamondItems[l].Price == (ulong)((long)this.defaultPrices[l]))
				{
					this.diamonds[l].transform.Find("Discount").gameObject.SetActive(l != 0);
					this.diamonds[l].transform.Find("Discount").GetComponent<Text>().text = string.Format("{0}" + Translations.GetTranslation("everything_else_40_2", "% Bonus"), num3.ToString());
				}
				else if (Store2.DiamondItems[l].Diamonds == this.defaultDiamonds[l])
				{
					float num4 = (1f - Store2.DiamondItems[l].Price / (float)this.defaultPrices[l]) * 100f;
					this.diamonds[l].transform.Find("Discount").gameObject.SetActive(true);
					this.diamonds[l].transform.Find("Discount").GetComponent<Text>().text = string.Format("{0}% Off", num4.ToString());
				}
				else
				{
					this.diamonds[l].transform.Find("Discount").gameObject.SetActive(true);
					float num5 = ((float)Store2.DiamondItems[l].Diamonds / (float)this.defaultDiamonds[l] - 1f) * 100f;
					if (num5 % 1f < 0.95f)
					{
						num5 = Mathf.Floor(num5);
					}
					else
					{
						num5 = Mathf.Round(num5);
					}
					this.diamonds[l].transform.Find("Discount").GetComponent<Text>().text = string.Format("{0}% Bonus!", ((int)num5).ToString());
				}
				this.diamonds[l].onClick.RemoveAllListeners();
				this.diamonds[l].onClick.AddListener(delegate()
				{
					for (int num29 = 0; num29 < this.diamonds.Length; num29++)
					{
						this.diamonds[num29].interactable = false;
					}
					this.PurchaseDiamonds(storeItem);
				});
				this.HandleDiamondsMetadata(storeItem, this.diamonds[l].transform);
			}
			if (flag2 && this.diamondsTab.Find("Sale") != null)
			{
				this.diamondsTab.Find("Sale").gameObject.SetActive(true);
			}
			foreach (Store2.BlayfapItem blayfapItem in Store2.BundleItems)
			{
				if (blayfapItem.Id.EndsWith(".starterpack"))
				{
					this.starterItem = blayfapItem;
				}
			}
			if (this.starterItem != null && this.starter != null)
			{
				this.starter.interactable = (Store2.DiamondItems != null && (Playfab.AwardedItems & Playfab.PlayfabItems.StarterPack) == (Playfab.PlayfabItems)0L);
				this.starter.transform.Find("Price").GetComponent<Text>().text = this.PriceToString(this.starterItem);
			}
			foreach (Store2.BlayfapItem blayfapItem2 in Store2.BundleItems)
			{
				if (blayfapItem2.Id.Contains(".darya"))
				{
					this.daryaItem = blayfapItem2;
				}
			}
			if (this.daryaItem != null && this.darya != null)
			{
				this.darya.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.Darya) == (Playfab.PlayfabItems)0L);
				this.darya.onClick.RemoveAllListeners();
				this.darya.onClick.AddListener(delegate()
				{
					this.darya.interactable = false;
					this.PurchaseDiamonds(this.daryaItem);
				});
				this.darya.transform.Find("Price").GetComponent<Text>().text = this.PriceToString(this.daryaItem);
			}
			foreach (Store2.BlayfapItem blayfapItem3 in Store2.BundleItems)
			{
				if (blayfapItem3.Id.Contains(".charlotte"))
				{
					this.charlotteItem = blayfapItem3;
				}
			}
			if (this.charlotteItem != null && this.charlotte != null)
			{
				this.charlotte.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.Charlotte) == (Playfab.PlayfabItems)0L);
				this.charlotte.onClick.RemoveAllListeners();
				this.charlotte.onClick.AddListener(delegate()
				{
					this.charlotte.interactable = false;
					this.PurchaseDiamonds(this.charlotteItem);
				});
				this.charlotte.transform.Find("Price").GetComponent<Text>().text = this.PriceToString(this.charlotteItem);
			}
			foreach (Store2.BlayfapItem blayfapItem4 in Store2.BundleItems)
			{
				if (blayfapItem4.Id.Contains(".odango"))
				{
					this.odangoItem = blayfapItem4;
				}
			}
			if (this.odangoItem != null && this.odango != null)
			{
				this.odango.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.Odango) == (Playfab.PlayfabItems)0L);
				this.odango.onClick.RemoveAllListeners();
				this.odango.onClick.AddListener(delegate()
				{
					this.odango.interactable = false;
					this.PurchaseItem(Store2.PurchaseType.Odango, (int)this.odangoItem.Price);
				});
				this.odango.transform.Find("Price").GetComponent<Text>().text = this.odangoItem.Price.ToString();
			}
			foreach (Store2.BlayfapItem blayfapItem5 in Store2.BundleItems)
			{
				if (blayfapItem5.Id.Contains(".shibuki"))
				{
					this.shibukiItem = blayfapItem5;
				}
			}
			if (this.shibukiItem != null && this.shibuki != null)
			{
				this.shibuki.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.Shibuki) == (Playfab.PlayfabItems)0L);
				this.shibuki.onClick.RemoveAllListeners();
				this.shibuki.onClick.AddListener(delegate()
				{
					this.shibuki.interactable = false;
					this.PurchaseItem(Store2.PurchaseType.Shibuki, (int)this.shibukiItem.Price);
				});
				this.shibuki.transform.Find("Price").GetComponent<Text>().text = this.shibukiItem.Price.ToString();
			}
			foreach (Store2.BlayfapItem blayfapItem6 in Store2.BundleItems)
			{
				if (blayfapItem6.Id.Contains(".sirina"))
				{
					this.sirinaItem = blayfapItem6;
				}
			}
			if (this.sirinaItem != null && this.sirina != null)
			{
				this.sirina.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.Sirina) == (Playfab.PlayfabItems)0L);
				this.sirina.onClick.RemoveAllListeners();
				this.sirina.onClick.AddListener(delegate()
				{
					this.sirina.interactable = false;
					this.PurchaseItem(Store2.PurchaseType.Sirina, (int)this.sirinaItem.Price);
				});
				this.sirina.transform.Find("Price").GetComponent<Text>().text = this.sirinaItem.Price.ToString();
			}
			foreach (Store2.BlayfapItem blayfapItem7 in Store2.BundleItems)
			{
				if (blayfapItem7.Id.Contains(".vellatrix"))
				{
					this.vellatrixItem = blayfapItem7;
				}
			}
			if (this.vellatrixItem != null && this.vellatrix != null)
			{
				this.vellatrix.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.Vellatrix) == (Playfab.PlayfabItems)0L);
				this.vellatrix.onClick.RemoveAllListeners();
				this.vellatrix.onClick.AddListener(delegate()
				{
					this.vellatrix.interactable = false;
					this.PurchaseItem(Store2.PurchaseType.Vellatrix, (int)this.vellatrixItem.Price);
				});
				this.vellatrix.transform.Find("Price").GetComponent<Text>().text = this.vellatrixItem.Price.ToString();
			}
			foreach (Store2.BlayfapItem blayfapItem8 in Store2.BundleItems)
			{
				if (blayfapItem8.Id.Contains(".roxxy"))
				{
					this.roxxyItem = blayfapItem8;
				}
			}
			if (this.roxxyItem != null && this.roxxy != null)
			{
				bool flag3 = TaskManager.IsCompletedEventSet(98) && TaskManager.IsCompletedEventSet(99);
				this.roxxy.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.Roxxy) == (Playfab.PlayfabItems)0L && !flag3);
				this.roxxy.onClick.RemoveAllListeners();
				this.roxxy.onClick.AddListener(delegate()
				{
					this.roxxy.interactable = false;
					this.PurchaseItem(Store2.PurchaseType.Roxxy, (int)this.roxxyItem.Price);
				});
				this.roxxy.transform.Find("Price").GetComponent<Text>().text = this.roxxyItem.Price.ToString();
			}
			foreach (Store2.BlayfapItem blayfapItem9 in Store2.BundleItems)
			{
				if (blayfapItem9.Id.Contains(".tessa"))
				{
					this.tessaItem = blayfapItem9;
				}
			}
			if (this.tessaItem != null && this.tessa != null)
			{
				bool flag4 = TaskManager.IsCompletedEventSet(101) && TaskManager.IsCompletedEventSet(102);
				this.tessa.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.Tessa) == (Playfab.PlayfabItems)0L && !flag4);
				this.tessa.onClick.RemoveAllListeners();
				this.tessa.onClick.AddListener(delegate()
				{
					this.tessa.interactable = false;
					this.PurchaseItem(Store2.PurchaseType.Tessa, (int)this.tessaItem.Price);
				});
				this.tessa.transform.Find("Price").GetComponent<Text>().text = this.tessaItem.Price.ToString();
			}
			foreach (Store2.BlayfapItem blayfapItem10 in Store2.BundleItems)
			{
				if (blayfapItem10.Id.Contains(".claudia"))
				{
					this.claudiaItem = blayfapItem10;
				}
			}
			if (this.claudiaItem != null && this.claudia != null)
			{
				bool flag5 = TaskManager.IsCompletedEventSet(104) && TaskManager.IsCompletedEventSet(105);
				this.claudia.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.Claudia) == (Playfab.PlayfabItems)0L && !flag5);
				this.claudia.onClick.RemoveAllListeners();
				this.claudia.onClick.AddListener(delegate()
				{
					this.claudia.interactable = false;
					this.PurchaseItem(Store2.PurchaseType.Claudia, (int)this.claudiaItem.Price);
				});
				this.claudia.transform.Find("Price").GetComponent<Text>().text = this.claudiaItem.Price.ToString();
			}
			foreach (Store2.BlayfapItem blayfapItem11 in Store2.BundleItems)
			{
				if (blayfapItem11.Id.Contains(".rosa"))
				{
					this.rosaItem = blayfapItem11;
				}
			}
			if (this.rosaItem != null && this.rosa != null)
			{
				bool flag6 = TaskManager.IsCompletedEventSet(107) && TaskManager.IsCompletedEventSet(108);
				this.rosa.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.Rosa) == (Playfab.PlayfabItems)0L && !flag6);
				this.rosa.onClick.RemoveAllListeners();
				this.rosa.onClick.AddListener(delegate()
				{
					this.rosa.interactable = false;
					this.PurchaseItem(Store2.PurchaseType.Rosa, (int)this.rosaItem.Price);
				});
				this.rosa.transform.Find("Price").GetComponent<Text>().text = this.rosaItem.Price.ToString();
			}
			foreach (Store2.BlayfapItem blayfapItem12 in Store2.BundleItems)
			{
				if (blayfapItem12.Id.Contains(".juliet"))
				{
					this.julietItem = blayfapItem12;
				}
			}
			if (this.julietItem != null && this.juliet != null)
			{
				bool flag7 = TaskManager.IsCompletedEventSet(110) && TaskManager.IsCompletedEventSet(111);
				this.juliet.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.Juliet) == (Playfab.PlayfabItems)0L && !flag7);
				this.juliet.onClick.RemoveAllListeners();
				this.juliet.onClick.AddListener(delegate()
				{
					this.juliet.interactable = false;
					this.PurchaseItem(Store2.PurchaseType.Juliet, (int)this.julietItem.Price);
				});
				this.juliet.transform.Find("Price").GetComponent<Text>().text = this.julietItem.Price.ToString();
			}
			foreach (Store2.BlayfapItem blayfapItem13 in Store2.BundleItems)
			{
				if (blayfapItem13.Id.Contains(".wendy"))
				{
					this.wendyItem = blayfapItem13;
				}
			}
			if (this.wendyItem != null && this.wendy != null)
			{
				bool flag8 = TaskManager.IsCompletedEventSet(113) && TaskManager.IsCompletedEventSet(114);
				this.wendy.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.Wendy) == (Playfab.PlayfabItems)0L && !flag8);
				this.wendy.onClick.RemoveAllListeners();
				this.wendy.onClick.AddListener(delegate()
				{
					this.wendy.interactable = false;
					this.PurchaseItem(Store2.PurchaseType.Wendy, (int)this.wendyItem.Price);
				});
				this.wendy.transform.Find("Price").GetComponent<Text>().text = this.wendyItem.Price.ToString();
			}
			foreach (Store2.BlayfapItem blayfapItem14 in Store2.BundleItems)
			{
				if (blayfapItem14.Id.Contains(".ruri"))
				{
					this.ruriItem = blayfapItem14;
				}
			}
			if (this.ruriItem != null && this.ruri != null)
			{
				bool flag9 = TaskManager.IsCompletedEventSet(116) && TaskManager.IsCompletedEventSet(117);
				this.ruri.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.Ruri) == (Playfab.PlayfabItems)0L && !flag9);
				this.ruri.onClick.RemoveAllListeners();
				this.ruri.onClick.AddListener(delegate()
				{
					this.ruri.interactable = false;
					this.PurchaseItem(Store2.PurchaseType.Ruri, (int)this.ruriItem.Price);
				});
				this.ruri.transform.Find("Price").GetComponent<Text>().text = this.ruriItem.Price.ToString();
			}
			foreach (Store2.BlayfapItem blayfapItem15 in Store2.BundleItems)
			{
				if (blayfapItem15.Id.Contains(".sawyer"))
				{
					this.sawyerItem = blayfapItem15;
				}
			}
			if (this.sawyerItem != null && this.sawyer != null)
			{
				bool flag10 = TaskManager.IsCompletedEventSet(140) && TaskManager.IsCompletedEventSet(141);
				this.sawyer.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.Sawyer) == (Playfab.PlayfabItems)0L && !flag10);
				this.sawyer.onClick.RemoveAllListeners();
				this.sawyer.onClick.AddListener(delegate()
				{
					this.sawyer.interactable = false;
					this.PurchaseItem(Store2.PurchaseType.Sawyer, (int)this.sawyerItem.Price);
				});
				this.sawyer.transform.Find("Price").GetComponent<Text>().text = this.sawyerItem.Price.ToString();
			}
			foreach (Store2.BlayfapItem blayfapItem16 in Store2.BundleItems)
			{
				if (blayfapItem16.Id.Contains(".renee"))
				{
					this.reneeItem = blayfapItem16;
				}
			}
			if (this.reneeItem != null && this.renee != null)
			{
				bool flag11 = TaskManager.IsCompletedEventSet(161) && TaskManager.IsCompletedEventSet(162);
				this.renee.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.Renee) == (Playfab.PlayfabItems)0L && !flag11);
				this.renee.onClick.RemoveAllListeners();
				this.renee.onClick.AddListener(delegate()
				{
					this.renee.interactable = false;
					this.PurchaseItem(Store2.PurchaseType.Renee, (int)this.reneeItem.Price);
				});
				this.renee.transform.Find("Price").GetComponent<Text>().text = this.reneeItem.Price.ToString();
			}
			foreach (Store2.BlayfapItem blayfapItem17 in Store2.BundleItems)
			{
				if (blayfapItem17.Id.Contains(".mallory"))
				{
					this.malloryItem = blayfapItem17;
				}
			}
			if (this.malloryItem != null && this.mallory != null)
			{
				this.mallory.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.Mallory) == (Playfab.PlayfabItems)0L);
				this.mallory.onClick.RemoveAllListeners();
				this.mallory.onClick.AddListener(delegate()
				{
					this.mallory.interactable = false;
					this.PurchaseDiamonds(this.malloryItem);
				});
				this.mallory.transform.Find("Price").GetComponent<Text>().text = this.PriceToString(this.malloryItem);
			}
			foreach (Store2.BlayfapItem blayfapItem18 in Store2.BundleItems)
			{
				if (blayfapItem18.Id.Contains(".explora"))
				{
					this.exploraItem = blayfapItem18;
				}
			}
			if (this.exploraItem != null && this.explora != null)
			{
				bool flag12 = TaskManager.IsCompletedEventSet(143) && TaskManager.IsCompletedEventSet(144);
				this.explora.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.Explora) == (Playfab.PlayfabItems)0L && !flag12);
				this.explora.onClick.RemoveAllListeners();
				this.explora.onClick.AddListener(delegate()
				{
					this.explora.interactable = false;
					this.PurchaseDiamonds(this.exploraItem);
				});
				this.explora.transform.Find("Price").GetComponent<Text>().text = this.PriceToString(this.exploraItem);
			}
			foreach (Store2.BlayfapItem blayfapItem19 in Store2.BundleItems)
			{
				if (blayfapItem19.Id.Contains(".generica"))
				{
					this.genericaItem = blayfapItem19;
				}
			}
			if (this.genericaItem != null && this.generica != null)
			{
				bool flag13 = TaskManager.IsCompletedEventSet(119) && TaskManager.IsCompletedEventSet(120);
				this.generica.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.Generica) == (Playfab.PlayfabItems)0L && !flag13);
				this.generica.onClick.RemoveAllListeners();
				this.generica.onClick.AddListener(delegate()
				{
					this.generica.interactable = false;
					this.PurchaseItem(Store2.PurchaseType.Generica, (int)this.genericaItem.Price);
				});
				this.generica.transform.Find("Price").GetComponent<Text>().text = this.genericaItem.Price.ToString();
			}
			foreach (Store2.BlayfapItem blayfapItem20 in Store2.BundleItems)
			{
				if (blayfapItem20.Id.Contains(".catara"))
				{
					this.cataraItem = blayfapItem20;
				}
			}
			if (this.cataraItem != null && this.catara != null)
			{
				this.catara.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.Catara) == (Playfab.PlayfabItems)0L);
				this.catara.onClick.RemoveAllListeners();
				this.catara.onClick.AddListener(delegate()
				{
					this.catara.interactable = false;
					this.PurchaseDiamonds(this.cataraItem);
				});
				this.catara.transform.Find("Price").GetComponent<Text>().text = this.PriceToString(this.cataraItem);
			}
			foreach (Store2.BlayfapItem blayfapItem21 in Store2.BundleItems)
			{
				if (blayfapItem21.Id.Contains(".suzu"))
				{
					this.suzuItem = blayfapItem21;
				}
			}
			if (this.suzuItem != null && this.suzu != null)
			{
				this.suzu.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.Suzu) == (Playfab.PlayfabItems)0L);
				this.suzu.onClick.RemoveAllListeners();
				this.suzu.onClick.AddListener(delegate()
				{
					this.suzu.interactable = false;
					this.PurchaseDiamonds(this.suzuItem);
				});
				this.suzu.transform.Find("Price").GetComponent<Text>().text = this.PriceToString(this.suzuItem);
			}
			foreach (Store2.BlayfapItem blayfapItem22 in Store2.BundleItems)
			{
				if (blayfapItem22.Id.Contains(".esper"))
				{
					this.esperItem = blayfapItem22;
				}
			}
			if (this.esperItem != null && this.esper != null)
			{
				bool flag14 = TaskManager.IsCompletedEventSet(146) && TaskManager.IsCompletedEventSet(147);
				this.esper.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.Esper) == (Playfab.PlayfabItems)0L && !flag14);
				this.esper.onClick.RemoveAllListeners();
				this.esper.onClick.AddListener(delegate()
				{
					this.esper.interactable = false;
					this.PurchaseItem(Store2.PurchaseType.Esper, (int)this.esperItem.Price);
				});
				this.esper.transform.Find("Price").GetComponent<Text>().text = this.esperItem.Price.ToString();
			}
			foreach (Store2.BlayfapItem blayfapItem23 in Store2.BundleItems)
			{
				if (blayfapItem23.Id.Contains(".lustat"))
				{
					this.lustatItem = blayfapItem23;
				}
			}
			if (this.lustatItem != null && this.lustat != null)
			{
				bool flag15 = TaskManager.IsCompletedEventSet(128) && TaskManager.IsCompletedEventSet(129);
				this.lustat.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.Lustat) == (Playfab.PlayfabItems)0L && !flag15);
				this.lustat.onClick.RemoveAllListeners();
				this.lustat.onClick.AddListener(delegate()
				{
					this.lustat.interactable = false;
					this.PurchaseItem(Store2.PurchaseType.Lustat, (int)this.lustatItem.Price);
				});
				this.lustat.transform.Find("Price").GetComponent<Text>().text = this.lustatItem.Price.ToString();
			}
			foreach (Store2.BlayfapItem blayfapItem24 in Store2.BundleItems)
			{
				if (blayfapItem24.Id.Contains(".lake"))
				{
					this.lakeItem = blayfapItem24;
				}
			}
			if (this.lakeItem != null && this.lake != null)
			{
				bool flag16 = TaskManager.IsCompletedEventSet(179) && TaskManager.IsCompletedEventSet(180);
				this.lake.interactable = ((Playfab.AwardedItems & Playfab.PlayfabItems.Lake) == (Playfab.PlayfabItems)0L && !flag16);
				this.lake.onClick.RemoveAllListeners();
				this.lake.onClick.AddListener(delegate()
				{
					this.lake.interactable = false;
					this.PurchaseItem(Store2.PurchaseType.Lake, (int)this.lakeItem.Price);
				});
				this.lake.transform.Find("Price").GetComponent<Text>().text = this.lakeItem.Price.ToString();
			}
			this.HandleOutfits();
			if (this.darya != null)
			{
				this.darya.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("darya."));
			}
			if (this.charlotte != null)
			{
				this.charlotte.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("charlotte."));
			}
			if (this.suzu != null)
			{
				this.suzu.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("suzu."));
			}
			if (this.odango != null)
			{
				this.odango.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("odango."));
			}
			if (this.shibuki != null)
			{
				this.shibuki.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("shibuki."));
			}
			if (this.sirina != null)
			{
				this.sirina.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("sirina."));
			}
			if (this.catara != null)
			{
				this.catara.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("catara."));
			}
			if (this.vellatrix != null)
			{
				this.vellatrix.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("vellatrix."));
			}
			if (this.roxxy != null)
			{
				this.roxxy.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("roxxy."));
			}
			if (this.tessa != null)
			{
				this.tessa.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("tessa."));
			}
			if (this.claudia != null)
			{
				this.claudia.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("claudia."));
			}
			if (this.rosa != null)
			{
				this.rosa.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("rosa."));
			}
			if (this.juliet != null)
			{
				this.juliet.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("juliet."));
			}
			if (this.wendy != null)
			{
				this.wendy.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("wendy."));
			}
			if (this.ruri != null)
			{
				this.ruri.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("ruri."));
			}
			if (this.generica != null)
			{
				this.generica.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("generica."));
			}
			if (this.sawyer != null)
			{
				this.sawyer.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("sawyer."));
			}
			if (this.explora != null)
			{
				this.explora.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("explora."));
			}
			if (this.monsters1 != null)
			{
				this.monsters1.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("monsters1."));
			}
			if (this.monsters2 != null)
			{
				this.monsters2.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("monsters2."));
			}
			if (this.lustat != null)
			{
				this.lustat.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("lustat."));
			}
			if (this.lake != null)
			{
				this.lake.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("lake."));
			}
			if (this.esper != null)
			{
				this.esper.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("esper."));
			}
			if (this.renee != null)
			{
				this.renee.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("renee."));
			}
			if (this.mallory != null)
			{
				this.mallory.transform.parent.gameObject.SetActive(Playfab.Promotion.Contains("mallory."));
			}
			foreach (Store2.BlayfapItem blayfapItem25 in Store2.BundleItems)
			{
				if (blayfapItem25.Id.Contains(".jellequillzone"))
				{
					this.monsterItem1 = blayfapItem25;
				}
				if (blayfapItem25.Id.Contains(".bonchovyspectrum"))
				{
					this.monsterItem2 = blayfapItem25;
				}
				if (blayfapItem25.Id.Contains(".timelord"))
				{
					this.timelordItem = blayfapItem25;
				}
			}
			if (this.monsterItem1 != null && this.monsterItem1.Price > 0U && this.monsters1 != null)
			{
				this.monsters1.onClick.RemoveAllListeners();
				this.monsters1.onClick.AddListener(delegate()
				{
					this.monsters1.interactable = false;
					this.PurchaseItem(Store2.PurchaseType.JelleQuillzone, (int)this.monsterItem1.Price);
				});
				this.monsters1.transform.Find("Price").GetComponent<Text>().text = string.Format("{0}", this.monsterItem1.Price.ToString());
			}
			else if (this.monsters1 != null)
			{
				this.monsters1.transform.Find("Price").GetComponent<Text>().text = "Coming Soon";
			}
			if (this.monsterItem2 != null && this.monsterItem2.Price > 0U && this.monsters2 != null)
			{
				this.monsters2.onClick.RemoveAllListeners();
				this.monsters2.onClick.AddListener(delegate()
				{
					this.monsters2.interactable = false;
					this.PurchaseItem(Store2.PurchaseType.BonchovySpectrum, (int)this.monsterItem2.Price);
				});
				this.monsters2.transform.Find("Price").GetComponent<Text>().text = string.Format("{0}", this.monsterItem2.Price.ToString());
			}
			else if (this.monsters2 != null)
			{
				this.monsters2.transform.Find("Price").GetComponent<Text>().text = "Coming Soon";
			}
			if (this.timelordItem != null && this.timelordItem.Price > 0U && this.timelord != null)
			{
				this.timelord.onClick.RemoveAllListeners();
				this.timelord.onClick.AddListener(delegate()
				{
					this.disabledTime = 5f;
					this.timelord.interactable = false;
					if (GameState.PurchasedMultiplier > 1024)
					{
						Transform popup = GameState.CurrentState.transform.Find("Popups/Confirm Timelord");
						Button component2 = popup.transform.Find("Dialog/Yes Button").GetComponent<Button>();
						component2.onClick.RemoveAllListeners();
						component2.onClick.AddListener(delegate()
						{
							this.PurchaseItem(Store2.PurchaseType.Timelord, (int)this.timelordItem.Price);
							popup.gameObject.SetActive(false);
						});
						popup.gameObject.SetActive(true);
					}
					else
					{
						this.PurchaseItem(Store2.PurchaseType.Timelord, (int)this.timelordItem.Price);
					}
				});
				this.timelord.transform.Find("Price").GetComponent<Text>().text = string.Format("{0}", this.timelordItem.Price.ToString());
				Button component = this.timelord.transform.parent.Find("Use/Use/Button").GetComponent<Button>();
				component.onClick.RemoveAllListeners();
				component.onClick.AddListener(delegate()
				{
					GameState.CurrentState.UpdateOfflineProgress(604800f, 604800f, OfflineProgress.ProgressReason.Timelord);
					Cellphone.TimeSkip(604800);
					GameState.CurrentState.QueueQuickSave();
					global::PlayerPrefs.DeleteKey("PendingTimelord", false);
					this.UpdateTimelord();
				});
				this.UpdateTimelord();
			}
			else if (this.timelord != null)
			{
				this.timelord.transform.Find("Price").GetComponent<Text>().text = "Coming Soon";
				this.timelord.interactable = false;
			}
			if (this.charlotte != null)
			{
				this.SetSaleVisible(this.charlotte.transform, this.charlotteItem, false, true);
			}
			if (this.catara != null)
			{
				this.SetSaleVisible(this.catara.transform, this.cataraItem, false, true);
			}
			if (this.darya != null)
			{
				this.SetSaleVisible(this.darya.transform, this.daryaItem, false, true);
			}
			if (this.suzu != null)
			{
				this.SetSaleVisible(this.suzu.transform, this.suzuItem, false, true);
			}
			if (this.generica != null)
			{
				this.SetSaleVisible(this.generica.transform, this.genericaItem, true, true);
			}
			if (this.wendy != null)
			{
				this.SetSaleVisible(this.wendy.transform, this.wendyItem, true, true);
			}
			if (this.ruri != null)
			{
				this.SetSaleVisible(this.ruri.transform, this.ruriItem, true, true);
			}
			if (this.sawyer != null)
			{
				this.SetSaleVisible(this.sawyer.transform, this.sawyerItem, true, true);
			}
			if (this.lustat != null)
			{
				this.SetSaleVisible(this.lustat.transform, this.lustatItem, true, true);
			}
			if (this.lake != null)
			{
				this.SetSaleVisible(this.lake.transform, this.lakeItem, true, true);
			}
			if (this.shibuki != null)
			{
				this.SetSaleVisible(this.shibuki.transform, this.shibukiItem, true, true);
			}
			if (this.monsters1 != null)
			{
				this.SetSaleVisible(this.monsters1.transform, this.monsterItem1, false, true);
			}
			if (this.monsters2 != null)
			{
				this.SetSaleVisible(this.monsters2.transform, this.monsterItem2, false, true);
			}
			if (this.timelord != null)
			{
				this.SetSaleVisible(this.timelord.transform, this.timelordItem, false, true);
			}
			if (this.vellatrix != null)
			{
				this.SetSaleVisible(this.vellatrix.transform, this.vellatrixItem, false, true);
			}
			if (this.odango != null)
			{
				this.SetSaleVisible(this.odango.transform, this.odangoItem, false, true);
			}
			if (this.juliet != null)
			{
				this.SetSaleVisible(this.juliet.transform, this.julietItem, false, true);
			}
			if (this.tessa != null)
			{
				this.SetSaleVisible(this.tessa.transform, this.tessaItem, false, true);
			}
			if (this.sirina != null)
			{
				this.SetSaleVisible(this.sirina.transform, this.sirinaItem, false, true);
			}
			if (this.explora != null)
			{
				this.SetSaleVisible(this.explora.transform, this.exploraItem, false, true);
			}
			if (this.roxxy != null)
			{
				this.SetSaleVisible(this.roxxy.transform, this.roxxyItem, false, true);
			}
			if (this.rosa != null)
			{
				this.SetSaleVisible(this.rosa.transform, this.rosaItem, false, true);
			}
			if (this.claudia != null)
			{
				this.SetSaleVisible(this.claudia.transform, this.claudiaItem, false, true);
			}
			if (this.esper != null)
			{
				this.SetSaleVisible(this.esper.transform, this.esperItem, true, true);
			}
			if (this.renee != null)
			{
				this.SetSaleVisible(this.renee.transform, this.reneeItem, true, true);
			}
			if (this.mallory != null)
			{
				this.SetSaleVisible(this.mallory.transform, this.malloryItem, false, true);
			}
			this.DisableBundles();
		}
		GameState.CurrentState.PendingPrestige += this.UpdateSkipResetText;
		GameState.CurrentState.TimeMultiplier += this.UpdateSkipResetText;
		this.UpdateSpeedBoosts();
		if (base.gameObject.activeInHierarchy && this.noConnection.gameObject.activeInHierarchy)
		{
			this.SelectTab(0);
		}
	}

	// Token: 0x0600076B RID: 1899 RVA: 0x000430B4 File Offset: 0x000412B4
	private void HandleDiamondsMetadata(Store2.BlayfapItem item, Transform diamondTransform)
	{
		if (diamondTransform == null || !item.GrantsAdditionalItems)
		{
			return;
		}
		CatalogMetadata metadata = item.Metadata;
		diamondTransform.Find("Bonus").gameObject.SetActive(false);
		Image component = diamondTransform.Find("Bonus").GetComponent<Image>();
		component.sprite = null;
		for (int i = 0; i < metadata.AdditionalItems.Length; i++)
		{
			string text = metadata.AdditionalItems[i].ToLowerInvariant();
			foreach (string text2 in this.OutfitRewardTypes)
			{
				if (text.Contains(text2))
				{
					this.SetDiamondsBonusIcon(component, text2);
					break;
				}
			}
			if (component.sprite != null)
			{
				break;
			}
			text = text.Substring(text.LastIndexOf('.') + 1);
			foreach (string text3 in Enum.GetNames(typeof(Balance.GirlName)))
			{
				if (text == text3.ToLowerInvariant())
				{
					this.SetDiamondsBonusIcon(component, "girl");
					break;
				}
			}
			if (component.sprite != null)
			{
				break;
			}
			this.SetDiamondsBonusIcon(component, "pinup");
		}
	}

	// Token: 0x0600076C RID: 1900 RVA: 0x00043218 File Offset: 0x00041418
	private void SetDiamondsBonusIcon(Image image, string rewardType)
	{
		switch (rewardType)
		{
		case "bikini":
			image.sprite = this.OutfitIcons[0];
			goto IL_167;
		case "xmas":
			image.sprite = this.OutfitIcons[1];
			goto IL_167;
		case "wedding":
			image.sprite = this.OutfitIcons[2];
			goto IL_167;
		case "school":
			image.sprite = this.OutfitIcons[3];
			goto IL_167;
		case "unique":
			image.sprite = this.OutfitIcons[4];
			goto IL_167;
		case "animated":
			image.sprite = this.OutfitIcons[5];
			goto IL_167;
		case "deluxe":
			image.sprite = this.OutfitIcons[6];
			goto IL_167;
		case "girl":
			image.sprite = this.MiscIcons[1];
			goto IL_167;
		}
		image.sprite = this.MiscIcons[0];
		IL_167:
		image.gameObject.SetActive(image.sprite != null);
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x000433A4 File Offset: 0x000415A4
	private void HandleInfoButtons()
	{
		SmallBio component = GameState.CurrentState.transform.Find("Popups/Store Small Bio").GetComponent<SmallBio>();
		this.HandleInfoButton(component, Balance.GirlName.Charlotte, this.charlotte);
		this.HandleInfoButton(component, Balance.GirlName.Darya, this.darya);
		this.HandleInfoButton(component, Balance.GirlName.Catara, this.catara);
		this.HandleInfoButton(component, Balance.GirlName.Odango, this.odango);
		this.HandleInfoButton(component, Balance.GirlName.Shibuki, this.shibuki);
		this.HandleInfoButton(component, Balance.GirlName.Sirina, this.sirina);
		this.HandleInfoButton(component, Balance.GirlName.Vellatrix, this.vellatrix);
		this.HandleInfoButton(component, Balance.GirlName.Roxxy, this.roxxy);
		this.HandleInfoButton(component, Balance.GirlName.Tessa, this.tessa);
		this.HandleInfoButton(component, Balance.GirlName.Rosa, this.rosa);
		this.HandleInfoButton(component, Balance.GirlName.Juliet, this.juliet);
		this.HandleInfoButton(component, Balance.GirlName.Wendy, this.wendy);
		this.HandleInfoButton(component, Balance.GirlName.Ruri, this.ruri);
		this.HandleInfoButton(component, Balance.GirlName.Generica, this.generica);
		this.HandleInfoButton(component, Balance.GirlName.Sawyer, this.sawyer);
		this.HandleInfoButton(component, Balance.GirlName.Explora, this.explora);
		this.HandleInfoButton(component, Balance.GirlName.Suzu, this.suzu);
		this.HandleInfoButton(component, Balance.GirlName.Lustat, this.lustat);
		this.HandleInfoButton(component, Balance.GirlName.Lake, this.lake);
		this.HandleInfoButton(component, Balance.GirlName.Claudia, this.claudia);
		this.HandleInfoButton(component, Balance.GirlName.Esper, this.esper);
		this.HandleInfoButton(component, Balance.GirlName.Renee, this.renee);
		this.HandleInfoButton(component, Balance.GirlName.Mallory, this.mallory);
	}

	// Token: 0x0600076E RID: 1902 RVA: 0x00043524 File Offset: 0x00041724
	private void HandleInfoButton(SmallBio smallBio, Balance.GirlName girlName, Button storeButton)
	{
		if (storeButton == null)
		{
			return;
		}
		Button component = storeButton.transform.parent.Find("Info").GetComponent<Button>();
		component.onClick.RemoveAllListeners();
		component.onClick.AddListener(delegate()
		{
			smallBio.Init(girlName);
		});
	}

	// Token: 0x0600076F RID: 1903 RVA: 0x00043590 File Offset: 0x00041790
	private void HandleMonsterInfoButtons()
	{
		if (this.monsters1 == null || this.monsters2 == null)
		{
			return;
		}
		MonsterBios monsterBio = GameState.CurrentState.transform.Find("Popups/Store Bio").GetComponent<MonsterBios>();
		Button component = this.monsters1.transform.parent.Find("Info").GetComponent<Button>();
		component.onClick.RemoveAllListeners();
		component.onClick.AddListener(delegate()
		{
			monsterBio.Init(Playfab.PlayfabItems.JelleQuillzone);
		});
		Button component2 = this.monsters2.transform.parent.Find("Info").GetComponent<Button>();
		component2.onClick.RemoveAllListeners();
		component2.onClick.AddListener(delegate()
		{
			monsterBio.Init(Playfab.PlayfabItems.BonchovySpectrum);
		});
	}

	// Token: 0x06000770 RID: 1904 RVA: 0x0004366C File Offset: 0x0004186C
	private void HandleOutfits()
	{
		this.HandleOutfit("outfits_wedding", Requirement.OutfitType.DiamondRing, Store2.PurchaseType.OutfitsWedding, this.weddingButton, ref this.weddingOutfitItem);
		this.HandleOutfit("outfits_school", Requirement.OutfitType.SchoolUniform, Store2.PurchaseType.OutfitsSchool, this.schoolButton, ref this.schoolOutfitItem);
		this.HandleOutfit("outfits_bikini", Requirement.OutfitType.BathingSuit, Store2.PurchaseType.OutfitsBikini, this.bikiniButton, ref this.bikiniOutfitItem);
		this.HandleOutfit("outfits_xmas", Requirement.OutfitType.Christmas, Store2.PurchaseType.OutfitsChristmas, this.xmasButton, ref this.xmasOutfitItemOutfitItem);
	}

	// Token: 0x06000771 RID: 1905 RVA: 0x000436F4 File Offset: 0x000418F4
	private bool HasOutfits(Requirement.OutfitType outfit)
	{
		if ((Playfab.InventoryObjects & outfit) != Requirement.OutfitType.None)
		{
			return true;
		}
		foreach (Girl girl in Girl.ActiveGirls)
		{
			if (!(girl == null))
			{
				if (girl.GirlName != Balance.GirlName.DarkOne && girl.GirlName != Balance.GirlName.QPiddy)
				{
					if (Girl.IsCoreGirl(girl.GirlName) && (girl.LifetimeOutfits & outfit) == Requirement.OutfitType.None)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	// Token: 0x06000772 RID: 1906 RVA: 0x000437B8 File Offset: 0x000419B8
	private void HandleOutfit(string itemId, Requirement.OutfitType outfitType, Store2.PurchaseType purchaseType, Button button, ref Store2.BlayfapItem blayfapItem)
	{
		if (button == null || Store2.BundleItems == null)
		{
			return;
		}
		foreach (Store2.BlayfapItem blayfapItem2 in Store2.BundleItems)
		{
			if (blayfapItem2 != null && blayfapItem2.Id.Contains(itemId))
			{
				blayfapItem = blayfapItem2;
				button.interactable = (Store2.DiamondItems != null && !this.HasOutfits(outfitType));
				button.onClick.RemoveAllListeners();
				int capturedPrice = (int)blayfapItem2.Price;
				button.onClick.AddListener(delegate()
				{
					button.interactable = false;
					this.PurchaseItem(purchaseType, capturedPrice);
				});
				button.transform.Find("Price").GetComponent<Text>().text = blayfapItem.Price.ToString("n0");
				this.SetSaleVisible(button.transform, blayfapItem2, false, false);
			}
		}
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x000438F4 File Offset: 0x00041AF4
	private void SetSaleVisible(Transform button, Store2.BlayfapItem item, bool moveInfo = false, bool moveButton = true)
	{
		Transform parent = button.transform.parent;
		Transform transform = parent.Find("Sale");
		if (transform != null)
		{
			transform.gameObject.SetActive(item.OnSale);
		}
		if (moveInfo && item.OnSale)
		{
			transform = parent.Find("Info");
			if (transform != null)
			{
				transform.transform.localPosition = new Vector3(79f, 112f, 0f);
			}
		}
		Transform transform2 = parent.Find("Discount");
		if (transform2 != null && item.Discount != 0f && item.OriginalPrice != 0U)
		{
			string text = string.Format("<voffset=0.08em><size=36><sprite index=4> </size></voffset>{0}", item.OriginalPrice.ToString());
			if (item == this.exploraItem)
			{
				try
				{
					text = "$" + (this.TryParseCurrency(this.PriceToString(this.exploraItem)) / item.Discount).ToString("0.00");
				}
				catch (Exception innerException)
				{
					Debug.LogException(new Exception("Failed to parse " + this.PriceToString(this.exploraItem), innerException));
					text = string.Empty;
				}
			}
			if (transform2.Find("Amount") != null)
			{
				transform2.Find("Amount").GetComponent<Text>().text = string.Format("-{0}%", Mathf.RoundToInt(item.Discount * 100f));
			}
			transform2.Find("Diamonds").GetComponent<Text>().text = text;
			if (moveButton)
			{
				button.transform.localPosition = new Vector3(-72f, -236f, 0f);
			}
			if (transform2.Find("Image") != null)
			{
				transform2.Find("Image").gameObject.SetActive(!string.IsNullOrEmpty(text));
			}
			transform2.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x00043B20 File Offset: 0x00041D20
	private float TryParseCurrency(string s)
	{
		int num = 0;
		int num2 = s.Length - 1;
		while (!char.IsNumber(s[num]))
		{
			num++;
		}
		while (!char.IsNumber(s[num2]))
		{
			num2--;
		}
		s = s.Substring(num, num2 - num + 1);
		if (s.Length > 2 && s[s.Length - 3] == ',')
		{
			s = s.Substring(0, s.Length - 3) + "." + s.Substring(s.Length - 2);
		}
		return float.Parse(s, CultureInfo.InvariantCulture);
	}

	// Token: 0x06000775 RID: 1909 RVA: 0x00043BD0 File Offset: 0x00041DD0
	public void UpdateSkipResetText(float value)
	{
		string format = Translations.GetTranslation("everything_else_129_0", "Current Boost: x{0}") + "\n" + Translations.GetTranslation("everything_else_129_1", "<b>Boost After Skip Reset: x{1}</b>");
		base.transform.Find("Skip Reset Tab/Skip Reset/Expected Boost").GetComponent<Text>().text = string.Format(format, GameState.CurrentState.TimeMultiplier.Value.ToString("0.00"), Mathf.Min(2048f, GameState.CurrentState.TimeMultiplier.Value + GameState.CurrentState.PendingPrestige.Value).ToString("0.00"));
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x00043C78 File Offset: 0x00041E78
	public void SelectTab(int tabId)
	{
		this.SelectTabOnLoad = tabId;
		this.InitTabs();
		Transform transform = base.transform.Find("Navigation");
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).GetComponent<Image>().color = ((i != tabId) ? Color.white : new Color(0.9490196f, 0.81960785f, 0.58431375f, 1f));
		}
		this.diamondsTab.gameObject.SetActive(tabId == 0 && Store2.DiamondItems != null);
		this.noConnection.gameObject.SetActive((tabId == 1 || tabId == 0) && Store2.DiamondItems == null);
		this.bundlesTab.gameObject.SetActive(tabId == 1 && Store2.DiamondItems != null);
		this.outfitsTab.gameObject.SetActive(tabId == 2);
		this.skipResetTab.gameObject.SetActive(tabId == 3);
		this.timeBlocksTab.gameObject.SetActive(tabId == 4);
		this.boostsTab.gameObject.SetActive(tabId == 5);
		if (tabId == 1)
		{
			GameState.CurrentState.StartCoroutine(this.ResetScrollPosition(this.bundlesTab));
		}
		else if (tabId == 2)
		{
			GameState.CurrentState.StartCoroutine(this.ResetScrollPosition(this.outfitsTab));
		}
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x00043DF8 File Offset: 0x00041FF8
	public void UpdateSpeedBoosts()
	{
		if (this.speedBoosts == null)
		{
			return;
		}
		this.speedBoosts[0].transform.Find("Button").GetComponent<Image>().color = new Color(1f, 1f, 1f, (GameState.PurchasedMultiplier > 4096) ? 0.5f : 1f);
		this.speedBoosts[0].interactable = (GameState.PurchasedMultiplier <= 4096);
		this.speedBoosts[1].transform.Find("Button").GetComponent<Image>().color = new Color(1f, 1f, 1f, (GameState.PurchasedMultiplier > 1024) ? 0.5f : 1f);
		this.speedBoosts[1].interactable = (GameState.PurchasedMultiplier <= 1024);
		this.speedBoosts[2].transform.Find("Button").GetComponent<Image>().color = new Color(1f, 1f, 1f, (GameState.PurchasedMultiplier > 128) ? 0.5f : 1f);
		this.speedBoosts[2].interactable = (GameState.PurchasedMultiplier <= 128);
		base.transform.Find("Boosts Tab/Speed Boosts/Current").GetComponent<Text>().text = string.Format(Translations.GetTranslation("everything_else_117_0", "Currently at x{0}"), GameState.PurchasedMultiplier.ToString("0.00"));
		base.transform.Find("Skip Reset Tab/Skip Reset/Purchase").GetComponent<Button>().interactable = (GameState.CurrentState.TimeMultiplier.Value < 2048f);
		base.transform.Find("Skip Reset Tab/Skip Reset/Purchase/Button").GetComponent<Image>().color = new Color(1f, 1f, 1f, (GameState.CurrentState.TimeMultiplier.Value >= 2048f) ? 0.5f : 1f);
	}

	// Token: 0x06000778 RID: 1912 RVA: 0x00044024 File Offset: 0x00042224
	private void OnConfirmDiamond(Store2.PurchaseType onConfirmPurchase, int cost)
	{
		Utilities.SendAnalytic(Utilities.AnalyticType.Conversion, onConfirmPurchase.ToString());
		switch (onConfirmPurchase)
		{
		case Store2.PurchaseType.Timeblock1:
			FreeTime.PurchasedTime++;
			global::PlayerPrefs.SetInt("PurchasedTime", FreeTime.PurchasedTime);
			break;
		case Store2.PurchaseType.Timeblock5:
			FreeTime.PurchasedTime += 5;
			global::PlayerPrefs.SetInt("PurchasedTime", FreeTime.PurchasedTime);
			break;
		case Store2.PurchaseType.Timeblock10:
			FreeTime.PurchasedTime += 10;
			global::PlayerPrefs.SetInt("PurchasedTime", FreeTime.PurchasedTime);
			break;
		case Store2.PurchaseType.SkipReset:
			GameState.CurrentState.TimeMultiplier.Value = Mathf.Min(2048f, GameState.CurrentState.TimeMultiplier.Value + GameState.CurrentState.PendingPrestige.Value);
			Achievements.HandleReset();
			global::PlayerPrefs.SetFloat("TimeMultiplier", GameState.CurrentState.TimeMultiplier.Value);
			break;
		case Store2.PurchaseType.SpeedBoost2:
			GameState.PurchasedMultiplier = Mathf.Max(2, GameState.PurchasedMultiplier * 2);
			break;
		case Store2.PurchaseType.SpeedBoost8:
			GameState.PurchasedMultiplier = Mathf.Max(8, GameState.PurchasedMultiplier * 8);
			break;
		case Store2.PurchaseType.SpeedBoost64:
			GameState.PurchasedMultiplier = Mathf.Max(64, GameState.PurchasedMultiplier * 64);
			break;
		case Store2.PurchaseType.JumpFourHours:
			GameState.CurrentState.UpdateOfflineProgress(14400f, 14400f, OfflineProgress.ProgressReason.Timeskip);
			Cellphone.TimeSkip(14400);
			break;
		case Store2.PurchaseType.Jump1Day:
			GameState.CurrentState.UpdateOfflineProgress(86400f, 86400f, OfflineProgress.ProgressReason.Timeskip);
			Cellphone.TimeSkip(86400);
			break;
		case Store2.PurchaseType.Jump7Day:
			GameState.CurrentState.UpdateOfflineProgress(604800f, 604800f, OfflineProgress.ProgressReason.Timeskip);
			Cellphone.TimeSkip(604800);
			break;
		case Store2.PurchaseType.JelleQuillzone:
			Playfab.AwardedItems |= Playfab.PlayfabItems.JelleQuillzone;
			GameState.GetGirlScreen().UnlockGirls();
			this.AwardBlayfapItem(Store2.PurchaseType.JelleQuillzone, false, -1, null, null);
			global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
			GameState.GetAchievements().Rebuild();
			break;
		case Store2.PurchaseType.BonchovySpectrum:
			Playfab.AwardedItems |= Playfab.PlayfabItems.BonchovySpectrum;
			GameState.GetGirlScreen().UnlockGirls();
			this.AwardBlayfapItem(Store2.PurchaseType.BonchovySpectrum, false, -1, null, null);
			global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
			GameState.GetAchievements().GetComponent<Achievements>().Rebuild();
			break;
		case Store2.PurchaseType.Timelord:
			GameState.PurchasedMultiplier = Math.Min(8192, GameState.PurchasedMultiplier * 8);
			FreeTime.PurchasedTime += 15;
			global::PlayerPrefs.SetInt("PurchasedTime", FreeTime.PurchasedTime);
			global::PlayerPrefs.SetInt("PendingTimelord", 1);
			this.UpdateTimelord();
			this.DisableBundles();
			this.UpdateSpeedBoosts();
			GameState.GetIntroScreen().DiamondPurchase();
			BlayFapClient.Instance.AddApiCount("Timelord");
			GameState.CurrentState.QueueQuickSave();
			break;
		case Store2.PurchaseType.Odango:
			Playfab.AwardedItems |= Playfab.PlayfabItems.Odango;
			GameState.GetGirlScreen().UnlockGirls();
			this.AwardBlayfapItem(Store2.PurchaseType.Odango, false, -1, null, null);
			global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
			GameState.GetAchievements().GetComponent<Achievements>().Rebuild();
			Girl.FindGirl(Balance.GirlName.Odango).LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
			Girl.FindGirl(Balance.GirlName.Odango).StoreState();
			break;
		case Store2.PurchaseType.Shibuki:
			Playfab.AwardedItems |= Playfab.PlayfabItems.Shibuki;
			GameState.GetGirlScreen().UnlockGirls();
			this.AwardBlayfapItem(Store2.PurchaseType.Shibuki, false, -1, null, null);
			global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
			GameState.GetAchievements().GetComponent<Achievements>().Rebuild();
			Girl.FindGirl(Balance.GirlName.Shibuki).LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
			Girl.FindGirl(Balance.GirlName.Shibuki).StoreState();
			break;
		case Store2.PurchaseType.Sirina:
			Playfab.AwardedItems |= Playfab.PlayfabItems.Sirina;
			GameState.GetGirlScreen().UnlockGirls();
			this.AwardBlayfapItem(Store2.PurchaseType.Sirina, false, -1, null, null);
			global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
			GameState.GetAchievements().GetComponent<Achievements>().Rebuild();
			Girl.FindGirl(Balance.GirlName.Sirina).LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
			Girl.FindGirl(Balance.GirlName.Sirina).StoreState();
			break;
		case Store2.PurchaseType.Vellatrix:
			Playfab.AwardedItems |= Playfab.PlayfabItems.Vellatrix;
			GameState.GetGirlScreen().UnlockGirls();
			this.AwardBlayfapItem(Store2.PurchaseType.Vellatrix, false, -1, null, null);
			global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
			GameState.GetAchievements().GetComponent<Achievements>().Rebuild();
			Girl.FindGirl(Balance.GirlName.Vellatrix).LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
			Girl.FindGirl(Balance.GirlName.Vellatrix).StoreState();
			break;
		case Store2.PurchaseType.Roxxy:
			Playfab.AwardedItems |= Playfab.PlayfabItems.Roxxy;
			GameState.GetGirlScreen().UnlockGirls();
			this.AwardBlayfapItem(Store2.PurchaseType.Roxxy, false, -1, null, null);
			global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
			GameState.GetAchievements().GetComponent<Achievements>().Rebuild();
			Girl.FindGirl(Balance.GirlName.Roxxy).LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
			Girl.FindGirl(Balance.GirlName.Roxxy).StoreState();
			break;
		case Store2.PurchaseType.Tessa:
			Playfab.AwardedItems |= Playfab.PlayfabItems.Tessa;
			GameState.GetGirlScreen().UnlockGirls();
			this.AwardBlayfapItem(Store2.PurchaseType.Tessa, false, -1, null, null);
			global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
			GameState.GetAchievements().GetComponent<Achievements>().Rebuild();
			Girl.FindGirl(Balance.GirlName.Tessa).LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
			Girl.FindGirl(Balance.GirlName.Tessa).StoreState();
			break;
		case Store2.PurchaseType.Claudia:
			Playfab.AwardedItems |= Playfab.PlayfabItems.Claudia;
			GameState.GetGirlScreen().UnlockGirls();
			this.AwardBlayfapItem(Store2.PurchaseType.Claudia, false, -1, null, null);
			global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
			GameState.GetAchievements().GetComponent<Achievements>().Rebuild();
			Girl.FindGirl(Balance.GirlName.Claudia).LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
			Girl.FindGirl(Balance.GirlName.Claudia).StoreState();
			break;
		case Store2.PurchaseType.Juliet:
			Playfab.AwardedItems |= Playfab.PlayfabItems.Juliet;
			GameState.GetGirlScreen().UnlockGirls();
			this.AwardBlayfapItem(Store2.PurchaseType.Juliet, false, -1, null, null);
			global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
			GameState.GetAchievements().GetComponent<Achievements>().Rebuild();
			Girl.FindGirl(Balance.GirlName.Juliet).LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
			Girl.FindGirl(Balance.GirlName.Juliet).StoreState();
			break;
		case Store2.PurchaseType.Rosa:
			Playfab.AwardedItems |= Playfab.PlayfabItems.Rosa;
			GameState.GetGirlScreen().UnlockGirls();
			this.AwardBlayfapItem(Store2.PurchaseType.Rosa, false, -1, null, null);
			global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
			GameState.GetAchievements().GetComponent<Achievements>().Rebuild();
			Girl.FindGirl(Balance.GirlName.Rosa).LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
			Girl.FindGirl(Balance.GirlName.Rosa).StoreState();
			break;
		case Store2.PurchaseType.Wendy:
			Playfab.AwardedItems |= Playfab.PlayfabItems.Wendy;
			GameState.GetGirlScreen().UnlockGirls();
			this.AwardBlayfapItem(Store2.PurchaseType.Wendy, false, -1, null, null);
			global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
			GameState.GetAchievements().GetComponent<Achievements>().Rebuild();
			Girl.FindGirl(Balance.GirlName.Wendy).LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
			Girl.FindGirl(Balance.GirlName.Wendy).StoreState();
			break;
		case Store2.PurchaseType.Ruri:
			Playfab.AwardedItems |= Playfab.PlayfabItems.Ruri;
			GameState.GetGirlScreen().UnlockGirls();
			this.AwardBlayfapItem(Store2.PurchaseType.Ruri, false, -1, null, null);
			global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
			GameState.GetAchievements().GetComponent<Achievements>().Rebuild();
			Girl.FindGirl(Balance.GirlName.Ruri).LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
			Girl.FindGirl(Balance.GirlName.Ruri).StoreState();
			break;
		case Store2.PurchaseType.Generica:
			Playfab.AwardedItems |= Playfab.PlayfabItems.Generica;
			GameState.GetGirlScreen().UnlockGirls();
			this.AwardBlayfapItem(Store2.PurchaseType.Generica, false, -1, null, null);
			global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
			GameState.GetAchievements().GetComponent<Achievements>().Rebuild();
			Girl.FindGirl(Balance.GirlName.Generica).LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
			Girl.FindGirl(Balance.GirlName.Generica).StoreState();
			break;
		case Store2.PurchaseType.OutfitsBikini:
			this.AwardBlayfapItem(Store2.PurchaseType.OutfitsBikini, true, cost, delegate
			{
				Playfab.InventoryObjects |= Requirement.OutfitType.BathingSuit;
				global::PlayerPrefs.SetInt("PlayfabInventory", (int)Playfab.InventoryObjects);
				this.PreloadOutfits(Requirement.OutfitType.BathingSuit);
			}, null);
			break;
		case Store2.PurchaseType.OutfitsSchool:
			this.AwardBlayfapItem(Store2.PurchaseType.OutfitsSchool, true, cost, delegate
			{
				Playfab.InventoryObjects |= Requirement.OutfitType.SchoolUniform;
				global::PlayerPrefs.SetInt("PlayfabInventory", (int)Playfab.InventoryObjects);
				this.PreloadOutfits(Requirement.OutfitType.SchoolUniform);
			}, null);
			break;
		case Store2.PurchaseType.OutfitsWedding:
			this.AwardBlayfapItem(Store2.PurchaseType.OutfitsWedding, true, cost, delegate
			{
				Playfab.InventoryObjects |= Requirement.OutfitType.DiamondRing;
				global::PlayerPrefs.SetInt("PlayfabInventory", (int)Playfab.InventoryObjects);
				this.PreloadOutfits(Requirement.OutfitType.DiamondRing);
			}, null);
			break;
		case Store2.PurchaseType.OutfitsChristmas:
			this.AwardBlayfapItem(Store2.PurchaseType.OutfitsChristmas, true, cost, delegate
			{
				Playfab.InventoryObjects |= Requirement.OutfitType.Christmas;
				global::PlayerPrefs.SetInt("PlayfabInventory", (int)Playfab.InventoryObjects);
				this.PreloadOutfits(Requirement.OutfitType.Christmas);
			}, null);
			break;
		case Store2.PurchaseType.Lustat:
			Playfab.AwardedItems |= Playfab.PlayfabItems.Lustat;
			GameState.GetGirlScreen().UnlockGirls();
			this.AwardBlayfapItem(Store2.PurchaseType.Lustat, false, -1, null, null);
			global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
			GameState.GetAchievements().GetComponent<Achievements>().Rebuild();
			Girl.FindGirl(Balance.GirlName.Lustat).LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
			Girl.FindGirl(Balance.GirlName.Lustat).StoreState();
			break;
		case Store2.PurchaseType.Sawyer:
			Playfab.AwardedItems |= Playfab.PlayfabItems.Sawyer;
			GameState.GetGirlScreen().UnlockGirls();
			this.AwardBlayfapItem(Store2.PurchaseType.Sawyer, false, -1, null, null);
			global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
			GameState.GetAchievements().GetComponent<Achievements>().Rebuild();
			Girl.FindGirl(Balance.GirlName.Sawyer).LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
			Girl.FindGirl(Balance.GirlName.Sawyer).StoreState();
			break;
		case Store2.PurchaseType.Esper:
			Playfab.AwardedItems |= Playfab.PlayfabItems.Esper;
			GameState.GetGirlScreen().UnlockGirls();
			this.AwardBlayfapItem(Store2.PurchaseType.Esper, false, -1, null, null);
			global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
			GameState.GetAchievements().GetComponent<Achievements>().Rebuild();
			Girl.FindGirl(Balance.GirlName.Esper).LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
			Girl.FindGirl(Balance.GirlName.Esper).StoreState();
			break;
		case Store2.PurchaseType.Renee:
			Playfab.AwardedItems |= Playfab.PlayfabItems.Renee;
			GameState.GetGirlScreen().UnlockGirls();
			this.AwardBlayfapItem(Store2.PurchaseType.Renee, false, -1, null, null);
			global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
			GameState.GetAchievements().GetComponent<Achievements>().Rebuild();
			Girl.FindGirl(Balance.GirlName.Renee).LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
			Girl.FindGirl(Balance.GirlName.Renee).StoreState();
			break;
		case Store2.PurchaseType.Lake:
			Playfab.AwardedItems |= Playfab.PlayfabItems.Lake;
			GameState.GetGirlScreen().UnlockGirls();
			this.AwardBlayfapItem(Store2.PurchaseType.Lake, false, -1, null, null);
			global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
			GameState.GetAchievements().GetComponent<Achievements>().Rebuild();
			Girl.FindGirl(Balance.GirlName.Lake).LifetimeOutfits |= (Requirement.OutfitType.Christmas | Requirement.OutfitType.SchoolUniform | Requirement.OutfitType.BathingSuit | Requirement.OutfitType.DiamondRing | Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude);
			Girl.FindGirl(Balance.GirlName.Lake).StoreState();
			break;
		}
		this.DisableBundles();
		this.UpdateSpeedBoosts();
		Girl.InitDeeplinking();
		GameState.CurrentState.QueueQuickSave();
		GameState.CurrentState.TaskSystem.UpdateGirlOutfits();
	}

	// Token: 0x06000779 RID: 1913 RVA: 0x00044B58 File Offset: 0x00042D58
	public void PreloadUnlockedOutfits()
	{
		this.PreloadOutfits(Requirement.OutfitType.BathingSuit);
		this.PreloadOutfits(Requirement.OutfitType.Christmas);
		this.PreloadOutfits(Requirement.OutfitType.SchoolUniform);
		this.PreloadOutfits(Requirement.OutfitType.DiamondRing);
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x00044B94 File Offset: 0x00042D94
	private void PreloadOutfits(Requirement.OutfitType outfit)
	{
	}

	// Token: 0x0600077B RID: 1915 RVA: 0x00044B98 File Offset: 0x00042D98
	private int GetOutfitBundleSize(Requirement.OutfitType outfit)
	{
		return 0;
	}

	// Token: 0x0600077C RID: 1916 RVA: 0x00044BA8 File Offset: 0x00042DA8
	private void ShowPurchaseErrorPopup()
	{
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x00044BAC File Offset: 0x00042DAC
	private void AwardBlayfapItem(Store2.PurchaseType purchaseType, bool showPopupOnFail = false, int diamondCost = -1, Action onSucess = null, Action onFail = null)
	{
		BlayFapClient.Instance.AwardPurchasedItem(purchaseType, delegate(BlayFapResponse response)
		{
			if (response.Error != null)
			{
				if (response.Error.ErrorType != BlayFapResponseError.BlayFapError.DurableItemAlreadyAwarded)
				{
					Notifications.AddNotification(Notifications.NotificationType.Message, Translations.GetTranslation("achievements_409_0", "There was a problem adding the purchased bundle to your inventory.  Please contact support@sadpandastudios.com!"));
					if (onFail != null)
					{
						onFail();
					}
					if (showPopupOnFail)
					{
						this.ShowPurchaseErrorPopup();
					}
					if (diamondCost > 0)
					{
						Utilities.AwardDiamonds(diamondCost);
					}
				}
			}
			else
			{
				if (onSucess != null)
				{
					onSucess();
				}
				this.DisableBundles();
				GameState.GetIntroScreen().DiamondPurchase();
			}
		});
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x00044BFC File Offset: 0x00042DFC
	public void PurchaseItem(Store2.PurchaseType purchaseType, int cost)
	{
		this.disabledTime = 5f;
		GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.Click2);
		Utilities.ConfirmPurchase(cost, delegate
		{
			this.OnConfirmDiamond(purchaseType, cost);
		});
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x00044C58 File Offset: 0x00042E58
	public void UpdateTimelord()
	{
		bool flag = global::PlayerPrefs.GetInt("PendingTimelord", 0) == 1;
		Transform transform = base.transform.Find("Bundles Tab/Scroll View/Content/Time Lord Bundle");
		transform.Find("Header").gameObject.SetActive(!flag);
		transform.Find("Time Skip").gameObject.SetActive(!flag);
		transform.Find("Time Blocks").gameObject.SetActive(!flag);
		transform.Find("Multiplier").gameObject.SetActive(!flag);
		transform.Find("Button").gameObject.SetActive(!flag);
		transform.Find("Use").gameObject.SetActive(flag);
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x00044D18 File Offset: 0x00042F18
	public void PurchaseDiamonds(Store2.BlayfapItem storeItem)
	{
		BlayFapIntegration.VerifyIsCurrentSession(delegate
		{
			this.disabledTime = 5f;
			if (storeItem == null)
			{
				return;
			}
			if (this._girlButtonPairs != null)
			{
				foreach (KeyValuePair<Balance.GirlName, Button> keyValuePair in this._girlButtonPairs)
				{
					if (keyValuePair.Value != null)
					{
						keyValuePair.Value.interactable = false;
					}
				}
			}
			if (this.diamonds != null)
			{
				foreach (Button button in this.diamonds)
				{
					button.interactable = false;
				}
			}
			if (Kongregate.IsKongregate())
			{
				Kongregate.PurchaseItem(storeItem.Id.ToLowerInvariant());
			}
			else if (Nutaku.Connected)
			{
				Nutaku.StartPurchase(storeItem, storeItem.Diamonds);
			}
			else if (SteamManager.Initialized)
			{
				StartPurchaseRequest startPurchaseRequest = new StartPurchaseRequest
				{
					Description = ((!storeItem.Id.EndsWith("starterpack")) ? ((!storeItem.Id.EndsWith("explora")) ? ((!storeItem.Id.EndsWith("mallory")) ? ((!storeItem.Id.EndsWith("darya")) ? ((!storeItem.Id.EndsWith("catara")) ? ((!storeItem.Id.EndsWith("charlotte")) ? ((!storeItem.Id.EndsWith("suzu")) ? ("Purchase " + storeItem.Diamonds.ToString() + " in-game diamond currency.") : "Purchase Suzu in-game character.") : "Purchase Charlotte in-game character.") : "Purchase Catara in-game character.") : "Purchase Darya in-game character.") : "Purchase Mallory in-game character.") : "Purchase Explora in-game character.") : "Purchase Starter Pack."),
					ItemID = storeItem.Id,
					BlayFapId = BlayFapClient.BlayFapId
				};
				Debug.Log("Starting purchase of " + startPurchaseRequest.ItemID);
				if (this.microTxnAuthorizationResponseCallback != null)
				{
					this.microTxnAuthorizationResponseCallback.Unregister();
				}
				BlayFapClient.Instance.StartPurchase(startPurchaseRequest, delegate(StartPurchaseResponse response)
				{
					if (response.Error != null)
					{
						Debug.LogErrorFormat("Steam Purchase failure: [{0}]", new object[]
						{
							response.Error.ErrorType.ToString()
						});
						return;
					}
					this.microTxnAuthorizationResponseCallback = Callback<MicroTxnAuthorizationResponse_t>.Create(delegate(MicroTxnAuthorizationResponse_t result)
					{
						ulong num = (ulong)result.m_unAppID;
						CGameID cgameID = new CGameID(SteamUtils.GetAppID());
						if (num != cgameID.m_GameID)
						{
							return;
						}
						this.microTxnAuthorizationResponseCallback.Unregister();
						this.microTxnAuthorizationResponseCallback = null;
						if (result.m_bAuthorized == 1)
						{
							FinishPurchaseRequest request = new FinishPurchaseRequest
							{
								BlayFapId = BlayFapClient.BlayFapId,
								OrderID = result.m_ulOrderID.ToString()
							};
							BlayFapClient.Instance.FinishPurchase(request, delegate(FinishPurchaseResponse finishResponse)
							{
								Store2.BlayfapItem blayfapItem = null;
								foreach (Store2.BlayfapItem blayfapItem2 in Store2.DiamondItems)
								{
									if (blayfapItem2.Id == finishResponse.ItemID)
									{
										blayfapItem = blayfapItem2;
									}
								}
								foreach (Store2.BlayfapItem blayfapItem3 in Store2.BundleItems)
								{
									if (blayfapItem3.Id == finishResponse.ItemID)
									{
										blayfapItem = blayfapItem3;
									}
								}
								if (blayfapItem == null)
								{
									blayfapItem = storeItem;
								}
								this.OnReceiptVerificationComplete(blayfapItem);
							});
						}
						else
						{
							Debug.Log("Purchase failed for an unknown reason.");
						}
					});
				});
			}
		});
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x00044D4C File Offset: 0x00042F4C
	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		Debug.Log("Store Initialized");
		Store2.m_StoreController = controller;
		Store2.m_StoreExtensionProvider = extensions;
		Product[] all = controller.products.all;
		foreach (Product product in all)
		{
			if (product.definition.id.EndsWith("starterpack") && product.hasReceipt)
			{
				Playfab.AwardedItems |= Playfab.PlayfabItems.StarterPack;
			}
			if (product.metadata != null && !string.IsNullOrEmpty(product.metadata.localizedPriceString))
			{
				foreach (Store2.BlayfapItem blayfapItem in Store2.DiamondItems)
				{
					if (blayfapItem.Id == product.definition.id)
					{
						blayfapItem.LocalizedPrice = product.metadata.localizedPriceString;
					}
				}
				foreach (Store2.BlayfapItem blayfapItem2 in Store2.BundleItems)
				{
					if (blayfapItem2.Id == product.definition.id)
					{
						blayfapItem2.LocalizedPrice = product.metadata.localizedPriceString;
					}
				}
			}
		}
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x00044E94 File Offset: 0x00043094
	public void OnInitializeFailed(InitializationFailureReason error)
	{
		Debug.LogError(string.Format("IAP initialization failed: [{0}]", error));
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x00044EAC File Offset: 0x000430AC
	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{
		Product purchasedProduct = args.purchasedProduct;
		Store2.BlayfapItem universeItem = new Store2.BlayfapItem(string.Empty, 0U, null);
		foreach (Store2.BlayfapItem blayfapItem in Store2.DiamondItems)
		{
			if (blayfapItem.Id == purchasedProduct.definition.id)
			{
				universeItem = blayfapItem;
			}
		}
		foreach (Store2.BlayfapItem blayfapItem2 in Store2.BundleItems)
		{
			if (blayfapItem2.Id == purchasedProduct.definition.id)
			{
				universeItem = blayfapItem2;
			}
		}
		Debug.Log(string.Format("ProcessPurchase product [{0}]", purchasedProduct.definition.id));
		Dictionary<string, object> dictionary = (Dictionary<string, object>)MiniJson.JsonDecode(purchasedProduct.receipt);
		if (dictionary == null)
		{
			return PurchaseProcessingResult.Complete;
		}
		string text = (string)dictionary["Payload"];
		this.OnReceiptVerificationComplete(universeItem);
		return PurchaseProcessingResult.Complete;
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x00044FA4 File Offset: 0x000431A4
	private void ValidateAndroidPurchase(string signature, string receiptJson, Action onSuccess, Action<BlayFapResponseError.BlayFapError> onFail)
	{
		ValidateGooglePlayReceiptRequest request = new ValidateGooglePlayReceiptRequest
		{
			Receipt = receiptJson,
			Signature = signature
		};
		BlayFapClient.Instance.ValidateGooglePlayReceipt(request, delegate(ValidateGooglePlayReceiptResponse response)
		{
			if (response.Error == null)
			{
				onSuccess();
			}
			else
			{
				onFail(response.Error.ErrorType);
			}
		});
	}

	// Token: 0x06000785 RID: 1925 RVA: 0x00044FF4 File Offset: 0x000431F4
	private void ValidateIosPurchase(string receipt, Action onSuccess, Action<BlayFapResponseError.BlayFapError> onFail)
	{
		ValidateItunesReceiptRequest request = new ValidateItunesReceiptRequest
		{
			Receipt = receipt
		};
		BlayFapClient.Instance.ValidateItunesReceipt(request, delegate(ValidateItunesReceiptResponse response)
		{
			if (response.Error == null)
			{
				onSuccess();
			}
			else
			{
				onFail(response.Error.ErrorType);
			}
		});
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x0004503C File Offset: 0x0004323C
	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		Debug.Log(string.Format("OnPurchaseFailed product [{0}], PurchaseFailureReason: [{1}]", product.definition.storeSpecificId, failureReason));
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x0004506C File Offset: 0x0004326C
	public void OnReceiptVerificationComplete(Store2.BlayfapItem universeItem)
	{
		try
		{
			if (universeItem.Id.Contains(".diamond"))
			{
				Utilities.AwardDiamonds(universeItem.Diamonds);
				if (universeItem.GrantsAdditionalItems)
				{
					foreach (string text in universeItem.Metadata.AdditionalItems)
					{
						BlayFapIntegration.ConsumeBlayFapItem(new InventoryItem
						{
							ItemId = text
						});
						string text2 = text.Substring(text.LastIndexOf('.') + 1);
						foreach (Girl girl in Girl.ActiveGirls)
						{
							if (!(girl == null))
							{
								if (text2.Contains(girl.GirlName.ToLowerFriendlyString()))
								{
									girl.StoreState();
									break;
								}
							}
						}
					}
					global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
					global::PlayerPrefs.SetInt("PlayfabInventory", (int)Playfab.InventoryObjects);
					GameState.CurrentState.QueueQuickSave();
				}
			}
			else if (universeItem.Id.Contains(".starterpack"))
			{
				Playfab.AwardedItems |= Playfab.PlayfabItems.StarterPack;
				FreeTime.PurchasedTime += 5;
				GameState.PurchasedMultiplier = Mathf.Max(2, GameState.PurchasedMultiplier * 2);
				global::PlayerPrefs.SetInt("PurchasedTime", FreeTime.PurchasedTime);
				Utilities.AwardDiamonds(130);
				GameState.CurrentState.transform.Find("Store Revamp").GetComponent<Store2>().UpdateSpeedBoosts();
				GameState.CurrentState.transform.Find("Store Revamp/Bundles Tab/Scroll View/Content/Starter Pack").gameObject.SetActive(false);
			}
			else if (universeItem.Id.Contains(".darya"))
			{
				global::PlayerPrefs.SetInt("ACH45", 0);
				Utilities.AwardDiamonds(50);
				FreeTime.PurchasedTime += 5;
				global::PlayerPrefs.SetInt("PurchasedTime", FreeTime.PurchasedTime);
				Playfab.AwardedItems |= Playfab.PlayfabItems.Darya;
				global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
				GameState.GetGirlScreen().UnlockGirls();
				this.DisableBundles();
				GameState.GetAchievements().GetComponent<Achievements>().Rebuild();
				GameState.CurrentState.QueueQuickSave();
			}
			else if (universeItem.Id.Contains(".charlotte"))
			{
				global::PlayerPrefs.SetInt("ACH50", 0);
				Playfab.AwardedItems |= Playfab.PlayfabItems.Charlotte;
				global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
				GameState.GetGirlScreen().UnlockGirls();
				this.DisableBundles();
				Transform transform = GameState.CurrentState.transform.Find("Jobs/Scroll View/Content Panel");
				for (int j = 0; j < transform.childCount; j++)
				{
					transform.GetChild(j).GetComponent<Job2>().CheckLock();
				}
				GameState.GetAchievements().GetComponent<Achievements>().Rebuild();
				GameState.CurrentState.QueueQuickSave();
			}
			else if (universeItem.Id.Contains(".explora"))
			{
				global::PlayerPrefs.SetInt("ACH70", 0);
				Playfab.AwardedItems |= Playfab.PlayfabItems.Explora;
				global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
				GameState.GetGirlScreen().UnlockGirls();
				this.DisableBundles();
				GameState.GetAchievements().GetComponent<Achievements>().Rebuild();
				Girl.FindGirl(Balance.GirlName.Explora).LifetimeOutfits |= Requirement.OutfitType.All;
				Girl.FindGirl(Balance.GirlName.Explora).StoreState();
				GameState.CurrentState.QueueQuickSave();
			}
			else if (universeItem.Id.Contains(".mallory"))
			{
				global::PlayerPrefs.SetInt("ACH73", 0);
				Playfab.AwardedItems |= Playfab.PlayfabItems.Mallory;
				global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
				GameState.GetGirlScreen().UnlockGirls();
				this.DisableBundles();
				GameState.GetAchievements().GetComponent<Achievements>().Rebuild();
				Girl.FindGirl(Balance.GirlName.Mallory).LifetimeOutfits |= Requirement.OutfitType.All;
				Girl.FindGirl(Balance.GirlName.Mallory).StoreState();
				GameState.CurrentState.QueueQuickSave();
			}
			else if (universeItem.Id.Contains(".catara"))
			{
				global::PlayerPrefs.SetInt("ACH55", 0);
				Playfab.AwardedItems |= Playfab.PlayfabItems.Catara;
				global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
				GameState.GetGirlScreen().UnlockGirls();
				this.DisableBundles();
				GameState.GetAchievements().GetComponent<Achievements>().Rebuild();
				GameState.CurrentState.transform.Find("Popups/Speed Dating Info").gameObject.SetActive(true);
				GameState.CurrentState.QueueQuickSave();
			}
			else if (universeItem.Id.Contains("suzu"))
			{
				global::PlayerPrefs.SetInt("ACH66", 0);
				global::PlayerPrefs.SetInt("ACH67", 0);
				Playfab.AwardedItems |= Playfab.PlayfabItems.Suzu;
				global::PlayerPrefs.SetLong("PlayfabAwardedItems", (long)Playfab.AwardedItems);
				GameState.GetGirlScreen().UnlockGirls();
				this.DisableBundles();
				Transform transform2 = GameState.CurrentState.transform.Find("Jobs/Scroll View/Content Panel");
				for (int k = 0; k < transform2.childCount; k++)
				{
					transform2.GetChild(k).GetComponent<Job2>().CheckLock();
				}
				string name = "Achievements";
				GameState.CurrentState.transform.Find(name).GetComponent<Achievements>().Rebuild();
				GameState.CurrentState.QueueQuickSave();
			}
			GameState.GetIntroScreen().DiamondPurchase();
			Girl.InitDeeplinking();
		}
		catch (Exception ex)
		{
		}
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x0004563C File Offset: 0x0004383C
	private IEnumerator WaitAndSetScroll(ScrollRect scrollRect, RectTransform target, float deviation)
	{
		yield return null;
		yield return null;
		if (!target.gameObject.activeSelf)
		{
			Debug.LogError("Trying to center an object that is still deactivated: " + target.name);
			yield break;
		}
		this.GetSnapToPositionToBringChildIntoView(scrollRect, target, deviation);
		yield break;
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x00045684 File Offset: 0x00043884
	public void ScrollToGirl(string girlName, bool isMonster, bool isValueBundle = false, string tab = "")
	{
		if (isMonster)
		{
			bool flag = girlName == Balance.GirlName.Jelle.ToFriendlyString() || girlName == Balance.GirlName.Quillzone.ToFriendlyString();
			girlName = ((!flag) ? "Bonchovy and Spectrum" : "Jelle and Quillzone");
		}
		Transform transform = base.transform.Find("Bundles Tab/Scroll View");
		string text = "Content/" + girlName;
		if (!isMonster)
		{
			text += " Bundle";
		}
		Transform transform2 = transform.Find(text);
		if (transform2 == null)
		{
			Debug.LogError("Target bundle shouldn't be null for: " + girlName);
			return;
		}
		GameState.CurrentState.StartCoroutine(this.WaitAndSetScroll(transform.GetComponent<ScrollRect>(), transform2.GetComponent<RectTransform>(), 0f));
	}

	// Token: 0x0600078A RID: 1930 RVA: 0x00045748 File Offset: 0x00043948
	public void GetSnapToPositionToBringChildIntoView(ScrollRect scrollRect, RectTransform target, float deviation)
	{
		float num = scrollRect.GetComponent<RectTransform>().rect.size.x / 2f;
		Vector2 v = new Vector2(deviation - target.anchoredPosition.x, scrollRect.content.localPosition.y);
		v.x = Mathf.Clamp(v.x, -scrollRect.content.sizeDelta.x + num, -num);
		scrollRect.content.localPosition = v;
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x000457E0 File Offset: 0x000439E0
	private IEnumerator ResetScrollPosition(Transform tab)
	{
		yield return null;
		RectTransform contentScrollRect = tab.Find("Scroll View/Content").GetComponent<RectTransform>();
		float halfOfWidth = contentScrollRect.parent.GetComponent<RectTransform>().sizeDelta.x / 2f;
		contentScrollRect.localPosition = new Vector2(-halfOfWidth, 0f);
		yield break;
	}

	// Token: 0x040007AC RID: 1964
	public static Store2.BlayfapItem[] DiamondItems;

	// Token: 0x040007AD RID: 1965
	public static Store2.BlayfapItem[] BundleItems;

	// Token: 0x040007AE RID: 1966
	private Transform boostsTab;

	// Token: 0x040007AF RID: 1967
	private Transform timeBlocksTab;

	// Token: 0x040007B0 RID: 1968
	private Transform diamondsTab;

	// Token: 0x040007B1 RID: 1969
	private Transform skipResetTab;

	// Token: 0x040007B2 RID: 1970
	private Transform noConnection;

	// Token: 0x040007B3 RID: 1971
	private Transform bundlesTab;

	// Token: 0x040007B4 RID: 1972
	private Transform outfitsTab;

	// Token: 0x040007B5 RID: 1973
	private Button[] speedBoosts;

	// Token: 0x040007B6 RID: 1974
	private Button[] timeSkips;

	// Token: 0x040007B7 RID: 1975
	private Button[] timeBlocks;

	// Token: 0x040007B8 RID: 1976
	private Button[] diamonds;

	// Token: 0x040007B9 RID: 1977
	private Button bundle1;

	// Token: 0x040007BA RID: 1978
	private Button darya;

	// Token: 0x040007BB RID: 1979
	private Button monsters1;

	// Token: 0x040007BC RID: 1980
	private Button monsters2;

	// Token: 0x040007BD RID: 1981
	private Button timelord;

	// Token: 0x040007BE RID: 1982
	private Button charlotte;

	// Token: 0x040007BF RID: 1983
	private Button starter;

	// Token: 0x040007C0 RID: 1984
	private Button odango;

	// Token: 0x040007C1 RID: 1985
	private Button shibuki;

	// Token: 0x040007C2 RID: 1986
	private Button sirina;

	// Token: 0x040007C3 RID: 1987
	private Button catara;

	// Token: 0x040007C4 RID: 1988
	private Button vellatrix;

	// Token: 0x040007C5 RID: 1989
	private Button roxxy;

	// Token: 0x040007C6 RID: 1990
	private Button tessa;

	// Token: 0x040007C7 RID: 1991
	private Button claudia;

	// Token: 0x040007C8 RID: 1992
	private Button rosa;

	// Token: 0x040007C9 RID: 1993
	private Button juliet;

	// Token: 0x040007CA RID: 1994
	private Button wendy;

	// Token: 0x040007CB RID: 1995
	private Button ruri;

	// Token: 0x040007CC RID: 1996
	private Button generica;

	// Token: 0x040007CD RID: 1997
	private Button suzu;

	// Token: 0x040007CE RID: 1998
	private Button lustat;

	// Token: 0x040007CF RID: 1999
	private Button sawyer;

	// Token: 0x040007D0 RID: 2000
	private Button explora;

	// Token: 0x040007D1 RID: 2001
	private Button esper;

	// Token: 0x040007D2 RID: 2002
	private Button renee;

	// Token: 0x040007D3 RID: 2003
	private Button mallory;

	// Token: 0x040007D4 RID: 2004
	private Button lake;

	// Token: 0x040007D5 RID: 2005
	private Button weddingButton;

	// Token: 0x040007D6 RID: 2006
	private Button bikiniButton;

	// Token: 0x040007D7 RID: 2007
	private Button xmasButton;

	// Token: 0x040007D8 RID: 2008
	private Button schoolButton;

	// Token: 0x040007D9 RID: 2009
	public Color BoostColor;

	// Token: 0x040007DA RID: 2010
	public Color TimeColor;

	// Token: 0x040007DB RID: 2011
	public Sprite[] DiamondIcons;

	// Token: 0x040007DC RID: 2012
	public Sprite[] OutfitIcons;

	// Token: 0x040007DD RID: 2013
	public Sprite[] MiscIcons;

	// Token: 0x040007DE RID: 2014
	private int[] speedBoostAmounts = new int[]
	{
		2,
		8,
		64
	};

	// Token: 0x040007DF RID: 2015
	private int[] speedBoostCosts = new int[]
	{
		40,
		100,
		180
	};

	// Token: 0x040007E0 RID: 2016
	private int[] timeSkipAmounts = new int[]
	{
		4,
		24,
		168
	};

	// Token: 0x040007E1 RID: 2017
	private int[] timeSkipCosts = new int[]
	{
		10,
		25,
		75
	};

	// Token: 0x040007E2 RID: 2018
	private int[] timeBlockAmounts = new int[]
	{
		1,
		5,
		10
	};

	// Token: 0x040007E3 RID: 2019
	private int[] timeBlockCosts = new int[]
	{
		15,
		50,
		80
	};

	// Token: 0x040007E4 RID: 2020
	public static readonly int SkipResetCost = 30;

	// Token: 0x040007E5 RID: 2021
	private int[] defaultPrices = new int[]
	{
		200,
		500,
		1000,
		2000,
		5000,
		10000
	};

	// Token: 0x040007E6 RID: 2022
	private int[] defaultDiamonds = new int[]
	{
		20,
		55,
		130,
		280,
		750,
		1700
	};

	// Token: 0x040007E7 RID: 2023
	public int SelectTabOnLoad;

	// Token: 0x040007E8 RID: 2024
	private List<KeyValuePair<Balance.GirlName, Button>> _girlButtonPairs;

	// Token: 0x040007E9 RID: 2025
	private static IStoreController m_StoreController;

	// Token: 0x040007EA RID: 2026
	private static IExtensionProvider m_StoreExtensionProvider;

	// Token: 0x040007EB RID: 2027
	private float disabledTime;

	// Token: 0x040007EC RID: 2028
	private Store2.BlayfapItem daryaItem;

	// Token: 0x040007ED RID: 2029
	private Store2.BlayfapItem monsterItem1;

	// Token: 0x040007EE RID: 2030
	private Store2.BlayfapItem monsterItem2;

	// Token: 0x040007EF RID: 2031
	private Store2.BlayfapItem timelordItem;

	// Token: 0x040007F0 RID: 2032
	private Store2.BlayfapItem charlotteItem;

	// Token: 0x040007F1 RID: 2033
	private Store2.BlayfapItem starterItem;

	// Token: 0x040007F2 RID: 2034
	private Store2.BlayfapItem odangoItem;

	// Token: 0x040007F3 RID: 2035
	private Store2.BlayfapItem shibukiItem;

	// Token: 0x040007F4 RID: 2036
	private Store2.BlayfapItem suzuItem;

	// Token: 0x040007F5 RID: 2037
	private Store2.BlayfapItem lustatItem;

	// Token: 0x040007F6 RID: 2038
	private Store2.BlayfapItem sirinaItem;

	// Token: 0x040007F7 RID: 2039
	private Store2.BlayfapItem cataraItem;

	// Token: 0x040007F8 RID: 2040
	private Store2.BlayfapItem vellatrixItem;

	// Token: 0x040007F9 RID: 2041
	private Store2.BlayfapItem roxxyItem;

	// Token: 0x040007FA RID: 2042
	private Store2.BlayfapItem tessaItem;

	// Token: 0x040007FB RID: 2043
	private Store2.BlayfapItem claudiaItem;

	// Token: 0x040007FC RID: 2044
	private Store2.BlayfapItem rosaItem;

	// Token: 0x040007FD RID: 2045
	private Store2.BlayfapItem julietItem;

	// Token: 0x040007FE RID: 2046
	private Store2.BlayfapItem wendyItem;

	// Token: 0x040007FF RID: 2047
	private Store2.BlayfapItem ruriItem;

	// Token: 0x04000800 RID: 2048
	private Store2.BlayfapItem genericaItem;

	// Token: 0x04000801 RID: 2049
	private Store2.BlayfapItem sawyerItem;

	// Token: 0x04000802 RID: 2050
	private Store2.BlayfapItem exploraItem;

	// Token: 0x04000803 RID: 2051
	private Store2.BlayfapItem esperItem;

	// Token: 0x04000804 RID: 2052
	private Store2.BlayfapItem reneeItem;

	// Token: 0x04000805 RID: 2053
	private Store2.BlayfapItem malloryItem;

	// Token: 0x04000806 RID: 2054
	private Store2.BlayfapItem bikiniOutfitItem;

	// Token: 0x04000807 RID: 2055
	private Store2.BlayfapItem schoolOutfitItem;

	// Token: 0x04000808 RID: 2056
	private Store2.BlayfapItem weddingOutfitItem;

	// Token: 0x04000809 RID: 2057
	private Store2.BlayfapItem xmasOutfitItemOutfitItem;

	// Token: 0x0400080A RID: 2058
	private Store2.BlayfapItem lakeItem;

	// Token: 0x0400080B RID: 2059
	private readonly string[] OutfitRewardTypes = new string[]
	{
		"unique",
		"bikini",
		"school",
		"wedding",
		"xmas",
		"animated",
		"deluxe"
	};

	// Token: 0x0400080C RID: 2060
	private Callback<MicroTxnAuthorizationResponse_t> microTxnAuthorizationResponseCallback;

	// Token: 0x02000128 RID: 296
	public enum PurchaseType
	{
		// Token: 0x04000810 RID: 2064
		Timeblock1,
		// Token: 0x04000811 RID: 2065
		Timeblock5,
		// Token: 0x04000812 RID: 2066
		Timeblock10,
		// Token: 0x04000813 RID: 2067
		SkipReset,
		// Token: 0x04000814 RID: 2068
		SpeedBoost2,
		// Token: 0x04000815 RID: 2069
		SpeedBoost8,
		// Token: 0x04000816 RID: 2070
		SpeedBoost64,
		// Token: 0x04000817 RID: 2071
		JumpFourHours,
		// Token: 0x04000818 RID: 2072
		Jump1Day,
		// Token: 0x04000819 RID: 2073
		Jump7Day,
		// Token: 0x0400081A RID: 2074
		JelleQuillzone,
		// Token: 0x0400081B RID: 2075
		BonchovySpectrum,
		// Token: 0x0400081C RID: 2076
		Timelord,
		// Token: 0x0400081D RID: 2077
		Odango,
		// Token: 0x0400081E RID: 2078
		Shibuki,
		// Token: 0x0400081F RID: 2079
		Sirina,
		// Token: 0x04000820 RID: 2080
		Vellatrix,
		// Token: 0x04000821 RID: 2081
		Roxxy,
		// Token: 0x04000822 RID: 2082
		Tessa,
		// Token: 0x04000823 RID: 2083
		Catara,
		// Token: 0x04000824 RID: 2084
		NinaUnique,
		// Token: 0x04000825 RID: 2085
		Claudia,
		// Token: 0x04000826 RID: 2086
		Juliet,
		// Token: 0x04000827 RID: 2087
		Rosa,
		// Token: 0x04000828 RID: 2088
		Wendy,
		// Token: 0x04000829 RID: 2089
		Ruri,
		// Token: 0x0400082A RID: 2090
		Generica,
		// Token: 0x0400082B RID: 2091
		FullVoices,
		// Token: 0x0400082C RID: 2092
		OutfitsBikini,
		// Token: 0x0400082D RID: 2093
		OutfitsSchool,
		// Token: 0x0400082E RID: 2094
		OutfitsWedding,
		// Token: 0x0400082F RID: 2095
		OutfitsChristmas,
		// Token: 0x04000830 RID: 2096
		Lustat,
		// Token: 0x04000831 RID: 2097
		Sawyer,
		// Token: 0x04000832 RID: 2098
		Explora,
		// Token: 0x04000833 RID: 2099
		Esper,
		// Token: 0x04000834 RID: 2100
		Renee,
		// Token: 0x04000835 RID: 2101
		Mallory,
		// Token: 0x04000836 RID: 2102
		OutfitsDeluxe,
		// Token: 0x04000837 RID: 2103
		Lake
	}

	// Token: 0x02000129 RID: 297
	public class BlayfapItem : IComparable<Store2.BlayfapItem>
	{
		// Token: 0x060007AD RID: 1965 RVA: 0x00045D6C File Offset: 0x00043F6C
		public BlayfapItem(string id, uint price, CatalogMetadata metadata = null)
		{
			this.Id = id;
			this.Price = price;
			this.Metadata = metadata;
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060007AE RID: 1966 RVA: 0x00045D94 File Offset: 0x00043F94
		// (set) Token: 0x060007AF RID: 1967 RVA: 0x00045D9C File Offset: 0x00043F9C
		public string Id { get; private set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x00045DA8 File Offset: 0x00043FA8
		public int Diamonds
		{
			get
			{
				int result;
				try
				{
					string[] array = this.Id.Split(new char[]
					{
						'.'
					});
					string text = array[array.Length - 1];
					int num = int.Parse(text.Substring(7));
					result = num;
				}
				catch (Exception)
				{
					result = 0;
				}
				return result;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x00045E18 File Offset: 0x00044018
		// (set) Token: 0x060007B2 RID: 1970 RVA: 0x00045E20 File Offset: 0x00044020
		public uint Price { get; private set; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x00045E2C File Offset: 0x0004402C
		// (set) Token: 0x060007B4 RID: 1972 RVA: 0x00045E34 File Offset: 0x00044034
		public string LocalizedPrice { get; set; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x00045E40 File Offset: 0x00044040
		public bool OnSale
		{
			get
			{
				return this.Price < this.OriginalPrice;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x00045E50 File Offset: 0x00044050
		public float Discount
		{
			get
			{
				return 1f - this.Price / this.OriginalPrice;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060007B7 RID: 1975 RVA: 0x00045E6C File Offset: 0x0004406C
		// (set) Token: 0x060007B8 RID: 1976 RVA: 0x00045E74 File Offset: 0x00044074
		public uint OriginalPrice { get; set; }

		// Token: 0x060007B9 RID: 1977 RVA: 0x00045E80 File Offset: 0x00044080
		public int CompareTo(Store2.BlayfapItem other)
		{
			return this.Price.CompareTo(other.Price);
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x00045EA4 File Offset: 0x000440A4
		public override string ToString()
		{
			return this.Id;
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060007BB RID: 1979 RVA: 0x00045EAC File Offset: 0x000440AC
		// (set) Token: 0x060007BC RID: 1980 RVA: 0x00045EB4 File Offset: 0x000440B4
		public CatalogMetadata Metadata { get; private set; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x00045EC0 File Offset: 0x000440C0
		public bool GrantsAdditionalItems
		{
			get
			{
				if (this.Metadata != null)
				{
					if (this.Metadata.AlternatePrice == null)
					{
						return this.Metadata.AdditionalItems != null && this.Metadata.AdditionalItems.Length > 0;
					}
					foreach (CatalogAlternatePrice catalogAlternatePrice in this.Metadata.AlternatePrice)
					{
						if (catalogAlternatePrice.AdditionalItems != null && catalogAlternatePrice.AdditionalItems.Length > 0)
						{
							return true;
						}
					}
				}
				return false;
			}
		}
	}
}
