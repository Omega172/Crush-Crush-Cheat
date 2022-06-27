using System;
using SadPanda.Platforms;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000099 RID: 153
public class Gift : MonoBehaviour
{
	// Token: 0x0600030A RID: 778 RVA: 0x00016538 File Offset: 0x00014738
	public void Init(OutfitModel outfit)
	{
		base.GetComponent<Button>().onClick.RemoveAllListeners();
		base.GetComponent<Button>().onClick.AddListener(delegate()
		{
			GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.Clicky00);
			GameState.CurrentState.transform.Find("Girls/Popups/Gift Buy Popup").GetComponent<GiftPurchase>().Init(this);
		});
		this.Title = Translations.TranslateOutfit(outfit.LegacyOutfitType);
		this.GiftImage = outfit.Sprite;
		this.OutfitType = outfit.LegacyOutfitType;
		this.OutfitData = outfit;
		this.GiftType = Requirement.GiftType.None;
		base.transform.Find("Gift Name").GetComponent<Text>().text = this.Title;
		base.transform.Find("Gift Image").GetComponent<Image>().sprite = this.GiftImage;
		base.transform.Find("Wear").gameObject.SetActive(false);
		base.transform.Find("Gift Price").gameObject.SetActive(true);
		base.transform.Find("Wear/Check").gameObject.SetActive(Girls.CurrentGirl.Clothing == this.OutfitType);
		base.transform.Find("Wear/Wear Text").GetComponent<Text>().text = Translations.GetTranslation("everything_else_14_2", "swap");
		if (Balance.GetOutfitDiamondCost(this.OutfitType, Girls.CurrentGirl.GirlName) == 0 && (Girls.CurrentGirl.LifetimeOutfits & this.OutfitType) == Requirement.OutfitType.None)
		{
			base.transform.Find("Diamond").gameObject.SetActive(false);
			base.transform.Find("Gift Price").gameObject.SetActive(false);
			base.transform.Find("Not Available").gameObject.SetActive(true);
			base.transform.Find("Not Available").GetComponent<Text>().text = Translations.GetTranslation("everything_else_108_0", "Outfit") + "\n" + Translations.GetTranslation("everything_else_108_1", "Locked");
		}
		else
		{
			base.transform.Find("Diamond").gameObject.SetActive((Girls.CurrentGirl.LifetimeOutfits & this.OutfitType) == Requirement.OutfitType.None && (Playfab.InventoryObjects & this.OutfitType) == Requirement.OutfitType.None);
			base.transform.Find("Gift Price").GetComponent<Text>().text = string.Format("{0}", Balance.GetOutfitDiamondCost(this.OutfitType, Girls.CurrentGirl.GirlName).ToString());
			base.transform.Find("Gift Price").gameObject.SetActive((Girls.CurrentGirl.LifetimeOutfits & this.OutfitType) == Requirement.OutfitType.None && (Playfab.InventoryObjects & this.OutfitType) == Requirement.OutfitType.None);
			base.transform.Find("Gift Price").GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
			base.transform.Find("Not Available").GetComponent<Text>().text = Translations.GetTranslation("everything_else_120_0", "Unlocks at Lover Level");
		}
		bool flag = (Playfab.InventoryObjects & (Requirement.OutfitType.Lingerie | Requirement.OutfitType.Nude) & this.OutfitType) != Requirement.OutfitType.None;
		flag |= ((Playfab.InventoryObjects & this.OutfitType) != Requirement.OutfitType.None && (Girls.CurrentGirl.Data.Id < 18 || Girls.CurrentGirl.Data.Id == 31));
		if ((Girls.CurrentGirl.LifetimeOutfits & this.OutfitType) != Requirement.OutfitType.None || flag)
		{
			if (Girls.CurrentGirl.Love >= 9)
			{
				base.transform.Find("Not Available").gameObject.SetActive(false);
				base.transform.Find("Wear").gameObject.SetActive(true);
				base.transform.Find("Gift Price").gameObject.SetActive(false);
				base.GetComponent<Button>().onClick.RemoveAllListeners();
				base.GetComponent<Button>().onClick.AddListener(delegate()
				{
					if (Girls.CurrentGirl.Clothing == this.OutfitType)
					{
						Girls.CurrentGirl.RequestClothing(Requirement.OutfitType.None, new Action(this.OnOutfitChanged));
					}
					else
					{
						Girls.CurrentGirl.RequestClothing(this.OutfitType, new Action(this.OnOutfitChanged));
					}
					GameState.GetGirlScreen().UpdateGifts();
				});
			}
			else
			{
				base.transform.Find("Not Available").gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x0600030B RID: 779 RVA: 0x0001696C File Offset: 0x00014B6C
	private void OnOutfitChanged()
	{
		this.SetCheck();
		GameState.GetGirlScreen().SetGirlOutfit();
	}

	// Token: 0x0600030C RID: 780 RVA: 0x00016980 File Offset: 0x00014B80
	public void Init(Requirement.GiftType gift)
	{
		for (int i = 0; i < 32; i++)
		{
			if (1 << i == (int)gift)
			{
				this.GiftData = Universe.Gifts[(short)(i + 1)];
			}
		}
		base.GetComponent<Button>().onClick.RemoveAllListeners();
		base.GetComponent<Button>().onClick.AddListener(delegate()
		{
			GameState.CurrentState.GetComponent<Audio>().PlayOnce(Audio.AudioFile.Clicky00);
			GameState.CurrentState.transform.Find("Girls/Popups/Gift Buy Popup").GetComponent<GiftPurchase>().Init(this);
		});
		this.Title = Translations.TranslateGift(gift, Girls.CurrentGirl.GirlName);
		this.GiftImage = ((Girls.CurrentGirl.GirlName != Balance.GirlName.Mallory) ? ((Girls.CurrentGirl.GirlName != Balance.GirlName.Explora) ? this.GiftData.Sprite : this.GiftData.ExploraSprite) : this.GiftData.MallorySprite);
		this.GiftType = gift;
		this.OutfitType = Requirement.OutfitType.None;
		base.transform.Find("Gift Name").GetComponent<Text>().text = this.Title;
		base.transform.Find("Gift Image").GetComponent<Image>().sprite = this.GiftImage;
		base.transform.Find("Wear").gameObject.SetActive(false);
		base.transform.Find("Gift Price").gameObject.SetActive(true);
		base.transform.Find("Wear/Check").gameObject.SetActive(false);
		base.transform.Find("Diamond").gameObject.SetActive(false);
		base.transform.Find("Gift Price").GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
		base.transform.Find("Gift Price").GetComponent<Text>().text = Utilities.ToPrefixedNumber((double)this.GiftData.Price, true, false);
		base.transform.Find("Not Available").GetComponent<Text>().text = Translations.GetTranslation("everything_else_120_0", "Unlocks at\nLover Level");
		base.transform.Find("Not Available").gameObject.SetActive(false);
	}

	// Token: 0x0600030D RID: 781 RVA: 0x00016B9C File Offset: 0x00014D9C
	public void SetCheck()
	{
		base.transform.Find("Wear/Check").gameObject.SetActive(Girls.CurrentGirl.Clothing == this.OutfitType);
	}

	// Token: 0x0600030E RID: 782 RVA: 0x00016BD8 File Offset: 0x00014DD8
	public void OnGift(int quantity)
	{
		if (this.OutfitType != Requirement.OutfitType.None)
		{
			Girls.CurrentGirl.RequestClothing(this.OutfitType, new Action(this.OnOutfitChanged));
			GameObject.Find("Girls").GetComponent<Girls>().GiftGirl(5L, true);
			GameState.GiftCount++;
			Kongregate.SubmitStat("GiftCount", (long)GameState.GiftCount);
			this.Init(this.OutfitData);
		}
		else
		{
			Balance.GirlName girlName = Girls.CurrentGirl.GirlName;
			double num = (double)this.GiftData.GetHearts(girlName) * (double)quantity;
			long heartPayout = (num <= 9.223372036854776E+18) ? ((num >= -9.223372036854776E+18) ? ((long)num) : long.MinValue) : long.MaxValue;
			Girls.CurrentGirl.GiveGift(this.GiftType, quantity);
			GameState.GetGirlScreen().GiftGirl(heartPayout, false);
			GameState.GiftCount += quantity;
			Kongregate.SubmitStat("GiftCount", (long)GameState.GiftCount);
			GameState.CurrentState.transform.Find("Girls/Popups/Gifting").gameObject.SetActive(false);
			GameState.GetGirlScreen().GiftButton.transform.Find("Text").gameObject.SetActive(true);
			GameState.GetGirlScreen().GiftButton.transform.Find("Close").gameObject.SetActive(false);
		}
	}

	// Token: 0x04000355 RID: 853
	public string Title;

	// Token: 0x04000356 RID: 854
	public Sprite GiftImage;

	// Token: 0x04000357 RID: 855
	public Requirement.GiftType GiftType;

	// Token: 0x04000358 RID: 856
	public Requirement.OutfitType OutfitType;

	// Token: 0x04000359 RID: 857
	public GiftModel GiftData;

	// Token: 0x0400035A RID: 858
	private OutfitModel OutfitData;
}
